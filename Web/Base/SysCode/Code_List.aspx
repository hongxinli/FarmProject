﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Code_List.aspx.cs" Inherits="Web.Base.SysCode.Code_List" %>

<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>基础信息</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <style type="text/css">
        .smallImg {
            width: 20px;
            height: 20px;
            cursor: pointer;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $(".div-body").PullBox({ dv: $(".div-body"), obj: $("#table1").find("tr") });
            divresize(90);
            FixedTableHeader("#table1", $(window).height() - 118);

            $(".grid img").click(function () {
                $("#container").html("<img src=\""+$(this).attr("src")+"\" style=\"width:450px;height:450px;cursor:pointer;\" />");
                top.art.dialog({
                    id: 'warningId',
                    title: '图标信息',
                    content: $("#container").html(),
                    background: '#000',
                    opacity: 0.1,
                    lock: true
                });
            });
        })
        //新增
        function add() {
            var url = "/Base/SysCode/Code_Form.aspx";
            Urlhref(url);
        }
        //编辑
        function edit() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/Base/SysCode/Code_Form.aspx?key=" + key;
                Urlhref(url);
            }
        }
        //删除
        function Delete() {
            var key = CheckboxValue();
            if (IsDelData(key)) {
                var url = 'Code.ashx';
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="btnbartitle">
            <div>
                基础信息列表
            </div>
        </div>
        <div class="btnbarcontetn">
            <div style="float: left; margin-left: 10px; margin-top: 5px;">
                <label style="color: red;">注：系统配置，谨慎操作！</label>
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
                        <td style="width: 120px; text-align: center;">域编码
                        </td>
                        <td style="width: 120px; text-align: center;">域名称
                        </td>
                        <td style="width: 80px; text-align: center;">名称编码
                        </td>
                        <td style="width: 120px; text-align: center;">名称
                        </td>
                        <td style="width: 80px; text-align: center;">图标</td>
                        <td style="width: 120px; text-align: center;">创建人员
                        </td>
                        <td>创建时间
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rp_Item" OnItemDataBound="rp_ItemDataBound" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 20px; text-align: left;">
                                    <input type="checkbox" value="<%#Eval("Id")%>" name="checkbox" />
                                </td>
                                <td style="width: 120px; text-align: center;">
                                    <%#Eval("DomainName")%>
                                </td>
                                <td style="width: 120px; text-align: center;">
                                    <%#Eval("DomainAliasName")%>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <%#Eval("SCode")%>
                                </td>
                                <td style="width: 120px; text-align: center;">
                                    <%#Eval("SName")%>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Image ID="lb_img" ImageUrl='<%#Eval("ImageUrl")%>' CssClass="smallImg" runat="server" />
                                </td>
                                <td style="width: 120px; text-align: center;">
                                    <%#Eval("CreateUserName")%>
                                </td>
                                <td style="text-align: left;">
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
        <div id="container"></div>
    </form>
</body>
</html>
