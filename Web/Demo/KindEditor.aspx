<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KindEditor.aspx.cs" Inherits="Web.Demo.KindEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/Themes/Scripts/kindeditor/js/themes/default/default.css" rel="stylesheet" />
    <script src="/Themes/Scripts/kindeditor/js/kindeditor-min.js"></script>
    <script src="/Themes/Scripts/kindeditor/js/lang/zh_CN.js"></script>
    <title></title>
    <style>
        form {
            margin: 0;
        }

        textarea {
            display: block;
        }
    </style>
    <script type="text/javascript">
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('textarea[name="content"]', {
                allowFileManager: true,
                uploadJson: '../Themes/Scripts/kindeditor/upload_json.ashx',
                fileManagerJson: '../Themes/Scripts/kindeditor/file_manager_json.ashx'
            });
            K('input[name=getHtml]').click(function (e) {
                alert(editor.html());
            });
            K('input[name=setHtml]').click(function (e) {
                editor.html('<h1>\n\t试试\n</h1>\n<img style=\"height:215px;width:227px;\" alt=\"\" src=\"/Themes/Scripts/kindeditor/attached/image/20170424/20170424170808_0525.jpg\" width=\"1700\" height=\"2337\" /> \n<p>\n\t&nbsp;\n</p>"');
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <textarea name="content" style="width: 800px; height: 400px; visibility: hidden;"></textarea>
        <p>
            <input type="button" name="getHtml" value="取得HTML" />
            <input type="button" name="setHtml" value="设置HTML" />
        </p>
    </form>
</body>
</html>
