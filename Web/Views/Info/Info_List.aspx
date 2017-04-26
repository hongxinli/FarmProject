<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Info_List.aspx.cs" Inherits="Web.Views.Info.Info_List" %>

<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>新闻信息</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".div-body").PullBox({ dv: $(".div-body"), obj: $("#table1").find("tr") });
            divresize(90);
            FixedTableHeader("#table1", $(window).height() - 118);
        })
        //新增
        function add() {
            var url = "/Views/Info/Info_Form.aspx";
            Urlhref(url);
        }
        //编辑
        function edit() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/Views/Info/Info_Form.aspx?key=" + key;
                Urlhref(url);
            }
        }
        //删除
        function Delete() {
            var key = CheckboxValue();
            if (IsDelData(key)) {
                var url = 'Info.ashx';
                var parm = "action=delete&key=" + key;
                showConfirmMsg('注：您确认要删除此数据吗？', function (r) {
                    if (r) {
                        getAjax(url, parm, function (rs) {
                            if (parseInt(rs) == 0) {
                                showTipsMsg("<span style='color:red'>删除失败，请稍后重试！</span>", 4000, 5);
                                return false;
                            }
                            else if (parseInt(rs) == 1) {
                                showTipsMsg("删除成功！", 2000, 4);
                                windowload();
                            }
                        });
                    }
                });
            }
        }
        //详细
        function detail() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/Views/Info/Info.html?key=" + key;
                Urlhref(url);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="btnbartitle">
            <div>
                新闻信息列表
            </div>
        </div>
        <div class="btnbarcontetn">
            <div style="text-align: right">
                <uc2:LoadButton ID="LoadButton1" runat="server" />
            </div>
        </div>
        <div class="div-body">
            <table id="table1" class="grid" singleselect="true">
                <thead>
                    <tr>
                        <td style="width: 20px; text-align: left;">
                            <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                                &nbsp;</label>
                        </td>
                        <td style="width: 120px; text-align: center;">新闻类型
                        </td>
                        <td style="width: 220px; text-align: center;">新闻标题
                        </td>
                        <td style="width: 120px; text-align: center;">发布人
                        </td>
                        <td>发布时间
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
                                <td style="width: 120px; text-align: center;">
                                    <%#Eval("InfoType")%>
                                </td>
                                <td style="width: 220px; text-align: center;">
                                    <%#Eval("InfoTitle")%>
                                </td>
                                <td style="width: 120px; text-align: center;">
                                    <%#Eval("CreateUserName")%>
                                </td>
                                <td>
                                    <%#Eval("CreateDate")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <% if (rp_Item != null)
                               {
                                   if (rp_Item.Items.Count == 0)
                                   {
                                       Response.Write("<tr><td colspan='8' style='color:red;text-align:center'>没有找到您要的相关数据！</td></tr>");
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
