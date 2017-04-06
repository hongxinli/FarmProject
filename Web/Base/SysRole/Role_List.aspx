<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Role_List.aspx.cs" Inherits="Web.Base.SysRole.Role_List" %>

<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>系统角色设置</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/TreeTable/jquery.treeTable.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeTable/css/jquery.treeTable.css" rel="stylesheet"
        type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            divresize(63);
            FixedTableHeader("#table1", $(window).height() - 91);
            GetClickTableValue();
        })
        /**
        获取table TD值
        主键ID
        column:列名
        **/
        var Roles_ID = '';
        var Roles_Name = '';
        function GetClickTableValue() {
            $('table tr').not('#td').click(function () {
                $(this).find('td').each(function (i) {
                    if (i == 6) {
                        Roles_ID = $(this).text();
                    }
                    if (i == 0) {
                        Roles_Name = $(this).text();
                    }
                });
            });
        }
        //新增
        function add() {
            var url = "/Base/SysRole/Role_Form.aspx?DeptId=<%=_deptId%>";
            top.openDialog(url, 'Role_Form', '系统角色信息 - 添加', 600, 355, 50, 50);
        }
        //编辑
        function edit() {
            var key = Roles_ID;
            if (IsEditdata(key)) {
                var url = 'SysRole.ashx';
                var parm = "action=IsAllowEdit&key=" + key;
                getAjax(url, parm, function (data) {
                    if (data == 0) {
                        var url = "/Base/SysRole/Role_Form.aspx?key=" + key + "&DeptId=<%=_deptId%>";
                        top.openDialog(url, 'Role_Form', '系统角色信息 - 编辑', 600, 355, 50, 50);
                    } else {
                        showWarningMsg("该数据不允许编辑！");
                        return false;
                    }
                });
            }
        }
        //删除
        function Delete() {
            var key = Roles_ID;
            if (IsDelData(key)) {
                var url = 'SysRole.ashx';
                var parm = "action=delete&key=" + key;
                showConfirmMsg('注：您确认要删除此数据吗？', function (r) {
                    if (r) {
                        getAjax(url, parm, function (rs) {
                            if (parseInt(rs) == 0) {
                                showTipsMsg("该数据不允许删除！", 3000, 3);
                                return false;
                            } else if (parseInt(rs) == 1) {
                                showTipsMsg("<span style='color:red'>删除失败，请稍后重试！</span>", 4000, 5);
                                return false;
                            }
                            else if (parseInt(rs) == 2) {
                                showTipsMsg("删除成功！", 2000, 4);
                                windowload();
                            }
                        });
                    }
                });
            }
        }
        //详细信息
        function detail() {
            var key = Roles_ID;
            if (IsEditdata(key)) {
                var url = "/Base/SysRole/Role_Info.aspx?key=" + key + '&Roles_Name=' + escape(Roles_Name);
                top.openDialog(url, 'Role_Form', '系统角色信息 - 详细信息查看', 600, 355, 50, 50);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="btnbartitle">
            <div>
                系统角色信息
            </div>
        </div>
        <div class="btnbarcontetn">
            <div style="float: left;">
            </div>
            <div style="text-align: right">
                <uc1:LoadButton ID="LoadButton1" runat="server" />
            </div>
        </div>
        <div class="div-body">
            <table id="table1" class="grid" singleselect="true">
                <thead>
                    <tr>
                        <td style="width: 180px; padding-left: 20px;">角色名称
                        </td>
                        <td style="width: 60px; text-align: center;">角色状态
                        </td>
                        <td style="width: 60px; text-align: center;">显示顺序
                        </td>
                        <td style="width: 120px; text-align: center;">允许删除
                        </td>
                        <td style="width: 120px; text-align: center;">允许修改
                        </td>
                        <td>角色描述
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <%=str_tableTree%>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
