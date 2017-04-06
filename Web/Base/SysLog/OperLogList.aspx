<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OperLogList.aspx.cs" Inherits="Web.Base.SysLog.OperLogList" %>

<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>操作日志</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        //回车键
        document.onkeydown = function (e) {
            if (!e) e = window.event; //火狐中是 window.event
            if ((e.keyCode || e.which) == 13) {

            }
        }
        $(function () {
            divresize(123);
            FixedTableHeader("#table1", $(window).height() - 151);
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="btnbartitle">
            <div>
                系统日志
            </div>
        </div>
        <div class="btnbarcontetn" style="margin-bottom: 1px;">
            <div style="float: left;">
                <table style="padding: 0px; margin: 0px; height: 100%;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td id="menutab" style="vertical-align: bottom;">
                            <div id="tab0" class="Tabsel" onclick="GetTabClick(this);Urlhref('OperLogList.aspx');">
                                操作日志
                            </div>
                            <div id="tab1" class="Tabremovesel" onclick="GetTabClick(this);Urlhref('LoginList.aspx');">
                                登录日志
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="btnbarcontetn">
            <div style="float: left;">
                登录账户:
            <input type="text" id="txt_Search" class="txt" runat="server" style="width: 110px;" />
                操作日期:
            <input type="text" id="BeginTime" class="txt" runat="server" style="width: 115px;"
                onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd 00:00:00' })" />
                至
            <input type="text" id="endTime" class="txt" runat="server" style="width: 115px;"
                onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd 23:59:59' })" />
                <asp:LinkButton ID="lbtSearch" runat="server" class="button green" OnClick="lbtSearch_Click"><span class="icon-botton"
            style="background: url('/Themes/images/Search.png') no-repeat scroll 0px 4px;">
        </span>查 询</asp:LinkButton>
            </div>
            <div style="text-align: right">
            </div>
        </div>
        <div class="div-body">
            <table id="table1" class="grid">
                <thead>
                    <tr>
                        <td style="width: 20px; text-align: left;">
                            <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                                &nbsp;</label>
                        </td>
                        <td style="width: 50px; text-align: center;">序号
                        </td>
                        <td style="width: 100px; text-align: center;">业务名称
                        </td>
                        <td style="width: 100px; text-align: center;">登录账户
                        </td>
                        <td style="width: 100px; text-align: center;">账户名称
                        </td>
                        <td style="width: 100px; text-align: center;">用户IP
                        </td>
                        <td style="width: 150px; text-align: center;">操作日期
                        </td>
                        <td style="width: 150px; text-align: center;">所属单位
                        </td>
                        <td>操作内容
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rp_Item" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 20px; text-align: left;">
                                    <input type="checkbox" value="<%#Eval("Id")%>" name="checkbox" />
                                </td>
                                <td style="width: 50px; text-align: center;">
                                    <%# Container.ItemIndex+1 %>
                                </td>
                                <td style="width: 100px; text-align: center;"><%#Eval("ModuleName")%>>
                                </td>
                                <td style="width: 100px; text-align: center;">
                                    <%#Eval("UserId")%>
                                </td>
                                <td style="width: 100px; text-align: center;">
                                    <%#Eval("UserName")%>
                                </td>
                                <td style="width: 100px; text-align: center;">
                                    <%#Eval("UserIp")%>
                                </td>
                                <td style="width: 150px; text-align: center;">
                                    <%#Eval("OperationTime")%>
                                </td>
                                <td style="width: 150px; text-align: center;">
                                    <%#Eval("DeptName")%>
                                </td>
                                <td>
                                    <%#Eval("Operation")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <% if (rp_Item != null)
                               {
                                   if (rp_Item.Items.Count == 0)
                                   {
                                       Response.Write("<tr><td colspan='6' style='color:red;text-align:center'>没有找到您要的相关数据！</td></tr>");
                                   }
                               } %>
                        </FooterTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <uc1:PageControl ID="PageControl1" runat="server" />
    </form>
</body>
</html>
