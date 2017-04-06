/// <reference path="jquery-1.8.2.min.js" />
$(function () {
    //加载审核人员信息
    var url = '/Base/SysFlow/SysFlow.ashx';
    var parm = "action=initFirstUserInfo&tabName=" + tabName + "&r=" + Math.random();
    getAjax(url, parm, function (rs) {
        $("#selDiv").html(rs); //初始化审批人员以及模板
    })
})

//提交审核
function commit() {
    var key = CheckCommitBoxVal();
    if (commitflag == 0) {
        var parm = "action=IsOpenFlow&key=" + key;
        var url = '/Base/SysFlow/SysFlow.ashx';
        getAjax(url, parm, function (rs) {
            var pageIndex = 1;
            if ($("#hid_PageIndex").length > 0) { pageIndex = $("#hid_PageIndex").val(); }
            var url = "/Base/SysFlow/FlowInfo_Commit.aspx?action=record&tabName=" + tabName + "&pageIndex=" + pageIndex;
            top.openDialog(url, 'FlowInfo_Commit', '开启审批流程 - 操作', 400, 150, 50, 50);
        })
    } else {
        showTipsMsg("请选择审批状态为<span style='color:red'>待提交</span>的数据！", 2000, 4);
    }
}
//审核记录
function record() {
    var key = CheckboxValue();
    if (IsEditdata(key)) {
        var url = "/Base/SysFlow/TranctProc_Record.aspx?action=record&key=" + key + "&tabName==" + tabName;
        top.openDialog(url, 'TranctProc_Record', '审批记录 - 详细信息', 600, 305, 50, 50);
    }
}
//审核
function audit() {
    var key = CheckAuditBoxVal();
    if (auditflag == 0) {
        var parm = "action=audit&key=" + key;
        var url = '/Base/SysFlow/SysFlow.ashx';
        getAjax(url, parm, function (rs) {
            if (rs == 0) {
                var pageIndex = 1;
                if ($("#hid_PageIndex").length > 0) { pageIndex = $("#hid_PageIndex").val(); }
                var url = "/Base/SysFlow/FlowInfo_Audit.aspx?action=record&tabName=" + tabName + "&key=" + key + "&pageIndex=" + pageIndex;
                top.openDialog(url, 'FlowInfo_Audit', '审批 - 操作', 700, 335, 50, 50);
            } else if (rs == 1) {
                showTipsMsg("<span style='color:red'>请等待其他人审批！</span>", 4000, 5);
                return false;
            }
            else if (rs == 2) {
                showTipsMsg("<span style='color:red'>已审批，请勿重复审核！</span>", 4000, 5);
                return false;
            } else if (rs == 3) {
                showTipsMsg("<span style='color:red'>审核流程未开启！</span>", 4000, 5);
                return false;
            } else if (rs == 4) {
                showTipsMsg("<span style='color:red'>审批流程已经结束！</span>", 4000, 5);
                return false;
            } else if (rs == 5) {
                showTipsMsg("<span style='color:red'>该用户不具有审批权限！</span>", 4000, 5);
                return false;
            }
        });
    } else {
        showTipsMsg("请选择审批状态为<span style='color:red'>待审核</span>的数据！", 2000, 4);
    }
}
function IsMultSelectdata(id) {
    var isOK = true;
    if (id == undefined || id == "") {
        isOK = false;
        showWarningMsg("未选中任何一行");
    }
    return isOK;
}
var commitflag = 0;
function CheckCommitBoxVal() {
    var reVal = '';
    $('[type = checkbox]').each(function () {
        if ($(this).attr("checked")) {
            var status = $.trim($(this).parent("td").next("td").first().text());
            if (status == "待审批" || status == "通过" || status == "审批中") {
                commitflag++;
                return false;
            } else {
                commitflag = 0;
            }
            reVal += $(this).val() + ",";
        }
    });
    reVal = reVal.substr(0, reVal.length - 1);
    return reVal;
}
var auditflag = 0;
function CheckAuditBoxVal() {
    var reVal = '';
    $('[type = checkbox]').each(function () {
        if ($(this).attr("checked")) {
            var status = $.trim($(this).parent("td").next("td").first().text());
            if (status == "待提交" || status == "通过" || status == "审批中") {
                auditflag++;
                return false;
            } else {
                auditflag = 0;
            }
            reVal += $(this).val() + ",";
        }
    });
    reVal = reVal.substr(0, reVal.length - 1);
    return reVal;
}


