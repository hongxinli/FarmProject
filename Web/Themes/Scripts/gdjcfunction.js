
//cad图片上传并预览
$(function () {
    $("#file_upload").change(function () {
      
        $.ajaxFileUpload
       (
           {
               url: 'UploadPic.ashx',
               secureuri: false,
               fileElementId: 'file_upload',
               dataType: 'text',
               beforeSend: function () {
                   $("#loading").show();
               },
               complete: function () {
                   $("#loading").hide();
               },
               success: function (data, status) {
                   //debugger;
                   if (data == "无图片") {
                       $("#preview").attr("src", "../../../../uploads/pic/管道检测/DefaultImg.png");
                       $("#PICNAME").val(data);
                   } else if (data == "文件格式错误") {
                       alert("文件格式错误");
                   } else {
                       $("#preview").attr("src", "../../../../uploads/pic/管道检测/" + data + "");
                       $("#PICNAME").val(data);
                   }
               },
               error: function (data, status, e) {
                   alert(e);
               }
           }
       );
        return false;
    });
});
$(function () {
    $("#file_upload1").change(function () {

        $.ajaxFileUpload
       (
           {
               url: 'UploadPic.ashx',
               secureuri: false,
               fileElementId: 'file_upload1',
               dataType: 'text',
               beforeSend: function () {
                   $("#loading").show();
               },
               complete: function () {
                   $("#loading").hide();
               },
               success: function (data, status) {
                   //debugger;
                   if (data == "无图片") {
                       $("#preview").attr("src", "../../../../uploads/pic/锅检站/DefaultImg.png");
                       $("#PICNAME").val(data);
                   } else if (data == "文件格式错误") {
                       alert("文件格式错误");
                   } else {
                       $("#preview").attr("src", "../../../../uploads/pic/锅检站/" + data + "");
                       $("#PICNAME").val(data);
                   }
               },
               error: function (data, status, e) {
                   alert(e);
               }
           }
       );
        return false;
    });
})
//声明全局变量
//var formvalue = "";
//var flag = 1;
//var index = 1;
//var firstCell = "";
//var secondCell = "";
//var thirdCell = "";
//var fourthCell = "";
//var fiveCell = "";
//var sixCell = "";
//var sevenCell = "";
//var eightCell = "";

//$(function () {
//    //初始化第一行
//    firstCell = $("#row0 td:eq(0)").html();
//    secondCell = $("#row0 td:eq(1)").html();
//    thirdCell = $("#row0 td:eq(2)").html();
//    fourthCell = $("#row0 td:eq(3)").html();
//    fiveCell = $("#row0 td:eq(4)").html();
//    sixCell = $("#row0 td:eq(5)").html();
//    sevenCell = $("#row0 td:eq(6)").html();
//    eightCell = $("#row0 td:eq(7)").html();

   
//});
        
////-----------------新增一行-----------start---------------
//function insertNewRow() {
//    //debugger;
//    //获取表格有多少行
//    var rowLength = $("#orderTable tr").length;
//    //这里的rowId就是row加上标志位的组合。是每新增一行的tr的id。
//    var rowId = "row" + flag;

//    //每次往下标为flag+1的下面添加tr,因为append是往标签内追加。所以用after
//    var insertStr = "<tr id=" + rowId + ">"
//                  + "<td style='width: 20%'>" + firstCell + "</td>"
//                  + "<td style='width: 20%'>" + secondCell + "</td>"
//                  + "<td style='width: 20%'>" + thirdCell + "</td>"
//                  + "<td style='width: 30%'>" + fourthCell + "</td>"
//                  + "<td colspan='2'><input type='button' name='delete' value='删除' style='width:80px' onclick='deleteSelectedRow(\"" + rowId + "\")' />"+"</tr>";
//    //这里的行数减2，是因为要减去底部的一行和顶部的一行，剩下的为开始要插入行的索引
//    $("#orderTable tr:eq(" + (rowLength - 2) + ")").after(insertStr); //将新拼接的一行插入到当前行的下面
//    //为新添加的行里面的控件添加新的id属性。
//    $("#" + rowId + " td:eq(0)").children().eq(0).attr("id", "gbhone" + flag);
//    $("#" + rowId + " td:eq(1)").children().eq(0).attr("id", "ghdone" + flag);
//    $("#" + rowId + " td:eq(2)").children().eq(0).attr("id", "gbhtwo" + flag);
//    $("#" + rowId + " td:eq(3)").children().eq(0).attr("id", "ghdtwo" + flag);
//    //每插入一行，flag自增一次
//    flag++;
//}
////-----------------删除一行，根据行ID删除-start--------    
//function deleteSelectedRow(rowID) {
//    if (confirm("确定删除该行吗？")) {
//        $("#" + rowID).remove();
//    }
//}
////-------------------接收表单中的值-----------------------------
//function ReceiveValue(str) {
//    //debugger;
//    var Str = str.split('@');
//    if (Str[0] != "") {
//        for (var i = 0; i < Str.length - 1; i++) {
//            var value = Str[i].split(',');
//            if (i>=1) {
//                firstCell = $("#row0 td:eq(0)").html();
//                secondCell = $("#row0 td:eq(1)").html();
//                thirdCell = $("#row0 td:eq(2)").html();
//                fourthCell = $("#row0 td:eq(3)").html();
//                //debugger;
//                //获取表格有多少行
//                var rowLength = $("#orderTable tr").length;
//                //这里的rowId就是row加上标志位的组合。是每新增一行的tr的id。
//                var rowId = "row" + flag;

