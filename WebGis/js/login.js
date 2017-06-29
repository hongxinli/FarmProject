function initPage() {
    var obj = document.getElementById("container");
    var height = window.document.documentElement.clientHeight;
    var width = window.document.documentElement.clientWidth;
    obj.style.left = (width - obj.clientWidth) / 2 + "px";
    obj.style.top = (height - obj.clientHeight) / 2 + "px";
    $("#username").focus()
}
$(function () {
    initPage();
});
window.onresize = initPage;

function check_login() {
    var username = $.trim($("#username").val());
    var pwd = $.trim($("#pwd").val());
    if (username == "") {
        alert("用户名不能为空！");
        return false;
    }
    if (pwd == "") {
        alert("密码不能为空！");
        return false;
    }
    return true;
}

function submit_login() {
    if (check_login()) {
        var username = $.trim($("#username").val());
        var pwd = $.trim($("#pwd").val());

        if (username != "admin") {
            alert("用户名不正确");
        } else if (pwd != "123") {
            alert("密码不正确");
        } else {
            $("#Login_btn").click();
        }
    }
}

$(function () {
    $("#userid").focus();
})

document.onkeydown = function () {
    var e = event.srcElement;
    if (event.keyCode == 13) {
        submit_login();
    }
}