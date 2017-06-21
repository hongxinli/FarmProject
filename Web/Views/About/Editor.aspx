<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Editor.aspx.cs" Inherits="Web.Views.About.Editor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/Themes/Scripts/kindeditor/js/themes/default/default.css" rel="stylesheet" />
    <script src="/Themes/Scripts/kindeditor/js/kindeditor-min.js"></script>
    <script src="/Themes/Scripts/kindeditor/js/lang/zh_CN.js"></script>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <title>关于我们</title>
    <script type="text/javascript">
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('textarea[name="Content"]', {
                allowFileManager: true,
                uploadJson: '/Themes/Scripts/kindeditor/upload_json.ashx',
                fileManagerJson: '/Themes/Scripts/kindeditor/file_manager_json.ashx'
            });
            $.ajax({
                type: 'get',
                dataType: "text",
                url: "content.txt",
                cache: false,
                async: true,
                success: function (msg) {
                    editor.html(msg);
                },
                error: function (error) {
                    alert(error);
                }
            });
            ;
        })

        function setHtml() {
            $("#AboutContent").val(editor.html());
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <textarea name="Content" id="Content" style="width: 800px; height: 400px;" runat="server"></textarea>
         <input type="hidden" id="AboutContent" runat="server" />
        <div class="frmbottom">
            <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClientClick="return setHtml();"
                OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保 存</span></asp:LinkButton>
        </div>
    </form>
</body>
</html>
