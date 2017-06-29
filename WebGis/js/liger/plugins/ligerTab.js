/**
* liger improve by LiKun v1.0
* 
* Author LiKun[469546858@qq.com]
* 
*/
(function ($) {

    $.fn.ligerTab = function (options) {
        return $.liger.run.call(this, "ligerTab", arguments);
    };

    $.fn.ligerGetTabManager = function () {

        return $.liger.run.call(this, "ligerGetTabManager", arguments);
    };

    $.ligerDefaults.Tab = {
        height: null,
        heightDiff: 0, // 高度补差 
        marginleft: 3,
        changeHeightOnResize: false,
        contextmenu: true,
        dblClickToClose: true, //是否双击时关闭
        dragToMove: false,    //是否允许拖动时改变tab项的位置
        onBeforeOverrideTabItem: null,
        onAfterOverrideTabItem: null,
        onBeforeRemoveTabItem: null,
        onAfterRemoveTabItem: null,
        onBeforeAddTabItem: null,
        onAfterAddTabItem: null,
        onBeforeSelectTabItem: null,
        onAfterSelectTabItem: null,
        onAfterDragTabItem: null,
        tools: null,
        tabListItem: null,//tablist扩展自定义选项
        isOnlyOneTabToClose:false//是否开启只有一个tab的时候不打开
    };
    $.ligerDefaults.TabString = {
        closeMessage: "关闭",
        closeOtherMessage: "关闭其他",
        closeAllMessage: "关闭全部",
        reloadMessage: "刷新",
        exitMessage: "退出",
        closeLeftMessage: "关闭左侧",
        closeRightMessage: "关闭右侧"
    };

    $.ligerMethos.Tab = {};

    $.liger.controls.Tab = function (element, options) {

        $.liger.controls.Tab.base.constructor.call(this, element, options);

    };
    $.liger.controls.Tab.ligerExtend($.liger.core.UIComponent, {
        __getType: function () {
            return 'Tab';
        },
        __idPrev: function () {
            return 'Tab';
        },
        _extendMethods: function () {
            return $.ligerMethos.Tab;
        },
        _render: function () {
            var g = this, p = this.options;
            if (p.height) g.makeFullHeight = true;
            g.tab = $(this.element);
            g.tab.addClass("l-tab");
            if (p.contextmenu && $.ligerMenu) {
                g.tab.menu = $.ligerMenu({ width: 120, items: [
                    { text: p.reloadMessage, id: 'reload',iconCss:'l-menu-reload', click: function () {

                        g._menuItemClick.apply(g, arguments);
                    }
                    },
                    { line: true },
                    { text: p.closeMessage, id: 'close',iconCss:'l-menu-close', click: function () {
                        g._menuItemClick.apply(g, arguments);
                    }
                    },
                    
                    { text: p.closeOtherMessage, id: 'closeother', click: function () {
                        g._menuItemClick.apply(g, arguments);
                    }
                    },
                    {
                        text: p.closeAllMessage, id: 'closeall', click: function () {
                            g._menuItemClick.apply(g, arguments);
                        }
                    },
                    { line: true },
                    {
                        text: p.closeLeftMessage, id: 'closeleft', iconCss: 'l-menu-left', click: function () {
                            g._menuItemClick.apply(g, arguments);
                        }
                    },
                     {
                         text: p.closeRightMessage, id: 'closeright', iconCss: 'l-menu-right', click: function () {
                             g._menuItemClick.apply(g, arguments);
                         }
                     },
                    { line: true },
                    {
                        text: p.exitMessage, id: 'exit', iconCss: 'l-menu-exit', click: function () {
                            g._menuItemClick.apply(g, arguments);
                        }
                    }

                ]

                });
                

            }
            g.tab.content = $('<div  class="l-tab-content"></div>');
            $("> div", g.tab).appendTo(g.tab.content);
            g.tab.content.appendTo(g.tab);
            g.tab.links = $('<div class="l-tab-links"><ul style="left: 0px; "></ul></div>');
            g.tab.links.prependTo(g.tab);


            g.tab.links.ul = $("ul", g.tab.links);
            var lselecteds = $("> div[lselected=true]", g.tab.content);
            var haslselected = lselecteds.length > 0;
            g.selectedTabId = lselecteds.attr("tabid");
            $("> div", g.tab.content).each(function (i, box) {
                var li = $('<li class=""><a></a><div class="l-tab-links-item-left"></div><div class="l-tab-links-item-right"></div></li>');
                var contentitem = $(this);
                if (contentitem.attr("title")) {
                    $("> a", li).html(contentitem.attr("title"));
                    contentitem.attr("title", "");
                }
                if ($(box).attr("iconcss")) {
                    $("> a", li).addClass($(box).attr("iconcss"));
                }
                var tabid = contentitem.attr("tabid");
                if (tabid == undefined) {
                    tabid = g.getNewTabid();
                    contentitem.attr("tabid", tabid);
                    if (contentitem.attr("lselected")) {
                        g.selectedTabId = tabid;
                    }
                }
                
                li.attr("tabid", tabid);
                if (!haslselected && i == 0) g.selectedTabId = tabid;
                var showClose = contentitem.attr("showClose");
                if (showClose) {
                    li.append("<div class='l-tab-links-item-close'></div>");
                }
                $("> ul", g.tab.links).append(li);
                if (!contentitem.hasClass("l-tab-content-item")) contentitem.addClass("l-tab-content-item");
                if (contentitem.find("iframe").length > 0) {
                    var iframe = $("iframe:first", contentitem);
                    if (iframe[0].readyState != "complete") {
                        if (contentitem.find(".l-tab-loading:first").length == 0)
                            contentitem.prepend("<div class='l-tab-loading' style='display:block;'></div>");
                        var iframeloading = $(".l-tab-loading:first", contentitem);
                        iframe.bind('load.tab', function () {
                            iframeloading.hide();
                        });
                    }
                }
            });
           
            //add tabsPage more
            g.toolFlag = 0;
            if ($(g.tab).attr("toolsid")) {

                g.tab.tools = $('<div id="' + $(g.tab).attr("toolsid") + '" class="l-tab-links-tools" ></div>');
                g.tab.links.append(g.tab.tools);
                g.tab.morelist = $('<ul  class="tabsMoreList"></ul>');
                g.tab.append(g.tab.morelist);
                g.tab.iframe = $('<iframe frameborder=0 class="l-tabs-shadow" style="z-index:0;position:absolute;display:none" scroll=no></iframe>').insertAfter(g.tab.morelist);
               
                g.tab.tools.bind("click", function () {
                  
                    //处理未加载tab时右侧morelist不显示的问题和连续点击出现的多次加载问题
                    if (p.tabListItem && $(p.tabListItem).length > 0 && g.getTabItemCount() == 0&&$("li",g.tab.morelist).length==0) {
                        //扩展自定义tab选项
                        p.tabListItem && $(p.tabListItem).each(function (i, item) {
                            var addTab = $('<li><a href="javascript:;"></a></li>');
                            addTab.a = $("> a", addTab);
                            item.text && addTab.a.text(item.text).css("color", "blue");
                            item.click && addTab.bind("click", function () {
                                item.click.apply(this, [addTab]);
                            })
                             g.tab.morelist.append(addTab);
                        })
                    }
                    var offset = $(this).offset(); //取得事件对象的位置
                    g.toolFlag = 0;
                    if (g.getTabItemCount() != 0 || (p.tabListItem && $(p.tabListItem).length > 0)) {

                        g.tab.morelist.show();
                        g.tab.iframe.show();
                        $(".toolBlur").focus();
                       
                    }
                    g.updateShadow();
                    return false;
                });
                g.tab.morelist.hover(function () { g.toolFlag=0 }, function () { g.toolFlag = 1});
             
                
                $(document).click(function () {

                    g.tab.morelist.hide();
                    g.tab.iframe.hide();

                });
            }
            //add tools
           
            var toolWidth = 17;
           
            if (p.tools)
            {
                toolWidth = g.tab.tools.width() + 17;
                p.tools && $.each(p.tools, function (i, item) {
                    var newTool = $("<div class='l-tab-tools'></div>");
                    newTool.css({ "line-height": "28px", "text-align": "center" }).css({
                        position: "absolute",
                        right: toolWidth,
                        cursor: "pointer",
                        color: "blue",
                        width: 60

                    });
                    item.styleCss && $.each(item.styleCss.split(','), function (i, cssItem) {
                        cssItem.indexOf(':') && newTool.css(cssItem.split(':')[0], cssItem.split(':')[1]);
                    });

                    newTool.html(item.text);
                    item.click && (typeof item.click == "function") && newTool.bind("click", function () {

                        item.click.apply(this, [item]);
                    })
                    g.tab.links.append(newTool);
                    toolWidth += newTool.width();

                })
            }
           
            g.tabScrollWidth = toolWidth;
            //第几个tab开始滚动的
            g.tabScrollCount = 0;
         
            //init 
            g.selectTabItem(g.selectedTabId);
            //set content height
            if (p.height) {
                if (typeof (p.height) == 'string' && p.height.indexOf('%') > 0) {
                    g.onResize();
                    if (p.changeHeightOnResize) {
                        $(window).resize(function () {
                            g.onResize.call(g);
                        });
                    }
                } else {
                    g.setHeight(p.height);
                }
            }
            if (g.makeFullHeight)
                g.setContentHeight();
            //add even 
            $("li", g.tab.links).each(function () {
                g._addTabItemEvent($(this));
            });

            g.tab.bind('dblclick.tab', function (e) {
                if (!p.dblClickToClose) return;
                g.dblclicking = true;
                var obj = (e.target || e.srcElement);
                var tagName = obj.tagName.toLowerCase();
                if (tagName == "a") {
                    var tabid = $(obj).parent().attr("tabid");
                    var allowClose = $(obj).parent().find("div.l-tab-links-item-close").length ? true : false;
                    if (allowClose) {
                        g.removeTabItem(tabid);
                    }
                }
                g.dblclicking = false;
            });
            g.set(p);


        },
        _applyDrag: function (tabItemDom) {
            var g = this, p = this.options;
            g.droptip = g.droptip || $("<div class='l-tab-drag-droptip' style='display:none'><div class='l-drop-move-up'></div><div class='l-drop-move-down'></div></div>").appendTo('body');
            var drag = $(tabItemDom).ligerDrag(
            {
                revert: true, animate: false,
                proxy: function () {
                    var name = $(this).find("a").html();
                    g.dragproxy = $("<div class='l-tab-drag-proxy' style='display:none'><div class='l-drop-icon l-drop-no'></div></div>").appendTo('body');
                    g.dragproxy.append(name);
                    return g.dragproxy;
                },
                onRendered: function () {
                    this.set('cursor', 'pointer');
                },
                onStartDrag: function (current, e) {
                    if (!$(tabItemDom).hasClass("l-selected")) return false;
                    if (e.button == 2) return false;
                    var obj = e.srcElement || e.target;
                    if ($(obj).hasClass("l-tab-links-item-close")) return false;
                },
                onDrag: function (current, e) {
                    if (g.dropIn == null)
                        g.dropIn = -1;
                    var tabItems = g.tab.links.ul.find('>li');
                    var targetIndex = tabItems.index(current.target);
                    tabItems.each(function (i, item) {
                        if (targetIndex == i) {
                            return;
                        }
                        var isAfter = i > targetIndex;
                        if (g.dropIn != -1 && g.dropIn != i) return;
                        var offset = $(this).offset();
                        var range = {
                            top: offset.top,
                            bottom: offset.top + $(this).height(),
                            left: offset.left - 10,
                            right: offset.left + 10
                        };
                        if (isAfter) {
                            range.left += $(this).width();
                            range.right += $(this).width();
                        }
                        var pageX = e.pageX || e.screenX;
                        var pageY = e.pageY || e.screenY;
                        if (pageX > range.left && pageX < range.right && pageY > range.top && pageY < range.bottom) {
                            g.droptip.css({
                                left: range.left + 5,
                                top: range.top - 9
                            }).show();
                            g.dropIn = i;
                            g.dragproxy.find(".l-drop-icon").removeClass("l-drop-no").addClass("l-drop-yes");
                        }
                        else {
                            g.dropIn = -1;
                            g.droptip.hide();
                            g.dragproxy.find(".l-drop-icon").removeClass("l-drop-yes").addClass("l-drop-no");
                        }
                    });
                },
                onStopDrag: function (current, e) {
                    if (g.dropIn > -1) {
                        var to = g.tab.links.ul.find('>li:eq(' + g.dropIn + ')').attr("tabid");
                        var from = $(current.target).attr("tabid");
                        setTimeout(function () {
                            g.moveTabItem(from, to);
                        }, 0);
                        g.dropIn = -1;
                        g.dragproxy.remove();
                    }
                    g.droptip.hide();
                    this.set('cursor', 'default');
                    g.trigger('afterDragTabItem', [to,from]);
                }
            });
            return drag;
        },
        updateShadow: function () {
            var g = this, p = this.options;
            if (!g.tab.iframe) return false;
            g.tab.iframe.css({
                right: 0,
                top: g.tab.morelist.css('top'),
                width: g.tab.morelist.outerWidth(),
                height: g.tab.morelist.outerHeight()
            });

        },
        _setDragToMove: function (value) {
            if (!$.fn.ligerDrag) return; //需要ligerDrag的支持
            var g = this, p = this.options;
            if (value) {
                if (g.drags) return;
                g.drags = g.drags || [];
                g.tab.links.ul.find('>li').each(function () {
                    g.drags.push(g._applyDrag(this));
                });
            }
        },
        moveTabItem: function (fromTabItemID, toTabItemID) {
            var g = this;
            var from = g.tab.links.ul.find(">li[tabid=" + fromTabItemID + "]");
            var to = g.tab.links.ul.find(">li[tabid=" + toTabItemID + "]");
            var index1 = g.tab.links.ul.find(">li").index(from);
            var index2 = g.tab.links.ul.find(">li").index(to);
            if (index1 < index2) {
                to.after(from);
            }
            else {
                to.before(from);
            }
        },
        //设置tab按钮(左和右),显示返回true,隐藏返回false
        setTabButton: function () {
            var g = this, p = this.options;
            var sumwidth = 0;
            $("li", g.tab.links.ul).each(function () {
                sumwidth += $(this).width() + 2;
            });
            
            var mainwidth = g.tab.width();
            if (p.tools)
                mainwidth = g.tab.width() - g.tabScrollWidth;
            if (sumwidth > mainwidth) {
                $(".l-tab-links-left,.l-tab-links-right", g.tab.links).remove();
                g.tab.links.append('<div class="l-tab-links-left"></div><div class="l-tab-links-right"></div>');
                if ($(g.tab).attr("toolsid")) {
                    $(".l-tab-links-right", g.tab.links).css("right", "17px");
                }
                $(".l-tab-links-right", g.tab.links).addClass("l-tab-links-right-invalid");
                g.setTabButtonEven();
                return true;
            } else {
                g.tab.links.ul.animate({ left: 0 });
                $(".l-tab-links-left,.l-tab-links-right", g.tab.links).remove();
                return false;
            }
        },
        //设置左右按钮的事件 标签超出最大宽度时，可左右拖动
        setTabButtonEven: function () {
            var g = this, p = this.options;
            $(".l-tab-links-left", g.tab.links).hover(function () {
                $(this).addClass("l-tab-links-left-over");
            }, function () {
                $(this).removeClass("l-tab-links-left-over");
            }).click(function () {
                g.moveToPrevTabItem();
            });
            $(".l-tab-links-right", g.tab.links).hover(function () {
                $(this).addClass("l-tab-links-right-over");
            }, function () {
                $(this).removeClass("l-tab-links-right-over");
            }).click(function () {
                g.moveToNextTabItem();
            });
        },
        //切换到上一个tab
        moveToPrevTabItem: function () {

            var g = this, p = this.options;
            var btnWitdth = $(".l-tab-links-left", g.tab.links).width();
            var leftList = new Array(); //记录每个tab的left,由左到右

            $("li", g.tab.links).each(function (i, item) {
                var currentItemLeft = -1 * btnWitdth;
                if (i > 0) {
                    currentItemLeft = parseInt(leftList[i - 1]) + $(this).prev().width() + p.marginleft;
                }
                leftList.push(currentItemLeft);
            });
            var currentLeft = -1 * parseInt(g.tab.links.ul.css("left"));

            for (var i = 0; i < leftList.length - 1; i++) {
                if (leftList[i] < currentLeft && leftList[i + 1] >= currentLeft) {
                    g.tab.links.ul.animate({ left: -1 * parseInt(leftList[i]) }, function () {
                        //增加可见区域左侧箭头不可用 
                        if (parseInt(g.tab.links.ul.css("left")) == btnWitdth) {
                            $(".l-tab-links-left", g.tab.links).addClass("l-tab-links-left-invalid");
                            $(".l-tab-links-right", g.tab.links).removeClass("l-tab-links-right-invalid");
                            return;
                        }
                        if(-1*parseInt(g.tab.links.ul.css("left"))<currentLeft)
                        {
                            $(".l-tab-links-right", g.tab.links).removeClass("l-tab-links-right-invalid");
                        }

                    });

                    return;
                }
            }

        },
        //切换到下一个tab
        moveToNextTabItem: function () {
            var g = this, p = this.options;
            var btnWitdth = $(".l-tab-links-right", g.tab).width() + $(".l-tab-links-tools", g.tab.links).width();
            var sumwidth = 0;
            var tabItems = $("li", g.tab.links.ul);
            tabItems.each(function () {
                sumwidth += $(this).width() + p.marginleft;
            });
            var mainwidth = g.tab.width();
            var leftList = new Array();
            for (var i = tabItems.length - 1; i >= 0; i--) {
                var currentItemLeft = sumwidth - mainwidth + btnWitdth + p.marginleft;
                if (p.tools)
                    currentItemLeft = sumwidth - mainwidth +g.tabScrollWidth + p.marginleft-11;
                if (i != tabItems.length - 1) {
                    currentItemLeft = parseInt(leftList[tabItems.length - 2 - i]) - ($(tabItems[i + 1]).width() + p.marginleft);
                }
                leftList.push(currentItemLeft);
            }
            var currentLeft = -1 * parseInt(g.tab.links.ul.css("left"));

            for (var i = 1; i < leftList.length; i++) {
                if (leftList[i] <= currentLeft && leftList[i - 1] > currentLeft) {
                    g.tab.links.ul.animate({
                        left: -1 * parseInt(leftList[i - 1])
                    }, function () {
                        //移除可见区域左侧箭头不可用状态 
                        if (parseInt(g.tab.links.ul.css("left")) < 0) {
                            $(".l-tab-links-left", g.tab.links).removeClass("l-tab-links-left-invalid");
                        }
                        //可见区域右侧箭头不可用
                        if (-1 * parseInt(g.tab.links.ul.css("left")) + mainwidth > sumwidth) {
                            $(".l-tab-links-right", g.tab.links).addClass("l-tab-links-right-invalid");

                        }
                    });
                    return;
                }
            }
        },
        getTabItemCount: function () {
            var g = this, p = this.options;
            return $("li", g.tab.links.ul).length;
        },
        getSelectedTabItemID: function () {
            var g = this, p = this.options;
            return $("li.l-selected", g.tab.links.ul).attr("tabid");
        },
        removeSelectedTabItem: function () {
            var g = this, p = this.options;
            g.removeTabItem(g.getSelectedTabItemID());
        },
        //覆盖选择的tabitem
        overrideSelectedTabItem: function (options) {
            var g = this, p = this.options;
            g.overrideTabItem(g.getSelectedTabItemID(), options);
        },
        //覆盖
        overrideTabItem: function (targettabid, options) {
            var g = this, p = this.options;
            if (g.trigger('beforeOverrideTabItem', [targettabid]) == false)
                return false;
            var tabid = options.tabid;
            if (tabid == undefined) tabid = g.getNewTabid();
            var url = options.url;
            var content = options.content;
            var target = options.target;
            var text = options.text;
            var showClose = options.showClose;
            var height = options.height;
            //如果已经存在
            if (g.isTabItemExist(tabid)) {
                return;
            }
            var tabitem = $("li[tabid=" + targettabid + "]", g.tab.links.ul);
            var contentitem = $(".l-tab-content-item[tabid=" + targettabid + "]", g.tab.content);
            if (!tabitem || !contentitem) return;
            tabitem.attr("tabid", tabid);
            contentitem.attr("tabid", tabid);
            if ($("iframe", contentitem).length == 0 && url) {
                contentitem.html("<iframe frameborder='0'></iframe>");
            }
            else if (content) {
                contentitem.html(content);
            }
            $("iframe", contentitem).attr("name", tabid);
            if (showClose == undefined) showClose = true;
            if (showClose == false) $(".l-tab-links-item-close", tabitem).remove();
            else {
                if ($(".l-tab-links-item-close", tabitem).length == 0)
                    tabitem.append("<div class='l-tab-links-item-close'></div>");
            }
            if (text == undefined) text = tabid;
            if (height) contentitem.height(height);
            $("a", tabitem).text(text);
            $("iframe", contentitem).attr("src", url);


            g.trigger('afterOverrideTabItem', [targettabid]);
        },
        //选中当前tab
        moveToCurrTabItem: function (tabid) {
            var g = this, p = this.options;
            var btnWitdth = $(".l-tab-links-left", g.tab.links).width();
            var mainwidth = g.tab.width();
            var selecttableftwidth = 0;
            var selecttabrightwidth = 0;
            $("li", g.tab.links.ul).each(function () {
                selecttabrightwidth += $(this).width() + p.marginleft;
                if ($(this).attr("tabid") == tabid) {
                    return false;
                }
                selecttableftwidth += $(this).width() + p.marginleft;
            });
            var tableftnum = parseInt($(g.tab.links.ul).css("left"));
            
            if (selecttableftwidth < (0 - tableftnum)) {
                g.tab.links.ul.animate({
                    left: -1 * (selecttableftwidth - btnWitdth)
                }, function () {
                    if (parseInt(g.tab.links.ul.css("left")) == btnWitdth) {
                        $(".l-tab-links-left", g.tab.links).addClass("l-tab-links-left-invalid");
                        $(".l-tab-links-right", g.tab.links).removeClass("l-tab-links-right-invalid");
                        return;
                    }
                    if (-1 * parseInt(g.tab.links.ul.css("left")) < -1 * tableftnum) {
                        $(".l-tab-links-right", g.tab.links).removeClass("l-tab-links-right-invalid");
                    }

                });
            } else if ((selecttabrightwidth + tableftnum) > mainwidth) {
                btnWitdth += $(".l-tab-links-tools", g.tab.links).width();
                
                var leftPositon=(selecttabrightwidth - mainwidth + btnWitdth + p.marginleft);
                if (p.tools)
                    leftPositon = (selecttabrightwidth - mainwidth +g.tabScrollWidth + p.marginleft-18);
                g.tab.links.ul.animate({
                    left: (-1 * leftPositon)
                }, function () {
                    //移除可见区域左侧箭头不可用状态 
                    if (parseInt(g.tab.links.ul.css("left")) < 0) {
                        $(".l-tab-links-left", g.tab.links).removeClass("l-tab-links-left-invalid");
                    }
                    //可见区域右侧箭头不可用
                    if (-1 * parseInt(g.tab.links.ul.css("left")) + mainwidth > selecttabrightwidth) {
                        $(".l-tab-links-right", g.tab.links).addClass("l-tab-links-right-invalid");

                    }
                
                });
            }
        },
        //设置页签项标题
        setHeader: function (tabid, header) {
            $("li[tabid=" + tabid + "] a", this.tab.links.ul).text(header);
        },
        //选中tab项
        selectTabItem: function (tabid) {
            var g = this, p = this.options;
            if (g.trigger('beforeSelectTabItem', [tabid]) == false)
                return false;
            g.selectedTabId = tabid;
            $("> .l-tab-content-item[tabid=" + tabid + "]", g.tab.content).show().siblings().hide();
            $("li[tabid=" + tabid + "]", g.tab.links.ul).addClass("l-selected").siblings().removeClass("l-selected");
            g.trigger('afterSelectTabItem', [tabid]);

            //tabmore
            if ($(g.tab).attr("toolsid")) {
                $("li", g.tab.morelist).removeClass("selected");
                $("li[id=" + tabid + "]", g.tab.morelist).addClass("selected");
            }


        },
        //移动到最后一个tab
        moveToLastTabItem: function () {
            var g = this, p = this.options;
            var sumwidth = 0;
           
            $("li", g.tab.links.ul).each(function () {
                sumwidth += $(this).width() + p.marginleft;
            });
            var mainwidth = g.tab.width();
            if(p.tools)
                mainwidth = g.tab.width() - g.tabScrollWidth;
            if (sumwidth > mainwidth) {
                var btnWitdth = $(".l-tab-links-right", g.tab.links).width() + $(".l-tab-links-tools", g.tab.links).width();
                if (!g.tabScrollCount)
                    g.tabScrollCount = g.getTabItemCount();
               
                if (p.tools)
                {
                    g.tab.links.ul.css({
                        left: -1 * (sumwidth - mainwidth + p.marginleft)+10
                    });
                    g.tab.links.ul.css("left", parseInt(g.tab.links.ul.css("left")) + g.getTabItemCount()-g.tabScrollCount+ "px");
                    return;
                }
                g.tab.links.ul.animate({
                    left: -1 * (sumwidth - mainwidth + btnWitdth + p.marginleft)
                });
            }
        },
        //判断tab是否存在
        isTabItemExist: function (tabid) {
            var g = this, p = this.options;
            return $("li[tabid=" + tabid + "]", g.tab.links.ul).length > 0;
        },
        //新增打开一组tab页面功能
        addTabItems:function(tabItemJson)
        {
            var g = this, p = this.options;
            if (p.tabs == undefined) {
                p.tabs = [];
                $.extend(p, p.tabs);
            }
            for (var i = 0; i < tabItemJson.length; i++)
            {
               if (i == tabItemJson.length - 1)
                    g.addTabItem(tabItemJson[i]);
                else {
                    g.addTabItem({ text: tabItemJson[i].text, tabid: tabItemJson[i].tabid, url: "null" });
                    p.tabs.push(tabItemJson[i]);
                }
            }
         
        },
        //增加一个tab
        addTabItem: function (options) {
            var g = this, p = this.options;
            if (g.trigger('beforeAddTabItem', [tabid]) == false)
                return false;
            var tabid = options.tabid;
            if (tabid == undefined) tabid = g.getNewTabid();
            var url = options.url;
            var iconcss = options.iconcss;
            var content = options.content;
            var text = options.text;
            var showClose = options.showClose;
            var height = options.height;
            //如果已经存在
            if (g.isTabItemExist(tabid)) {
                g.selectTabItem(tabid);
                g.moveToCurrTabItem(tabid);
                //  g.reload(tabid);

                return;
            }
            var tabitem = $("<li><a></a><div class='l-tab-links-item-left'></div><div class='l-tab-links-item-right'></div><div class='l-tab-links-item-close'></div></li>");
            var contentitem = $("<div class='l-tab-content-item'><div class='l-tab-loading' style='display:block;'></div><iframe frameborder='0'></iframe></div>");
            var iframeloading = $("div:first", contentitem);
            var iframe = $("iframe:first", contentitem);
            if (g.makeFullHeight) {
                var newheight = g.tab.height() - g.tab.links.height();
                contentitem.height(newheight);
            }
            tabitem.attr("tabid", tabid);
            contentitem.attr("tabid", tabid);
            if (url) {
              //url=null为加载多tab时处理
              if (url != "null")
              { 
                 iframe.attr("name", tabid)
                 .attr("id", tabid)
                 .attr("src", url)
                 .bind('load.tab', function () {
                     iframeloading.hide();
                     if (options.callback)
                         options.callback();
                    
                 });
                 //增加点击空白右键菜单隐藏
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
              }
                
            }
            else {
                iframe.remove();
                iframeloading.remove();
            }
            if (content) {
                contentitem.html(content);
            }
            else if (options.target) {
                contentitem.append(options.target);
            }
            if (showClose == undefined) showClose = true;
            if (showClose == false) $(".l-tab-links-item-close", tabitem).remove();
            if (text == undefined) text = tabid;
            if (height) contentitem.height(height);
            // $("a", tabitem).text(text);
            //修改支持html内容
            $("a", tabitem).html(text);
            $.browser.msie && $.browser.version == "6.0" && $("a", tabitem).find("img").length && ("a", tabitem).css("padding-top", "3");
            if (iconcss) $("a", tabitem).addClass(iconcss);
             g.tab.links.ul.append(tabitem);
            g.tab.content.append(contentitem);
            g.selectTabItem(tabid);
            if (g.setTabButton()) {
                g.moveToLastTabItem();
            }
            if(p.isOnlyOneTabToClose&&g.getTabItemCount()==1)
                g.tab.links.ul.hide();
            else
                g.tab.links.ul.show();
            //增加tabmorelist
            g.addTabMoreList();
            //增加事件
            g._addTabItemEvent(tabitem);
            if (p.dragToMove && $.fn.ligerDrag) {
                g.drags = g.drags || [];
                tabitem.each(function () {
                    g.drags.push(g._applyDrag(this));
                });
            }
            g.trigger('afterAddTabItem', [tabid]);
        },
        addTabMoreList: function () {
            
            var g = this, p = this.options;
            var tabid = g.getSelectedTabItemID();
            if ($(g.tab).attr("toolsid")) {
                g.tab.morelist.html("");
                var tabidlist = g.getTabidList('0', false);
                if ($("> li:first", g.tab.morelist).length == 0 && g.getTabItemCount() != 0) {
                    var exitLi = $('<li><a href="javascript:;">退出<input type="text" class="toolBlur" style="width:1px;height:1px;border:0px"  /></a></li>');
                    $("> a", exitLi).css("color", "blue");
                    exitLi.bind("click", function () {
                        g.tab.morelist.hide();
                    });
                    !p.tabListItem&&g.tab.morelist.append(exitLi);
                    
                    //扩展自定义tab选项
                    p.tabListItem && $(p.tabListItem).each(function (i, item) {
                        var  addTab= $('<li><a href="javascript:;"></a></li>');
                        addTab.a = $("> a", addTab);
                        item.text && addTab.a.text(item.text).css("color", "blue");
                        var blurInput = $('<input type="text" class="toolBlur" style="width:1px;height:1px;border:0px"  />');
                        if (i == 0) addTab.a.append(blurInput);
                        item.click && addTab.bind("click", function () {
                            item.click.apply(this, [addTab]);
                        })
                       
                        g.tab.morelist.append(addTab);
                    })
                   
                    $(".toolBlur").blur(function () {
                       
                        if (g.toolFlag) {
                            g.tab.morelist.hide();
                            g.tab.iframe.hide();
                        }

                    });
                }
                
                $(tabidlist).each(function () {
                    
                    $("li[id=" + this + "]", g.tab.morelist).removeClass("selected");
                    if ($("li[id=" + this + "]", g.tab.morelist).length == 0) {
                        var li = $('<li ><a href="javascript:;" >' + $("li[tabid=" + this + "]", g.tab.links.ul).text() + '</a></li>');
                        li.attr('id', this);
                        if (this == tabid)
                            li.addClass("selected");
                        g.tab.morelist.append(li);
                       li.bind("click", function () {
                            g.selectTabItem($(this).attr("id"));
                            g.moveToCurrTabItem($(this).attr("id"));
                            return;
                        });

                    }

                })
            }

        },
        _addTabItemEvent: function (tabitem) {
            var g = this, p = this.options;
            tabitem.click(function () {
                var tabid = $(this).attr("tabid");
                g.selectTabItem(tabid);
                if (p.tabs) {
                    for (var i = 0; i < p.tabs.length;i++)
                    {
                        if (p.tabs[i].tabid == tabid)
                        {
                            g.reload(tabid,p.tabs[i].url);
                            p.tabs.splice(i, 1);
                            break;
                        }
                    }
                }
            });
            //右键事件支持
            g.tab.menu && g._addTabItemContextMenuEven(tabitem);
            $(".l-tab-links-item-close", tabitem).hover(function () {
                $(this).addClass("l-tab-links-item-close-over");
            }, function () {
                $(this).removeClass("l-tab-links-item-close-over");
            }).click(function () {
                var tabid = $(this).parent().attr("tabid");
                g.removeTabItem(tabid);
            });

        },
        //移除tab项
        removeTabItem: function (tabid) {

            var g = this, p = this.options;
            if (g.trigger('beforeRemoveTabItem', [tabid]) == false)
                return false;
            var currentIsSelected = $("li[tabid=" + tabid + "]", g.tab.links.ul).hasClass("l-selected");
            if (currentIsSelected) {
                $(".l-tab-content-item[tabid=" + tabid + "]", g.tab.content).prev().show();
                $("li[tabid=" + tabid + "]", g.tab.links.ul).prev().addClass("l-selected").siblings().removeClass("l-selected");
            }
            $(".l-tab-content-item[tabid=" + tabid + "]", g.tab.content).remove();
            $("li[tabid=" + tabid + "]", g.tab.links.ul).remove();
            g.setTabButton();
            g.trigger('afterRemoveTabItem', [tabid]);
            //增加移除more列表选项
            g.addTabMoreList();
        },
        addHeight: function (heightDiff) {
            var g = this, p = this.options;
            var newHeight = g.tab.height() + heightDiff;
            g.setHeight(newHeight);
        },
        _setHeight: function (height) {
            var g = this, p = this.options;
            if (!height) return;
            g.tab.height(height);
            g.setContentHeight();
        },
        setHeight: function (height) {
            var g = this, p = this.options;
            g.tab.height(height);
            g.setContentHeight();
        },
        setContentHeight: function () {
            var g = this, p = this.options;
            var newheight = g.tab.height() - g.tab.links.height();
            g.tab.content.height(newheight);
            $("> .l-tab-content-item", g.tab.content).height(newheight);
        },
        getNewTabid: function () {
            var g = this, p = this.options;
            g.getnewidcount = g.getnewidcount || 0;
            return 'tabitem' + (++g.getnewidcount);
        },
        //notabid 过滤掉tabid的
        //noclose 过滤掉没有关闭按钮的
        getTabidList: function (notabid, noclose) {
            var g = this, p = this.options;
            var tabidlist = [];
            var tablist = null;
            var position = arguments[2];
            if (position == null)
                tablist = $("> li", g.tab.links.ul);
            else if (position == "right") {
                tablist = $("li[tabid=" + notabid + "]", g.tab.links.ul).nextAll();
            }
            else if (position == "left") {
                tablist = $("li[tabid=" + notabid + "]", g.tab.links.ul).prevAll();
            }
            tablist.each(function () {
                if ($(this).attr("tabid")
                        && $(this).attr("tabid") != notabid
                        && (!noclose || $(".l-tab-links-item-close", this).length > 0)) {
                    tabidlist.push($(this).attr("tabid"));
                }
            });
            return tabidlist;
        },
        removeOther: function (tabid, compel) {
            var g = this, p = this.options;
            var tabidlist = g.getTabidList(tabid, true);
            $(tabidlist).each(function () {
                g.removeTabItem(this);
            });
            g.addTabMoreList();
        },
        removeRight: function (tabid, compel) {
            var g = this, p = this.options;
            var tabidlist = g.getTabidList(tabid, true, "right");
            $(tabidlist).each(function () {
                g.removeTabItem(this);
            });
            g.addTabMoreList();
        },
        removeLeft: function (tabid, compel) {
            var g = this, p = this.options;
            var tabidlist = g.getTabidList(tabid, true, "left");
            $(tabidlist).each(function () {
                g.removeTabItem(this);
            });
            g.addTabMoreList();
        },
        reload: function (tabid,newUrl) {
            var g = this, p = this.options;
            var contentitem = $(".l-tab-content-item[tabid=" + tabid + "]");
            var iframeloading = $(".l-tab-loading:first", contentitem);
            var iframe = $("iframe:first", contentitem);
            var url = $(iframe).attr("src") || newUrl;
            iframeloading.show();
            iframe.length&&iframe.attr("src", url).unbind('load.tab').bind('load.tab', function () {
                iframeloading.hide();
            });

        },
        removeAll: function (compel) {
            var g = this, p = this.options;
            var tabidlist = g.getTabidList(null, true);
            $(tabidlist).each(function () {
                g.removeTabItem(this);
            });
            g.addTabMoreList();
        },
        onResize: function () {
            var g = this, p = this.options;
            if (!p.height || typeof (p.height) != 'string' || p.height.indexOf('%') == -1) return false;
            //set tab height
            if (g.tab.parent()[0].tagName.toLowerCase() == "body") {
                var windowHeight = $(window).height();
                windowHeight -= parseInt(g.tab.parent().css('paddingTop'));
                windowHeight -= parseInt(g.tab.parent().css('paddingBottom'));
                g.height = p.heightDiff + windowHeight * parseFloat(g.height) * 0.01;
            }
            else {
                g.height = p.heightDiff + (g.tab.parent().height() * parseFloat(p.height) * 0.01);
            }
            g.tab.height(g.height);
            g.setContentHeight();
        },
        _menuItemClick: function (item) {
            var g = this, p = this.options;
            if (!item.id || !g.actionTabid) return;
            switch (item.id) {
                case "close":
                    g.removeTabItem(g.actionTabid);
                    g.actionTabid = null;
                    break;
                case "closeother":
                    g.removeOther(g.actionTabid);
                    break;
                case "closeall":
                    g.removeAll();
                    g.actionTabid = null;
                    break;
                case "reload":
                    g.selectTabItem(g.actionTabid);
                    g.reload(g.actionTabid);
                    break;
                case "closeright":
                    g.removeRight(g.actionTabid);
                    break;
                case "closeleft":
                    g.removeLeft(g.actionTabid);
                    break;
            }
        },
        _addTabItemContextMenuEven: function (tabitem) {

            var g = this, p = this.options;
            tabitem.bind("contextmenu", function (e) {

                if (!g.tab.menu) return;
                g.actionTabid = tabitem.attr("tabid");
                g.tab.menu.show({ top: e.pageY, left: e.pageX });
                if ($(".l-tab-links-item-close", this).length == 0) {
                    g.tab.menu.setDisabled('close');
                }
                else {
                    g.tab.menu.setEnabled('close');
                }
               if ($.ligerMenu)
                {
                    if ($(this).next().length == 0)
                        g.tab.menu.setDisabled("closeright");
                    else
                        g.tab.menu.setEnabled("closeright");
                    if ($(this).prev().length == 0)
                        g.tab.menu.setDisabled("closeleft");
                    else
                        g.tab.menu.setEnabled("closeleft");
                    if ($(this).next().length == 0 && $(this).prev().length == 0)
                        g.tab.menu.setDisabled("closeother");
                    else
                        g.tab.menu.setEnabled("closeother");

                }
                return false;
            });
        }
    });


})(jQuery);