<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="WebGis.LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>土壤养分监测与管理云平台</title>
    <link type="text/css" rel="stylesheet" href="css/login.css" />
    <script src="js/jquery-1.7.2.js"></script>
    <script src="js/login.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="bgDiv" style="">
            <img src="images/login/bg.png" />
        </div>
        <div id="container">
            <div id="logo">
                <%--<span style="font-size: 30px; color: blue; font-family: 黑体;">土壤养分监测与管理云平台</span>--%>
                <img src="images/login/title.png" style="height:50px;width:450px;" />
            </div>
            <div id="login">
                <table>
                    <tr>
                        <td>
                            <div class="td_title" style="width: 60px;">
                                用户名:
                            </div>
                        </td>
                        <td>
                            <input type="text" id="username" class="input" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="td_title">
                                密&nbsp;&nbsp;&nbsp;码:
                            </div>
                        </td>
                        <td>
                            <asp:TextBox TextMode="Password" CssClass="input" runat="server" autocomplete="off" ID="pwd" />
                        </td>
                    </tr>
                    <tr>
                        <td class="savePwd">&nbsp;</td>
                        <td class="savePwd">
                            <asp:CheckBox ID="saveCookie" Width="10px" runat="server" Checked="false"></asp:CheckBox>
                            &nbsp;&nbsp;<span style="font-family: 宋体; font-size: 12px; color: #474747;">记住密码</span>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:ImageButton ID="Login_btn" runat="server" Style="display: none" ImageUrl="~/images/login/btn_login.png" OnClick="Login_btn_Click"></asp:ImageButton>
                            <img src="images/login/btn_login.png" onclick="submit_login()" style="cursor: pointer" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="footer">
            技术支持：山东天元信息技术股份有限公司&nbsp;&nbsp;0546-8302577
        </div>
    </form>
</body>
</html>
