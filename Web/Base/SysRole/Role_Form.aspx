<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Role_Form.aspx.cs" Inherits="Web.Base.SysRole.Role_Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>系统角色设置表单</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/TreeTable/jquery.treeTable.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeTable/css/jquery.treeTable.css" rel="stylesheet"
        type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        //初始化
        $(function () {
            treeAttrCss();
            $("#table2").height(270);
            $("#UserInfolistright").height(273);
            $("#table2").hide();
            $("#table3").hide();
            $("#table4").hide();
            //SubmitCheckForRC();
        })
        //双击添加员工
        function addUserInfo(userName, userID, deptName) {
            var IsExist = true;
            $("#tbSelectUsers tbody tr").each(function (i) {
                if ($(this).find('td:eq(0)').html() == userName) {
                    IsExist = false;
                    return false;
                }
            })
            if (IsExist == true) {
                var url = 'SysRole.ashx';
                var parm = "action=IsExists&key=" + userID;
                getAjax(url, parm, function (data) {
                    if (data == 1) {
                        showTipsMsg("<span style='color:red'>该员工已经包含其他角色，请勿重复添加！</span>", 4000, 5);
                        return false;
                    } else {
                        $("#tbSelectUsers tbody").append("<tr ondblclick='$(this).remove()'><td>" + userName + "</td><td>" + deptName + "</td><td  style='display:none'>" + userID + "|角色成员</td></tr>");
                        publicobjcss();
                    }
                });
            } else {
                showWarningMsg("【" + userName + "】员工已经存在");
            }
        }
        //点击切换面板
        var IsFixedTableLoad = 1;
        function TabPanel(id) {
            if (id == 1) {
                $("#table1").show();
                $("#table2").hide();
                $("#table3").hide();
                $("#table4").hide();
            } else if (id == 2) {
                $("#table1").hide();
                $("#table2").show();
                $("#table3").hide();
                $("#table4").hide();
                //固定表头
                FixedTableHeader("#tbSelectUsers", $(window).height() - 127);
            } else if (id == 3) {
                $("#table1").hide();
                $("#table2").hide();
                $("#table3").show();
                $("#table4").hide();
                //固定表头
                $("#dnd-example").treeTable({
                    initialState: "expanded" //collapsed 收缩 expanded展开的
                });
                if (IsFixedTableLoad == 1) {
                    FixedTableHeader("#dnd-example", $(window).height() - 127);
                    IsFixedTableLoad = 0;
                }
            } else if (id == 4) {
                $("#table1").hide();
                $("#table2").hide();
                $("#table3").hide();
                $("#table4").show();
            }
        }
        //获取表单值
        function CheckValid() {
            if (!CheckDataValid('#form1')) {
                return false;
            }
            var item_value = "";
            $("#tbSelectUsers tbody tr").each(function (i) {
                item_value += $(this).find('td:eq(2)').html() + ",";
            })
            var item = CheckboxValue();
            item_value = item_value + item + ",";
            if (item_value.indexOf("模块权限") <= 0) {
                showWarningMsg("点击面板模块权限，勾选可查看权限模块！");
                return false;
            }
            if (item_value.indexOf("数据权限") <= 0) {
                showWarningMsg("点击面板数据权限，勾选可查看数据单位！");
                return false;
            }
            $("#Item_Hidden").val(item_value);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input id="Item_Hidden" type="hidden" runat="server" />
        <div class="frmtop">
            <table style="padding: 0px; margin: 0px; height: 100%;" cellpadding="0" cellspacing="0">
                <tr>
                    <td id="menutab" style="vertical-align: bottom;">
                        <div id="tab0" class="Tabsel" onclick="GetTabClick(this);TabPanel(1)">
                            基本信息
                        </div>
                        <div id="tab1" class="Tabremovesel" onclick="GetTabClick(this);TabPanel(2)">
                            角色成员
                        </div>
                        <div id="tab2" class="Tabremovesel" onclick="GetTabClick(this);TabPanel(3)">
                            模块权限
                        </div>
                        <div id="tab3" class="Tabremovesel" onclick="GetTabClick(this);TabPanel(4)">
                            数据权限
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%--基本信息--%>
        <table id="table1" border="0" cellpadding="0" cellspacing="0" class="frm">
            <tr>
                <th>角色名称：
                </th>
                <td>
                    <input id="RolesName" runat="server" type="text" class="txt" datacol="yes" err="角色名称"
                        checkexpession="NotNull" style="width: 85%" />
                </td>
            </tr>
            <tr>
                <th>节点位置：
                </th>
                <td>
                    <select id="DeptId" class="select" runat="server" style="width: 86.5%">
                    </select>
                </td>
            </tr>
            <tr>
                <th>显示顺序：
                </th>
                <td>
                    <input id="SortCode" runat="server" type="text" class="txt" datacol="yes" err="显示顺序"
                        checkexpession="Num" style="width: 85%" />
                </td>
            </tr>
            <tr>
                <th>是否允许编辑：
                </th>
                <td>
                    <select id="AllowEdit" class="select" runat="server" style="width: 86.5%">
                        <option value="0" selected="selected">允许</option>
                        <option value="1">不允许</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th>是否允许删除：
                </th>
                <td>
                    <select id="AllowDelete" class="select" runat="server" style="width: 86.5%">
                        <option value="0" selected="selected">允许</option>
                        <option value="1">不允许</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th>角色描述：
                </th>
                <td>
                    <textarea id="Remarks" class="txtRemark" runat="server" style="width: 85.5%; height: 110px;"></textarea>
                </td>
            </tr>
        </table>
        <%--角色成员--%>
        <div id="table2">
            <div id="UserInfolistleft" style="float: left; width: 48.6%; border: 1px solid #ccc; margin-right: 1px;">
                <div class="box-title" style="height: 27px;">
                    所有成员;<span style="color: Blue;">双击添加</span>
                </div>
                <div class="div-overflow" id="allUserInfo" style="overflow-x: hidden; overflow-y: scroll; padding-bottom: 5px; height: 240px;">
                    <ul class="strTree">
                        <%=str_allUserInfo %>
                    </ul>
                </div>
            </div>
            <div id="UserInfolistright" style="float: left; width: 50%; border: 1px solid #ccc; border-top: 0px solid #ccc;">
                <div class="div-overflow" id="selectedUserInfo" style="overflow-x: hidden; overflow-y: auto; padding-bottom: 5px;">
                    <table id="tbSelectUsers" class="grid">
                        <thead>
                            <tr>
                                <td>已选成员;<span style="color: Red;">双击移除</span>
                                </td>
                                <td>所属部门
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                            <%=str_seleteUserInfo%>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <%--模块权限--%>
        <div id="table3">
            <div class="div-body" style="height: 273px;">
                <table class="example" id="dnd-example">
                    <thead>
                        <tr>
                            <td style="width: 200px; padding-left: 20px;">URL权限
                            </td>
                            <td style="width: 30px; text-align: center;">图标
                            </td>
                            <td style="width: 20px; text-align: center;">
                                <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                                    &nbsp;</label>
                            </td>
                            <td>操作按钮权限
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <%=strModuleHtml%>
                    </tbody>
                </table>
            </div>
        </div>
        <%--数据权限--%>
        <div id="table4">
            <div class="btnbartitle">
                <div>
                    组织机构
                </div>
            </div>
            <div class="div-body" style="height: 245px;">
                <ul class="strTree">
                    <%=strDeptHtml%>
                </ul>
            </div>
        </div>
        <div class="frmbottom">
            <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClientClick="return CheckValid();"
                OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保 存</span></asp:LinkButton>
            <a id="Close" class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span
                class="l-btn-left">
                <img src="/Themes/Images/cancel.png" alt="" />关 闭</span></a>
        </div>
    </form>
</body>
</html>
