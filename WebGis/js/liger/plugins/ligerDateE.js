/**
* liger improve by LiKun v1.0
* 
* Author LiKun 农历 公历日期控件
* ligerDateE
*/
(function ($) {
    $.fn.ligerDateE = function () {
        return $.liger.run.call(this, "ligerDateE", arguments);
    };

    $.fn.ligerGetDateEManager = function () {
        return $.liger.run.call(this, "ligerGetDateEManager", arguments);
    };

    $.ligerDefaults.DateE = {
        format: "yyyy-MM-dd hh:mm",
        showTime: false,//显示时间（时分)
        onChangeDate: false,
        absolute: true,  //选择框是否在附加到body,并绝对定位
        cancelable: true,//可取消选择 显示X
        readonly: false, //是否只读
        colonShow: true, //默认冒号显示
        leftSpace: 0,   //默认日期左侧偏移量  
        isMaskOcx: false //默认不启用报表遮挡功能  
    };

    $.ligerDefaults.DateEString = {
        lunarInfo: [0x4bd8, 0x4ae0, 0xa570, 0x54d5, 0xd260, 0xd950, 0x5554, 0x56af, 0x9ad0, 0x55d2,
        0x4ae0, 0xa5b6, 0xa4d0, 0xd250, 0xd255, 0xb54f, 0xd6a0, 0xada2, 0x95b0, 0x4977,
        0x497f, 0xa4b0, 0xb4b5, 0x6a50, 0x6d40, 0xab54, 0x2b6f, 0x9570, 0x52f2, 0x4970,
        0x6566, 0xd4a0, 0xea50, 0x6a95, 0x5adf, 0x2b60, 0x86e3, 0x92ef, 0xc8d7, 0xc95f,
        0xd4a0, 0xd8a6, 0xb55f, 0x56a0, 0xa5b4, 0x25df, 0x92d0, 0xd2b2, 0xa950, 0xb557,
        0x6ca0, 0xb550, 0x5355, 0x4daf, 0xa5b0, 0x4573, 0x52bf, 0xa9a8, 0xe950, 0x6aa0,
        0xaea6, 0xab50, 0x4b60, 0xaae4, 0xa570, 0x5260, 0xf263, 0xd950, 0x5b57, 0x56a0,
        0x96d0, 0x4dd5, 0x4ad0, 0xa4d0, 0xd4d4, 0xd250, 0xd558, 0xb540, 0xb6a0, 0x95a6,
        0x95bf, 0x49b0, 0xa974, 0xa4b0, 0xb27a, 0x6a50, 0x6d40, 0xaf46, 0xab60, 0x9570,
        0x4af5, 0x4970, 0x64b0, 0x74a3, 0xea50, 0x6b58, 0x5ac0, 0xab60, 0x96d5, 0x92e0,
        0xc960, 0xd954, 0xd4a0, 0xda50, 0x7552, 0x56a0, 0xabb7, 0x25d0, 0x92d0, 0xcab5,
        0xa950, 0xb4a0, 0xbaa4, 0xad50, 0x55d9, 0x4ba0, 0xa5b0, 0x5176, 0x52bf, 0xa930,
        0x7954, 0x6aa0, 0xad50, 0x5b52, 0x4b60, 0xa6e6, 0xa4e0, 0xd260, 0xea65, 0xd530,
        0x5aa0, 0x76a3, 0x96d0, 0x4afb, 0x4ad0, 0xa4d0, 0xd0b6, 0xd25f, 0xd520, 0xdd45,
        0xb5a0, 0x56d0, 0x55b2, 0x49b0, 0xa577, 0xa4b0, 0xaa50, 0xb255, 0x6d2f, 0xada0,
        0x4b63, 0x937f, 0x49f8, 0x4970, 0x64b0, 0x68a6, 0xea5f, 0x6b20, 0xa6c4, 0xaaef,
        0x92e0, 0xd2e3, 0xc960, 0xd557, 0xd4a0, 0xda50, 0x5d55, 0x56a0, 0xa6d0, 0x55d4,
        0x52d0, 0xa9b8, 0xa950, 0xb4a0, 0xb6a6, 0xad50, 0x55a0, 0xaba4, 0xa5b0, 0x52b0,
        0xb273, 0x6930, 0x7337, 0x6aa0, 0xad50, 0x4b55, 0x4b6f, 0xa570, 0x54e4, 0xd260,
        0xe968, 0xd520, 0xdaa0, 0x6aa6, 0x56df, 0x4ae0, 0xa9d4, 0xa4d0, 0xd150, 0xf252,
        0xd520],
        sTermInfo: [0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758],
        solarMonth: [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31],
        sxYearMessage: ["鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪"],
        solarTermMessage: ["小寒", "大寒", "立春", "雨水", "惊蛰", "春分", "清明", "谷雨", "立夏", "小满", "芒种", "夏至", "小暑", "大暑", "立秋", "处暑", "白露", "秋分", "寒露", "霜降", "立冬", "小雪", "大雪", "冬至"],
        lFtvMessage: ["0101*春节", "0102*大年初二", "0103*大年初三", "0115  元宵节", "0202  龙抬头", "0404  寒食节", "0408  佛诞节 ", "0505*端午节", "0606  天贶节", "0624  火把节",
            "0707  情人节", "0714  鬼节", "0715  盂兰节", "0730  地藏节", "0815*中秋节", "0909  重阳节", "1001  祭祖节", "1208  腊八节 ", "1223  小年", "0100*除夕"],
        sFtvMessage: ["0101*元旦",
            "0214  情人节",
            "0308  妇女节",
            "0312  植树节",
            "0701  建党节",
            "0910  教师节",
            "1001*国庆节",

            "1031  万圣节"],
        GanMessage: ["甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸"],
        ZhiMessage: ["子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥"],
        nStr1Message: ['日', '一', '二', '三', '四', '五', '六', '七', '八', '九', '十'],
        nStr2Message: ['初', '十', '廿', '卅', ' '],
        dayMessage: ["日", "一", "二", "三", "四", "五", "六"],
        monthMessage: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
        GLdayMessage: ["初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十", "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十"],
        todayMessage: "今天",
        closeMessage: "关闭"
    };


    $.ligerMethos.DateE = {};

    $.liger.controls.DateE = function (element, options) {
        $.liger.controls.DateE.base.constructor.call(this, element, options);
    };
    $.liger.controls.DateE.ligerExtend($.liger.controls.Input, {
        __getType: function () {
            return 'DateE';
        },
        __idPrev: function () {
            return 'DateE';
        },
        _extendMethods: function () {
            return $.ligerMethos.DateE;
        },
        //渲染
        _render: function () {
            var g = this, p = this.options;
            if (!p.showTime && p.format.indexOf(" hh:mm") > -1) {
                p.format = p.format.replace(" hh:mm", "");
            }
            if (this.element.tagName.toLowerCase() != "input" || this.element.type != "text")
                return;
            g.inputText = $(this.element);
            if (!g.inputText.hasClass("l-text-field")) {
                g.inputText.addClass("l-text-field");
            }
            g.link = $('<div class="l-trigger"><div class="l-trigger-icon"></div></div>');//日期点击图标
            g.text = g.inputText.wrap('<div class="l-text l-text-date"></div>').parent();
            g.text.append('<div class="l-text-l"></div><div class="l-text-r"></div>');
            g.text.append(g.link);
            //添加个包裹，
            g.textwrapper = g.text.wrap('<div class="l-text-wrapper"></div>').parent();
            var dateeditorHTML = "";
            dateeditorHTML += "<div class='l-box-dateeditor' style='display:none'>";
            dateeditorHTML += "    <div class='l-box-dateeditor-header'>";
            dateeditorHTML += "        <div class='l-box-dateeditor-header-btn l-box-dateeditor-header-prevyear'><span></span></div>";
            dateeditorHTML += "        <div class='l-box-dateeditor-header-btn l-box-dateeditor-header-prevmonth'><span></span></div>";
            dateeditorHTML += "        <div class='l-box-dateeditor-header-text'><a class='l-box-dateeditor-header-month'></a> , <a  class='l-box-dateeditor-header-year'></a>,  <a  class='l-box-dateeditor-header-jzyear'></a>,<a  class='l-box-dateeditor-header-sxyear'></a></div>";
            dateeditorHTML += "        <div class='l-box-dateeditor-header-btn l-box-dateeditor-header-nextmonth'><span></span></div>";
            dateeditorHTML += "        <div class='l-box-dateeditor-header-btn l-box-dateeditor-header-nextyear'><span></span></div>";
            dateeditorHTML += "    </div>";
            dateeditorHTML += "    <div class='l-box-dateeditor-body'>";
            dateeditorHTML += "        <table cellpadding='0' cellspacing='0' border='0' class='l-box-dateeditor-calendar'>";
            dateeditorHTML += "            <thead>";
            dateeditorHTML += "                <tr><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td></tr>";
            dateeditorHTML += "            </thead>";
            dateeditorHTML += "            <tbody>";
            dateeditorHTML += "                <tr class='l-first'><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td></tr><tr><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td></tr><tr><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td></tr><tr><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td></tr><tr><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td></tr><tr><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td><td align='center'></td></tr>";
            dateeditorHTML += "            </tbody>";
            dateeditorHTML += "        </table>";
            dateeditorHTML += "        <ul class='l-box-dateeditor-monthselector'><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li></ul>";
            dateeditorHTML += "        <ul class='l-box-dateeditor-yearselector'><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li></ul>";
            dateeditorHTML += "        <ul class='l-box-dateeditor-hourselector'><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li></ul>";
            dateeditorHTML += "        <ul class='l-box-dateeditor-minuteselector'><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li></ul>";
            dateeditorHTML += "    </div>";
            dateeditorHTML += "    <div class='l-box-dateeditor-toolbar'>";
            dateeditorHTML += "        <div class='l-box-dateeditor-time'></div>";
            dateeditorHTML += "        <div class='l-button l-button-today l-button-today2'></div>";
            dateeditorHTML += "        <div class='l-button l-button-close'></div>";
            dateeditorHTML += "        <div class='l-clear'></div>";
            dateeditorHTML += "    </div>";
            dateeditorHTML += "</div>";
            g.dateeditor = $(dateeditorHTML);


            if (p.absolute)
                g.dateeditor.appendTo('body').addClass("l-box-dateeditor-absolute");
            else
                g.textwrapper.append(g.dateeditor);

            //加载默认报表的遮盖
            g.dateeditor.shadow = $("<iframe frameborder=0 class='l-box-dateeditor-shadow' style='position:absolute;display:none' ></iframe>");
            p.isMaskOcx && g.dateeditor.after(g.dateeditor.shadow);

            g.header = $(".l-box-dateeditor-header", g.dateeditor);
            g.body = $(".l-box-dateeditor-body", g.dateeditor);
            g.toolbar = $(".l-box-dateeditor-toolbar", g.dateeditor);

            g.body.thead = $("thead", g.body);
            g.body.tbody = $("tbody", g.body);
            g.body.monthselector = $(".l-box-dateeditor-monthselector", g.body);
            g.body.yearselector = $(".l-box-dateeditor-yearselector", g.body);
            g.body.hourselector = $(".l-box-dateeditor-hourselector", g.body);
            g.body.minuteselector = $(".l-box-dateeditor-minuteselector", g.body);

            g.toolbar.time = $(".l-box-dateeditor-time", g.toolbar);
            g.toolbar.time.hour = $("<a></a>");
            g.toolbar.time.minute = $("<a></a>");
            g.buttons = {
                btnPrevYear: $(".l-box-dateeditor-header-prevyear", g.header),
                btnNextYear: $(".l-box-dateeditor-header-nextyear", g.header),
                btnPrevMonth: $(".l-box-dateeditor-header-prevmonth", g.header),
                btnNextMonth: $(".l-box-dateeditor-header-nextmonth", g.header),
                btnYear: $(".l-box-dateeditor-header-year", g.header),
                btsxYear: $(".l-box-dateeditor-header-sxyear", g.header),//生肖年 比如马年
                btnlYear: $(".l-box-dateeditor-header-jzyear", g.header),//农历年 比如甲午年
                btnMonth: $(".l-box-dateeditor-header-month", g.header),
                btnToday: $(".l-button-today", g.toolbar),
                btnClose: $(".l-button-close", g.toolbar)
            };
            var nowDate = new Date();
            g.now = {
                year: nowDate.getFullYear(),
                month: nowDate.getMonth() + 1, //注意这里
                day: nowDate.getDay(),
                date: nowDate.getDate(),
                hour: nowDate.getHours(),
                minute: nowDate.getMinutes()
            };
            //当前的时间
            g.currentDateDE = {
                year: nowDate.getFullYear(),
                month: nowDate.getMonth() + 1,
                day: nowDate.getDay(),
                date: nowDate.getDate(),
                hour: nowDate.getHours(),
                minute: nowDate.getMinutes()
            };
            //选择的时间
            g.selectedDate = null;
            //使用的时间
            g.usedDate = null;
            //初始化数据
            //设置周日至周六
            $("td", g.body.thead).each(function (i, td) {
                $(td).html(p.dayMessage[i]);
            });
            //设置一月到十一二月
            $("li", g.body.monthselector).each(function (i, li) {
                $(li).html(p.monthMessage[i]);
            });
            //设置按钮
            g.buttons.btnToday.html(p.todayMessage);
            g.buttons.btnClose.html(p.closeMessage);
            //设置时间
            if (p.showTime) {
                g.toolbar.time.show();
                g.toolbar.time.append(g.toolbar.time.hour).append(":").append(g.toolbar.time.minute);
                $("li", g.body.hourselector).each(function (i, item) {
                    var str = i;
                    if (i < 10) str = "0" + i.toString();
                    $(this).html(str);
                });
                $("li", g.body.minuteselector).each(function (i, item) {
                    var str = i;
                    if (i < 10) str = "0" + i.toString();
                    $(this).html(str);
                });
            }
            //设置主体
            g.bulidContent();
            //初始化   
            //初始值
            if (p.initValue) {
                g.inputText.val(p.initValue);
            }
            if (g.inputText.val() != "") {
                g.onTextChange();
            }
            /**************
            **bulid evens**
            *************/
            g.dateeditor.hover(null, function (e) {
                if (g.dateeditor.is(":visible") && !g.editorToggling) {
                    //console.info(g.dateeditor.is(":visible"));
                    //g.toggleDateEditor(true); //暂时注销 取消滑动事件
                }
            });
            //toggle even
            g.link.hover(function () {
                if (p.disabled) return;
                this.className = "l-trigger-hover";
            }, function () {
                if (p.disabled) return;
                this.className = "l-trigger";
            }).mousedown(function () {
                if (p.disabled) return;
                this.className = "l-trigger-pressed";
            }).mouseup(function () {
                if (p.disabled) return;
                this.className = "l-trigger-hover";
            }).click(function () {
                if (p.disabled) return;
                g.bulidContent();
                g.toggleDateEditor(g.dateeditor.is(":visible"));
            });
            //绑定全局点击事件lbq
            $(document).bind("click.datee", function (e) {
                var tag = (e.target || e.srcElement).tagName.toLowerCase();
                if (tag == "html" || tag == "body") {
                    if (g.dateeditor.is(":visible") && !g.editorToggling) {
                        g.toggleDateEditor(true); //暂时注销 取消滑动事件
                    }
                }
            });

            //不可用属性时处理
            if (p.disabled) {
                g.inputText.attr("readonly", "readonly");
                g.text.addClass('l-text-disabled');
            }
            g.buttons.btnClose.click(function () {
                g.toggleDateEditor(true);
            });
            //日期 点击
            $("td", g.body.tbody).hover(function () {
                if ($(this).hasClass("l-box-dateeditor-today")) return;
                $(this).addClass("l-box-dateeditor-over");
            }, function () {
                $(this).removeClass("l-box-dateeditor-over");
            }).click(function () {
                $(".l-box-dateeditor-selected", g.body.tbody).removeClass("l-box-dateeditor-selected");
                if (!$(this).hasClass("l-box-dateeditor-today"))
                    $(this).addClass("l-box-dateeditor-selected");

                var jform2 = $(this);
                var content2 = jform2.find(".op-calendar-new-relative div:first").text();
                g.currentDateDE.date = parseInt(content2);
                g.currentDateDE.day = new Date(g.currentDateDE.year, g.currentDateDE.month - 1, 1).getDay();
                if ($(this).hasClass("l-box-dateeditor-out")) {
                    if ($("tr", g.body.tbody).index($(this).parent()) == 0) {
                        if (--g.currentDateDE.month == 0) {
                            g.currentDateDE.month = 12;
                            g.currentDateDE.year--;
                        }
                    } else {
                        if (++g.currentDateDE.month == 13) {
                            g.currentDateDE.month = 1;
                            g.currentDateDE.year++;
                        }
                    }
                }
                g.selectedDate = {
                    year: g.currentDateDE.year,
                    month: g.currentDateDE.month,
                    date: g.currentDateDE.date
                };
                g.showDate();
                g.editorToggling = true;
                g.dateeditor.slideToggle('fast', function () {
                    g.editorToggling = false;
                });
                p.isMaskOcx && g.dateeditor.shadow.hide();
            });

            $(".l-box-dateeditor-header-btn", g.header).hover(function () {
                $(this).addClass("l-box-dateeditor-header-btn-over");
            }, function () {
                $(this).removeClass("l-box-dateeditor-header-btn-over");
            });
            //选择年份
            g.buttons.btnYear.click(function () {
                //build year list
                if (!g.body.yearselector.is(":visible")) {
                    $("li", g.body.yearselector).each(function (i, item) {
                        var currentYear = g.currentDateDE.year + (i - 4);
                        if (currentYear == g.currentDateDE.year)
                            $(this).addClass("l-selected");
                        else
                            $(this).removeClass("l-selected");
                        $(this).html(currentYear);
                    });
                }

                g.body.yearselector.slideToggle();
            });
            g.body.yearselector.hover(function () { }, function () {
                $(this).slideUp();
            });
            $("li", g.body.yearselector).click(function () {
                g.currentDateDE.year = parseInt($(this).html());
                g.body.yearselector.slideToggle();
                g.bulidContent();
            });
            //select month
            g.buttons.btnMonth.click(function () {
                $("li", g.body.monthselector).each(function (i, item) {
                    //add selected style
                    if (g.currentDateDE.month == i + 1)
                        $(this).addClass("l-selected");
                    else
                        $(this).removeClass("l-selected");
                });
                g.body.monthselector.slideToggle();
            });
            g.body.monthselector.hover(function () { }, function () {
                $(this).slideUp("fast");
            });
            $("li", g.body.monthselector).click(function () {
                var index = $("li", g.body.monthselector).index(this);
                g.currentDateDE.month = index + 1;
                g.body.monthselector.slideToggle();
                g.bulidContent();
            });

            //选择小时
            g.toolbar.time.hour.click(function () {
                $("li", g.body.hourselector).each(function (i, item) {
                    //add selected style
                    if (g.currentDateDE.hour == i)
                        $(this).addClass("l-selected");
                    else
                        $(this).removeClass("l-selected");
                });
                g.body.hourselector.slideToggle();
            });
            g.body.hourselector.hover(function () { }, function () {
                $(this).slideUp("fast");
            });
            $("li", g.body.hourselector).click(function () {
                var index = $("li", g.body.hourselector).index(this);
                g.currentDateDE.hour = index;
                g.body.hourselector.slideToggle();
                g.bulidContent();
                g.showDate();
            });
            //选择分钟
            g.toolbar.time.minute.click(function () {
                $("li", g.body.minuteselector).each(function (i, item) {
                    //add selected style
                    if (g.currentDateDE.minute == i)
                        $(this).addClass("l-selected");
                    else
                        $(this).removeClass("l-selected");
                });
                g.body.minuteselector.slideToggle("fast", function () {
                    var index = $("li", this).index($('li.l-selected', this));
                    if (index > 29) {
                        var offSet = ($('li.l-selected', this).offset().top - $(this).offset().top);
                        $(this).animate({ scrollTop: offSet });
                    }
                });
            });
            g.body.minuteselector.hover(function () { }, function () {
                $(this).slideUp("fast");
            });
            $("li", g.body.minuteselector).click(function () {
                var index = $("li", g.body.minuteselector).index(this);
                g.currentDateDE.minute = index;
                g.body.minuteselector.slideToggle("fast");
                g.bulidContent();
                g.showDate();
            });

            //上个月
            g.buttons.btnPrevMonth.click(function () {
                if (--g.currentDateDE.month == 0) {
                    g.currentDateDE.month = 12;
                    g.currentDateDE.year--;
                }
                g.bulidContent();
            });
            //下个月
            g.buttons.btnNextMonth.click(function () {
                if (++g.currentDateDE.month == 13) {
                    g.currentDateDE.month = 1;
                    g.currentDateDE.year++;
                }
                g.bulidContent();
            });
            //上一年
            g.buttons.btnPrevYear.click(function () {
                g.currentDateDE.year--;
                g.bulidContent();
            });
            //下一年
            g.buttons.btnNextYear.click(function () {
                g.currentDateDE.year++;
                g.bulidContent();
            });
            //今天
            g.buttons.btnToday.click(function () {
                g.currentDateDE = {
                    year: g.now.year,
                    month: g.now.month,
                    day: g.now.day,
                    date: g.now.date
                };
                g.selectedDate = {
                    year: g.now.year,
                    month: g.now.month,
                    day: g.now.day,
                    date: g.now.date
                };
                g.showDate();
                g.dateeditor.slideToggle("fast");
                p.isMaskOcx && g.dateeditor.shadow.hide();
            });
            //文本框
            g.inputText.change(function () {
                g.onTextChange();
            }).blur(function () {
                g.text.removeClass("l-text-focus");
            }).focus(function () {
                g.text.addClass("l-text-focus");
            });
            g.text.hover(function () {
                g.text.addClass("l-text-over");
            }, function () {
                g.text.removeClass("l-text-over");
            });
            //LEABEL 支持
            if (p.label) {
                g.labelwrapper = g.textwrapper.wrap('<div class="l-labeltext" ></div>').parent();
                //增加去掉冒号设置
                if (p.colonShow)
                    g.colon = ":&nbsp";
                else
                    g.colon = "&nbsp";
                g.labelwrapper.prepend('<div class="l-text-label" style="float:left;display:inline;">' + p.label + g.colon + '</div>');
                g.textwrapper.css('float', 'left');
                if (!p.labelWidth) {
                    p.labelWidth = $('.l-text-label', g.labelwrapper).outerWidth();
                } else {
                    $('.l-text-label', g.labelwrapper).outerWidth(p.labelWidth);
                }
                $('.l-text-label', g.labelwrapper).width(p.labelWidth);
                $('.l-text-label', g.labelwrapper).height(g.text.height());
                g.labelwrapper.append('<br style="clear:both;" />');
                if (p.labelAlign) {
                    $('.l-text-label', g.labelwrapper).css('text-align', p.labelAlign);
                }
                g.textwrapper.css({ display: 'inline' });
                if (p.width)
                    g.labelwrapper.width(p.width + p.labelWidth + 2); //修复不可见的多余宽度问题
                else
                    g.labelwrapper.width(g.text.outerWidth() + p.labelWidth + 2);

            }
            g.set(p);

        },

        //报表遮挡
        updateShadow: function () {
            var g = this, p = this.options;
            g.dateeditor.shadow.css({
                left: g.dateeditor.css("left"),
                top: g.dateeditor.css("top"),
                width: g.dateeditor.outerWidth(),
                height: g.dateeditor.outerHeight()
            }).show();

        },
        //销毁
        destroy: function () {
            if (this.textwrapper) this.textwrapper.remove();
            if (this.dateeditor) this.dateeditor.remove();
            this.options = null;
            $.liger.remove(this);
        },
        //创建日期内容----主要
        bulidContent: function () {
            var g = this, p = this.options;


            //当前月第一天星期--当前行下标数
            var thismonthFirstDay = new Date(g.currentDateDE.year, g.currentDateDE.month - 1, 1).getDay();

            //当前月天数
            var nextMonth = g.currentDateDE.month;
            var nextYear = g.currentDateDE.year;
            if (++nextMonth == 13) {
                nextMonth = 1;
                nextYear++;
            }
            var monthDayNum = new Date(nextYear, nextMonth - 1, 0).getDate();
            //当前上个月天数
            var prevMonthDayNum = new Date(g.currentDateDE.year, g.currentDateDE.month - 1, 0).getDate();

            g.cld = g.calendarFn(g.currentDateDE.year, (g.currentDateDE.month - 1));//获取当前月份的农历信息类================
         
            g.buttons.btnMonth.html(g.currentDateDE.month + "月");//公历月 改小写
            g.buttons.btnYear.html(g.currentDateDE.year);
            g.buttons.btsxYear.html(p.monthMessage[g.cld[0].lMonth - 1]);//生肖年赋值
            g.buttons.btnlYear.html("农历" + g.cld[0].lYear + "(" + g._cyclical(g.currentDateDE.year - 1900 + 36) + "、" + p.sxYearMessage[(g.currentDateDE.year - 4) % 12] + ")年");//农历年赋值




            g.toolbar.time.hour.html(g.currentDateDE.hour);
            g.toolbar.time.minute.html(g.currentDateDE.minute);
            if (g.toolbar.time.hour.html().length == 1)
                g.toolbar.time.hour.html("0" + g.toolbar.time.hour.html());
            if (g.toolbar.time.minute.html().length == 1)
                g.toolbar.time.minute.html("0" + g.toolbar.time.minute.html());
            $("td", this.body.tbody).each(function () { this.className = "" });
            $("tr", this.body.tbody).each(function (i, tr) {

                $("td", tr).each(function (j, td) {
                    var id = i * 7 + (j - thismonthFirstDay);

                    var showDay = id + 1;

                    if (g.selectedDate && g.currentDateDE.year == g.selectedDate.year &&
                            g.currentDateDE.month == g.selectedDate.month &&
                            id + 1 == g.selectedDate.date) {
                        if (j == 0 || j == 6) {
                            $(td).addClass("l-box-dateeditor-holiday")
                        }
                        $(td).addClass("l-box-dateeditor-selected");
                        $(td).siblings().removeClass("l-box-dateeditor-selected");
                    }
                    else if (g.currentDateDE.year == g.now.year &&
                            g.currentDateDE.month == g.now.month &&
                            id + 1 == g.now.date) {
                        if (j == 0 || j == 6) {
                            $(td).addClass("l-box-dateeditor-holiday")
                        }
                        $(td).addClass("l-box-dateeditor-today");
                    }
                    else if (id < 0) {//当月之前的数据
                        showDay = prevMonthDayNum + showDay;
                        $(td).addClass("l-box-dateeditor-out")
                                .removeClass("l-box-dateeditor-selected");
                    }
                    else if (id > monthDayNum - 1) {//当月之后的数据
                        showDay = showDay - monthDayNum;
                        $(td).addClass("l-box-dateeditor-out")
                                .removeClass("l-box-dateeditor-selected");
                    }
                    else if (j == 0 || j == 6) {
                        $(td).addClass("l-box-dateeditor-holiday")
                                .removeClass("l-box-dateeditor-selected");
                    }
                    else {
                        td.className = "";
                    }
                    var glDat;
                    var htfont;

                    if ($(td).hasClass("l-box-dateeditor-out")) {
                        glDat = ""
                        htfont = "<div class='op-calendar-new-relative'><span  style='font-size:17px;' >" + showDay + "</span></div>"
                    } else {
                        glDat = g.cDay(g.cld[showDay - 1].lDay);


                        var s = g.cld[id].lunarFestival;
                        if (s.length > 0) {  //农历节日
                            if (s.length > 4) s = s.substr(0, 4);
                            s = s.fontcolor('red');
                        }
                        else {  //国历节日
                            s = g.cld[id].solarFestival;
                            if (s.length > 0) {
                                if (s.length > 4) s = s.substr(0, 4);
                                s = (s == '黑色星期五') ? s.fontcolor('black') : s.fontcolor('#0066FF');
                            }
                            else {  //廿四节气
                                s = g.cld[id].solarTerms;
                                if (s.length > 0) s = s.fontcolor('limegreen');
                            }
                        }
                        if (s.length > 0) { glDat = s; }
                        htfont = "<div class='op-calendar-new-relative'><div  style='font-size:17px;' >" + showDay + "</div><div  style='font-size:12px; color:#909090;' class='lin3'>" + glDat + "</div></div>"
                    }
                    $(td).html(htfont);//显示当前月天数在内的42个天数
                });
            });
        },
        //更新下拉框位置
        updateSelectBoxPosition: function () {
            var g = this, p = this.options;
            if (p.absolute) {
                var contentHeight = $(document).height();
                var contentWidth = $(window).width();
                if (Number(g.text.offset().top + 1 + g.text.outerHeight() + g.dateeditor.height()) > contentHeight
            			&& contentHeight > Number(g.dateeditor.height() + 1)) {
                    //若下拉框大小超过当前document下边框,且当前document上留白大于下拉内容高度,下拉内容向上展现
                    g.dateeditor.css({ left: g.text.offset().left - p.leftSpace, top: g.text.offset().top - 1 - g.dateeditor.height() });
                } else {
                    g.dateeditor.css({ left: g.text.offset().left - p.leftSpace, top: g.text.offset().top + 1 + g.text.outerHeight() });
                }
                //若下拉框宽度超过当前document的宽度,且当前document左留白大于下拉内容宽度,下拉内容向左展现
                if (Number(g.text.offset().left + 1 + g.dateeditor.width()) > contentWidth
            			&& contentWidth > Number(g.dateeditor.width() + 1)) {
                    g.dateeditor.css({ left: g.text.offset().left - p.leftSpace - (g.dateeditor.width() - g.text.outerWidth()) });
                }

            }
            else {
                if (g.text.offset().top + 4 > g.dateeditor.height() && g.text.offset().top + g.dateeditor.height() + textHeight + 4 - $(window).scrollTop() > $(window).height()) {
                    g.dateeditor.css("marginTop", -1 * (g.dateeditor.height() + textHeight + 5));
                    g.showOnTop = true;
                }
                else {
                    g.showOnTop = false;
                }
            }
        },
        //日期显示隐藏
        toggleDateEditor: function (isHide) {
            var g = this, p = this.options;
            var textHeight = g.text.height();
            g.editorToggling = true;
            if (isHide) {
                g.dateeditor.hide('fast', function () {
                    g.editorToggling = false;
                });
                p.isMaskOcx && g.dateeditor.shadow.hide();
            }
            else {

                g.updateSelectBoxPosition();
                //修复默认弹窗被报表遮盖的问题
                p.isMaskOcx && g.updateShadow();
                g.dateeditor.slideDown('fast', function () {
                    g.editorToggling = false;
                });

            }
        },
        //显示日期-赋值文本框
        showDate: function () {
            var g = this, p = this.options;
            if (!this.currentDateDE) return;
            this.currentDateDE.hour = parseInt(g.toolbar.time.hour.html(), 10);
            this.currentDateDE.minute = parseInt(g.toolbar.time.minute.html(), 10);
            var mth = this.currentDateDE.month;
            if (mth >= 1)
                mth -= 1;
            var myDate = new Date(this.currentDateDE.year, mth, this.currentDateDE.date, this.currentDateDE.hour, this.currentDateDE.minute);
            dateStr = g.getFormatDate(myDate);  //.info("myDate:" + myDate);Fri Jan 10 2014 00:00:00 GMT+0800         
            this.inputText.val(dateStr);   //.info("dateStr:" + dateStr);2014-01-10
            this.onTextChange();
        },
        isDateTime: function (dateStr) {
            var g = this, p = this.options;
            var r = dateStr.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
            if (r == null) return false;
            var d = new Date(r[1], r[3] - 1, r[4]);
            if (d == "NaN") return false;
            return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
        },
        isLongDateTime: function (dateStr) {
            var g = this, p = this.options;
            var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2})$/;
            var r = dateStr.match(reg);
            if (r == null) return false;
            var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6]);
            if (d == "NaN") return false;
            return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6]);
        },
        //格式化日期 将国际标准转化为普通日期格式
        getFormatDate: function (date) {
            var g = this, p = this.options;
            if (date == "NaN") return null;
            var format = p.format;
            var o = {
                "M+": date.getMonth() + 1,
                "d+": date.getDate(),
                "h+": date.getHours(),
                "m+": date.getMinutes(),
                "s+": date.getSeconds(),
                "q+": Math.floor((date.getMonth() + 3) / 3),
                "S": date.getMilliseconds()
            }
            if (/(y+)/.test(format)) {
                format = format.replace(RegExp.$1, (date.getFullYear() + "")
                .substr(4 - RegExp.$1.length));
            }
            for (var k in o) {
                if (new RegExp("(" + k + ")").test(format)) {
                    format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k]
                    : ("00" + o[k]).substr(("" + o[k]).length));
                }
            }
            return format;
        },
        //置空函数
        clear: function () {
            this.set('value', '');
            this.usedDate = null;
        },
        //取消选择 
        _setCancelable: function (value) {
            var g = this, p = this.options;
            if (!value && g.unselect) {
                g.unselect.remove();
                g.unselect = null;
            }
            if (!value && !g.unselect) return;
            g.unselect = $('<div class="l-trigger l-trigger-cancel"><div class="l-trigger-icon"></div></div>').hide();
            g.text.hover(function () {
                g.unselect.show();
            }, function () {
                g.unselect.hide();
            })
            if (!p.disabled && p.cancelable) {
                g.text.append(g.unselect);
            }
            g.unselect.hover(function () {
                this.className = "l-trigger-hover l-trigger-cancel";
            }, function () {
                this.className = "l-trigger l-trigger-cancel";
            }).click(function () {
                if (p.readonly) return;
                g.clear();
            });
        },
        //恢复
        _rever: function () {
            var g = this, p = this.options;
            if (!g.usedDate) {
                g.inputText.val("");
            } else {
                g.inputText.val(g.getFormatDate(g.usedDate));
            }
        },
        _getMatch: function (format) {
            var r = [-1, -1, -1, -1, -1, -1], groupIndex = 0, regStr = "^", str = format || this.options.format;
            while (true) {
                var tmp_r = str.match(/^yyyy|MM|dd|mm|hh|HH|ss|-|\/|:|\s/);
                if (tmp_r) {
                    var c = tmp_r[0].charAt(0);
                    var mathLength = tmp_r[0].length;
                    var index = 'yMdhms'.indexOf(c);
                    if (index != -1) {
                        r[index] = groupIndex + 1;
                        regStr += "(\\d{1," + mathLength + "})";
                    } else {
                        var st = c == ' ' ? '\\s' : c;
                        regStr += "(" + st + ")";
                    }
                    groupIndex++;
                    if (mathLength == str.length) {
                        regStr += "$";
                        break;
                    }
                    str = str.substring(mathLength);
                } else {
                    return null;
                }
            }
            return { reg: new RegExp(regStr), position: r };
        },
        _bulidDate: function (dateStr) {
            var g = this, p = this.options;
            var r = this._getMatch();
            if (!r) return null;
            var t = dateStr.match(r.reg);
            if (!t) return null;
            var tt = {
                y: r.position[0] == -1 ? 1900 : t[r.position[0]],
                M: r.position[1] == -1 ? 0 : parseInt(t[r.position[1]], 10) - 1,
                d: r.position[2] == -1 ? 1 : parseInt(t[r.position[2]], 10),
                h: r.position[3] == -1 ? 0 : parseInt(t[r.position[3]], 10),
                m: r.position[4] == -1 ? 0 : parseInt(t[r.position[4]], 10),
                s: r.position[5] == -1 ? 0 : parseInt(t[r.position[5]], 10)
            };
            if (tt.M < 0 || tt.M > 11 || tt.d < 0 || tt.d > 31) return null;
            var d = new Date(tt.y, tt.M, tt.d);
            if (p.showTime) {
                if (tt.m < 0 || tt.m > 59 || tt.h < 0 || tt.h > 23 || tt.s < 0 || tt.s > 59) return null;
                d.setHours(tt.h);
                d.setMinutes(tt.m);
                d.setSeconds(tt.s);
            }
            return d;
        },
        updateStyle: function () {
            this.onTextChange();
        },
        onTextChange: function () {
            var g = this, p = this.options;
            var val = g.inputText.val();
            if (!val) {
                g.selectedDate = null;
                return true;
            }
            var newDate = g._bulidDate(val);
            if (!newDate) {
                g._rever();
                return;
            }
            else {
                g.usedDate = newDate;
            }
            g.selectedDate = {
                year: g.usedDate.getFullYear(),
                month: g.usedDate.getMonth() + 1, //注意这里
                day: g.usedDate.getDay(),
                date: g.usedDate.getDate(),
                hour: g.usedDate.getHours(),
                minute: g.usedDate.getMinutes()
            };
            g.currentDateDE = {
                year: g.usedDate.getFullYear(),
                month: g.usedDate.getMonth() + 1, //注意这里
                day: g.usedDate.getDay(),
                date: g.usedDate.getDate(),
                hour: g.usedDate.getHours(),
                minute: g.usedDate.getMinutes()
            };
            var formatVal = g.getFormatDate(newDate);
            g.inputText.val(formatVal);
            g.trigger('changeDate', [formatVal]);
            if ($(g.dateeditor).is(":visible"))
                g.bulidContent();
        },
        _setHeight: function (value) {
            var g = this;
            if (value > 4) {
                g.text.css({ height: value });
                g.inputText.css({ height: value });
                g.textwrapper.css({ height: value });
            }
        },
        _setWidth: function (value) {
            var g = this;
            if (value > 20) {
                g.text.css({ width: value });
                g.inputText.css({ width: value - 20 });
                g.textwrapper.css({ width: value });
            }
        },
        _setValue: function (value) {
            var g = this;
            if (!value) g.inputText.val('');
            if (typeof value == "string") {
                if (/^\/Date/.test(value)) {
                    value = value.replace(/^\//, "new ").replace(/\/$/, "");
                    eval("value = " + value);
                }
                else {
                    g.inputText.val(value);
                    g.onTextChange();//新增
                }
            }
            if (typeof value == "object") {
                if (value instanceof Date) {
                    g.inputText.val(g.getFormatDate(value));
                    g.onTextChange();
                }
            }
        },
        _getValue: function () {
            return this.usedDate;
        },
        //启用控件
        setEnabled: function () {
            var g = this, p = this.options;
            this.inputText.removeAttr("readonly");
            this.text.removeClass('l-text-disabled');
            p.disabled = false;
        },
        //禁用控件
        setDisabled: function () {
            var g = this, p = this.options;
            this.inputText.attr("readonly", "readonly");
            this.text.addClass('l-text-disabled');
            p.disabled = true;
        },
        //#region 获取农历年月天信息      
        //获取农历年
        _cyclical: function (num) {
            var g = this, p = this.options;
            return (p.GanMessage[num % 10] + p.ZhiMessage[num % 12]);
        },
        //返回农历月(y年闰哪个月 1-12 没闰返回0)
        _leapMonth: function (y) {
            var g = this, p = this.options;
            var lm = p.lunarInfo[y - 1900] & 0xf;
            return (lm == 0xf ? 0 : lm);
        },
        //返回农历天（y年闰月的天数）
        _leapDays: function (y) {
            var g = this, p = this.options;
            if (g._leapMonth(y)) return ((p.lunarInfo[y - 1899] & 0xf) == 0xf ? 30 : 29);
            else return (0);
        },
        //返回农历 y年的总天数
        _lYearDays: function (y) {
            var g = this, p = this.options;
            var i, sum = 348;
            for (i = 0x8000; i > 0x8; i >>= 1) sum += (p.lunarInfo[y - 1900] & i) ? 1 : 0;
            return (sum + g._leapDays(y));
        },
        //返回农历 y年m月的总天数
        _monthDays: function (y, m) {
            var g = this, p = this.options;
            return ((p.lunarInfo[y - 1900] & (0x10000 >> m)) ? 30 : 29);
        },
        //#endregion

        //算出农历, 传入日期控件, 返回农历日期控件 该控件属性有 .year .month .day .isLeap
        Lunar: function (objDate) {
            var LunarDa = {
                Lunaryear: "",
                LunarisLeap: "",
                Lunarmonth: "",
                Lunarday: ""
            }
            var g = this, p = this.options;
            var i, leap = 0, temp = 0;
            var offset = (Date.UTC(objDate.getFullYear(), objDate.getMonth(), objDate.getDate()) - Date.UTC(1900, 0, 31)) / 86400000;

            for (i = 1900; i < 2100 && offset > 0; i++) {
                temp = g._lYearDays(i);
                offset -= temp;
            }
            if (offset < 0) {
                offset += temp;
                i--;
            }
            LunarDa.Lunaryear = i;
            leap = g._leapMonth(i); //闰哪个月
            LunarDa.LunarisLeap = false;
            for (i = 1; i < 13 && offset > 0; i++) {
                //闰月
                if (leap > 0 && i == (leap + 1) && LunarDa.LunarisLeap == false) {
                    --i;
                    LunarDa.LunarisLeap = true;
                    temp = g._leapDays(LunarDa.Lunaryear);
                }
                else {
                    temp = g._monthDays(LunarDa.Lunaryear, i);
                }
                //解除闰月
                if (LunarDa.LunarisLeap == true && i == (leap + 1)) LunarDa.LunarisLeap = false;
                offset -= temp;
            }
            if (offset == 0 && leap > 0 && i == leap + 1)
                if (LunarDa.LunarisLeap) {
                    LunarDa.LunarisLeap = false;
                }
                else {
                    LunarDa.LunarisLeap = true;
                    --i;
                }
            if (offset < 0) {
                offset += temp;
                --i;
            }
            LunarDa.Lunarmonth = i;
            LunarDa.Lunarday = offset + 1;
            return LunarDa;
        },
        //获取农历天
        cDay: function (d) {
            var g = this, p = this.options;
            var s;
            switch (d) {
                case 10:
                    s = '初十'; break;
                case 20:
                    s = '二十'; break;
                    break;
                case 30:
                    s = '三十'; break;
                    break;
                default:
                    s = p.nStr2Message[Math.floor(d / 10)];
                    s += p.nStr1Message[d % 10];
            }
            return (s);
        },
        //获取公历某月天数
        _solarDays: function solarDays(y, m) {
            var g = this, p = this.options;
            if (m == 1)
                return (((y % 4 == 0) && (y % 100 != 0) || (y % 400 == 0)) ? 29 : 28);
            else
                return (p.solarMonth[m]);
        },
        //返回阴历 (y年,m+1月)----作废
        cyclical6: function (num, num2) {
            if (num == 0) return (jcName0[num2]);
            if (num == 1) return (jcName1[num2]);
            if (num == 2) return (jcName2[num2]);
            if (num == 3) return (jcName3[num2]);
            if (num == 4) return (jcName4[num2]);
            if (num == 5) return (jcName5[num2]);
            if (num == 6) return (jcName6[num2]);
            if (num == 7) return (jcName7[num2]);
            if (num == 8) return (jcName8[num2]);
            if (num == 9) return (jcName9[num2]);
            if (num == 10) return (jcName10[num2]);
            if (num == 11) return (jcName11[num2]);
        },
        // 某年的第n个节气为几日(从0小寒起算)
        sTerm: function (y, n) {
            var g = this, p = this.options;
            var offDate = new Date((31556925974.7 * (y - 1900) + p.sTermInfo[n] * 60000) + Date.UTC(1900, 0, 6, 2, 5));
            return (offDate.getUTCDate());
        },
        //阴历属性--返回属性对象
        calElement: function (sYear, sMonth, sDay, week, lYear, lMonth, lDay, isLeap, cYear, cMonth, cDay) {
            var g = this, p = this.options;
            var calElementArray = {
                isToday: false,  //瓣句          
                sYear: sYear,   //公元年4位数字
                sMonth: sMonth, //公元月数字
                sDay: sDay,    //公元日数字
                week: week,   //星期, 1个中文
                //农历
                lYear: lYear,  //公元年4位数字
                lMonth: lMonth,  //农历月数字
                lDay: lDay,    //农历日数字
                isLeap: isLeap,  //是否为农历闰月?
                //八字
                cYear: cYear,   //年柱, 2个中文
                cMonth: cMonth,  //月柱, 2个中文
                cDay: cDay,    //日柱, 2个中文
                color: '',
                lunarFestival: '', //农历节日
                solarFestival: '', //公历节日
                solarTerms: ''//节气
            }
            return calElementArray;
        },
        //返回日历对象
        calendarFn: function (y, m) {
            var g = this, p = this.options;
            //获取当前时间
            var Today = new Date();
            var tY = Today.getFullYear();
            var tM = Today.getMonth();
            var tD = Today.getDate();
            //debugger;
            var sDObj, lY, lM, lD = 1, lL, lX = 0, tmp1, tmp2, lM2, lY2, lD2, tmp3, dayglus, bsg, xs, xs1, fs, fs1, cs, cs1
            var cY, cM, cD; //年柱,月柱,日柱
            var lDPOS = new Array(3);
            var n = 0;
            var firstLM = 0;

            sDObj = new Date(y, m, 1, 0, 0, 0, 0);    //当月一日日期
            g.calendar = { "firstWeek": "", "length": "" };

            g.calendar.firstWeek = sDObj.getDay();//公历当月1日星期几
            g.calendar.length = g._solarDays(y, m);//公历当月1日星期几
            //.info("g.calendar.length:" + g.calendar.length);
            //年柱 1900年立春后为庚子年(60进制36)
            if (m < 2) cY = g._cyclical(y - 1900 + 36 - 1);
            else cY = g._cyclical(y - 1900 + 36);
            var term2 = g.sTerm(y, 2); //立春日期

            //月柱 1900年1月小寒以前为 丙子月(60进制12)
            var firstNode = g.sTerm(y, m * 2) //返回当月「节」为几日开始
            cM = g._cyclical((y - 1900) * 12 + m + 12);

            lM2 = (y - 1900) * 12 + m + 12;
            //当月一日与 1900/1/1 相差天数
            //1900/1/1与 1970/1/1 相差25567日, 1900/1/1 日柱为甲戌日(60进制10)
            var dayCyclical = Date.UTC(y, m, 1, 0, 0, 0, 0) / 86400000 + 25567 + 10;

            for (var i = 0; i < g.calendar.length; i++) {
              
                if (lD > lX) {
                    sDObj = new Date(y, m, i + 1);    //当月一日日期

                    var lDObj = g.Lunar(sDObj);     //获取农历信息

                    lY = lDObj.Lunaryear;           //农历年
                    lM = lDObj.Lunarmonth;          //农历月
                    lD = lDObj.Lunarday;            //农历日
                    lL = lDObj.LunarisLeap;         //农历是否闰月
                    lX = lL ? g._leapDays(lY) : g._monthDays(lY, lM); //农历当月最后一天


                    if (n == 0) { firstLM = lM };
                    lDPOS[n++] = i - lD + 1;
                }
               
                //依节气调整二月分的年柱, 以立春为界
                if (m == 1 && (i + 1) == term2) {
                    cY = g._cyclical(y - 1900 + 36);
                    lY2 = (y - 1900 + 36);
                }
                //依节气月柱, 以「节」为界
                if ((i + 1) == firstNode) {
                    cM = g._cyclical((y - 1900) * 12 + m + 13);
                    lM2 = (y - 1900) * 12 + m + 13;
                }
                //日柱
                cD = g._cyclical(dayCyclical + i);
                lD2 = (dayCyclical + i);
                g.calendar[i] = g.calElement(y, m + 1, i + 1, p.nStr1Message[(i + g.calendar.firstWeek) % 7], lY, lM, lD++, lL, cY, cM, cD);
            }
            //节气
            tmp1 = g.sTerm(y, m * 2) - 1;
            tmp2 = g.sTerm(y, m * 2 + 1) - 1;

            g.calendar[tmp1].solarTerms = p.solarTermMessage[m * 2];
            g.calendar[tmp2].solarTerms = p.solarTermMessage[m * 2 + 1];
            //if (m == 3) calendar[tmp1].color = 'red'; //清明颜色
            //国历节日
            for (i in p.sFtvMessage) {
                if (p.sFtvMessage[i].match(/^(\d{2})(\d{2})([\s\*])(.+)$/)) {
                    if (Number(RegExp.$1) == (m + 1)) {
                        g.calendar[Number(RegExp.$2) - 1].solarFestival += RegExp.$4 + '  '
                        // if (RegExp.$3 == '*') g.calendar[Number(RegExp.$2) - 1].color = 'red'
                    }
                }
            } 

            //农历节日
            for (i in p.lFtvMessage) {
                
                if (p.lFtvMessage[i].match(/^(\d{2})(.{2})([\s\*])(.+)$/)) {
                    tmp1 = Number(RegExp.$1) - firstLM
                    //console.info("tmp1:" + tmp1);
                    if (tmp1 == -11) { tmp1 = 1 }
                    if (tmp1 >= 0 && tmp1 < n) {
                        tmp2 = lDPOS[tmp1] + Number(RegExp.$2) - 1
                        if (tmp2 >= 0 && tmp2 < g.calendar.length) {
                            g.calendar[tmp2].lunarFestival += RegExp.$4 + '  '
                            if (RegExp.$3 == '*') g.calendar[tmp2].color = 'red'
                        }
                    }
                }
            }

            //今日
            if (y == tY && m == tM) {
                g.calendar[tD - 1].isToday = true;
            }

            return g.calendar;
        },
        getDate: function (strDate) {
            var date = eval('new Date(' + strDate.replace(/\d+(?=-[^-]+$)/,
             function (a) { return parseInt(a, 10) - 1; }).match(/\d+/g) + ')');
            return date;
        },
        //传递一个日期 输出农历节日
        getFtvMessage: function (data) {
           
            var g = this, p = this.options;
            var sDObj2 = g.getDate(data);
            var nlob = g.Lunar(sDObj2);     //获取农历信息
         
            var nlmonth = nlob.Lunarmonth < 10 ? "0" + nlob.Lunarmonth : nlob.Lunarmonth;//农历月
            var nlday = nlob.Lunarday < 10 ? "0" + nlob.Lunarday : nlob.Lunarday;//农历日
            var my2 = nlmonth + nlday;
            var FtvStr = "";
            for (i in p.lFtvMessage) {
                if (p.lFtvMessage[i].match(/^(\d{2})(.{2})([\s\*])(.+)$/)) {
                    var tmp11 = Number(RegExp.$1) - nlob.Lunarmonth;                  
                    var my = "" + RegExp.$1 + RegExp.$2;                   
                    var my2 = nlmonth + nlday;                           
                    if (my == my2) {                       
                        FtvStr = RegExp.$4;
                    }                 
                }
            }
            if (FtvStr == "") {
               
                var ftvm = sDObj2.getMonth() + 1 < 10 ? "" + sDObj2.getMonth() + 1 : sDObj2.getMonth() + 1;
               
                var ftvd = sDObj2.getDate() < 10 ? "0" + sDObj2.getDate() : sDObj2.getDate();
                my2 = ftvm + ftvd;
                for (i in p.sFtvMessage) {
                    if (p.sFtvMessage[i].match(/^(\d{2})(\d{2})([\s\*])(.+)$/)) {
                       
                        var my = "" + RegExp.$1 + RegExp.$2;
                      
                      
                        if (my == my2) {
                            FtvStr = RegExp.$4;
                        }
                    }
                }
            }
            return FtvStr;
        }
    });


})(jQuery);