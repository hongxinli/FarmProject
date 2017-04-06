<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Role_Info.aspx.cs" Inherits="Web.Base.SysRole.Role_Info" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>系统角色设置详细</title>
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
        $(function () {
            // divresize(63);
            treeAttrCss();
            $("#UserInfolistright").height(273);
            $("#table2").hide();
            $("#table3").hide();
            $("#table4").hide();
            //FixedTableHeader("#dnd-example", $(window).height() - 91);
            //$("#dnd-example").treeTable({
            //    initialState: "expanded" //collapsed 收缩 expanded展开的
            //});
            $('input[type="checkbox"]').attr('disabled', 'disabled');
        })
        //面板切换
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input id="item_hidden" type="hidden" runat="server" />
        <div class="btnbartitle">
            <div>
                所属角色【<%=_Roles_Name.ToString()%>】
            </div>
        </div>
        <div class="btnbarcontetn">
            <div style="float: left;">
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
        </div>

        <%--基本信息--%>
        <table id="table1" border="0" cellpadding="0" cellspacing="0" class="frm">
            <tr>
                <th>角色名称：
                </th>
                <td>
                    <input id="RolesName" disabled="disabled" runat="server" type="text" class="txt" datacol="yes" err="角色名称"
                        checkexpession="NotNull" style="width: 85%" />
                </td>
            </tr>
            <tr>
                <th>节点位置：
                </th>
                <td>
                    <select disabled="disabled" id="DeptId" class="select" runat="server" style="width: 86.5%">
                    </select>
                </td>
            </tr>
            <tr>
                <th>显示顺序：
                </th>
                <td>
                    <input id="SortCode" disabled="disabled" runat="server" type="text" class="txt" datacol="yes" err="显示顺序"
                        checkexpession="Num" style="width: 85%" />
                </td>
            </tr>
            <tr>
                <th>角色描述：
                </th>
                <td>
                    <textarea id="Remarks" disabled="disabled" class="txtRemark" runat="server" style="width: 85.5%; height: 170px;"></textarea>
                </td>
            </tr>
        </table>
        <%--角色成员--%>
        <div id="table2">
            <div id="UserInfolistright" style="float: left; width: auto; border: 1px solid #ccc; border-top: 0px solid #ccc;">
                <div id="selectedUserInfo" class="grid">
                    <table id="tbSelectUsers" class="grid">
                        <thead>
                            <tr>
                                <td>角色成员
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

    </form>
</body>
</html>
