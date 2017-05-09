<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Code_Form.aspx.cs" Inherits="Web.Base.SysCode.Code_Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>配置信息表单</title>
    <link href="/Themes/Scripts/kindeditor/js/themes/default/default.css" rel="stylesheet" />
    <script src="/Themes/Scripts/kindeditor/js/kindeditor-min.js"></script>
    <script src="/Themes/Scripts/kindeditor/js/lang/zh_CN.js"></script>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script>
        KindEditor.ready(function (K) {
            var editor = K.editor({
                allowFileManager: true,
                uploadJson: '/Themes/Scripts/kindeditor/upload_json.ashx',
                fileManagerJson: '/Themes/Scripts/kindeditor/file_manager_json.ashx'
            });
            K('#image1').click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        imageUrl: K('#ImageUrl').val(),
                        clickFn: function (url, title, width, height, border, align) {
                            K('#ImageUrl').val(url);
                            editor.hideDialog();
                        }
                    });
                });
            });
        });
        //返回
        function back() {
            var url = "/Base/SysCode/Code_List.aspx?r=" + Math.random();
            Urlhref(url);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" class="frm">
            <tr>
                <th class="auto-style1">域编码:
                </th>
                <td class="auto-style1">
                    <input id="DomainName" maxlength="14" runat="server" type="text" class="txt"
                        datacol="yes" err="域编码" checkexpession="NotNull" style="width: 200px" />
                </td>
                <th class="auto-style1">域名称:
                </th>
                <td class="auto-style1">
                    <input id="DomainAliasName" runat="server" type="text" class="txt" datacol="yes"
                        err="编码名称" checkexpession="NotNull" style="width: 200px" />
                </td>
            </tr>
            <tr>
                <th class="auto-style1">编码:
                </th>
                <td class="auto-style1">
                    <input id="SCode" maxlength="14" runat="server" type="text" class="txt"
                        datacol="yes" err="编码" checkexpession="NotNull" style="width: 200px" />
                </td>
                <th class="auto-style1">编码名称:
                </th>
                <td class="auto-style1">
                    <input id="SName" runat="server" type="text" class="txt" datacol="yes"
                        err="编码名称" checkexpession="NotNull" style="width: 200px" />
                </td>
            </tr>
            <tr>
                <td class="auto-style1">图标路径</td>
                <td colspan="3" class="auto-style1">
                    <input type="text" id="ImageUrl" readonly="readonly" class="txt" style="width: 400px;" runat="server" />
                    <input type="button" id="image1" value="选择图片" />
                </td>
            </tr>
        </table>
        <div class="frmbottom">
            <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClientClick="return CheckDataValid('#form1');"
                OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保 存</span></asp:LinkButton>
            <a class="l-btn" href="javascript:void(0)" onclick="back();"><span class="l-btn-left">
                <img src="/Themes/Images/cancel.png" alt="" />关 闭</span></a>
        </div>
    </form>
</body>
</html>
