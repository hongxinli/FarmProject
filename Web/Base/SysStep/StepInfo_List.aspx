<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StepInfo_List.aspx.cs" Inherits="Web.Base.SysStep.StepInfo_List" %>

<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>步骤信息列表</title>
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
        //添加
        function add() {
            var url = "/Base/SysStep/StepInfo_Form.aspx?DeptId=" + "<%=_DeptId%>";
            top.openDialog(url, 'StepInfo_Form', '步骤信息 - 添加', 700, 350, 50, 50);
        }
        //修改
        function edit() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/Base/SysStep/StepInfo_Form.aspx?key=" + key + "&DeptId=<%=_DeptId%>";
                top.openDialog(url, 'StepInfo_Form', '步骤信息 - 编辑', 700, 350, 50, 50);
            }
        }
        //删除
        function Delete() {
            var key = CheckboxValue();
            if (IsDelData(key)) {
                var url = 'SysStep.ashx';
                var parm = "action=delete&key=" + key;
                showConfirmMsg('注：您确认要删除此数据吗？', function (r) {
                    if (r) {
                        getAjax(url, parm, function (data) {
                            if (data == 1) {
                                showTipsMsg("删除成功！", 2000, 4);
                                windowload();
                            } else {
                                showTipsMsg("<span style='color:red'>删除失败，请稍后重试！</span>", 4000, 5);
                                return false;
                            }
                        });
                    }
                })
            }
        }
        //详细信息
        function detail() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/Base/SysStep/Step_Info.aspx?key=" + key;
                top.openDialog(url, 'Step_Form', '步骤信息 - 步骤信息查看', 600, 355, 50, 50);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="btnbartitle">
            <div>
                步骤信息列表
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
                        <td style="width: 150px; text-align: center;">所属单位
                        </td>
                        <td style="width: 120px; text-align: center;">步骤名称
                        </td>
                        <td style="width: 80px; text-align: center;">创建人
                        </td>
                        <td style="width: 120px; text-align: center;">创建时间
                        </td>
                        <td style="width: 80px; text-align: center;">状态
                        </td>
                        <td>步骤描述
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 20px; text-align: left;">
                                    <input type="checkbox" value="<%#Eval("StepId")%>" name="checkbox" />
                                </td>
                                <td style="width: 150px; text-align: center;">
                                    <%#Eval("DeptName")%>
                                </td>
                                <td style="width: 120px; text-align: center;">
                                    <a href="#">
                                        <%#Eval("StepName")%></a>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <%#Eval("Creator")%>
                                </td>
                                <td style="width: 120px; text-align: center;">
                                    <%#Eval("CreateDate")%>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="lblIsDelete" runat="server" Text='<%#Eval("DeleteMark")%>'></asp:Label>
                                </td>
                                <td>
                                    <%#Eval("StepDesc")%>
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
