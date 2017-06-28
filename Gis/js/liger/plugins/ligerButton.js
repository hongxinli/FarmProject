/**
* liger improve by LiKun v1.0
* 
* Author LiKun[469546858@qq.com]
* 
*/
(function ($) {

    $.fn.ligerButton = function (options) {
        return $.liger.run.call(this, "ligerButton", arguments);
    };
    $.fn.ligerGetButtonManager = function () {
        return $.liger.run.call(this, "ligerGetButtonManager", arguments);
    };

    $.ligerDefaults.Button = {
        width: 60,
        text: 'Button',
        disabled: false,
        click: null,
        icon: null
    };

    $.ligerMethos.Button = {};

    $.liger.controls.Button = function (element, options) {
        $.liger.controls.Button.base.constructor.call(this, element, options);
    };
    $.liger.controls.Button.ligerExtend($.liger.controls.Input, {
        __getType: function () {
            return 'Button';
        },
        __idPrev: function () {
            return 'Button';
        },
        _extendMethods: function () {
            return $.ligerMethos.Button;
        },
        _render: function () {
            var g = this, p = this.options;
            g.button = $(g.element);
            var otherDiv = $("<div></div>");
            if ($(g.button)[0].tagName == "INPUT")
            {
                otherDiv.attr("id", $(g.button).attr("id"));
                p.text = $(g.button).val() ? $(g.button).val() : p.text;
                g.button.after(otherDiv).remove();
                g.button = otherDiv;
            }
            g.button.addClass("l-button");
            g.button.append('<div class="l-button-l"></div><div class="l-button-r"></div><span></span>');
            g.button.hover(function () {
                if (p.disabled) return;
                g.button.addClass("l-button-over");
            }, function () {
                if (p.disabled) return;
                g.button.removeClass("l-button-over");
            });
            p.click && g.button.click(function () {
                if (!p.disabled)
                    p.click();
            });
          g.set(p);
        },
        _setIcon: function (url) {
            var g = this;
            if (!url) {
                g.button.removeClass("l-button-hasicon");
                g.button.find('img').remove();
            } else {
                g.button.addClass("l-button-hasicon");
                g.button.append('<img src="' + url + '" />');
            }
        },
        _setEnabled: function (value) {
            if (value)
                this.button.removeClass("l-button-disabled");
        },
        _setDisabled: function (value) {
            if (value) {
                this.button.addClass("l-button-disabled");
                this.options.disabled = true;
            } else {
                this.button.removeClass("l-button-disabled");
                this.options.disabled = false;
            }
        },
        _setWidth: function (value) {
            this.button.width(value);
        },
        _setText: function (value) {
            $("span", this.button).html(value);
        },
        setValue: function (value) {
            this.set('text', value);
        },
        getValue: function () {
            return this.options.text;
        },
        setEnabled: function () {
            this.set('disabled', false);
        },
        setDisabled: function () {
            this.set('disabled', true);
        }
    });


})(jQuery);