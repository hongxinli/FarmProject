<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo_List.aspx.cs" Inherits="Web.Base.SysUser.UserInfo_List" %>

<%@ Register Src="../../UserControl/PageControl.ascx" TagName="PageControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>用户信息</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        //回车键
        document.onkeydown = function (e) {
            if (!e) e = window.event; //火狐中是 window.event
            if ((e.keyCode || e.which) == 13) {
                var obtnSearch = document.getElementById("lbtSearch");
                obtnSearch.click();
            }
        }
        $(function () {
            $(".div-body").PullBox({ dv: $(".div-body"), obj: $("#table1").find("tr") });
            divresize(90);
            FixedTableHeader("#table1", $(window).height() - 118);
        })
        //添加
        function add() {
            var url = "/Base/SysUser/UserInfo_Form.aspx?DeptId=" + "<%=_DeptId%>";
            top.openDialog(url, 'UserInfo_Form', '用户信息 - 添加', 700, 350, 50, 50);
        }
        //修改
        function edit() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/Base/SysUser/UserInfo_Form.aspx?key=" + key + "&DeptId=<%=_DeptId%>";
                top.openDialog(url, 'UserInfo_Form', '用户信息 - 编辑', 700, 350, 50, 50);
            }
        }
        //删除
        function Delete() {
            var key = CheckboxValue();
            if (IsDelData(key)) {
                var url = 'SysUser.ashx';
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
        //授 权
        function accredit() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var parm = 'action=accredit&key=' + key;
                showConfirmMsg('注：您确认要【授 权】当前选中用户吗？', function (r) {
                    if (r) {
                        getAjax('SysUser.ashx', parm, function (rs) {
                            if (parseInt(rs) == 1) {
                                showTipsMsg("恭喜授权成功！", 2000, 4);
                                windowload();
                            }
                            else {
                                showTipsMsg("<span style='color:red'>授权失败，请稍后重试！</span>", 4000, 5);
                            }
                        });
                    }
                });
            }
        }
        //锁 定
        function lock() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var parm = 'action=lock&key=' + key;
                showConfirmMsg('注：您确认要【锁 定】当前选中用户吗？', function (r) {
                    if (r) {
                        getAjax('SysUser.ashx', parm, function (rs) {
                            if (parseInt(rs) == 1) {
                                showTipsMsg("锁定成功！", 2000, 4);
                                windowload();
                            }
                            else {
                                showTipsMsg("<span style='color:red'>锁定失败，请稍后重试！</span>", 4000, 5);
                            }
                        });
                    }
                });
            }
        }
        //详细信息
        function detail() {
            var key = CheckboxValue();
            if (IsEditdata(key)) {
                var url = "/Base/SysUser/User_Info.aspx?key=" + key;
                top.openDialog(url, 'Role_Form', '用户信息 - 详细信息查看', 600, 355, 50, 50);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="btnbartitle">
            <div>
                用户信息列表
            </div>
        </div>
        <div class="btnbarcontetn">
            <div style="float: left;">
                <select id="Searchwhere" class="Searchwhere" runat="server">
                    <option value="UserId">登录账户</option>
                    <option value="UserName">用户姓名</option>
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
                        <td style="width: 80px; text-align: center;">登录账户
                        </td>
                        <td style="width: 80px; text-align: center;">用户姓名
                        </td>
                        <td style="width: 100px; text-align: center;">联系电话
                        </td>
                        <td style="width: 180px; text-align: center;">电子邮件
                        </td>
                         <td style="width: 100px; text-align: center;">AM名称
                        </td>
                        <td style="width: 80px; text-align: center;">用户类型
                        </td>
                        <td style="width: 80px; text-align: center;">用户状态
                        </td>
                        <td>备注说明
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 20px; text-align: left;">
                                    <input type="checkbox" value="<%#Eval("UserId")%>" name="checkbox" />
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <%#Eval("UserId")%>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <a href="#">
                                        <%#Eval("UserName")%></a>
                                </td>
                                <td style="width: 100px; text-align: center;">
                                    <%#Eval("Phone")%>
                                </td>
                                <td style="width: 180px; text-align: center;">
                                    <%#Eval("Email")%>
                                </td>
                                <td style="width: 100px; text-align: center;">
                                    <%#Eval("Am")%>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="lblIsAdmin" runat="server" Text='<%#Eval("IsAdmin")%>'></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="lblIsState" runat="server" Text='<%#Eval("IsState")%>'></asp:Label>
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
