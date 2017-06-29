/**
* liger improve by LiKun v1.0
* 
* Author LiKun[469546858@qq.com]
* 
*/
(function ($) {
    $.fn.ligerLayout = function (options) {
        return $.liger.run.call(this, "ligerLayout", arguments);
    };

    $.fn.ligerGetLayoutManager = function () {

        return $.liger.run.call(this, "ligerGetLayoutManager", arguments);
    };

    $.fn.cssv = function (pre) {
        var cssPre = $(this).css(pre);
        return cssPre.substring(0, cssPre.indexOf("px")) * 1;
    }; 
     $.ligerDefaults.Layout = {
        topHeight: 50,
        bottomHeight: 50,
        leftWidth: 200,
        centerWidth: 300,
        rightWidth: 200,
        InWindow: true,     //是否以窗口的高度为准 height设置为百分比时可用
        heightDiff: 0,     //高度补差
        height: '100%',      //高度
        onHeightChanged: null,
        isLeftCollapse: false,      //初始化时 左边是否隐藏
        isRightCollapse: false,     //初始化时 右边是否隐藏
        allowLeftCollapse: true,      //是否允许 左边可以隐藏
        allowRightCollapse: true,     //是否允许 右边可以隐藏
        allowLeftResize: true,      //是否允许 左边可以调整大小
        allowRightResize: true,     //是否允许 右边可以调整大小
        allowTopResize: true,      //是否允许 头部可以调整大小
        allowBottomResize: true,     //是否允许 底部可以调整大小
        space: 3, //间隔 
        onEndResize: null,          //调整大小结束事件
        minLeftWidth: 80,            //调整左侧宽度时的最小允许宽度
        minRightWidth: 80,           //调整右侧宽度时的最小允许宽度
        isOpenLeftAnimation: true,  //默认左侧展开动画效果开启
        isOpenRightAnimation: true,  //默认右侧展开动画效果开启
        isLeftShare: true,           //默认左侧分享
        isRightShare: true,           // 默认右侧分享
        isCollapseHover: false,       //默认鼠标移动关闭滑动效果   
        isLeftShareMaskOcx: false,        //默认左侧导航功能报表遮盖关闭  
        isRightShareMaskOcx: false        //默认右侧导航功能报表遮盖关闭  
       
    };

    $.ligerMethos.Layout = {};

    $.liger.controls.Layout = function (element, options) {
        $.liger.controls.Layout.base.constructor.call(this, element, options);
    };
    $.liger.controls.Layout.ligerExtend($.liger.core.UIComponent, {
        __getType: function () {
            return 'Layout';
        },
        __idPrev: function () {
            return 'Layout';
        },
        _extendMethods: function () {
            return $.ligerMethos.Layout;
        },

        _render: function () {
            var g = this, p = this.options;
            g.layout = $(this.element);
            g.layout.addClass("l-layout");
            g.width = g.layout.width();
            //top
            if ($("> div[position=top]", g.layout).length > 0) {
                g.top = $("> div[position=top]", g.layout).wrap('<div class="l-layout-top" style="top:0px;"></div>').parent();
                g.top.content = $("> div[position=top]", g.top);
                if (!g.top.content.hasClass("l-layout-content"))
                    g.top.content.addClass("l-layout-content");
                g.topHeight = p.topHeight;
                if (g.topHeight) {
                    g.top.height(g.topHeight);
                }
            }
            //bottom
            if ($("> div[position=bottom]", g.layout).length > 0) {
                g.bottom = $("> div[position=bottom]", g.layout).wrap('<div class="l-layout-bottom"></div>').parent();
                g.bottom.content = $("> div[position=bottom]", g.bottom);
                if (!g.bottom.content.hasClass("l-layout-content"))
                    g.bottom.content.addClass("l-layout-content");

                g.bottomHeight = p.bottomHeight;
                if (g.bottomHeight) {
                    g.bottom.height(g.bottomHeight);
                }
                //set title
                var bottomtitle = g.bottom.content.attr("title");
                if (bottomtitle) {
                    g.bottom.header = $('<div class="l-layout-header"></div>');
                    g.bottom.prepend(g.bottom.header);
                    g.bottom.header.html(bottomtitle);
                    g.bottom.content.attr("title", "");
                }
            }
            //left
            if ($("> div[position=left]", g.layout).length > 0) {
                g.left = $("> div[position=left]", g.layout).wrap('<div class="l-layout-left" style="left:0px;"></div>').parent();
                g.left.header = $('<div class="l-layout-header"><div id="l-layout-header-lock-left" class="l-layout-header-lock-left"></div><div class="l-layout-header-toggle"></div><div class="l-layout-header-inner"></div></div>');
                g.left.prepend(g.left.header);
                g.left.shadow = $('<iframe class="l-shadow" style="z-index:11;position:absolute;display:none" frameborder=0></iframe>').appendTo(g.layout)
                g.left.header.toggle = $(".l-layout-header-toggle", g.left.header);
                g.left.content = $("> div[position=left]", g.left);
                if (!g.left.content.hasClass("l-layout-content"))
                    g.left.content.addClass("l-layout-content");
                if (!p.allowLeftCollapse) $(".l-layout-header-toggle,#l-layout-header-lock-left", g.left.header).remove();
                //set title
                var lefttitle = g.left.content.attr("title");
                if (lefttitle) {
                    g.left.content.attr("title", "");
                    $(".l-layout-header-inner", g.left.header).html(lefttitle);
                }
                //set width
                g.leftWidth = p.leftWidth;
                if (g.leftWidth)
                    g.left.width(g.leftWidth);
            }
            //新增悬浮停靠
            $("#l-layout-header-lock-left").click(function () {
               if ($(this).hasClass("l-layout-header-lock-left")) {
                    g.setLeftHoverCollapse(true);
                    $(this).removeClass("l-layout-header-lock-left").addClass("l-layout-header-unlock-left");
                }
                else {
                    g.setLeftHoverCollapse(false);
                    $(this).removeClass("l-layout-header-unlock-left").addClass("l-layout-header-lock-left");
                }
               
            }
			)
      
            //center
            if ($("> div[position=center]", g.layout).length > 0) {
                g.center = $("> div[position=center]", g.layout).wrap('<div class="l-layout-center" ></div>').parent();
                g.center.content = $("> div[position=center]", g.center);
                g.center.content.addClass("l-layout-content");
                //set title
                var centertitle = g.center.content.attr("title");
                if (centertitle) {
                    g.center.content.attr("title", "");
                    g.center.header = $('<div class="l-layout-header"></div>');
                    g.center.prepend(g.center.header);
                    g.center.header.html(centertitle);
                }
                //set width
                g.centerWidth = p.centerWidth;
                if (g.centerWidth)
                    g.center.width(g.centerWidth);
            }
            //right
            if ($("> div[position=right]", g.layout).length > 0) {
                g.right = $("> div[position=right]", g.layout).wrap('<div class="l-layout-right"></div>').parent();
                g.right.header = $('<div class="l-layout-header"><div id="l-layout-header-lock-right" class="l-layout-header-lock-right"></div><div class="l-layout-header-toggle"></div><div class="l-layout-header-inner"></div></div>');
                g.right.prepend(g.right.header);
                g.right.shadow = $('<iframe class="l-shadow" style="z-index:13;position:absolute;display:none" frameborder=0></iframe>').appendTo(g.layout)
                g.right.header.toggle = $(".l-layout-header-toggle", g.right.header);
                if (!p.allowRightCollapse) $(".l-layout-header-toggle,#l-layout-header-lock-right", g.right.header).remove();
                g.right.content = $("> div[position=right]", g.right);
                if (!g.right.content.hasClass("l-layout-content"))
                    g.right.content.addClass("l-layout-content");
               //set title
                var righttitle = g.right.content.attr("title");
                if (righttitle) {
                    g.right.content.attr("title", "");
                    $(".l-layout-header-inner", g.right.header).html(righttitle).css("padding-left",50);
                }
                //set width
                g.rightWidth = p.rightWidth;
                if (g.rightWidth)
                    g.right.width(g.rightWidth);
            }
            //新增悬浮停靠
            $("#l-layout-header-lock-right").click(function () {
             
             if ($(this).hasClass("l-layout-header-lock-right")) {
                 g.setRightHoverCollapse(true);
                 $(this).removeClass("l-layout-header-lock-right").addClass("l-layout-header-unlock-right");
             }
             else
             {
                 g.setRightHoverCollapse(false);
                 $(this).removeClass("l-layout-header-unlock-right").addClass("l-layout-header-lock-right");
              }
                

            })

            //lock
            g.layout.lock = $("<div class='l-layout-lock'></div>");
            g.layout.append(g.layout.lock);
            //DropHandle
            g._addDropHandle();

            //Collapse
            g.isLeftCollapse = p.isLeftCollapse;
            g.isRightCollapse = p.isRightCollapse;
            g.leftCollapse = $('<div class="l-layout-collapse-left" style="display: none; "><div class="l-layout-collapse-left-toggle"></div></div>');
            g.leftShare = $('<div class="l-layout-collapse-left-share"></div>');
            //修复关于左侧和右侧功能导航被报表遮挡的问题
            g.leftShareIframe = $("<iframe frameborder=0 style='position:absolute;z-index:10;display:none' scrolling='no' ></iframe>");
            g.rightShare = $('<div class="l-layout-collapse-right-share"></div>');
            g.rightShareIframe = $("<iframe frameborder=0 style='position:aboslute;z-index:10;display:none' scrolling='no'></iframe>");
            g.rightCollapse = $('<div class="l-layout-collapse-right" style="display: none; "><div class="l-layout-collapse-right-toggle"></div></div>');
            g.layout.append(g.leftCollapse).append(g.rightCollapse).append(g.leftShare).append(g.rightShare);
            p.isLeftShareMaskOcx && g.layout.append(g.leftShareIframe);
            p.isRightShareMaskOcx && g.layout.append(g.rightShareIframe);
            g.leftCollapse.toggle = $("> .l-layout-collapse-left-toggle", g.leftCollapse);
            g.rightCollapse.toggle = $("> .l-layout-collapse-right-toggle", g.rightCollapse);
            g._setCollapse();

            //init
            g._bulid();
            $(window).resize(function () {
                g._onResize();
            });
            g.set(p);

        },
        //左侧动画收缩
        setLeftAnimateCollapse: function (isCollapse) {

            var g = this, p = this.options;
            if (!g.left) return false;
            g.isLeftCollapse = isCollapse;
            g.left.css("z-index", 90000);
            var barleft = g.left.outerWidth() * -1;
            var cleft = p.space + g.leftCollapse.outerWidth();
            var cwidth = g.left.outerWidth() - g.leftCollapse.outerWidth() + $(g.center).outerWidth() - 1;
            if (g.isLeftCollapse) {
                //防止开启滑动后点击左侧收缩后出现的左边丢失问题
                $(".l-layout-collapse-left,.l-layout-left").unbind('mouseenter mouseleave');
                g.center.animate({ left: cleft, width: cwidth }, 10, function () {
                    g.left.animate({ left: barleft }, 300, function () {
                        g.left.hide();
                        g.leftDropHandle && g.leftDropHandle.hide();
                        g.leftCollapse.css({ left: 0 }).show();

                    });

                })
                //g._onResize();
            }
            else {
                g.left.show();
                g.leftCollapse.hide();
                g.left.animate({ left: 0 }, 300, function () {

                    g.leftDropHandle && g.leftDropHandle.show();
                    var cleft = p.space + g.left.outerWidth();
                    var cwidth = g.center.outerWidth() - cleft;
                    $(g.center).css({ left: cleft, width: cwidth });
                    g._onResize();
                });

            }

        },
        //布局左侧普通展开收缩
        setLeftCollapse: function (isCollapse) {
            var g = this, p = this.options;
            if (!g.left) return false;
            g.isLeftCollapse = isCollapse;
            if (g.isLeftCollapse) {
                g.leftCollapse.show();
                g.leftDropHandle && g.leftDropHandle.hide();
                g.left.hide();

            }
            else {
                g.leftCollapse.hide();
                g.leftDropHandle && g.leftDropHandle.show();
                g.left.show();

            }
            g._onResize();
            //修复右侧布局出现的空白宽度
            if (g.left.offset().left < 5)
            {
                //修复点击左侧浮动按钮后，右侧tabmorelist不出现的问题
                if (isCollapse)
                    g.center.width(g.center.outerWidth() + g.leftCollapse.width()-25);
                else
                    g.center.width(g.center.outerWidth()-2);
            }
            
        },
        //布局右侧普通收缩
        setRightCollapse: function (isCollapse) {
            var g = this, p = this.options;
            if (!g.right) return false;
            g.isRightCollapse = isCollapse;
            g._onResize();
           
            if (g.isRightCollapse) {
                g.rightCollapse.show();
                g.rightDropHandle && g.rightDropHandle.hide();
                g.right.hide();
            }
            else {
                g.rightCollapse.hide();
                g.rightDropHandle && g.rightDropHandle.show();
                g.right.show();
            }
            g._onResize();
            //修复右侧布局出现的空白宽度
            
            if (!g.left.is(":visible")) {
               if (!isCollapse)
                    g.center.width(g.center.outerWidth() + g.rightCollapse.width());
                else
                    g.center.width(g.center.outerWidth() - 2);
            }
            
        },
        //增加右侧动画收缩
        setRightAnimateCollapse: function (isCollapse) {
            
            var g = this, p = this.options;
            if (!g.right) return false;
            g.isRightCollapse = isCollapse;
            g.right.css("z-index", 90000);
            var barright = g.right.outerWidth() + g.right.offset().left;
            var cright = p.space + g.rightCollapse.outerWidth();
            var cwidth = g.right.outerWidth() - g.rightCollapse.outerWidth() + $(g.center).outerWidth() - p.space;
            if (g.isRightCollapse) {
                $("body").eq(0).css("overflow", "hidden");
                //防止开启滑动后点击左侧收缩后出现的左边丢失问题
                $(".l-layout-collapse-right,.l-layout-right").unbind('mouseenter mouseleave');
               g.center.animate({ right: cright, width: cwidth }, 10, function () {
                g.right.animate({ left: barright }, 300, function () {
                        g.right.hide();
                        g.rightDropHandle && g.rightDropHandle.hide();
                        g.rightCollapse.css({ right: 0 }).show();
                        g.right.shadow.hide();

                    });
              })
                
            }
            else {
                g.right.show();
                g.rightCollapse.hide();
                g.right.animate({ left: g.right.offset().left-g.right.outerWidth() }, 0, function () {

                    g.rightDropHandle && g.rightDropHandle.show();
                    var cright = p.space + g.right.outerWidth();
                    var cwidth = g.center.outerWidth() - cright;
                    $(g.center).css({ right: cright, width: cwidth });
                    g._onResize();
                    //修复右侧布局出现的空白宽度
                    if (!g.left.is(":visible")) {
                        if (!isCollapse)
                            g.center.width(g.center.outerWidth() + g.rightCollapse.width());
                        else
                            g.center.width(g.center.outerWidth() - p.space);
                    }
                });

            }
        },
        //增加左侧鼠标滑动效果
        setLeftHoverCollapse: function (isCollapse) {
            var g = this, p = this.options;
            g.onHoverCollapse = isCollapse;
            if (!g.left) return;
            //左侧分享
            p.isLeftShare && g.leftShare.css({
                top: g.leftCollapse.outerHeight() / 2 - g.leftShare.height()
            }).show().bind("click.leftshare", function () {
                g.left.css({ "z-index": "15" }).attr("fl", "1").show();
                g.updateLeftShadow(g.left);
            });
            //左侧分享遮盖修复
           p.isLeftShareMaskOcx && g.leftShareIframe.css({ "height": g.leftShare.height(), "width": g.leftShare.width()-1, "top": g.leftShare.css("top"), "left": g.leftShare.css("left") }).show();

            var cleft = g.leftCollapse.outerWidth() * -1+2;
            if (g.left.offset().left > 5)
                cleft = 0;
            if (g.onHoverCollapse) {
             $(".l-layout-header-toggle", g.left.header).hide();
             $("#l-layout-header-lock-left").removeClass("l-layout-header-lock-left").addClass("l-layout-header-unlock-left");
             g.leftCollapse.css({ "cursor": "pointer", "left": cleft });
             p.isCollapseHover && g.leftCollapse.hover(function () {
                 g.left.css({ "z-index": "15" }).attr("fl", "1").show();
                 g.updateLeftShadow(g.left);

             });
             g.left.hover(function () {
                
             }, function () {
                 if ($(this).attr("fl")) {
                     
                     if ($.liger.rightMenuID && $("div[ligerid=" + $.liger.rightMenuID + "]"))
                     {
                         if ($("div[ligerid=" + $.liger.rightMenuID + "]").is(":visible")) return;
                         
                     }
                     g.left.shadow.hide();
                      $(this).hide();
                    }
                    $(this).attr("fl", "");

                })
            } else {
                $("#l-layout-header-lock-left").removeClass("l-layout-header-unlock-left").addClass("l-layout-header-lock-left");
                $(".l-layout-collapse-left,.l-layout-left").unbind('mouseenter mouseleave')
                g.left.shadow.hide();
                $(".l-layout-header-toggle", g.left.header).show();

            }
            g.setLeftCollapse(isCollapse);
           
        },
        //增加右侧鼠标滑动效果
        setRightHoverCollapse: function (isCollapse) {
            var g = this, p = this.options;
            g.onHoverCollapse = isCollapse;
            if (!g.right) return;
            //左侧分享
            p.isRightShare && g.rightShare.css({
                top: g.rightCollapse.outerHeight() / 2 - g.rightShare.height()
            }).show().bind("click.leftshare", function () {
                g.right.attr("style", g.right.attr("style").replace(/left/g, "12").replace(/LEFT/, "12"));
                g.right.css({ right: 0, "z-index": "15" }).attr("fl", "1").show();
                g.updateRightShadow(g.right);
            });
            //右侧分享覆盖
            p.isRightShareMaskOcx && g.rightShareIframe.css({ "height": g.rightShare.height(), "width": g.rightShare.width() - 1, "top": g.rightShare.css("top"), "left":"1","right":0 }).show();
            var cleft = g.rightCollapse.outerWidth() * -1 + 2;
            if (g.left.offset().left > 5)
                cleft = 0;
            if (g.onHoverCollapse) {
                //隐藏箭头功能防止引发的各种其他问题
               $(".l-layout-header-toggle", g.right.header).hide();
                //调用接口改变锁定图标
                $("#l-layout-header-lock-right").removeClass("l-layout-header-lock-right").addClass("l-layout-header-unlock-right");
                g.rightCollapse.css({ "cursor": "pointer", "right": cleft });
                p.isCollapseHover&&g.rightCollapse.hover(function () {
                    //left和right不能共存
                    g.right.attr("style", g.right.attr("style").replace(/left/g, "12").replace(/LEFT/,"12"));
                    g.right.css({ right: 0, "z-index": "15" }).attr("fl", "1").show();
                    g.updateRightShadow(g.right);

                });

                g.right.hover(function () {
                }, function () {
                    if ($(this).attr("fl")) {
                        g.right.shadow.hide();
                        $(this).hide();
                    }
                    $(this).attr("fl", "");
                })
            } else {
                $("#l-layout-header-lock-right").removeClass("l-layout-header-unlock-right").addClass("l-layout-header-lock-right");
                //开启箭头功能
                $(".l-layout-header-toggle", g.right.header).show()
                $(".l-layout-collapse-right,.l-layout-right").unbind('mouseenter mouseleave')
                g.right.shadow.hide();

            }
            g.setRightCollapse(isCollapse);
        },
        _bulid: function () {
            var g = this, p = this.options;
            $("> .l-layout-left .l-layout-header,> .l-layout-right .l-layout-header", g.layout).hover(function () {
                $(this).addClass("l-layout-header-over");
            }, function () {
                $(this).removeClass("l-layout-header-over");

            });
            $(".l-layout-header-toggle", g.layout).hover(function () {
                $(this).addClass("l-layout-header-toggle-over");
            }, function () {
                $(this).removeClass("l-layout-header-toggle-over");

            });
            $(".l-layout-header-toggle", g.left).click(function () {
                
                if (p.isOpenLeftAnimation == true)
                {
                    g.setLeftAnimateCollapse(true);
                    g.left.shadow.hide();
                }
                else
                {
                    g.setLeftCollapse(true);
                }
                //去掉分享按钮
                g.leftShare.hide();
                    
            });
            $(".l-layout-header-toggle", g.right).click(function () {
                
                if (p.isOpenRightAnimation == true)
                {
                    g.setRightAnimateCollapse(true);
                    g.left.shadow.hide();
                }
                else
                {
                    g.setRightCollapse(true);
                }
                //去掉分享按钮
                g.rightShare.hide();

            });
            //set top
            g.middleTop = 0;
            if (g.top) {
                g.middleTop += g.top.height();
                g.middleTop += parseInt(g.top.css('borderTopWidth'));
                g.middleTop += parseInt(g.top.css('borderBottomWidth'));
                g.middleTop += p.space;
            }
            if (g.left) {
                g.left.css({ top: g.middleTop });
                g.leftCollapse.css({ top: g.middleTop });
            }
            if (g.center) g.center.css({ top: g.middleTop });
            if (g.right) {
                g.right.css({ top: g.middleTop });
                g.rightCollapse.css({ top: g.middleTop });
            }
            //set left
            if (g.left) g.left.css({ left: 0 });
            g._onResize();
           
        },
        _setCollapse: function () {
            var g = this, p = this.options;
            g.leftCollapse.hover(function () {
                $(this).addClass("l-layout-collapse-left-over");
            }, function () {
                $(this).removeClass("l-layout-collapse-left-over");
            });
            g.leftCollapse.toggle.hover(function () {
                $(this).addClass("l-layout-collapse-left-toggle-over");
            }, function () {
                $(this).removeClass("l-layout-collapse-left-toggle-over");
            });
            g.rightCollapse.hover(function () {
                $(this).addClass("l-layout-collapse-right-over");
            }, function () {
                $(this).removeClass("l-layout-collapse-right-over");
            });
            g.rightCollapse.toggle.hover(function () {
                $(this).addClass("l-layout-collapse-right-toggle-over");
            }, function () {
                $(this).removeClass("l-layout-collapse-right-toggle-over");
            });
            g.leftCollapse.hover(function () {
                $(this).addClass("l-layout-collapse-left-toggle-over").css("cursor", "pointer");
            }, function () {
                $(this).removeClass("l-layout-collapse-left-toggle-over");
            });
            g.leftCollapse.click(function () {
               //修复收缩状态下能点击边缘边框的问题
                if (!g.left.attr("fl")) return;
                if (g.left.is(":visible")) return;
                if (p.isOpenLeftAnimation == true)
                    g.setLeftAnimateCollapse(false);
                else
                    g.setLeftCollapse(false);
                //去掉分享按钮
                g.leftShare.hide();
                //修复左侧收缩下双击边框浮动按钮的状态
                !g.left.is("visible") && $("#l-layout-header-lock-left").removeClass("l-layout-header-unlock-left").addClass("l-layout-header-lock-left") && g.left.header.toggle.show();

            });
            g.leftCollapse.toggle.click(function () {
                if (p.isOpenLeftAnimation == true)
                   g.setLeftAnimateCollapse(false);
               else
                    g.setLeftCollapse(false);
                //去掉分享按钮
                g.leftShare.hide();
                 
            });
            g.rightCollapse.click(function () {
                //修复收缩状态下能点击边缘边框的问题
                if (!g.right.attr("fl")) return;
                if (p.isOpenRightAnimation == true)
                    g.setRightAnimateCollapse(false);
                else
                    g.setRightCollapse(false);
                //去掉分享按钮
                g.rightShare.hide();
                //修复左侧收缩下双击边框浮动按钮的状态
                !g.right.is("visible") && $("#l-layout-header-lock-right").removeClass("l-layout-header-unlock-right").addClass("l-layout-header-lock-right") && g.right.header.toggle.show();

            });
            g.rightCollapse.toggle.click(function () {
                if (p.isOpenRightAnimation == true)
                    g.setRightAnimateCollapse(false);
                else
                    g.setRightCollapse(false);
                //去掉分享按钮
                g.rightShare.hide();
            });
            if (g.left && g.isLeftCollapse) {
                g.leftCollapse.show();
                g.leftDropHandle && g.leftDropHandle.hide();
                g.left.hide();
            }
            if (g.right && g.isRightCollapse) {
                g.rightCollapse.show();
                g.rightDropHandle && g.rightDropHandle.hide();
                g.right.hide();
            }
        },
        _addDropHandle: function () {
            var g = this, p = this.options;
            if (g.left && p.allowLeftResize) {
                g.leftDropHandle = $("<div class='l-layout-drophandle-left' style='z-index:8901'></div>");
                g.layout.append(g.leftDropHandle);
                g.leftDropHandle && g.leftDropHandle.show();

                g.leftDropHandle.mousedown(function (e) {
                    g._start('leftresize', e);
                    
                });
            }
            if (g.right && p.allowRightResize) {
                g.rightDropHandle = $("<div class='l-layout-drophandle-right'></div>");
                g.layout.append(g.rightDropHandle);
                g.rightDropHandle && g.rightDropHandle.show();
                g.rightDropHandle.mousedown(function (e) {
                    g._start('rightresize', e);
                });
            }
            if (g.top && p.allowTopResize) {
                g.topDropHandle = $("<div class='l-layout-drophandle-top'></div>");
                g.layout.append(g.topDropHandle);
                g.topDropHandle.show();
                g.topDropHandle.mousedown(function (e) {
                    g._start('topresize', e);
                });
            }
            if (g.bottom && p.allowBottomResize) {
                g.bottomDropHandle = $("<div class='l-layout-drophandle-bottom'></div>");
                g.layout.append(g.bottomDropHandle);
                g.bottomDropHandle.show();
                g.bottomDropHandle.mousedown(function (e) {
                    g._start('bottomresize', e);
                });
            }
            g.draggingxline = $("<div class='l-layout-dragging-xline'></div>");
            g.draggingyline = $("<div class='l-layout-dragging-yline'></div>");
            g.layout.append(g.draggingxline).append(g.draggingyline);
        },
        _setDropHandlePosition: function () {
            var g = this, p = this.options;
            if (g.leftDropHandle) {
                g.leftDropHandle.css({ left: g.left.width() + parseInt(g.left.css('left')), height: g.middleHeight, top: g.middleTop });
            }
            if (g.rightDropHandle) {
                g.rightDropHandle.css({ left: parseInt(g.right.css('left')) - p.space, height: g.middleHeight, top: g.middleTop });
            }
            if (g.topDropHandle) {
                g.topDropHandle.css({ top: g.top.height() + parseInt(g.top.css('top')), width: g.top.width() });
            }
            if (g.bottomDropHandle) {
                g.bottomDropHandle.css({ top: parseInt(g.bottom.css('top')) - p.space, width: g.bottom.width() });
            }
        },
        updateLeftShadow: function (menu) {
            var g = this, p = this.options;
            g.left.shadow.css({
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
        updateRightShadow: function (menu) {
            var g = this, p = this.options;
            g.right.shadow.css({
                right:0,
                top: menu.css('top'),
                width: menu.outerWidth(),
                height: menu.outerHeight()

            });
            if (menu.is(":visible"))
                menu.shadow.show();
            else
                menu.shadow.hide();
        },
        _onResize: function () {
            var g = this, p = this.options;
            var oldheight = g.layout.height();
            //set layout height 
            var h = 0;
            var windowHeight = $(window).height();
            var parentHeight = null;
            if (typeof (p.height) == "string" && p.height.indexOf('%') > 0) {
                var layoutparent = g.layout.parent();
                if (p.InWindow || layoutparent[0].tagName.toLowerCase() == "body") {
                    parentHeight = windowHeight;
                    parentHeight -= parseInt($('body').css('paddingTop'));
                    parentHeight -= parseInt($('body').css('paddingBottom'));
                }
                else {
                    parentHeight = layoutparent.height();
                }
                h = parentHeight * parseFloat(p.height) * 0.01;
                if (p.InWindow || layoutparent[0].tagName.toLowerCase() == "body")
                    h -= (g.layout.offset().top - parseInt($('body').css('paddingTop')));
            }
            else {
                h = parseInt(p.height);
            }
            h += p.heightDiff;
            g.layout.height(h);
            g.layoutHeight = g.layout.height();
            g.middleWidth = g.layout.width();
            g.middleHeight = g.layout.height();
            if (g.top) {
                g.middleHeight -= g.top.height();
                g.middleHeight -= parseInt(g.top.css('borderTopWidth'));
                g.middleHeight -= parseInt(g.top.css('borderBottomWidth'));
                g.middleHeight -= p.space;
            }
            if (g.bottom) {
                g.middleHeight -= g.bottom.height();
                g.middleHeight -= parseInt(g.bottom.css('borderTopWidth'));
                g.middleHeight -= parseInt(g.bottom.css('borderBottomWidth'));
                g.middleHeight -= p.space;
            }
            //specific
            g.middleHeight -= 2;

            if (g.hasBind('heightChanged') && g.layoutHeight != oldheight) {

                g.trigger('heightChanged', [{ layoutHeight: g.layoutHeight, diff: g.layoutHeight - oldheight, middleHeight: g.middleHeight}]);
            }

            if (g.center) {
                g.centerWidth = g.middleWidth;
                if (g.left) {
                    if (g.isLeftCollapse) {
                        g.centerWidth -= g.leftCollapse.width();
                        g.centerWidth -= parseInt(g.leftCollapse.css('borderLeftWidth'));
                        g.centerWidth -= parseInt(g.leftCollapse.css('borderRightWidth'));
                        g.centerWidth -= parseInt(g.leftCollapse.css('left'));
                        g.centerWidth -= p.space;
                    }
                    else {
                        g.centerWidth -= g.leftWidth;
                        g.centerWidth -= parseInt(g.left.css('borderLeftWidth'));
                        g.centerWidth -= parseInt(g.left.css('borderRightWidth'));
                        g.centerWidth -= parseInt(g.left.css('left'));
                        g.centerWidth -= p.space;
                    }
                }
                if (g.right) {
                    if (g.isRightCollapse) {
                        g.centerWidth -= g.rightCollapse.width();
                        g.centerWidth -= parseInt(g.rightCollapse.css('borderLeftWidth'));
                        g.centerWidth -= parseInt(g.rightCollapse.css('borderRightWidth'));
                        g.centerWidth -= parseInt(g.rightCollapse.css('right'));
                        g.centerWidth -= p.space;
                    }
                    else {
                        g.centerWidth -= g.rightWidth;
                        g.centerWidth -= parseInt(g.right.css('borderLeftWidth'));
                        g.centerWidth -= parseInt(g.right.css('borderRightWidth'));
                        g.centerWidth -= p.space;
                    }
                }
                g.centerLeft = 0;
                if (g.left) {
                    if (g.isLeftCollapse) {
                        g.centerLeft += g.leftCollapse.width();
                        g.centerLeft += parseInt(g.leftCollapse.css('borderLeftWidth'));
                        g.centerLeft += parseInt(g.leftCollapse.css('borderRightWidth'));
                        g.centerLeft += parseInt(g.leftCollapse.css('left'));
                        g.centerLeft += p.space;
                    }
                    else {
                        g.centerLeft += g.left.width();
                        g.centerLeft += parseInt(g.left.css('borderLeftWidth'));
                        g.centerLeft += parseInt(g.left.css('borderRightWidth'));
                        g.centerLeft += p.space;
                    }
                }
                g.center.css({ left: g.centerLeft });
                g.center.width(g.centerWidth);
                g.center.height(g.middleHeight);
                var contentHeight = g.middleHeight;
                if (g.center.header) contentHeight -= g.center.header.height();
                g.center.content.height(contentHeight);
            }
            if (g.left) {
                g.leftCollapse.height(g.middleHeight);
                g.left.height(g.middleHeight);
            }
            if (g.right) {
                g.rightCollapse.height(g.middleHeight);
                g.right.height(g.middleHeight);
                //set left
                g.rightLeft = 0;

                if (g.left) {
                    if (g.isLeftCollapse) {
                        g.rightLeft += g.leftCollapse.width();
                        g.rightLeft += parseInt(g.leftCollapse.css('borderLeftWidth'));
                        g.rightLeft += parseInt(g.leftCollapse.css('borderRightWidth'));
                        g.rightLeft += p.space;
                    }
                    else {
                        g.rightLeft += g.left.width();
                        g.rightLeft += parseInt(g.left.css('borderLeftWidth'));
                        g.rightLeft += parseInt(g.left.css('borderRightWidth'));
                        g.rightLeft += parseInt(g.left.css('left'));
                        g.rightLeft += p.space;
                    }
                }
                if (g.center) {
                    g.rightLeft += g.center.width();
                    g.rightLeft += parseInt(g.center.css('borderLeftWidth'));
                    g.rightLeft += parseInt(g.center.css('borderRightWidth'));
                    g.rightLeft += p.space;
                }
                g.right.css({ left: g.rightLeft });
            }
            if (g.bottom) {
                g.bottomTop = g.layoutHeight - g.bottom.height() - 2;
                g.bottom.css({ top: g.bottomTop });
            }
            g._setDropHandlePosition();

        },
        _start: function (dragtype, e) {
            var g = this, p = this.options;
            g.dragtype = dragtype;
            if (dragtype == 'leftresize' || dragtype == 'rightresize') {
                g.xresize = { startX: e.pageX };
                g.draggingyline.css({ left: e.pageX - g.layout.offset().left, height: g.middleHeight, top: g.middleTop }).show();
                $('body').css('cursor', 'col-resize');

            }
            else if (dragtype == 'topresize' || dragtype == 'bottomresize') {
                g.yresize = { startY: e.pageY };
                g.draggingxline.css({ top: e.pageY - g.layout.offset().top, width: g.layout.width() }).show();
                $('body').css('cursor', 'row-resize');
            }
            else {
                return;
            }

            g.layout.lock.width(g.layout.width());
            g.layout.lock.height(g.layout.height());
            g.layout.lock.show();
            if ($.browser.msie || $.browser.safari) $('body').bind('selectstart', function () { return false; }); // 不能选择

            $(document).bind('mouseup', function () {
                g._stop.apply(g, arguments);
            });
            $(document).bind('mousemove', function () {
                g._drag.apply(g, arguments);
            });
        },
        _drag: function (e) {
            var g = this, p = this.options;
            if (g.xresize) {
                g.xresize.diff = e.pageX - g.xresize.startX;
                g.draggingyline.css({ left: e.pageX - g.layout.offset().left });
                $('body').css('cursor', 'col-resize');
            }
            else if (g.yresize) {
                g.yresize.diff = e.pageY - g.yresize.startY;
                g.draggingxline.css({ top: e.pageY - g.layout.offset().top });
                $('body').css('cursor', 'row-resize');
            }
        },
        _stop: function (e) {
            var g = this, p = this.options;
            var diff;
            if (g.xresize && g.xresize.diff != undefined) {
                diff = g.xresize.diff;
                if (g.dragtype == 'leftresize') {
                    if (p.minLeftWidth) {
                        if (g.leftWidth + g.xresize.diff < p.minLeftWidth)
                            return;
                    }
                    g.leftWidth += g.xresize.diff;
                    g.left.width(g.leftWidth);
                    if (g.center)
                        g.center.width(g.center.width() - g.xresize.diff).css({ left: parseInt(g.center.css('left')) + g.xresize.diff });
                    else if (g.right)
                        g.right.width(g.left.width() - g.xresize.diff).css({ left: parseInt(g.right.css('left')) + g.xresize.diff });
                }
                else if (g.dragtype == 'rightresize') {
                    if (p.minRightWidth) {
                        if (g.rightWidth - g.xresize.diff < p.minRightWidth)
                            return;
                    }
                    g.rightWidth -= g.xresize.diff;
                    g.right.width(g.rightWidth).css({ left: parseInt(g.right.css('left')) + g.xresize.diff });
                    if (g.center)
                        g.center.width(g.center.width() + g.xresize.diff);
                    else if (g.left)
                        g.left.width(g.left.width() + g.xresize.diff);
                }
            }
            else if (g.yresize && g.yresize.diff != undefined) {
                diff = g.yresize.diff;
                if (g.dragtype == 'topresize') {
                    g.top.height(g.top.height() + g.yresize.diff);
                    g.middleTop += g.yresize.diff;
                    g.middleHeight -= g.yresize.diff;
                    if (g.left) {
                        g.left.css({ top: g.middleTop }).height(g.middleHeight);
                        g.leftCollapse.css({ top: g.middleTop }).height(g.middleHeight);
                    }
                    if (g.center) g.center.css({ top: g.middleTop }).height(g.middleHeight);
                    if (g.right) {
                        g.right.css({ top: g.middleTop }).height(g.middleHeight);
                        g.rightCollapse.css({ top: g.middleTop }).height(g.middleHeight);
                    }
                }
                else if (g.dragtype == 'bottomresize') {
                    g.bottom.height(g.bottom.height() - g.yresize.diff);
                    g.middleHeight += g.yresize.diff;
                    g.bottomTop += g.yresize.diff;
                    g.bottom.css({ top: g.bottomTop });
                    if (g.left) {
                        g.left.height(g.middleHeight);
                        g.leftCollapse.height(g.middleHeight);
                    }
                    if (g.center) g.center.height(g.middleHeight);
                    if (g.right) {
                        g.right.height(g.middleHeight);
                        g.rightCollapse.height(g.middleHeight);
                    }
                }
            }
            g.trigger('endResize', [{
                direction: g.dragtype ? g.dragtype.replace(/resize/, '') : '',
                diff: diff
            }, e]);
            g._setDropHandlePosition();
            g.draggingxline.hide();
            g.draggingyline.hide();
            g.xresize = g.yresize = g.dragtype = false;
            g.layout.lock.hide();
            if ($.browser.msie || $.browser.safari)
                $('body').unbind('selectstart');
            $(document).unbind('mousemove', g._drag);
            $(document).unbind('mouseup', g._stop);
            $('body').css('cursor', '');

        }
    });

})(jQuery);