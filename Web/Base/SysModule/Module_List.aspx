<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Module_List.aspx.cs" Inherits="Web.Base.SysModule.Module_List" %>

<%@ Register Src="/UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>菜单导航设置</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js"></script>
    <script src="/Themes/Scripts/TreeTable/jquery.treeTable.js"></script>
    <link href="/Themes/Scripts/TreeTable/css/jquery.treeTable.css" rel="stylesheet" />
    <script src="../../Themes/Scripts/FunctionJS.js"></script>
    <script type="text/javascript">
        $(function () {
            divresize(63);
            FixedTableHeader("#dnd-example", $(window).height() - 91);
            GetClickTableValue();
            $("#dnd-example").treeTable({
                initialState: "collapsed", //collapsed 收缩 expanded展开的
                treeColumn: 0
            });
        })
        /**
        获取table TD值
        主键ID
        column:列名
        **/
        var Menu_Id = '';
        var Dept_Id = '';
        function GetClickTableValue() {
            $('table tr').not('#td').click(function () {
                $(this).find('td').each(function (i) {
                    if (i == 6) {
                        Menu_Id = $(this).text();
                    }
                    if (i == 7) {
                        Dept_Id = $(this).text();
                    }
                });
            });
        }
        //新增
        function add() {
            var url = "/Base/SysModule/Module_Form.aspx?ParentId=" + Menu_Id;
            top.openDialog(url, 'Module_Form', '模块信息 - 添加', 450, 325, 50, 50);
        }
        //编辑
        function edit() {
            var key = Menu_Id;
            if (IsEditdata(key)) {
                var url = "/Base/SysModule/Module_Form.aspx?key=" + key + "&DeptId=" + Dept_Id;
                top.openDialog(url, 'Menu_Form', '模块信息 - 编辑', 450, 325, 50, 50);
            }
        }
        //删除
        function Delete() {
            var key = Menu_Id;

            var url = "SysModule.ashx";
            var parm = "action=delete&key=" + key;
            showConfirmMsg('注：您确认要删除此数据吗？', function (r) {
                if (r) {
                    getAjax(url, parm, function (rs) {
                        if (parseInt(rs) == 0) {
                            showTipsMsg("该数据不允许删除！", 3000, 3);
                            return false;
                        } else if (parseInt(rs) == 1) {
                            showTipsMsg("该模块含有子级菜单，请先删除子级菜单！", 3000, 3);
                            return false;
                        } else if (parseInt(rs) == 2) {
                            showTipsMsg("<span style='color:red'>删除失败，请稍后重试！</span>", 4000, 5);
                            return false;
                        }
                        else if (parseInt(rs) == 3) {
                            showTipsMsg("删除成功！", 2000, 4);
                            windowload();
                        }
                    });
                }
            });
        }
        //分配按钮
        function allotButton() {
            var key = Menu_Id;
            if (IsEditdata(key)) {
                var url = "/Base/SysModule/AllotButton_Form.aspx?key=" + key;
                top.openDialog(url, 'AllotButton_Form', '导航菜单信息 - 分配按钮', 580, 370, 50, 50);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="btnbartitle">
            <div>
                导航菜单信息 &nbsp;&nbsp;<span style="color: Red;">注：该功能谨慎使用！</span>
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
            <table class="example" id="dnd-example">
                <thead>
                    <tr>
                        <td style="width: 300px; padding-left: 20px;">菜单名称
                        </td>
                        <td style="width: 30px; text-align: center;">图标
                        </td>
                        <td style="width: 60px; text-align: center;">类型
                        </td>
                        <td style="width: 60px; text-align: center;">连接目标
                        </td>
                        <td style="width: 60px;">显示顺序
                        </td>
                        <td>连接地址
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <%=TableTree_Menu%>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
