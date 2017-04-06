<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dept_Form.aspx.cs" Inherits="Web.Base.SysDept.Dept_Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>组织机构部门表单</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" />
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <style type="text/css">
        .auto-style1 {
            height: 25px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="frm">
        <tr>
            <th class="auto-style1">
                单位编号:
            </th>
            <td class="auto-style1">
                <input id="DeptId" maxlength="14" runat="server" type="text" class="txt"
                    datacol="yes" err="单位编号" checkexpession="NotNull" style="width: 200px" />
            </td>
            <th class="auto-style1">
                单位名称:
            </th>
            <td class="auto-style1">
                <input id="DeptName" runat="server" type="text" class="txt" datacol="yes"
                    err="单位名称" checkexpession="NotNull" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                单位级别:
            </th>
            <td>
                <input id="Dlevel" runat="server" type="text" class="txt" datacol="yes"
                    err="单位级别" checkexpession="NotNull" style="width: 200px" />
            </td>
             <th>
                显示顺序:
            </th>
            <td>
                <input id="SortCode" runat="server" type="text" class="txt" datacol="yes" err="显示顺序"
                    checkexpession="Num" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                节点位置:
            </th>
            <td colspan="3">
                <select id="ParentId" class="select" runat="server" style="width: 206px">
                </select>
            </td>
           
        </tr>
        <tr>
            <th>
                备注说明:
            </th>
            <td colspan="3">
                <textarea id="Remarks" class="txtRemark" runat="server" style="width: 552px;
                    height: 100px;"></textarea>
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
