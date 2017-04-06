<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StepInfo_Form.aspx.cs" Inherits="Web.Base.SysStep.StepInfo_Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>步骤信息表单</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/TreeTable/jquery.treeTable.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeTable/css/jquery.treeTable.css" rel="stylesheet"
        type="text/css" />
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        //初始化
        $(function () {
            treeAttrCss();
            $('#table2').hide();
            $('#table3').hide();
            CheckDeptClick();
            CheckRolesClick();
        })

        function panel(obj) {
            if (obj == 1) {
                $('#table1').show();
                $('#table2').hide();
                $('#table3').hide();
            } else if (obj == 2) {
                $('#table1').hide();
                $("#table2").show();
                $('#table3').hide();
            } else if (obj == 3) {
                $('#table1').hide();
                $("#table2").hide();
                $('#table3').show();
            }
        }
        //获取表单值
        function CheckValid() {
            if (!CheckDataValid('#form1')) {
                return false;
            }
            if (!IsNullOrEmpty(ChekOrgVale)) {
                showWarningMsg("点击面板所属部门，选择部门！");
                return false;
            }
            if (!IsNullOrEmpty(ChekRolesVale)) {
                showWarningMsg("点击面板执行角色，选择角色！");
                return false;
            }
            $("#Item_Hidden").val(CheckboxValue());
        }
        //验证所属单位必填
        var ChekOrgVale = "";
        function CheckDeptClick() {
            var pk_id = GetQuery('key');
            if (IsNullOrEmpty(pk_id)) {
                ChekOrgVale = 1;
            }
            $("#table2 [type = checkbox]").click(function () {
                var val = $(this).val();
                ChekOrgVale = 1;
                $("#table2 [type = checkbox]").each(function () {
                    if ($(this).val() != val)
                        $(this).attr("checked", false);
                })
            })
        }
        var ChekRolesVale = "";
        function CheckRolesClick() {
            var pk_id = GetQuery('key');
            if (IsNullOrEmpty(pk_id)) {
                ChekRolesVale = 1;
            }
            $("#table3 [type = checkbox]").click(function () {
                ChekRolesVale = 1;
                var val = $(this).val();
                //$("#table3 [type = checkbox]").each(function () {
                //    if ($(this).val() != val)
                //        $(this).attr("checked", false);
                //})
            })
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
                        <div id="tab0" class="Tabsel" onclick="GetTabClick(this);panel(1)">
                            基本信息
                        </div>
                        <div id="tab1" class="Tabremovesel" onclick="GetTabClick(this);panel(2);">
                            所属单位
                        </div>
                        <div id="tab2" class="Tabremovesel" onclick="GetTabClick(this);panel(3);">
                            所属角色
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="div-frm" style="height: 275px;">
            <%--基本信息--%>
            <table id="table1" border="0" cellpadding="0" cellspacing="0" class="frm">
                <tr>
                    <th>步骤名称:
                    </th>
                    <td colspan="3">
                        <input id="StepName" runat="server" type="text" class="txt" datacol="yes" err="步骤名称"
                            checkexpession="NotNull" style="width: 552px;" />
                    </td>
                </tr>
                <tr>
                    <th>创建用户:
                    </th>
                    <td>
                        <input id="Creator" disabled="disabled" runat="server" type="text" class="txt" style="width: 200px" />
                    </td>
                    <th>创建时间:
                    </th>
                    <td>
                        <input id="CreateDate" disabled="disabled" runat="server" type="text" class="txt" style="width: 200px" />
                    </td>
                </tr>
                <tr>
                    <th>备注描述:
                    </th>
                    <td colspan="3">
                        <textarea id="StepDesc" class="txtRemark" runat="server" style="width: 552px; height: 83px;"></textarea>
                    </td>
                </tr>
            </table>
            <%--所属单位--%>
            <div id="table2">
                <div class="btnbartitle">
                    <div>
                        组织机构
                    </div>
                </div>
                <div class="div-body" style="height: 245px;">
                    <ul class="strTree">
                        <%=strDeptHtml.ToString()%>
                    </ul>
                </div>
            </div>
            <%--所属角色--%>
            <div id="table3">
                <div class="btnbartitle">
                    <div>
                        所属角色
                    </div>
                </div>
                <div class="div-body" style="height: 245px;">
                    <ul class="strTree">
                        <%=strRoleHtml.ToString()%>
                    </ul>
                </div>
            </div>
        </div>
        <div class="frmbottom">
            <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClientClick="return CheckValid();"
                OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保 存</span></asp:LinkButton>
            <a class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span class="l-btn-left">
                <img src="/Themes/Images/cancel.png" alt="" />关 闭</span></a>
        </div>
    </form>
</body>
</html>
