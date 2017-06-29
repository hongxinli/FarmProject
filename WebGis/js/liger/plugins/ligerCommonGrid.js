/**
*liger improve by LiKun v1.0
*  
*Author LiKun
* 
*/
(function ($) {

    $.fn.ligerCommonGrid = function () {
        $.extend($.ligerDefaults.CommonGrid, $.ligerDefaults[$.liger.controls.CommonGrid.base.__getType()]);//继承属性
        return $.liger.run.call(this, "ligerCommonGrid", arguments);
    };

    $.ligerDefaults = $.ligerDefaults || {};
    $.ligerDefaults.CommonGrid = {
        width: "auto",
        height: "auto",
        Pager: true,
        url: false,                  //使用url时候 ajax属性
        dataType: 'server',
        method: "post",             //使用url时候 ajax属性
        async: false,              //使用url时候 ajax属性
        root: 'Rows',               //数据源字段名
        focusObjid: "objid",
        editForm: false,
        minheight_commongridbar: 125, //设置工具条最小高度
        record: 'Total',            //数据源记录数字段名
        page: 1,                    //默认当前页      
        TextBoxBorderOnly: true,    //设置textbox只显示底边lbq
        GroupClomus: "auto",        //设置每行几列显示 默认一列 数值或者auto
        YSData: null,                //记录原始数据  
        borderCss: true,          //给表单添加边框
        statusName: '__status',     //记录表单数据状态 add  updata  delete nochange 
        DasMode: false,
        layMode: { auto: true, align: 'center' }//排列控制，auto为原来控件的宽度，不随着页面宽度拉伸；align为自由格式在页面中的对齐方式left|center|right
    };


    $.ligerDefaults.CommonGridString = {
        TitleStatMessage: '撤销分列 恢复到默认',
        TitleFirstMessage: '显示1列',
        TitleSecondMessage: '显示2列',
        TitleThirdMessage: '显示3列',
        TitleWinMessage: '打开自由格式配置界面'
    };
    //表单组件
    $.liger.controls.CommonGrid = function (element, options) {
        $.liger.controls.CommonGrid.base.constructor.call(this, element, options);
    };
    //grid的格式化器  先用着
    $.ligerDefaults.Grid.formatters = $.ligerDefaults.Grid.formatters || {};

    $.liger.controls.CommonGrid.ligerExtend($.liger.controls.Form, {
        __getType: function () {
            return 'CommonGrid'
        },
        __idPrev: function () {
            return 'CommonGrid';
        },
        _extendMethods: function () {
            return $.ligerMethos.CommonGrid;
        },
        _init: function () {
            var g = this, p = this.options;
            $.liger.controls.CommonGrid.base._init.call(this);
            p.dataType = p.url ? "server" : "local";
            if (p.dataType == "local") {
                // p.data = p.data || [];
                p.dataAction = "local";
            }

        },
        _render: function () {
            var g = this, p = this.options;
            //$.liger.controls.CommonGrid.base._render.call(this);
            var jform = $(this.element);
            g.form = jform.is("form") ? jform : jform.parents("form:first");
            //生成liger表单样式
            $("input,select,textarea", jform).each(function () {
              p.editorBulider.call(g, $(this));
            });
            
            $(p.fields).each(function (fieldIndex, field) {
                if(field.render)
                    field.render.call(g,this);
            })
            
            g.commonGrid = $(g.element);
            g.editor = { editing: false };  //单编辑器,配置clickToEdit
            g.set(p);

            if (p.data) {
                g.YSData = $.extend({}, p.data.Rows);//原始data存储              
                g.YSDataDs = g.getYsDasData(p.data.Rows);//原始采集data存储   
            }


        },
        _setUrl: function (value) {
            //出现两次请求的问题  待解决        
            var g = this, p = this.options;
            this.options.url = value;
            if (value) {
                g.loadData(true);
            }
        },

        //#region 设置字段       
        _setFields: function (fields, filedsnum) {
            var g = this, p = this.options;
            if ($.isFunction(p.prefixID)) p.prefixID = p.prefixID(g);
          

            g.createformGrid(fields, filedsnum);//创建表单  
        
            if (p.data) {
                p.pageCount = parseInt(p.data[p.root].length);//获取总页数
                p.total = parseInt(p.data[p.root].length);//获取总条数        
            }
            this._bulid();//扩展            
        },
        //清空表单并创建
        createformGrid: function (fields, filedsnum)
        {
            var g = this, p = this.options;
            p.labelWidth = g._getMaxLableWidthColmun();//获取lable的宽度
            p.space = g._getMaxSpaceWidthColmun();//获取space的宽度
          
         
            //清空表单和工具条
            var jform = $(this.element);
            $(".l-form-container,.l-freegrid-bar", jform).remove();

            var Pdata = null;
            //自动创建表单
            if (fields && fields.length) {
                g._preSetFields(fields);
                if (!jform.hasClass("l-form"))
                    jform.addClass("l-form");
             
              
                if (p.width == "auto") {
                    jform.width($(window).width());//添加              
                    p.width = $(window).width();
                }
                else if (p.width == "100%" && !p.data) {
                    jform.width($(".l-das").width());//添加              
                    p.width = $(".l-das").width();
                }
              
           
                if (p.borderCss) {
                    jform.addClass("l-formborder");
                }
                var out = ['<div class="l-form-container">'];
          
                var appendULStartTag = false, lastVisitedGroup = null;
                var groups = [];
                $(fields).each(function (index, field) {
                    var name = field.name,
                    readonly = (field.readonly || (field.editor && field.editor.readonly)) ? true : false,
                    txtInputName = (p.prefixID || "") + (field.textField || field.id || field.name);

                    if ($.inArray(field.group, groups) == -1)
                        groups.push(field.group);
                });

                var num = g._setGroupClomusFn(p.GroupClomus, fields);//设置num为几列换行
               
                var Maxwidth = g._getMaxWidthColmun(fields, filedsnum);//获取最大宽度
               // console.info("Maxwidth:" + Maxwidth);
                if (filedsnum && filedsnum != "auto") {//修改几列换行
                    num = filedsnum;
                }

                $(groups).each(function (groupIndex, group) {
                    var numIndex = 0;
                    //遍历每个群组中的字段                
                    $(fields).each(function (i, field) {
                        if (field.group != group) return;
                        var index = $.inArray(field, fields);
                        var name = field.id || field.name;
                        var newline = false;//为true则换行
                        var re = /^[1-9]+[0-9]*]*$/;
                        if (re.test(numIndex / num)) {
                            newline = true;
                        }
                        var inputName = (p.prefixID || "") + (field.id || field.name);
                        if (!name) return;
                        if (field.type == "hidden") {
                            if (!$("#" + inputName).length)
                                out.push('<div style="display:none" id="' + (g.id + "|" + i) + '"></div>');
                            return;
                        }
                        var toAppendGroupRow = field.group && field.group != lastVisitedGroup;
                        if (index == 0 || toAppendGroupRow) newline = true;
                        if (newline) {
                            if (appendULStartTag) {
                                out.push('</ul>');
                                appendULStartTag = false;
                            }
                            if (toAppendGroupRow) {
                                out.push('<div class="l-group');
                                if (field.groupicon)
                                    out.push(' l-group-hasicon');
                                out.push('">');
                                if (field.groupicon)
                                    out.push('<img src="' + field.groupicon + '" />');
                                out.push('<span>' + field.group + '</span></div>');
                                lastVisitedGroup = field.group;
                            }
                            out.push('<ul>');
                            appendULStartTag = true;
                        }
                        //代表一列-开始
                        out.push('<li class="l-fieldcontainer');
                        if (newline) {
                            out.push(' l-fieldcontainer-first');
                        }
                        out.push('"');
                        out.push(' fieldindex=' + index);
                        out.push('><ul>');
                        out.push(g._buliderLabelContainer(field, index));
                        out.push(g._buliderControlContainer(field, index, Pdata, Maxwidth));
                        out.push(g._buliderSpaceContainer(field, index));
                        out.push('</ul></li>');
                        //代表一列-结束
                        numIndex++;
                    });
                });
                if (appendULStartTag) {
                    out.push('</ul>');
                    appendULStartTag = false;
                }            

                out.push('</div>');
                jform.append(out.join(''));
                //创建右侧工具栏
                var formhtmlarr = [];
                formhtmlarr.push(" <div class='l-freegrid-bar' style='width:23px;  height:" + p.height + "px; min-height:" + p.minheight_commongridbar + "px; _height:" + p.minheight_commongridbar + "px; float:right; '>");
                formhtmlarr.push("<div class='l-bar-top'></div>");
                formhtmlarr.push("<div class='l-bar-colcx' title='" + p.TitleStatMessage + "' ></div>");
                formhtmlarr.push("<div class='l-bar-tline'></div>");
                formhtmlarr.push("<div class='l-bar-col1' title='" + p.TitleFirstMessage + "' ></div>");
                formhtmlarr.push("<div  class='l-bar-col2' title='" + p.TitleSecondMessage + "'></div>");
                formhtmlarr.push("<div class='l-bar-col3' title='" + p.TitleThirdMessage + "' ></div>");
                formhtmlarr.push("<div class='l-bar-tline'></div>");
                formhtmlarr.push("<div class='l-bar-freegridmessage' ></div>");
                formhtmlarr.push("</div>");

                jform.append(formhtmlarr.join(''));
                //设置宽高
                var container = jform.find(".l-form-container");              
                jform.height(p.height);
                var w = jform.width();
              
                $(container).addClass("l-form-containerlt").css({ "width": (w -60), "height": p.height  });
                var w2 = $(container).width();
             
                if (w2 > w) {
                    $(container).width(w - 60);
                }

                //分组类
                $(".l-group .togglebtn", jform).remove();
                $(".l-group", jform).width(jform.width() * 0.95).append("<div class='togglebtn'></div>");
            }
            //渲染字段
            //if (!filedsnum) {//存在就不创建
            (function () {
                g.editors = g.editors || {};
                $(fields).each(function (fieldIndex, field) {
                    var type = undefined;
                    if (field.editor) {
                        type = field.editor.type
                    } else {
                        type = field.type;
                    }
                  
                    var container = document.getElementById(g.id + "|" + fieldIndex), editor = p.editors[type];
                    $(container).addClass("l-form-editor");//
                    if (!container) return;
                    container = $(container);

                    var editorControl = g._createEditor(editor, container, {
                        field: field
                    }, container.width(), container.height());

                    if (g.editors[fieldIndex] && g.editors[fieldIndex].control && g.editors[fieldIndex].control.destroy) {
                        g.editors[fieldIndex].control.destroy();
                    }
                    g.editors[fieldIndex] = {
                        control: editorControl,
                        editor: editor
                    };
                   
                    //lbq  判断是否调用方法  设置text边框  只有底部
                    if (p.TextBoxBorderOnly) {
                        if (type == "text" || type == "string" || type == "number" || type == "int" || type == "float") {
                            var t = g.editors[fieldIndex].control.wrapper;
                            $(t).addClass("BBottomCls");
                        } else if (type == "textarea") {
                            var t = g.editors[fieldIndex].control;
                           
                            $(t).addClass("BBottomCls");
                        }
                    }

                });
                g.trigger('afterSetFields');
            }).ligerDefer(g, 10);
            // }
        },
        //#endregion  
        _bulid: function () {
            var g = this, p = this.options;
            if (p.data) {
                g._bulidFoot();
                g.currentData = g._getCurrentPageData();
                g._showData(1);//展示一条数据  
            }

            //创建事件(点击)
            g._setEvent();
        },
        //结束编辑事件
        endEdit: function (name, value) {
            var g = this, p = this.options;
            //var o = g.editor;
            //更新采集数据
            if (p.data) {
                var rowdata = g.getData();//获取当前表单数据 
                var listdata = p.data[p.root];//获取全部数据 
                var index = $.inArray(rowdata, listdata);
                if (index != -1) {
                    var oldval = g.YSDataDs.Data[index].RowData[name].OldValue;
                    //值不相同才更新
                    if (oldval != value) {
                        g.YSDataDs.Data[index].RowState = "update";
                        for (var _p in g.YSDataDs.Data[index].RowData) {
                            var Value2 = rowdata[_p];
                            if (_p == name) {
                                g.YSDataDs.Data[index].RowData[_p] = { "OldValue": g.YSDataDs.Data[index].RowData[_p].OldValue, "NewValue": Value2 }
                            }
                        }
                    }

                }
            }

            //if (o.input && o.input.inputText) {
            //    var input = o.input.inputText.get(0);
            //    if ($(input).attr('validate')) {

            //        if (Das.validator.check(input) == false)//验证未通过,每验证一次就创建个errorList项，所以要先删除，否则会重复添加
            //        {
            //            if (!Das.checkout(input, g)) {
            //                input.focus();
            //                //$(cellObj).attr('editor_oldid', input.id);
            //                return;
            //            }
            //            else {
            //                //给单元格增加验证不通过样式
            //                $(input).addClass('l-text-invalid-cell');
            //                //增加悬浮提示
            //                $(input).removeAttr("title").ligerHideTip();
            //                $(input).attr("title", $(input).attr('error')).ligerTip({
            //                    distanceX: 5,
            //                    distanceY: -3,
            //                    auto: true
            //                });
            //            }
            //        }
            //        else {
            //            if ($(input).hasClass("l-text-invalid-cell"))
            //                $(input).removeClass("l-text-invalid-cell");
            //            $(input).removeAttr("title").ligerHideTip();
            //            //Das.validator.errorList = $.grep(Das.validator.errorList, function (error, i) { return error.element.id == $(cellObj).attr('editor_id'); })
            //        }
            //    }
            //}
           // alert(value);
            g.trigger('afterEdit', [name, value]);
        },

        //添加一条数据lbq
        addData: function (rowdata, isBefore) {
            var g = this, p = this.options;
            rowdata = rowdata || {};

            var neardata = g._getCurrentPageData();
            g._addData(rowdata, neardata, isBefore);
            rowdata[p.statusName] = 'add';
            //标识状态
            p.total = p.total ? (p.total + 1) : 1;
            p.data[p.record] = p.total;

            p.pageCount = p.total;
            g._bulidFoot();

            g.setFormData(rowdata);//给表单赋值           
            return rowdata;
        },
        _addData: function (rowdata, neardata, isBefore) {
            var g = this, p = this.options;
            var listdata = p.data[p.root];//获取全部数据  
            var listCJdata = g.YSDataDs.Data;
            if ($.isEmptyObject(rowdata)) {//添加的是空对象
                $(p.fields).each(function (fieldIndex, field) {
                    var name = field.name;
                    rowdata[name] = "";
                });
            }
            if (neardata) {
                var index = $.inArray(neardata, listdata);
                listdata.splice(index == -1 ? -1 : index + (isBefore ? 0 : 1), 0, rowdata);//插入到响应的位置
                //同步采集格式
                var o = { "RowState": "add", "RowData": {} };
                $.extend(o.RowData, rowdata);
                for (var _p in o.RowData) {
                    var Value = rowdata[_p];
                    o.RowData[_p] = { "OldValue": Value, "NewValue": Value }
                }
                listCJdata.splice(index == -1 ? -1 : index + (isBefore ? 0 : 1), 0, o);//插入到响应的位置2

                g.YSDataDs.Data = listCJdata;
            }
            else {
                listdata.push(rowdata);
            }

        },
        //删除一条数据lbq
        deleteData: function () {
            var g = this, p = this.options;
            rowdata = g.getData();
            var listdata = p.data[p.root];//获取全部数据          

            var index = $.inArray(rowdata, listdata);//获取删除的下标  

            if (index != -1) {
                g.YSDataDs.Data[index].RowState = "delete";
            }
            //p.pageCount = parseInt(p.data[p.root].length);//获取总页数
            //p.total = parseInt(p.data[p.root].length);//获取总条数  
            p.pageCount = g.getUndelete();;//获取总页数
            p.total = g.getUndelete();//获取总条数  
            var newindex = index + 1;
            if (p.total == 1) {
                g.clearData();//清空前台数据
            } else if (newindex < p.total) {
                var data = p.data[p.root][newindex];
                data[p.statusName] = 'delete';//添加删除状态               
                g.setFormData(data);//给表单赋值     
            }

            //清空后给表单赋值下一条数据  最后一条数据判断
        },
        //删除所有行数据（单个插件用）
        deleteAllData: function () {
            var g = this, p = this.options;
            var listdata = p.data[p.root];//获取全部数据 

            $(p.data[p.root]).each(function (index, rowdata) {

                rowdata[p.statusName] = 'delete';
                g.YSDataDs.Data[index].RowState = "delete";
            });
            g.clearData();//清空前台数据          
        },
        //删除一定范围内的数据
        delRangeData: function (start, end) {
            var g = this, p = this.options;
            var dd = p.data[p.root];//获取全部数据          

            if (start > end) {
                alert("开始行数不能大于结束行数");
            } else if (start < 1 || end < 1) {
                alert("输入行数必须大于1");
            } else if (end > dd.length) {
                alert("输入行数大于总行数,请修改");
            }
            else if (typeof (start) == "number" && typeof (end) == "number") {
                var index = start - 1;
                var length = end - start;//删除个数
                for (var i = 0; i <= length; i++) {
                    dd[index][p.statusName] = 'delete';
                    g.YSDataDs.Data[index].RowState = "delete";
                    index++;
                }
                if (end < dd.length) {
                    var enddata = dd[end + 1];
                    g.setFormData(enddata);//给表单赋值     
                } else if (end == dd.length) {
                    g.setFormData(dd[0]);//给表单赋值     
                }
            }

        },
        //删除(未用到)
        _removeData: function (rowdata) {
            var g = this, p = this.options;
            var index = $.inArray(rowdata, g.data);
            if (index != -1) {
                g.selected.splice(index, 1);

            }
        },
        //获取采集格式数据
        getDasData: function () {
            var g = this, p = this.options;
            if (g.YSDataDs) {
                return g.YSDataDs;
            } else {
                var data = { "Cols": [], "Data": [] };
                for (var j in p.fields) {
                    var o = { "colname": p.fields[j].name, "type": p.fields[j].type };
                    data.Cols.push(o);
                }
                return data;
            }
        },
        //获取原始采集格式数据
        getYsDasData: function (ysdata) {
            var g = this, p = this.options;
            var data = { "Cols": [], "Data": [] };
            for (var j in p.fields) {
                var o = { "colname": p.fields[j].name, "type": p.fields[j].type };
                data.Cols.push(o);
            }
            data.Data = g._getYsDasData(ysdata);
            return data;
        },
        //将原始数据恢复到nochange
        updateDataStatus: function () {
            var g = this, p = this.options;
            //g.YSDataDs
            var data = g.getDasData();
            var CloData = data.Data;
            $(CloData).each(function (index,val) {
                v.RowState = "nochanged";
            });
            g.YSDataDs.Data = CloData;
        },
        //获取采集格式Data数据
        _getYsDasData: function (ysdata) {

            var g = this, p = this.options;
            var data = [];
            //var yso = g._formatRecord(ysdata);//格式化数据 
            //状态为nochanged的数据 
            $.each(ysdata, function (i, v) {
                var yso = g._formatRecord(ysdata[i]);//格式化数据  
                var o = { "RowState": "nochanged", "RowData": {} };

                $.extend(o.RowData, yso);
                for (var _p in o.RowData) {
                    var Value = ysdata[i][_p];
                    o.RowData[_p] = { "OldValue": Value, "NewValue": Value }
                }
                data.push(o)
            });

            return data;
        },
        //扩展2013-11-15
        _bulidFoot: function () {
            var g = this, p = this.options;
            // p.data = p.data || [];
            if (!p.pageCount) p.pageCount = 1;
            if (!p.total) p.total = 0;
        },
        //改变分页lbq
        goTo: function (ctype) {
            var g = this, p = this.options;
            // p.pageCount = parseInt(p.data[p.root].length);
            p.pageCount = parseInt(g.getUndeleteNum());

            switch (ctype) {
                case 'first':
                    if (p.page == 1) { alert("已到达首行!"); return };
                    p.newPage = 1;
                    break;
                case 'prev':
                    if (p.page == 1) { alert("已到达首行!"); return };
                    if (p.page > 1) p.newPage = parseInt(p.page) - 1;
                    break;
                case 'next': if (p.page >= p.pageCount) { alert("已到达尾行!"); return; }
                    p.newPage = parseInt(p.page) + 1; break;
                case 'end': if (p.page >= p.pageCount) { alert("已到达尾行!"); return; };
                    p.newPage = p.pageCount; break;
            }

            if (p.newPage == p.page) return false;
            g.currentData = g._getCurrentPageData();
            while (g.currentData.__status && g.currentData.__status == "delete") {

                if (ctype == "next" && p.newPage != p.pageCount) {
                    p.newPage++;
                } else if (ctype == "prev" && p.newPage >= 2) {
                    p.newPage--;
                }
                g.currentData = g._getCurrentPageData();
            }
            if (p.data) {
                g._showData();
            }           


        },
        //判断是否修改
        ExistUpdate: function () {
            var g = this, p = this.options;
            var data = g.YSDataDs.Data;

            var isExist = false;
            $(data).each(function (index, v) {

                if (v.RowState == "update") {

                    isExist = true;
                }
            });

            return isExist;

        },
        //设置事件lbq
        _setEvent: function () {
            var g = this, p = this.options;
            $(document).bind("click.CommonGrid", function (e) {
                g._onClick.call(g, e);
            });
        },
        //改变为几列
        changeColmun: function (ctype) {
            var g = this, p = this.options;
            switch (ctype) {
                case 'auto':
                    p.GroupClomus = "auto";
                    break;
                case 'first':
                    p.GroupClomus = "1";
                    break;
                case 'second':
                    p.GroupClomus = "2";
                    break;
                case 'three':
                    p.GroupClomus = "3";
                    break;
            }
            g.createformGrid(p.fields, p.GroupClomus);//重置渲染表单
            //这里赋值bug  获取当前数据 而不是第一条g.getData()
            if (p.data) {
                g.setFormData(g.getData());
                //g.currentData = g._getCurrentPageData();
                //g.s(1);//展示一条数据    
            } else {
                if (g.currentData.__status && g.currentData.__status == "delete") {                   
                    g.setAllControlDisable();
                } else {
                    g.setFormData(g.currentData);
                }                
            }
            //设置禁用全局状态
            if (!g.disableState) {

            }
        },

        //标签部分
        _buliderLabelContainer: function (field) {
            var g = this, p = this.options;
            var label = field.label || field.display;
            var labelWidth = field.labelWidth || field.labelwidth || p.labelWidth;
            var labelAlign = field.labelAlign || p.labelAlign;
            if (label) label += p.rightToken;
            var out = [];
            out.push('<li');
            if (p.labelCss) {
                out.push(' class="bq ' + p.labelCss + '"');
            } else {
                out.push(' class="bq "');
            }
            out.push(' style="');
            if (labelWidth) {
                out.push('width:' + labelWidth + 'px;');
            }
            if (labelAlign) {
                out.push('text-align:' + labelAlign + ';');
            }
            //lbq
            if (field.notnull == true && field.primarykey != true) {
                out.push('color:blue;');
            } else if (field.primarykey == true) {
                out.push('color:red;');
            }
            out.push('">');
            if (label) {
                out.push(label);
            }
            out.push('</li>');
            return out.join('');
        },
        //控件部分 lbq添加data参数 Maxwidth 最大宽度
        _buliderControlContainer: function (field, fieldIndex, data, Maxwidth) {
            var g = this, p = this.options;
            var width = "";
            width = Maxwidth;
            var align = field.align || field.textAlign || field.textalign || p.align;
           
            //非自动模式  
            var layModealign = "";
            if (!p.layMode.auto) {
                if (p.layMode.align == "left") {
                    layModealign = "flleft";
                } else if (p.layMode.align == "right") {
                    layModealign = "flright";
                } else if (p.layMode.align == "center") {
                    layModealign = "flauto";
                }
            }
            var out = [];
            out.push('<li');
            out.push(' id="' + (g.id + "|" + fieldIndex) + '"');
            if (p.fieldCss || !p.layMode.auto) {
                out.push(' class="bq ' + p.fieldCss + '  ' + layModealign + '"');
            } else {
                out.push(' class="bq "');
            }
            out.push(' style="');
            if (width) {
                out.push('width:' + width + 'px;');

            }
            if (align) {
                //out.push('text-align:' + align + ';');
                out.push('text-align:left;'); //靠右和中会出现边框问题
            }
            out.push('" InitValue="' + g._getValueByName(data, field.name) + '">');//获取初始值？？          
            out.push('</li>');
            return out.join('');
        },
        //间隔部分
        _buliderSpaceContainer: function (field) {
            var g = this, p = this.options;
            var spaceWidth = field.space || field.spaceWidth || p.space;
            var out = [];
            out.push('<li');
            if (p.spaceCss) {
                out.push(' class="bq ' + p.spaceCss + '"');
            } else {
                out.push(' class="bq "');
            }
            out.push(' style="');
            if (spaceWidth) {
                out.push('width:' + spaceWidth + 'px;');
            }
            out.push('">');

            //lbq  添加计量单位
            if (field.unit) {
                out.push("<span class='l-star'>&nbsp;" + field.unit + "</span>");
            }
            out.push('</li>');
            return out.join('');
        },

        //设置单个单元格的值
        setValueByNameEx: function (data, name, value,grid) {
            var g = this, p = this.options;
            var fields = g.get('fields');
            var datatmp = data;
            var nameParm = name;
            (function () {
                $(fields).each(function (fieldIndex, field) {
                    if (nameParm == field.name) {
                        var name = field.name, textField = field.textField, editor = g.editors[fieldIndex];
                        if (!editor) return;
                        if (name && (name in datatmp)) {
                            var value = g._getValueByName(datatmp, name);
                            /*if (editor.control.type == "ComboBox" && editor.control.grid) {
                                var cellobj = grid && grid.getCellObj(datatmp.__id, name);
                                value = $(cellobj).text();
                            }*/
                            editor.editor.setValue(editor.control, value);
                            
                        }
                        if (textField && (textField in datatmp)) {
                            var text = g._getValueByName(datatmp, textField);
                            editor.editor.setText(editor.control, text);
                        }
                    }
                });
            }).ligerDefer(g, 20);
        },
        //控制高度方法
        _setHeight: function (h) {
            var g = this, p = this.options;
            var jform = $(this.element);
            var content = jform.find(".l-form-container");

            var bar = jform.find(".l-freegrid-bar");//form对象          
            if (h) {
                p.height = h;
                $(jform).height(h - 2);
                $(content).height(h - 2);
             
                $(bar).height(h - 2);
            }


        },
        //获取删除之外的记录数
        getUndeleteNum: function () {
            var g = this, p = this.options;
            return g.getUndelete().length;

        },
        //获取删除之外的记录
        getUndelete: function () {
            var g = this, p = this.options;
            var d = p.data.Rows;
            var data = [];
            $(d).each(function (index, v) {
                if (v.__status != "delete") {
                    data.push(v);
                }
            });
            return data;
        },
        //获取排除删除记录之外的索引
        getUndelcurrentIndex: function () {
            var g = this, p = this.options;
            var d = p.data.Rows;
            var Undelcurrent = [];//获取排除删除记录之外的数组
            $(d).each(function (index, v) {
                if (v.__status != "delete") {
                    Undelcurrent.push(v);
                }
            });
            var index = $.inArray(g.getData(), Undelcurrent);
            return index;
        },
        //检查主键是否重复
        checkPkRepeak: function () {
            var g = this, p = this.options;
            var bPk = false;//是否有主键列
            var bPkRe = false;//是否有主键重复          
            for (var j in p.fields) {
                if (p.fields[j].primarykey) {
                    bPk = true;
                    break;
                }
            }
            if (bPk) {//是否有主键列
                var gridrows = [];
                var gdata = g.getUndelete();//非删除数据数组
                $.each(gdata, function (i, row) {

                    var d = {};
                    $.each(p.fields, function (j, column) {

                        if (column.primarykey) {

                            if (column.type == 'number') {
                                if (column.datalength && column.datalength.toString().indexOf('.') != -1)//float型
                                    d[column.name] = parseFloat(row[column.name], 10);//提取键值
                                else
                                    d[column.name] = parseInt(row[column.name], 10);//提取键值
                            }
                            else {
                                d[column.name] = row[column.name];//提取键值                             
                            }

                        }
                    })
                    gridrows.push(d);

                })
                var primarykeyDataArr = $.uniqueJson(gridrows);//取唯一,uniqueJson方法在Das中定义

                if (primarykeyDataArr.length < gdata.length)//唯一的长度小于原来长度
                    bPkRe = true;
            }
            return bPkRe;
        }

    });


})(jQuery);