//                //每次往下标为flag+1的下面添加tr,因为append是往标签内追加。所以用after
//                var insertStr = "<tr id=" + rowId + ">"
//                              + "<td style='width: 20%'>" + firstCell + "</td>"
//                              + "<td style='width: 20%'>" + secondCell + "</td>"
//                              + "<td style='width: 20%'>" + thirdCell + "</td>"
//                              + "<td style='width: 30%'>" + fourthCell + "</td>"
//                              + "<td colspan='2'><input type='button' name='delete' value='删除' style='width:80px' onclick='deleteSelectedRow(\"" + rowId + "\")' />" + "</tr>";
//                //这里的行数减3，是因为要减去底部的一行和顶部的一行，剩下的为开始要插入行的索引
//                $("#orderTable tr:eq(" + (rowLength - 2) + ")").after(insertStr); //将新拼接的一行插入到当前行的下面
//                //为新添加的行里面的控件添加新的id属性。
//                $("#" + rowId + " td:eq(0)").children().eq(0).attr("id", "gbhone" + flag);
//                $("#" + rowId + " td:eq(1)").children().eq(0).attr("id", "ghdone" + flag);
//                $("#" + rowId + " td:eq(2)").children().eq(0).attr("id", "gbhtwo" + flag);
//                $("#" + rowId + " td:eq(3)").children().eq(0).attr("id", "ghdtwo" + flag);
//                //每插入一行，flag自增一次
//                flag++;
//            }
//            $("#gbhone"+i).val(value[0]);
//            $("#ghdone" + i).val(value[1]);
//            $("#gbhtwo" + i).val(value[2]);
//            $("#ghdtwo" + i).val(value[3]);
//        }
//    }
//}
////-----------------新增一行-----------start---------------
//function insertNewRowPr() {
//    //debugger;
//    //获取表格有多少行
//    var rowLength = $("#orderTable tr").length;
//    //这里的rowId就是row加上标志位的组合。是每新增一行的tr的id。
//    var rowId = "row" + flag;

//    //每次往下标为flag+1的下面添加tr,因为append是往标签内追加。所以用after
//    var insertStr = "<tr id=" + rowId + ">"
//        + "<td >" + firstCell + "</td>"
//        + "<td >" + secondCell + "</td>"
//        + "<td >" + thirdCell + "</td>"
//        + "<td >" + fourthCell + "</td>"
//        + "<td >" + fiveCell + "</td>"
//        + "<td >" + sixCell + "</td>"
//        + "<td >" + sevenCell + "</td>"
//        + "<td >" + eightCell + "</td>"
//        + "<td ><input type='button' name='delete' value='删除' style='width:80px' onclick='deleteSelectedRow(\"" + rowId + "\")' />" + "</tr>";
//    //这里的行数减2，是因为要减去底部的一行和顶部的一行，剩下的为开始要插入行的索引
//    $("#orderTable tr:eq(" + (rowLength - 2) + ")").after(insertStr); //将新拼接的一行插入到当前行的下面
//    //为新添加的行里面的控件添加新的id属性。
//    $("#" + rowId + " td:eq(0)").children().eq(0).attr("id", "gbhone" + flag);
//    $("#" + rowId + " td:eq(1)").children().eq(0).attr("id", "ghdone" + flag);
//    $("#" + rowId + " td:eq(2)").children().eq(0).attr("id", "gbhtwo" + flag);
//    $("#" + rowId + " td:eq(3)").children().eq(0).attr("id", "ghdtwo" + flag);
//    $("#" + rowId + " td:eq(4)").children().eq(0).attr("id", "gbhthree" + flag);
//    $("#" + rowId + " td:eq(5)").children().eq(0).attr("id", "ghdthree" + flag);
//    $("#" + rowId + " td:eq(6)").children().eq(0).attr("id", "gbhfour" + flag);
//    $("#" + rowId + " td:eq(7)").children().eq(0).attr("id", "ghdfour" + flag);
//    //每插入一行，flag自增一次
//    flag++;
//}
////-----------------删除一行，根据行ID删除-start--------    
//function deleteSelectedRowPr(rowID) {
//    if (confirm("确定删除该行吗？")) {
//        $("#" + rowID).remove();
//    }
//}

