<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Module_Form.aspx.cs" Inherits="Web.Base.SysModule.Module_Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>菜单导航设置表单</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js"></script>
    <link href="/Themes/Scripts/ShowMsg/msgbox.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/ShowMsg/msgbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js"></script>
    <script src="/Themes/Scripts/FunctionJS.js"></script>
    <script type="text/javascript">
        function onkeyMenu_Name(text) {
            $("#Menu_Title").val(text);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" class="frm">
            <tr>
                <th>模块名称：
                </th>
                <td>
                    <input id="ModuleName" runat="server" type="text" class="txt" datacol="yes" err="模块名称"
                        checkexpession="NotNull" style="width: 90%" onkeyup="onkeyMenu_Name(this.value)" />
                </td>
            </tr>
            <tr>
                <th>模块标记：
                </th>
                <td>
                    <input id="ModuleTitle" runat="server" type="text" class="txt" datacol="yes" err="模块标记"
                        checkexpession="NotNull" style="width: 90%" />
                </td>
            </tr>
            <tr>
                <th>节点位置：
                </th>
                <td>
                    <select id="ParentId" class="select" runat="server" style="width: 92%">
                    </select>
                </td>
            </tr>
            <tr>
                <th>模块图标：
                </th>
                <td>
                    <input id="ModuleImg" runat="server" type="text" class="txt" datacol="yes"
                        style="width: 90%" />
                </td>
            </tr>
            <tr>
                <th>连接目标：
                </th>
                <td>
                    <select id="Target" class="select" runat="server" style="width: 92%">
                        <option value="Iframe">Iframe</option>
                        <option value="Open">Open</option>
                        <option value="href">href</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th>显示顺序：
                </th>
                <td>
                    <input id="SortCode" runat="server" type="text" class="txt" datacol="yes" err="显示顺序"
                        checkexpession="Num" style="width: 90%" />
                </td>
            </tr>
            <tr>
                <th>连接地址：
                </th>
                <td>
                    <textarea id="NavigateUrl" class="txtRemark" runat="server" style="width: 90.5%; height: 50px;"></textarea>
                </td>
            </tr>
            <tr>
                <th>基层单位：
                </th>
                <td>
                    <select id="sel_DeptName" runat="server"  err="所属单位"
                        style="width: 90%">
                    </select>
                </td>
            </tr>
        </table>
        <div class="frmbottom">
            <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClientClick="return CheckDataValid('#form1');"
                OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保 存</span></asp:LinkButton>
            <a class="l-btn" href="javascript:void(0)" onclick="OpenClose();"><span class="l-btn-left">
                <img src="/Themes/Images/cancel.png" alt="" />关 闭</span></a>
        </div>
    </form>
</body>
</html>
