/**
* liger improve by LiKun v1.0
* 
* Author LiKun[469546858@qq.com]
* 
*/

(function ($) {
    
    //气泡,可以在制定位置显示
    $.ligerTip = function (p) {
        return $.liger.run.call(null, "ligerTip", arguments);
    };

    //在指定Dom Element右侧显示气泡
    //target：将liger对象ID附加上
    $.fn.ligerTip = function (options) {
        this.each(function () {
           var p = $.extend({}, $.ligerDefaults.ElementTip, options || {});
           p.target = p.target || this;
          //如果是自动模式：鼠标经过时显示，移开时关闭
            if (p.auto || options == undefined ||p.width!=undefined) {
                if (!p.content) {
                    p.content = this.title;
                    if (p.removeTitle)
                        $(this).removeAttr("title");
                }
                p.content = p.content || this.title;
                $(this).bind('mouseover.tip', function () {
                   p.x = $(this).offset().left + $(this).width() + (p.distanceX || 0);
                   p.y = $(this).offset().top + (p.distanceY || 0);
                   p.target.width = $(this).width();
                    $.ligerTip(p);
                }).bind('mouseout.tip', function () {

                    var tipmanager = $.liger.managers[this.ligertipid];
                    if (tipmanager) {
                        tipmanager.remove();
                    }
                });
            }
            else {
                if (p.target.ligertipid) return;
                p.x = $(this).offset().left + $(this).width() + (p.distanceX || 0);
                p.y = $(this).offset().top + (p.distanceY || 0);
                p.x = p.x || 0;
                p.y = p.y || 0;
                $.ligerTip(p);
            }
        });
        return $.liger.get(this, 'ligertipid');
    };
    //关闭指定在Dom Element(附加了liger对象ID,属性名"ligertipid")显示的气泡
    $.fn.ligerHideTip = function (options) {
        return this.each(function () {
            var p = options || {};
            if (p.isLabel == undefined) {
                //如果是lable，将查找指定的input，并找到liger对象ID
                p.isLabel = this.tagName.toLowerCase() == "label" && $(this).attr("for") != null;
            }
            var target = this;
            if (p.isLabel) {
                var forele = $("#" + $(this).attr("for"));
                if (forele.length == 0) return;
                target = forele[0];
            }
            var tipmanager = $.liger.managers[target.ligertipid];
            if (tipmanager) {
                tipmanager.remove();
            }
        }).unbind('mouseover.tip').unbind('mouseout.tip');
    };


    $.fn.ligerGetTipManager = function () {
        return $.liger.get(this);
    };


    $.ligerDefaults = $.ligerDefaults || {};


    //隐藏气泡
    $.ligerDefaults.HideTip = {};

    //气泡
    $.ligerDefaults.Tip = {
        content: null,
        callback: null,
        width: 150,
        height: null,
        x: 0,
        y: 0,
        appendIdTo: null,       //保存ID到那一个对象(jQuery)(待移除)
        target: null,
        auto: null,             //是否自动模式，如果是，那么：鼠标经过时显示，移开时关闭,并且当content为空时自动读取attr[title]
        removeTitle: true,        //自动模式时，默认是否移除掉title
        isMaskOcx:false           //是否被报表遮盖

    };

    //在指定Dom Element右侧显示气泡,通过$.fn.ligerTip调用
    $.ligerDefaults.ElementTip = {
        distanceX: 1,
        distanceY: -3,
        auto: null,
        removeTitle: true
    };

    $.ligerMethos.Tip = {};

    $.liger.controls.Tip = function (options) {
        $.liger.controls.Tip.base.constructor.call(this, null, options);
    };
    $.liger.controls.Tip.ligerExtend($.liger.core.UIComponent, {
        __getType: function () {
            return 'Tip';
        },
        __idPrev: function () {
            return 'Tip';
        },
        _extendMethods: function () {
            return $.ligerMethos.Tip;
        },
        _render: function () {
            var g = this, p = this.options;
            var tip = $('<div class="l-verify-tip"><div class="l-verify-tip-corner"></div><div class="l-verify-tip-content"></div></div>');
            g.tip = tip;
            if(p.isMaskOcx)
             g.tip.frame = $("<iframe frameborder='0'  class='l-tip-shadow' style='z-index:8900;position:absolute;filter:alpha(opacity:0);opacity:0' scrolling=no></iframe>").appendTo(g.tip);
            g.tip.content = $("> .l-verify-tip-content:first", tip);
            g.tip.attr("id", g.id);
            if (p.content) {
                $("> .l-verify-tip-content:first", tip).html(p.content);
                tip.appendTo('body');
            }
            else {
                return;
            }
            //增加对tip显示位置的处理，若tip的宽度超过document的宽度，在document的左侧显示
            var contentWidth = $(window).width();
            if (p.x + p.width > contentWidth && contentWidth > p.width && p.target.width) {
                p.x = p.x - p.width - p.target.width;
                $(">div:first", g.tip).removeClass().addClass("l-verify-tip-corner-right").css("left", p.width+16);
            }
            tip.css({ left: p.x, top: p.y }).show();
            p.width && $("> .l-verify-tip-content:first", tip).width(p.width - 8);
            p.height && $("> .l-verify-tip-content:first", tip).width(p.height);
            eee = p.appendIdTo;
            if (p.appendIdTo) {
                p.appendIdTo.attr("ligerTipId", g.id);
            }
            if (p.target) {
                $(p.target).attr("ligerTipId", g.id);
                p.target.ligertipid = g.id;
            }
            p.callback && p.callback(tip);
            
            g.set(p);
          
           p.isMaskOcx&&g.tip.frame.css({
                left: 0,
                top: 0,
                width: g.tip.content.outerWidth()+7,
                height: g.tip.content.outerHeight()
            });
        },
        _setContent: function (content) {
            $("> .l-verify-tip-content:first", this.tip).html(content);
        },
        remove: function () {
            if (this.options.appendIdTo) {
                this.options.appendIdTo.removeAttr("ligerTipId");
            }
            if (this.options.target) {
                $(this.options.target).removeAttr("ligerTipId");
                this.options.target.ligertipid = null;
            }
            this.tip.remove();
        }
    });
})(jQuery);