<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dept_List.aspx.cs" Inherits="Web.Base.SysDept.Dept_List" %>

<%@ Register Src="../../UserControl/LoadButton.ascx" TagName="LoadButton" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>组织机构部门</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js"></script>
    <script src="/Themes/Scripts/TreeTable/jquery.treeTable.js"></script>
    <link href="/Themes/Scripts/TreeTable/css/jquery.treeTable.css" rel="stylesheet" />
    <script src="/Themes/Scripts/FunctionJS.js"></script>
    <script type="text/javascript">
        $(function () {
            divresize(63);
            FixedTableHeader("#dnd-example", $(window).height() - 91);
            GetClickTableValue();
        })
        /**
        获取table TD值
        主键ID
        column:列名
        **/
        var Dept_Id = '';
        function GetClickTableValue() {
            $('table tr').not('#td').click(function () {
                $(this).find('td').each(function (i) {
                    if (i == 6) {
                        Dept_Id = $(this).text();
                    }
                });
            });
            $("#dnd-example").treeTable({
                initialState: "expanded" //collapsed 收缩 expanded展开的
            });
        }
        //新增
        function add() {
            var url = "/Base/SysDept/Dept_Form.aspx?ParentId=" + Dept_Id;
            top.openDialog(url, 'Dept_Form', '单位信息 - 添加', 700, 335, 50, 50);
        }
        //编辑
        function edit() {
            var key = Dept_Id;
            if (IsEditdata(key)) {
                var url = "/Base/SysDept/Dept_Form.aspx?key=" + key;
                top.openDialog(url, 'Dept_Form', '单位信息 - 编辑', 700, 335, 50, 50);
            }
        }
        //删除
        function Delete() {
            var key = Dept_Id;
            if (IsDelData(key)) {
                var url = 'SysDept.ashx';
                var parm = "action=delete&key=" + key;
                showConfirmMsg('注：您确认要删除此数据吗？', function (r) {
                    if (r) {
                        getAjax(url, parm, function (rs) {
                            if (parseInt(rs) == 2) {
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
                单位信息列表
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
                        <td style="width: 200px; padding-left: 20px;">组织机构
                        </td>
                        <td style="width: 100px; text-align: center;">单位级别
                        </td>
                        <td style="width: 100px; text-align: center;">显示顺序
                        </td>
                        <td style="width: 100px; text-align: center;">创建人
                        </td>
                        <td style="width: 120px; text-align: center;">创建时间
                        </td>
                        <td>备注说明
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
