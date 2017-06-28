/**
*liger improve by LiKun v1.0
* 
* 
*  
* Author LiKun
* 
*/
(function ($) {

    $.fn.ligerForm = function () {
        return $.liger.run.call(this, "ligerForm", arguments);
    };

    $.liger.getConditions = function (form, options) {
        if (!form) return null;
        form = liger.get($(form));
        if (form && form.toConditions) return form.toConditions();
    };

    $.ligerDefaults = $.ligerDefaults || {};
    $.ligerDefaults.Form = {
        //控件宽度
        inputWidth: 180,
        //标签宽度
        labelWidth: 0,
        labelWidthEv: 15,
        //间隔宽度
        space: 20,
        rightToken: '：',
        //标签对齐方式
        labelAlign: 'left',
        //控件对齐方式
        align: 'left',
        fields: [], //字段
        //创建的表单元素是否附加ID
        appendID: true,
        //生成表单元素ID、Name的前缀
        prefixID: null,
        //json解析函数
        toJSON: $.liger.toJSON,
        labelCss: null,
        fieldCss: "",
        spaceCss: null,
        onAfterSetFields: null,
        buttons: null,              //按钮组
        readonly: false,              //是否只读
        editors: {},              //编辑器集合,使用同$.ligerDefaults.Grid.editors
        //验证
        validate: null

    };

    $.ligerDefaults.FormString = {
        invalidMessage: '存在{errorCount}个字段验证不通过，请检查!',
        detailMessage: '详细',
        okMessage: '确定'
    };
    //#region格式化器扩展  
    $.ligerDefaults.Form.editors.textarea =
   {
       create: function (container, editParm, p) {
           var editor = $('<textarea class="l-textarea" />');
           var id = (p.prefixID || "") + editParm.field.name;
           if ($("#" + id).length) {
               editor = $("#" + id);
           }
           editor.attr({
               id: id,
               name: id
           });
           if (p.readonly) editor.attr("readonly", true);
           container.append(editor);
           return editor;
       },
       getValue: function (editor, editParm) {          
           return editor.val();
       },
       setValue: function (editor, value, editParm) {
           editor.val(value);
       },
       resize: function (editor, width, height, editParm) {
           editor.css({
               width: width - 2
           }).parent().css("width", "auto");
       }
       //,setEnabled: function () {
       //    this.inputText.removeAttr("readonly");
       //    this.wrapper.removeClass('l-text-disabled');
       //},
       //setDisabled: function () {
       //    this.inputText.attr("readonly", "readonly");
       //    this.wrapper.addClass("l-text-disabled");
       //}

   };

    $.ligerDefaults.Form.editors.hidden =
    {
        create: function (container, editParm, p) {
            var editor = $('<input type = "hidden"  />');
            var id = (p.prefixID || "") + editParm.field.name;
            if ($("#" + id).length) {
                editor = $("#" + id);
            }
            editor.attr({
                id: id,
                name: id
            });
            container.append(editor);
            return editor;
        },
        getValue: function (editor, editParm) {           
            return editor.val();
        },
        setValue: function (editor, value, editParm) {
            editor.val(value);
        }
    };
    //#endregion


    $.ligerDefaults.Form_fields = {
        name: null,             //字段name
        textField: null,       //文本框name
        type: null,             //表单类型
        editor: null,           //编辑器扩展类型
        label: null,            //Label
        newline: true,          //换行显示
        op: null,               //操作符 附加到input
        vt: null,               //值类型 附加到input
        attr: null,             //属性列表 附加到input
        validate: null
    };

    $.ligerDefaults.Form_editor = {};

    //description 自动创建liger风格的表单-编辑器构造函数
    //editorBulider -> editorBuilder 命名错误 
    //param {jinput} 表单元素jQuery对象 比如input、select、textarea 
    $.ligerDefaults.Form.editorBulider = function (jinput) {
        //这里this就是form的liger对象
        var g = this, p = this.options;
        var options = {}, ltype = jinput.attr("ltype"), field = {};
        if (p.readonly) options.readonly = true;
        options = $.extend({
            width: (field.width || p.inputWidth) - 2
        }, field.options, field.editor, options);
        if (ltype == "autocomplete")
            options.autocomplete = true;
        if (jinput.is("select")) {
            jinput.ligerComboBox(options);
        }
        else if (jinput.is(":password")) {
            jinput.ligerTextBox(options);
        }
        else if (jinput.is(":text")) {
            switch (ltype) {
                case "select":
                case "combobox":
                case "autocomplete":
                    jinput.ligerComboBox(options);
                    break;
                case "spinner":
                    jinput.ligerSpinner(options);
                    break;
                case "date":
                    jinput.ligerDateEditor(options);
                    break;
                case "popup":
                    jinput.ligerPopupEdit(options);
                    break;
                case "currency":
                    options.currency = true;
                case "float":
                case "number":
                    options.number = true;
                    jinput.ligerTextBox(options);
                    break;
                case "int":
                case "digits":
                    options.digits = true;
                default:
                    jinput.ligerTextBox(options);
                    break;
            }
        }
        else if (jinput.is(":radio")) {
            jinput.ligerRadio(options);
        }
        else if (jinput.is(":checkbox")) {
            jinput.ligerCheckBox(options);
        }
        else if (jinput.is("textarea")) {
            jinput.addClass("l-textarea");
        }
    }

    //表单组件
    $.liger.controls.Form = function (element, options) {
        $.liger.controls.Form.base.constructor.call(this, element, options);
    };

    $.liger.controls.Form.ligerExtend($.liger.core.UIComponent, {
        __getType: function () {
            return 'Form'
        },
        __idPrev: function () {
            return 'Form';
        },
        _init: function () {
            var g = this, p = this.options;
            $.liger.controls.Form.base._init.call(this);
            //编辑构造器初始化
            for (var type in liger.editors) {
                var editor = liger.editors[type];
                //如果没有默认的或者已经定义
                if (!editor || type in p.editors) continue;
                p.editors[type] = liger.getEditor($.extend({
                    type: type
                }, editor));
            }
        },
        _render: function () {
            var g = this, p = this.options;
            var jform = $(this.element);
            g.form = jform.is("form") ? jform : jform.parents("form:first");
            //生成liger表单样式
            $("input,select,textarea", jform).each(function () {
                p.editorBulider.call(g, $(this));
            });

            if (p.buttons) {
                var jbuttons = $('<ul class="l-form-buttons"></ul>').appendTo(jform);
                $(p.buttons).each(function () {
                    var jbutton = $('<li><div></div></li>').appendTo(jbuttons);
                    $("div:first", jbutton).ligerButton(this);
                });
            }

            if (!g.element.id) g.element.id = g.id;
            //分组 收缩/展开
            $("#" + g.element.id + " .togglebtn").live('click', function () {
                if ($(this).hasClass("togglebtn-down")) $(this).removeClass("togglebtn-down");
                else $(this).addClass("togglebtn-down");
                var boxs = $(this).parent().nextAll("ul,div");
                for (var i = 0; i < boxs.length; i++) {
                    var jbox = $(boxs[i]);
                    if (jbox.hasClass("l-group")) break;
                    if ($(this).hasClass("togglebtn-down")) {
                        jbox.hide();
                    } else {
                        jbox.show();
                    }

                }
            });
        },
        getEditor: function (name) {
            var g = this, p = this.options;
            if (!g.editors) return;
            for (var i = 0, l = p.fields.length; i < l; i++) {
                var field = p.fields[i];
                if (field.name == name && g.editors[i]) {
                    return g.editors[i].control;
                }
            }
        },
        getField: function (index) {
            var g = this, p = this.options;
            if (!p.fields) return null;
            return p.fields[index];
        },
        toConditions: function () {
            var g = this, p = this.options;
            var conditions = [];
            $(p.fields).each(function (fieldIndex, field) {
                var name = field.name, textField = field.textField, editor = g.editors[fieldIndex];
                if (!editor || !name) return;
                var value = editor.editor.getValue(editor.control)
                if (value) {
                    conditions.push({
                        op: field.operator || "like",
                        field: name,
                        value: value,
                        type: field.type || "string"
                    });
                }
            });
            return conditions;
        },
        //预处理字段 , 排序和分组
        _preSetFields: function (fields) {
            var g = this, p = this.options, lastVisitedGroup = null, lastVisitedGroupIcon = null;
            //分组： 先填充没有设置分组的字段，然后按照分组排序
            $(p.fields).each(function (i, field) {
                if (p.readonly || field.readonly || (field.editor && field.editor.readonly))
                    delete field.validate;
                if (field.type == "hidden") return;
                field.type = field.type || "text";
                if (field.newline == null) field.newline = true;
                if (lastVisitedGroup && !field.group) {
                    field.group = lastVisitedGroup;
                    field.groupicon = lastVisitedGroupIcon;
                }
                if (field.group) {
                    //trim
                    field.group = field.group.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '');
                    lastVisitedGroup = field.group;
                    lastVisitedGroupIcon = field.groupicon;
                }
            });

        },
        _setReadonly: function (readonly) {
            var g = this, p = this.options;
            if (readonly && g.editors) {
                for (var index in g.editors) {
                    var control = g.editors[index].control;
                    if (control && control._setReadonly) control._setReadonly(true);
                }
            }
        },
        _setFields: function (fields) {
           
            var g = this, p = this.options;
            if ($.isFunction(p.prefixID)) p.prefixID = p.prefixID(g);
            g.validate = {};
            var jform = $(this.element);
            $(".l-form-container",jform).remove();
            //自动创建表单

            if (fields && fields.length) {

                g._preSetFields(fields);//预处理字段
                if (!jform.hasClass("l-form")) {
                    jform.addClass("l-form");
                }

                var out = ['<div class="l-form-container">'];
                var appendULStartTag = false, lastVisitedGroup = null;
                var groups = [];
                $(fields).each(function (index, field) {
                    var name = field.name,
                    readonly = (field.readonly || (field.editor && field.editor.readonly)) ? true : false,
                    txtInputName = (p.prefixID || "") + (field.textField || field.id || field.name);
                    if (field.validate && !readonly) {
                        g.validate.rules = g.validate.rules || {};
                        g.validate.rules[txtInputName] = field.validate;
                        if (field.validateMessage) {
                            g.validate.messages = g.validate.messages || {};
                            g.validate.messages[txtInputName] = field.validateMessage;
                        }
                    }

                    if ($.inArray(field.group, groups) == -1)
                        groups.push(field.group);
                });
                //遍历分组
                $(groups).each(function (groupIndex, group) {
                    //遍历字段
                    $(fields).each(function (i, field) {
                        if (field.group != group) return;
                        var index = $.inArray(field, fields);
                        var name = field.id || field.name, newline = field.newline;
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
                        out.push('<li class="l-fieldcontainer');
                        if (newline) {
                            out.push(' l-fieldcontainer-first');
                        }
                        out.push('"');
                        out.push(' fieldindex=' + index);
                        out.push('><ul>');
                        //append label
                        out.push(g._buliderLabelContainer(field, index));
                        //append input 
                        out.push(g._buliderControlContainer(field, index));
                        //append space
                        out.push(g._buliderSpaceContainer(field, index));
                        out.push('</ul></li>');

                    });
                });
                if (appendULStartTag) {
                    out.push('</ul>');
                    appendULStartTag = false;
                }
                out.push('</div>');
                jform.append(out.join(''));

                $(".l-group .togglebtn", jform).remove();
                $(".l-group", jform).width(jform.width() * 0.95).append("<div class='togglebtn'></div>");
            }
            (function () {
                g.editors = g.editors || {};

                $(fields).each(function (fieldIndex, field) {
                    var container = document.getElementById(g.id + "|" + fieldIndex), editor = p.editors[field.type];


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
                });
                g.initValidate();
                g.trigger('afterSetFields');
            }).ligerDefer(g, 10);
        },
        //获取当前条表单数据
        getData: function () {
            var g = this, p = this.options;
            g.data = g.data || {};
            var newdata = g.data;
            $(p.fields).each(function (fieldIndex, field) {               
                var name = field.name, textField = field.textField, editor = g.editors[fieldIndex];
                if (!editor) return;
                if (name) { //debugger;
					var value=null;
                    if (editor.control.type == "ComboBox"&&(editor.control.grid||editor.control.tree)) {//下拉表格和树特殊处理
                        value = editor.control.selectedValue;
                        //执行
                        //g.trigger('afterEdit', [name, value]);
                    }
					else
						value = editor.editor.getValue(editor.control);

                    g._setValueByName(newdata, name, value);
                }
                /*if (textField) {
                    var value = editor.editor.getText(editor.control)
                    g._setValueByName(newdata, textField, value);
                }*/
            });
           // g.setFormData(newdata);
            return g.data;
        },
        loadData: function () {

            var g = this, p = this.options;
            g.trigger('loadData');

            var ajaxOptions = {
                type: p.method,
                url: p.url,
                async: p.async,
                dataType: 'json',
                beforeSend: function () { },
                success: function (data) {
                    g.trigger('success', [data, g]);
                    if (!data || !data[p.root] || !data[p.root].length) {
                        g.currentData = g.data = {};
                        g.currentData[p.root] = g.data[p.root] = [];
                        if (data && data[p.record]) {
                            g.currentData[p.record] = g.data[p.record] = data[p.record];
                        } else {
                            g.currentData[p.record] = g.data[p.record] = 0;
                        }

                        return;
                    }

                    p.data = data;
                    g.currentData = g._getCurrentPageData();
                    g._showData();//展示一条数据   

                },
                complete: function () {
                    //g.trigger('complete', [g]);
                    //if (g.hasBind('loaded')) {
                    //    g.trigger('loaded', [g]);
                    //}
                    //else {
                    //    g.toggleLoading.ligerDefer(g, 10, [false]);
                    //}
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    g.currentData = g.data = {};
                    g.currentData[p.root] = g.data[p.root] = [];
                    g.currentData[p.record] = g.data[p.record] = 0;
                    g.toggleLoading.ligerDefer(g, 10, [false]);
                    g.trigger('error', [XMLHttpRequest, textStatus, errorThrown]);
                }
            };
            if (p.contentType) ajaxOptions.contentType = p.contentType;
            $.ajax(ajaxOptions);
        },
        setData: function (data) {
            var g = this, p = this.options;
            var fields = g.get('fields');
            g.data = data || {};
            (function () {
                $(fields).each(function (fieldIndex, field) {
                    var name = field.name, textField = field.textField, editor = g.editors[fieldIndex];
                    if (!editor) return;
                    if (name && (name in g.data)) {
                        var value = g._getValueByName(g.data, name);
                        editor.editor.setValue(editor.control, value);
                    }
                    if (textField && (textField in g.data)) {
                        var text = g._getValueByName(g.data, textField);
                        editor.editor.setText(editor.control, text);
                    }
                });
            }).ligerDefer(g, 20);
        },
        //根据字段给表单设置值
        setFormData: function (_data) {

            var g = this, p = this.options;
            if (!p.data) {//两个插件时候data为空 _data是当前表单参数
                g.currentData = _data;
            }
            ////设置禁用全局状态
           
            var fields = g.get('fields');          
            (function () {             
                $(fields).each(function (fieldIndex, field) {                  
                    var name = field.name, textField = field.textField, editor = g.editors[fieldIndex];                  
                    if (!editor) return;                
                    if (name && (name in _data)) {                      
                        var value = g._getValueByName(_data, name);
                        if (editor.control.type == "DateEditor") {  //格式化日期                        
                            value = $.ligerDefaults.Grid.formatters['date'](value, field);
                        }                     
                        editor.editor.setValue(editor.control, value);
                                            
                    } else {                       
                        editor.editor.setValue(editor.control, "");
                    }

                    if (!editor.control[0]) {

                        editor.control.setEnabled();//禁用表单控件  这个处理的是textarea   
                         $("input[ligerid='" + name + "']").removeAttr("disabled", "");
                    }
                    else {
                        if (name)
                            $("#" + name).removeClass("l-text-disabled").removeAttr("disabled");//禁用textarea                     
                    }

                    if (textField && (textField in _data)) {                    
                        var text = g._getValueByName(_data, textField);
                        editor.editor.setText(editor.control, text);
                    } else if (textField) {                     
                        editor.editor.setText(editor.control, "");
                    }
                });
                g.data = _data || {};
            }).ligerDefer(g, 50);
           
        },
        //设置控件值通过列名
        _setValueByName: function (data, name, value) {         
            if (!data || !name) return null;
            if (name.indexOf('.') == -1) {             
                data[name] = value;
            }
            else {
                try {
                    new Function("data,value", "data." + name + "=value;")(data, value);
                }
                catch (e) {
                }
            }
        },
        _getValueByName: function (data, name) {
        
            if (!data || !name) return null;
            if (name.indexOf('.') == -1) {
                return data[name];
            }
            else {
                try {
                    return new Function("data", "return data." + name + ";")(data);
                }
                catch (e) {
                    return null;
                }
            }
        },
        //验证
        valid: function () {
            var g = this, p = this.options;
            if (!g.form || !g.validator) return;
            return g.form.valid();
        },
        //设置验证
        initValidate: function () {
            var g = this, p = this.options;
            if (!g.form || !p.validate || !g.form.validate) {
                g.validator = null;
                return;
            }
            var validate = p.validate == true ? {} : p.validate;
            var validateOptions = $.extend({
                errorPlacement: function (lable, element) {
                    if (!element.attr("id"))
                        element.attr("id", new Date().getTime());
                    if (element.hasClass("l-textarea")) {
                        element.addClass("l-textarea-invalid");
                    }
                    else if (element.hasClass("l-text-field")) {
                        element.parent().addClass("l-text-invalid");
                    }
                    $(element).removeAttr("title").ligerHideTip();
                    $(element).attr("title", lable.html()).ligerTip({
                        distanceX: 5,
                        distanceY: -3,
                        auto: true
                    });
                },
                success: function (lable) {
                    if (!lable.attr("for")) return;
                    var element = $("#" + lable.attr("for"));

                    if (element.hasClass("l-textarea")) {
                        element.removeClass("l-textarea-invalid");
                    }
                    else if (element.hasClass("l-text-field")) {
                        element.parent().removeClass("l-text-invalid");
                    }
                    $(element).removeAttr("title").ligerHideTip();
                }
            }, validate, {
                rules: g.validate.rules,
                messages: g.validate.messages
            });
            g.validator = g.form.validate(validateOptions);
        },
        //提示 验证错误信息
        showInvalid: function (validator) {
            var g = this, p = this.options;
            if (!g.validator) return;
            var jmessage = $('<div><div class="invalid">' + p.invalidMessage.replace('{errorCount}', g.validator.errorList.length) + '<a class="viewInvalidDetail" href="javascript:void(0)">' + p.detailMessage + '</a></div><div class="invalidDetail" style="display:none;">' + getInvalidInf(g.validator.errorList) + '</div></div>');
            jmessage.find("a.viewInvalidDetail:first").bind('click', function () {
                $(this).parent().next("div.invalidDetail").toggle();
            });
            $.ligerDialog.open({
                type: 'error',
                width: 350,
                showMax: false,
                showToggle: false,
                showMin: false,
                target: jmessage,
                buttons: [
                    {
                        text: p.okMessage, onclick: function (item, dailog) {
                            dailog.close();
                        }
                    }
                ]
            });
        },
        _createEditor: function (editorBuilder, container, editParm, width, height) {
            var g = this, p = this.options;
            var editor = editorBuilder.create.call(this, container, editParm, p);
            if (editor && editorBuilder.resize)
                editorBuilder.resize.call(this, editor, width, height, editParm);
            return editor;
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
                out.push(' class="' + p.labelCss + '"');
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
                out.push(' class="' + p.fieldCss + '  ' + layModealign + '"');
            }
            out.push(' style="');
            if (width) {
                 out.push('width:' + width + 'px;');
               
            }
            if (align) {
                out.push('text-align:' + align + ';');
            }
            out.push('" InitValue="' + g._getValueByName(data, field.name) + '">');//获取初始值？？
            //out.push('">');
            //out.push(g._buliderControl(field, fieldIndex));
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
                out.push(' class="' + p.spaceCss + '"');
            }
            out.push(' style="');
            if (spaceWidth) {
                out.push('width:' + spaceWidth + 'px;');
            }
            out.push('">');
            //if (field.validate && field.validate.required) {
            //    out.push("<span class='l-star'>*</span>");
            //}
            //lbq  添加计量单位
            if (field.unit) {
                out.push("<span class='l-star'>&nbsp;" + field.unit + "</span>");
            }
            out.push('</li>');
            return out.join('');
        },
        _buliderControl: function (field, fieldIndex) {
            var g = this, p = this.options;
            var width = field.width || p.inputWidth,
            name = field.name || field.id,
            type = (field.type || "text").toLowerCase(),
            readonly = (field.readonly || (field.editor && field.editor.readonly)) ? true : false;
            var out = [];
            if (type == "textarea" || type == "htmleditor") {
                out.push('<textarea ');
            }
            else if ($.inArray(type, ["checkbox", "radio", "password", "file"]) != -1) {
                out.push('<input type="' + type + '" ');
            }
            else if ($.inArray(type, ["select", "combobox", "autocomplete", "popup", "radiolist", "checkboxlist", "listbox"]) != -1) {
                out.push('<input type="hidden" ');
            }
            else {
                out.push('<input type="text" ');
            }
            out.push('name="' + name + '" ');
            out.push('fieldindex="' + fieldIndex + '" ');
            field.cssClass && out.push('class="' + field.cssClass + '" ');
            p.appendID && out.push(' id="' + name + '" ');
            out.push(g._getInputAttrHtml(field));
            if (field.validate && !readonly) {
                out.push(" validate='" + p.toJSON(field.validate) + "' ");
                g.validate = g.validate || {};
                g.validate.rules = g.validate.rules || {};
                g.validate.rules[name] = field.validate;
                if (field.validateMessage) {
                    g.validate.messages = g.validate.messages || {};
                    g.validate.messages[name] = field.validateMessage;
                }
            }
            out.push(' />');
            return out.join('');
        },
        _getInputAttrHtml: function (field) {
            var out = [], type = (field.type || "text").toLowerCase();
            if (type == "textarea") {
                field.cols && out.push('cols="' + field.cols + '" ');
                field.rows && out.push('rows="' + field.rows + '" ');
            }
            out.push('ltype="' + type + '" ');
            field.op && out.push('op="' + field.op + '" ');
            field.vt && out.push('vt="' + field.vt + '" ');
            if (field.attr) {
                for (var attrp in field.attr) {
                    out.push(attrp + '="' + field.attr[attrp] + '" ');
                }
            }
            return out.join('');
        },
        //单独扩展方法
        //返回节点对象lbq
        _getSrcElementByEvent: function (e) {
            var g = this;
            var obj = (e.target || e.srcElement);
            var jobjs = $(obj).parents().add(obj);
            var fn = function (parm) {
                for (var i = 0, l = jobjs.length; i < l; i++) {
                    if (typeof parm == "string") {
                        if ($(jobjs[i]).hasClass(parm)) return jobjs[i];
                    }
                    else if (typeof parm == "object") {
                        if (jobjs[i] == parm) return jobjs[i];
                    }
                }
                return null;
            };
         
            var r = {
                Colmuntoolbar: fn("l-freegrid-bar"),
                freegrid: fn("l-form"),               
                freegridbody: fn("l-form-editor"),
                freedas: $("body")[0],
                freeselect1: fn("l-box-select"),
                freeselect2: fn("l-box-select-absolute"),
                freedata1: fn("l-box-select"),
                freedata2: fn("l-box-dateeditor-absolute")
            };
          
            if (r.freegridbody) {
                r.linput = fn("l-text");
                r.ltextarea = fn("l-textarea");
                r.lcombobox = fn("l-text-combobox");
            }

            if (r.Colmuntoolbar) {
                r.auto = fn("l-bar-colcx");
                r.first = fn("l-bar-col1");
                r.second = fn("l-bar-col2");
                r.three = fn("l-bar-col3");
            }
            return r;
        },
        getValueByDataname: function (data, name) {
            var value = "";
            for (var v in data) {
                if (v == name) {
                    value = data[v];
                }
            }
            return value;
        },
        //设置事件lbq
        _onClick: function (e) {
            var g = this, p = this.options;
            var jform = $(this.element);
            var fls = p.fields;          
           
            var obj = (e.target || e.srcElement);
            var src = g._getSrcElementByEvent(e);          
          //判断当前数据是否编辑过                      
            //var rowdata = g.getData();//获取当前表单数据           
                var el = $(this.element);               
                var focusObj = p.focusObjid;             
                if (src.freegridbody) {//控件          
                    var currentdate = g.getData();
                    p.editForm = true;
                    // p.focusObjid = $(src.freegridbody).find("input[type='hidden']").attr("id") || $(src.freegridbody).find("input").attr("id") || $(src.freegridbody).find("textarea").attr("id"); //初次赋值              
                    p.focusObjid = $(src.freegridbody).find("input[type='hidden']").attr("id") || $(src.freegridbody).find("input").attr("id") || $(src.freegridbody).find("textarea").attr("ligerid") || $(src.freegridbody).find("textarea").attr("id");//初次赋值              
                    
                    if (focusObj != p.focusObjid && focusObj != "objid") {
                        p.editForm = false;
                        var v = currentdate;
                        var value = g.getValueByDataname(v, focusObj);
                        g.endEdit(focusObj, value);//执行结束编辑事件                  
                    }
                }              
                if (!src.freegridbody && !src.freeselect1 && !src.freeselect2 && !src.freedata1 && !src.freedata2) {//外层区域   
                    p.editForm = true;
                    if (focusObj != "objid" && p.editForm) {
                       
                        var currentdate = g.getData();
                        var v = currentdate;
                    
                        var value = g.getValueByDataname(v, focusObj);
                       
                        g.endEdit(focusObj, value);//执行结束编辑事件                  
                        p.focusObjid = "objid";
                        p.editForm = false;
                    }
                }      
          
               
            //点击右侧工具栏事件
            if (src.Colmuntoolbar) {
               
                if (src.auto) {
                    g.changeColmun('auto');
                }
                else if (src.first) {
                    g.changeColmun('first');
                }
                else if (src.second) {
                    g.changeColmun('second');
                }
                else if (src.three) {
                    g.changeColmun('three');
                }
            }
        },
        //#region设置表单的初始值lbq
        //_setInitValue: function (currentdata) {
        //    var g = this, p = this.options;
        //    var el = $(this.element);
        //    var id = $(el).attr("id") || $(el).attr("ligerid");
        //    var lis = $("li[id^=" + id + "]", el);
        //    for (var cl in currentdata) {
        //        $(lis).each(function (k, v) {
        //            var node = $(this).find("input");
        //            var comleid = "";//字段名   
        //            if (node.length > 1) {
        //                comleid = $(node[1]).attr("id");
        //            }
        //            else {
        //                comleid = $(node).attr("id");
        //            }
        //            if (comleid == cl) {
        //                $(this).attr("initvalue", currentdata[cl])
        //            }
        //        });
        //    }
        //#endregion},
        //展示一条数据lbq
        _showData: function () {
            var g = this, p = this.options;
            var data = g.currentData;//获取当前条页数据
            p.page = p.newPage;
            if (!p.total) p.total = 0;
            if (!p.page) p.page = 1;
            if (!p.pageCount) p.pageCount = 1;
            //更新分页           
            g.setFormData(data);
        },
        //清空一条数据lbq
        clearData: function (data) {
            var g = this, p = this.options;
            var fields = g.get('fields');
            g.data = data || {};
            //设置禁用全局状态
            g.disableState = false;
            (function () {
                $(p.fields).each(function (fieldIndex, field) {
                    var name = field.name, textField = field.textField, editor = g.editors[fieldIndex];
                 
                    if (!editor) return; //删掉上面的 textarea 会未定义
                   
                    if (name) {
                        editor.editor.setValue(editor.control, "");                      
                        if (!editor.control[0]){
                            editor.control.setDisabled();//禁用表单控件  这个处理的是textarea  
                            $("input[ligerid='" + name + "']").attr("disabled", "disabled");//禁用textarea 
                        }                            
                        else {
                            if(name)
                                $("#" + name).addClass("l-text-disabled").attr("disabled", "disabled");//禁用textarea                     
                        }                    
                    }
                    if (textField) {                       
                        editor.editor.setText(editor.control, "");
                        editor.control.setDisabled();
                    }
                });
            }).ligerDefer(g, 20);

        },
        //禁用所有控件
        setAllControlDisable: function () {
          
            var g = this, p = this.options;
            (function () {
                $(p.fields).each(function (fieldIndex, field) {
                    var name = field.name, textField = field.textField, editor = g.editors[fieldIndex];                  
                    if (!editor) return; //删掉上面的 textarea 会未定义

                    if (name) {
                        editor.editor.setValue(editor.control, "");
                        if (!editor.control[0]) {
                            editor.control.setDisabled();//禁用表单控件  这个处理的是textarea  
                            $("input[ligerid='" + name + "']").attr("disabled", "disabled");//禁用textarea 
                        }
                        else {
                            if (name)
                                $("#" + name).addClass("l-text-disabled").attr("disabled", "disabled");//禁用textarea                     
                        }
                    }
                    if (textField) {
                        editor.editor.setText(editor.control, "");
                        editor.control.setDisabled();
                    }
                });
            }).ligerDefer(g, 20);
        },
        //格式化数据lbq-返回现有字段
        _formatRecord: function (o) {
            var g = this, p = this.options;
            var fields = g.get('fields');
            for (var cl in o) {
                var t = false;
                $(fields).each(function (fieldIndex, field) {
                    var name = field.name;
                    if (name == cl) t = true;
                });
                if (!t && cl != "__status") {
                    delete o[cl];
                }
            }
            return o;
        },
        //获取字段中最大宽度的值
        _getMaxWidthColmun: function (fields, filedsnum) {
            var g = this, p = this.options;
            var num = g._setGroupClomusFn(p.GroupClomus, p.fields);//设置num为几列换行
            var winWidth = $(this.element).width() - 55;
            var spaceWidth = "0";

            //自动宽度 控件根据列数改变
            if (p.layMode.auto) {
                if (p.GroupClomus == "auto") {
                    spaceWidth = (winWidth / num) - p.labelWidth - p.space - 30;
                  
                } else {
                    spaceWidth = (winWidth / p.GroupClomus) - p.labelWidth - p.space - 30;
                }
            } else {//固定宽度 控件不根据列数改变
                var maxwidth = 0;
                var fmwidth = $(this.element).width() - 55; //form宽度  
                //获取控件最大宽度
                $(fields).each(function (index, field) {
                    var colwidth = field.width;
                    if (colwidth) {
                        if (p.inputWidth < colwidth) {
                            maxwidth = colwidth;
                        } else {
                            maxwidth = p.inputWidth;
                        }
                    }
                });
               // console.info(maxwidth);
                spaceWidth = maxwidth;

                //spaceWidth = (maxwidth + p.labelWidth + p.space) * p.GroupClomus;
                //if (p.GroupClomus == "auto") {                  
                //    spaceWidth = (maxwidth + p.labelWidth + p.space) * num;
                //} 
            }
    
            return parseInt(spaceWidth);
        },
        //获取标签最大宽度的值
        _getMaxLableWidthColmun: function () {
            var g = this, p = this.options;
            var maxwidth = 0;
            //获取控件最大宽度
            $(p.fields).each(function (index, field) {              
                var colwidth = field.display.length;
                var colw = (colwidth + 1) * p.labelWidthEv;            
                if (maxwidth < colw) {
                    maxwidth = colw;
                }
            });          
            return maxwidth;
        },
        //获取空格间隙最大宽度的值
        _getMaxSpaceWidthColmun: function () {
            var g = this, p = this.options;
            var maxwidth = 0;
            //获取控件最大宽度space 
            $(p.fields).each(function (index, field) {
                var colwidth = $(field.unit).text().length;
                if (field.unit) {
                    var tt = field.unit.replace(/<[^>]+>/g, "") //去掉所有的html标记);
                    var colwidth = tt.length;
                    var colw = (colwidth+1) * p.labelWidthEv;
                    if (maxwidth < colw) {
                        maxwidth = colw;
                    }
                }  
            });       
            return maxwidth;
        },
     
        //自适应 可以分几列lbq
        _setGroupClomusFn: function (clomus, fields) {
            var g = this, p = this.options;
            var maxwidth = 0;
            if (clomus == "auto") {
                var fmwidth = $(this.element).width() - 55; //form宽度  

                $(fields).each(function (index, field) {
                    var colwidth = field.width;
                    if (colwidth != undefined) {
                        if (p.inputWidth < colwidth) {
                            maxwidth = colwidth;
                        } else {
                            maxwidth = p.inputWidth;
                        }
                    }
                });
                var Inwidth = maxwidth + p.labelWidth + p.space;//单个控件的宽度                  
                var n = Math.floor(fmwidth / Inwidth);
                if (n > 1) return n; else return 1;

            } else {
                return clomus;
            }
        },
        //获取当前条数据lbq
        _getCurrentPageData: function () {
            var g = this, p = this.options;
            var data = {};            
            if (!p.newPage) p.newPage = 1;
            data = p.data["Rows"][p.newPage - 1];
            return data;
        }
    });


    function getInvalidInf(errorList) {
        var out = [];
        $(errorList).each(function (i, error) {
            var label = $(error.element).parents("li:first").prev("li:first").html();
            var message = error.message;
            out.push('<div>' + label + ' ' + message + '</div>');
        });
        return out.join('');
    }

})(jQuery);