/**
* liger improve by LiKun v1.0
* 
* Author LiKun[469546858@qq.com]
* 
*/
(function ($) {
    $.ligerMenu = function (options) {
        return $.liger.run.call(null, "ligerMenu", arguments);
    };

    $.ligerDefaults.Menu = {
        width: 120,
        top: 0,
        left: 0,
        items: null,
        click: null,
        shadow: true,
        onHoverLeftClick:null,
        iconPath: null,
        iconCss:null
    };

    $.ligerMethos.Menu = {};

    $.liger.controls.Menu = function (options) {

        $.liger.controls.Menu.base.constructor.call(this, null, options);

    };
    $.liger.controls.Menu.ligerExtend($.liger.core.UIComponent, {
        __getType: function () {
            return 'Menu';
        },
        __idPrev: function () {
            return 'Menu';
        },
        _extendMethods: function () {
            return $.ligerMethos.Menu;
        },
        _render: function () {
            
            var g = this, p = this.options;
            g.menuItemCount = 0;
            //全部菜单
            g.menus = {};
            //顶级菜单
            g.menu = g.createMenu();
            g.element = g.menu[0];
            g.menu.css({ top: p.top, left: p.left, width: p.width });
            p.items && $(p.items).each(function (i, item) {
                g.addItem(item);
            });

            $(document).bind('click.menu', function () {

                for (var menuid in g.menus) {
                    var menu = g.menus[menuid];
                    if (!menu) return;
                    menu.hide();
                    if (menu.shadow) menu.shadow.hide();
                }
            });
            //增加点击空白右键菜单隐藏
            var iframe = $("iframe");
            if (iframe != null) {
                if (iframe[0].attachEvent) {
                    iframe[0].attachEvent("onload", function () {
                        $(iframe[0]).contents().find("body").bind("click", function () { $(".l-menu").hide(); $(".l-menu-shadow").hide(); });
                    });
                } else {
                    iframe[0].onload = function () {
                        $(iframe[0]).contents().find("body").bind("click", function () { $(".l-menu").hide(); $(".l-menu-shadow").hide(); });
                    };
                }
            }
            g.set(p);


        },
        show: function (options, menu) {
            var g = this, p = this.options;
            if (menu == undefined) menu = g.menu;
            if (options && options.left != undefined) {
                menu.css({ left: options.left });
            }
            if (options && options.top != undefined) {
                menu.css({ top: options.top });
            }
            menu.show();
            g.updateShadow(menu);

        },
        updateShadow: function (menu) {
            var g = this, p = this.options;
            if (!p.shadow) return;
            menu.shadow.css({
                left: menu.css('left'),
                top: menu.css('top'),
                width: menu.outerWidth(),
                height: menu.outerHeight()
            });
            if (menu.is(":visible"))
                menu.shadow.show();
            else
                menu.shadow.hide();
        },
        hide: function (menu) {
            var g = this, p = this.options;
            if (menu == undefined) menu = g.menu;
            g.hideAllSubMenu(menu);
            menu.hide();
            g.updateShadow(menu);
        },
        toggle: function () {
            var g = this, p = this.options;
            g.menu.toggle();
            g.updateShadow(g.menu);
        },
        removeItem: function (itemid) {
            var g = this, p = this.options;
            $("> .l-menu-item[menuitemid=" + itemid + "]", g.menu.items).remove();
        },
        setEnabled: function (itemid) {
            var g = this, p = this.options;
            $("> .l-menu-item[menuitemid=" + itemid + "]", g.menu.items).removeClass("l-menu-item-disable");
        },
        setDisabled: function (itemid) {
            var g = this, p = this.options;
            $("> .l-menu-item[menuitemid=" + itemid + "]", g.menu.items).addClass("l-menu-item-disable");
        },
        isEnable: function (itemid) {
            var g = this, p = this.options;
            return !$("> .l-menu-item[menuitemid=" + itemid + "]", g.menu.items).hasClass("l-menu-item-disable");
        },
        getItemCount: function () {
            var g = this, p = this.options;
            return $("> .l-menu-item", g.menu.items).length;
        },
        addItem: function (item, menu) {

            var g = this, p = this.options;
            if (!item) return;
            if (menu == undefined) menu = g.menu;

            if (item.line) {
                menu.items.append('<div class="l-menu-item-line"></div>');
                return;
            }
            var ditem = $('<div class="l-menu-item" ><div class="l-menu-item-text"></div> </div>');
            var itemcount = $("> .l-menu-item", menu.items).length;
            menu.items.append(ditem);
            ditem.attr("ligermenutemid", ++g.menuItemCount);
            item.id && ditem.attr("menuitemid", item.id);
            item.text && $(">.l-menu-item-text:first", ditem).html(item.text);
            item.icon && ditem.prepend('<div class="l-menu-item-icon l-icon-' + item.icon + '"></div>');
            item.iconCss && ditem.prepend('<div class="' + item.iconCss + '"></div>');
            if (item.disable || item.disabled)
                ditem.addClass("l-menu-item-disable");
            if (item.children) {
                ditem.append('<div class="l-menu-item-arrow"></div>');
                var newmenu = g.createMenu(ditem.attr("ligermenutemid"));
                g.menus[ditem.attr("ligermenutemid")] = newmenu;
                newmenu.width(p.width);
                newmenu.hover(null, function () {
                    if (!newmenu.showedSubMenu)
                        g.hide(newmenu);
                });
                $(item.children).each(function () {
                    g.addItem(this, newmenu);
                });
            }

            (item.click || !item.click) && ditem.click(function () {

                if ($(this).hasClass("l-menu-item-disable")) return;
                if (typeof item.click == "function")
                    item.click(item, itemcount);
                if (typeof item.click == "string") {
                    try {
                        eval(item.click)(item, itemcount);
                    } catch (ex) {
                        alert(item.click + "未定义");
                    }
                }

                if (typeof item.click == "undefined")
                    g.trigger('click', { item: item, itemcount: itemcount });

            });
            item.dblclick && ditem.dblclick(function () {
                if ($(this).hasClass("l-menu-item-disable")) return;
                item.dblclick(item, itemcount);
            });

            var menuover = $("> .l-menu-over:first", menu);

            ditem.hover(function () {

                if ($(this).hasClass("l-menu-item-disable")) return;

                var itemtop = $(this).offset().top;
                var top = itemtop - menu.offset().top;
                var curobj = $(this);
                menuover.css({ top: top });
                //新增鼠标滑动图标显示功能
                if (g.hasBind("HoverLeftClick")) {
                    menuover.oleft = $("> .l-menu-over-l:first", menuover);
                    menuover.oleft.unbind();
                    menuover.oleft.icon = $('<img src="' + p.iconPath + '" style="padding-left:5px" />');
                    menuover.oleft.html(menuover.oleft.icon).css("cursor", "pointer");
                    menuover.oleft.bind("click", function () {
                       g.trigger("HoverLeftClick", { text:item.text, id: item.id });
                    });

                }

                g.hideAllSubMenu(menu);
                if (item.children) {
                    var ligermenutemid = $(this).attr("ligermenutemid");
                    if (!ligermenutemid) return;
                    if (g.menus[ligermenutemid]) {
                        g.show({ top: itemtop, left: $(this).offset().left + $(this).width() - 5 }, g.menus[ligermenutemid]);
                        menu.showedSubMenu = true;
                    }
                }
            }, function () {

                if ($(this).hasClass("l-menu-item-disable")) return;
                var ligermenutemid = $(this).attr("ligermenutemid");
                if (item.children) {
                    var ligermenutemid = $(this).attr("ligermenutemid");
                    if (!ligermenutemid) return;
                };
            });
        },
        hideAllSubMenu: function (menu) {

            var g = this, p = this.options;
            if (menu == undefined) menu = g.menu;
            $("> .l-menu-item", menu.items).each(function () {
                if ($("> .l-menu-item-arrow", this).length > 0) {
                    var ligermenutemid = $(this).attr("ligermenutemid");
                    if (!ligermenutemid) return;
                    g.menus[ligermenutemid] && g.hide(g.menus[ligermenutemid]);
                }
            });
            menu.showedSubMenu = false;
        },
        createMenu: function (parentMenuItemID) {

            var g = this, p = this.options;
            var menu = $('<div  class="l-menu" style="display:none"><div class="l-menu-yline"></div><div class="l-menu-over"><div class="l-menu-over-l"></div> <div class="l-menu-over-r"></div></div><div class="l-menu-inner"></div></div>');
            parentMenuItemID && menu.attr("ligerparentmenuitemid", parentMenuItemID);
            menu.items = $("> .l-menu-inner:first", menu);
            menu.appendTo('body');

            if (p.shadow) {
                menu.shadow = $('<iframe class="l-menu-shadow" frameborder=0></iframe>').insertAfter(menu);
                g.updateShadow(menu);
            }
            menu.hover(null, function () {

                if (!menu.showedSubMenu)
                    $("> .l-menu-over:first", menu).css({ top: -24 });
            });
            if (parentMenuItemID)
                g.menus[parentMenuItemID] = menu;
            else
                g.menus[0] = menu;
            return menu;
        }
    });
    //旧写法保留
    $.liger.controls.Menu.prototype.setEnable = $.liger.controls.Menu.prototype.setEnabled;
    $.liger.controls.Menu.prototype.setDisable = $.liger.controls.Menu.prototype.setDisabled;


})(jQuery);