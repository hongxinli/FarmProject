﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Crop_List.aspx.cs" Inherits="Web.Views.Crop.Crop_List" %>

<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>农作物信息</title>
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
            var url = "/Views/Crop/Crop_Form.aspx";
            Urlhref(url);

        }
        //编辑
        function edit() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/Views/Crop/Crop_Form.aspx?key=" + key;
                Urlhref(url);
            }
        }
        //删除
        function Delete() {
            var key = CheckboxValue();
            if (IsDelData(key)) {
                var url = 'Crop.ashx';
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
                var url = "/Views/Crop/Crop.html?key=" + key;
                Urlhref(url);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="btnbartitle">
            <div>
                农作物信息列表
            </div>
        </div>
        <div class="btnbarcontetn">
            <div style="float: left;">
                <select id="CropType" class="Searchwhere" runat="server">
                    
                </select>
                <input type="text" id="txt_Search" class="txtSearch SearchImg" runat="server" style="width: 100px;" />
                <asp:LinkButton ID="lbtSearch" runat="server" class="button green" OnClick="lbtSearch_Click">
                <span class="icon-botton" style="background: url('/Themes/images/Search.png') no-repeat scroll 0px 4px;">
        </span>查 询</asp:LinkButton>
            </div>
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
                        <td style="width: 120px; text-align: center;">作物名称
                        </td>
                        <td style="width: 120px; text-align: center;">作物种类
                        </td>
                        <td style="width: 120px; text-align: center;">创建人员
                        </td>
                        <td style="width: 200px; text-align: center;">创建时间
                        </td>
                        <td>备注
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
                                <td style="width: 120px; text-align: left;">
                                 <a href="Crop.html?key=<%#Eval("Id")%>"> <%#Eval("CropName")%></a> 
                                </td>
                                <td style="width: 120px; text-align: center;">
                                    <%#Eval("CropType")%> 
                                </td>
                                <td style="width: 120px; text-align: center;">
                                    <%#Eval("CreateUserName")%>
                                </td>
                                <td style="width: 200px; text-align: center;">
                                    <%#Eval("CreateDate")%>
                                </td>
                                <td>
                                    <%#Eval("Remarks")%>
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
