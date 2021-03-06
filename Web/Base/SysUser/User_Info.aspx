﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Info.aspx.cs" Inherits="Web.Base.SysUser.User_Info" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>用户详细信息</title>
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
            $('input[type="checkbox"]').attr('disabled', 'disabled');
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
                    <th>登录账号:
                    </th>
                    <td>
                        <input disabled="disabled" id="UserId" runat="server" type="text" class="txt" datacol="yes" err="登录账号"
                            checkexpession="NotNull" style="width: 200px" />
                    </td>
                    <th>用户姓名:
                    </th>
                    <td>
                        <input disabled="disabled" id="UserName" runat="server" type="text" class="txt" datacol="yes" err="用户姓名"
                            checkexpession="NotNull" style="width: 200px" />
                    </td>
                </tr>
                <tr>
                    <th>登录密码:
                    </th>
                    <td>
                        <input disabled="disabled" id="UserPwd" runat="server" type="text" class="txt" datacol="yes" err="登录密码"
                            checkexpession="NotNull" style="width: 200px" />
                    </td>
                    <th>联系电话:
                    </th>
                    <td>
                        <input disabled="disabled" id="Phone" runat="server" type="text" class="txt" datacol="yes" err="联系电话"
                            checkexpession="NotNull" style="width: 200px" />
                    </td>
                </tr>
                <tr>
                    <th>电子邮件:
                    </th>
                    <td>
                        <input disabled="disabled" id="Email" runat="server" type="text" class="txt" datacol="yes" err="电子邮箱"
                            checkexpession="EmailOrNull" style="width: 200px" />
                    </td>
                    <th>用户类型:
                    </th>
                    <td>
                        <select disabled="disabled" id="IsAdmin" class="select" runat="server" style="width: 206px">
                            <option value="0">0 - 普通用户</option>
                            <option value="1">1 - 管理员</option>
                        </select>
                    </td>

                </tr>
                <tr>
                    <th>AM名称:
                    </th>
                    <td>
                        <input id="Am" runat="server" disabled="disabled"  type="text" class="txt" datacol="yes" err="AM鸽子"
                            style="width: 200px" />
                    </td>
                    <th>系统样式:
                    </th>
                    <td>
                        <select id="Theme" class="select" disabled="disabled"  runat="server" style="width: 206px">
                            <option value="1">1 - Left菜单</option>
                            <option value="2">2 - Top菜单</option>
                        </select>
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
                        <textarea id="Remarks" class="txtRemark" runat="server" style="width: 500px; height: 83px;"></textarea>
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
    </form>
</body>
</html>
