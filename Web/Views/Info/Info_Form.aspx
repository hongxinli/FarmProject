﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Info_Form.aspx.cs" Inherits="Web.Views.Info.Info_Form" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/Themes/Scripts/kindeditor/js/themes/default/default.css" rel="stylesheet" />
    <script src="/Themes/Scripts/kindeditor/js/kindeditor-min.js"></script>
    <script src="/Themes/Scripts/kindeditor/js/lang/zh_CN.js"></script>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('textarea[name="Content"]', {
                allowFileManager: true,
                uploadJson: '/Themes/Scripts/kindeditor/upload_json.ashx',
                fileManagerJson: '/Themes/Scripts/kindeditor/file_manager_json.ashx'
            });
            editor.html($("#InfoContent").val());
        })

        function setHtml() {
            $("#InfoContent").val(editor.html());
        }
        //返回
        function back() {
            var url = "/Views/Info/Info_List.aspx?r=" + Math.random();
            Urlhref(url);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input id="Id" type="hidden" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" class="frm">
            <tr>
                <th class="auto-style1">新闻类型:
                </th>
                <td class="auto-style1">
                    <select id="InfoType" style="width: 200px" runat="server">
                        
                    </select>
                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <input type="checkbox" id="Top" runat="server" />是否推荐置顶
                </td>
            </tr>
            <tr>
                <th class="auto-style1">新闻标题:
                </th>
                <td class="auto-style1">
                    <input id="InfoTitle" runat="server" type="text" class="txt" datacol="yes"
                        err="新闻标题" checkexpession="NotNull" style="width: 800px" />
                </td>
            </tr>
            <tr>
                <th class="auto-style1">新闻内容:
                </th>
                <td class="auto-style1">
                    <textarea name="Content" id="Content" style="width: 800px; height: 400px;"></textarea>
                    <input type="hidden" id="InfoContent" runat="server" />
                </td>
            </tr>
        </table>
        <div class="frmbottom">
            <asp:LinkButton ID="Save" runat="server" class="l-btn" OnClientClick="return setHtml(); CheckDataValid('#form1');"
                OnClick="Save_Click"><span class="l-btn-left">
            <img src="/Themes/Images/disk.png" alt="" />保 存</span></asp:LinkButton>
            <a class="l-btn" href="javascript:void(0)" onclick="back();"><span class="l-btn-left">
                <img src="/Themes/Images/cancel.png" alt="" />关 闭</span></a>
        </div>
    </form>
</body>
</html>
