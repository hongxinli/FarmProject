<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Code_Form.aspx.cs" Inherits="Web.Base.SysCode.Code_Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>配置信息表单</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" class="frm">
            <tr>
                <th class="auto-style1">域编码:
                </th>
                <td class="auto-style1">
                    <input id="DomainName" maxlength="14" runat="server" type="text" class="txt"
                        datacol="yes" err="域编码" checkexpession="NotNull" style="width: 200px" />
                </td>
                <th class="auto-style1">域名称:
                </th>
                <td class="auto-style1">
                    <input id="DomainAliasName" runat="server" type="text" class="txt" datacol="yes"
                        err="编码名称" checkexpession="NotNull" style="width: 200px" />
                </td>
            </tr>
            <tr>
                <th class="auto-style1">编码:
                </th>
                <td class="auto-style1">
                    <input id="SCode" maxlength="14" runat="server" type="text" class="txt"
                        datacol="yes" err="编码" checkexpession="NotNull" style="width: 200px" />
                </td>
                <th class="auto-style1">编码名称:
                </th>
                <td class="auto-style1">
                    <input id="SName" runat="server" type="text" class="txt" datacol="yes"
                        err="编码名称" checkexpession="NotNull" style="width: 200px" />
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
