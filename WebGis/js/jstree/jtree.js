(function (doc, undefined) {
    var window = this;
    window.Sys = function (ua) {
        var b = {
            ie: /msie/.test(ua) && !/opera/.test(ua),
            opera: /opera/.test(ua),
            safari: /webkit/.test(ua) && !/chrome/.test(ua),
            firefox: /firefox/.test(ua),
            chrome: /chrome/.test(ua)
        }, vMark = "";
        for (var i in b) {
            if (b[i]) { vMark = "safari" == i ? "version" : i; break; }
        }
        b.version = vMark && RegExp("(?:" + vMark + ")[\\/: ]([\\d.]+)").test(ua) ? RegExp.$1 : "0";
        b.ie6 = b.ie && parseInt(b.version, 10) == 6;
        b.ie7 = b.ie && parseInt(b.version, 10) == 7;
        b.ie8 = b.ie && parseInt(b.version, 10) == 8;
        return b;
    }(window.navigator.userAgent.toLowerCase());
    window.Sys.ie6 && doc.execCommand("BackgroundImageCache", false, true);
    window.$$ = function (Id) {
        return doc.getElementById(Id);
    };
    window.$q = function (name, parent) {
        return parent.getElementsByTagName(name);
    };
    window.$c = function (name, parent) {
        var elem = typeof name === 'object' ? name : doc.createElement(name);
        parent && parent.appendChild(elem);
        return elem;
    };
    window.addListener = function (element, e, fn) {
        !element.events && (element.events = {});
        element.events[e] && (element.events[e][addListener.guid++] = fn) || (element.events[e] = { '0': fn });
        element.addEventListener ? element.addEventListener(e, fn, false) : element.attachEvent("on" + e, fn);
    };
    window.addListener.guid = 1;
    window.removeListener = function (element, e, fn) {
        var handlers = element.events[e], type;
        if (fn) {
            for (type in handlers)
                if (handlers[type] === fn) {
                    element.removeEventListener ? element.removeEventListener(e, fn, false) : element.detachEvent("on" + e, fn);
                    delete handlers[type];
                }
        } else {
            for (type in handlers) {
                element.removeEventListener ? element.removeEventListener(e, handlers[type], false) : element.detachEvent("on" + e, handlers[type]);
                delete handlers[type];
            }
        }
    };
    window.setStyle = function (e, o) {
        if (typeof o == "string")
            e.style.cssText = o;
        else
            for (var i in o)
                e.style[i] = o[i];
    };
    var slice = Array.prototype.slice;
    window.Bind = function (object, fun) {
        var args = slice.call(arguments).slice(2);
        return function () {
            return fun.apply(object, args);
        };
    };
    window.BindAsEventListener = function (object, fun, args) {
        var args = slice.call(arguments).slice(2);
        return function (event) {
            return fun.apply(object, [event || window.event].concat(args));
        }
    };
    //copy from jQ
    window.extend = function () {
        var target = arguments[0] || {}, i = 1, length = arguments.length, deep = true, options;
        if (typeof target === "boolean") {
            deep = target;
            target = arguments[1] || {};
            i = 2;
        }
        if (typeof target !== "object" && Object.prototype.toString.call(target) != "[object Function]")
            target = {};
        for (; i < length; i++) {
            if ((options = arguments[i]) != null)
                for (var name in options) {
                    var src = target[name], copy = options[name];
                    if (target === copy)
                        continue;
                    if (deep && copy && typeof copy === "object" && !copy.nodeType) {
                        target[name] = arguments.callee(deep, src || (copy.length != null ? [] : {}), copy);
                    }
                    else if (copy !== undefined)
                        target[name] = copy;
                }
        }
        return target;
    };
    //copy from jQ
    window.each = function (object, callback, args) {
        var name, i = 0, length = object.length;
        if (args) {
            args = Array.prototype.slice.call(arguments).slice(2);
            if (length === undefined) {
                for (name in object)
                    if (callback.apply(object[name], [name, object[name]].concat(args)) === false)
                        break;
            } else
                for (; i < length; i++)
                    if (callback.apply(object[i], [i, object[i]].concat(args)) === false)   //
                        break;
        } else {
            if (length === undefined) {
                for (name in object)
                    if (callback.call(object[name], name, object[name]) === false)
                        break;
            } else
                for (var value = object[0];
                    i < length && callback.call(value, i, value) !== false; value = object[++i]) { }
        }
        return object;
    };
    window.currentStyle = function (element) {
        return element.currentStyle || doc.defaultView.getComputedStyle(element, null);
    };
    window.objPos = function (elem) {
        var left = 0, top = 0, right = 0, bottom = 0, doc = elem ? elem.ownerDocument : doc;
        if (!elem.getBoundingClientRect || window.Sys.ie8) {
            var n = elem;
            while (n) { left += n.offsetLeft, top += n.offsetTop; n = n.offsetParent; };
            right = left + elem.offsetWidth; bottom = top + elem.offsetHeight;
        } else {
            var rect = elem.getBoundingClientRect();
            left = right = doc.documentElement.scrollLeft || doc.body.scrollLeft;
            top = bottom = doc.documentElement.scrollLeft || doc.body.scrollLeft;
            left += rect.left; right += rect.right;
            top += rect.top; bottom += rect.bottom;
        }
        return { "left": left, "top": top, "right": right, "bottom": bottom };
    };
    window.hasClass = function (element, className) {
        return element.className.match(new RegExp('(\\s|^)' + className + '(\\s|$)'));
    };
    window.addClass = function (element, className) {
        !window.hasClass(element, className) && (element.className += " " + className);
    };
    window.removeClass = function (element, className) {
        window.hasClass(element, className) && (element.className = element.className.replace(new RegExp('(\\s|^)' + className + '(\\s|$)'), ' '));
    }
})(document);
//------------------------------------------------------------------------------------
(function (doc, undefined) {
    var win = this,
        primary;
    win.easyTree = function () {
        this.init.apply(this, arguments);
    };
    win.easyTree.prototype = {
        options: {
            primary: 'id',
            isInput: false,
            isIco: false
        },
        init: function (options) {
            this.deflaut = extend(this.options, options);
            this.container = options.container;
            //数据源 形式{id:data[i]}
            this.data = {};
            //被勾选的项 
            this.selected = {};
            var ids = map.getLayer("dataLayer").visibleLayers;
            for (var i = 0; i < ids.length; i++) {
                this.selected[ids[i]] = ids[i];
            }
            //根节点直接在这里生成		
            if (options.data.length >= 1) {
                primary = this.deflaut.primary;
                var frag = document.createDocumentFragment()
                var ul = $c('ul', frag),//this.container
                    self = this,
                    input = this.deflaut.isInput ? '<input type="checkbox">' : '',
                    li;
                each(options.data, function (i, o) {
                    li = $c('li', ul);
                    li.className = 'root';
                    li.innerHTML = [
                        '<span class="tvicon-open"></span>',
                        input,
                        '<a href="#">' + o.name + '</a>'
                    ].join('');
                    li.setAttribute('primary', o[primary]);
                    self.data[o[primary]] = { elem: li, data: o };
                    o.child
                        && self.bulidTree(o.child, li, 't');
                });
                //只绑定一个节点 展开和checkedbox判断都在里面
                addListener(ul, 'click', BindAsEventListener(self, self.operation));
                this.container.appendChild(frag);
            }
        },
        bulidTree: function (data, parent, first) {
            if (data.length === 0)
                return;
            var ul = $c('ul', parent),
                self = this,
                len = data.length,
                isInput = this.deflaut.isInput,
                input = isInput ? '<input type="checkbox">' : '',
                isIco = this.deflaut.isIco,
                imgClassName,
                img,
                li;
            first
                ? ul.style.paddingLeft = '0px'
                : ul.style.display = 'none';
            parent.child = [];
            each(data, function (i, o) {
                var isLast = (len - 1) === i,
                    ico = isIco
                        ? o.child
                            ? '<span class="tvicon-f"></span>'
                            : '<span ><img src="' + o.img + '" style="width:20px;height:20px;" /></span>'
                        : '';
                input = isInput ? o.checked ? '<input checked="checked" type="checkbox">' : '<input type="checkbox">' : '';
                if (o.checked)
                    self.selected[o.id] = o.id;
                li = $c('li', ul);
                li.className = 'folder' + (isLast ? 'l' : '');
                imgClassName = o.child
                    ? isLast ? 'tvdash-fl' : 'tvdash-f'
                    : isLast ? 'tvdash-tl' : 'tvdash-t';
                li.innerHTML = [
                    '<span class="' + imgClassName + '"></span>',
                    ico,
                    input,
                    '<a href="#">' + o.name + '</a>'
                ].join('');
                //当前li保存父li的相关信息 只是一个对象
                li.parent = {
                    li: parent,
                    input: isInput ? $q('input', parent)[0] : undefined
                };
                //当前li保存子li的相关信息 是一个数组
                parent.child.push({
                    li: li,
                    input: isInput ? $q('input', li)[0] : undefined,
                    data: o
                });
                li.setAttribute('primary', o[primary]);
                li.getElementsByTagName('span')[0].setAttribute('mark', 'mark');
                self.data[o[primary]] = { elem: li, data: o };
                o.child
                    && self.bulidTree(o.child, li);
            });
        },
        operation: function (e) {
            var elem = e.srcElement || e.target,
                nodeName = elem.nodeName.toLocaleLowerCase();
            /*显示隐藏子菜单*/
            if ((nodeName === 'span' && elem.getAttribute('mark')) || (nodeName === 'a')) {
                var li = elem.parentNode,
                    ul = $q('ul', li)[0],
                    img = $q('span', li)[0],
                    ico = $q('span', li)[1];
                if (ul) {
                    ul.style.display = ul.style.display === 'none' ? '' : 'none';
                    var name = img.className,
                        is = img.className.indexOf('open') > -1;
                    img.className = is ? name.replace('-open', '') : name + '-open';
                    if (ico) {
                        ico.className = is ? ico.className.replace('-open', '') : ico.className + '-open';
                    }
                }
            };
            //选中checkedbox的时候的相应的操作
            if (nodeName === 'input') {
                var isCheck = elem.checked,
                    li = elem.parentNode,
                    ul = $q('ul', li)[0],
                    id = li.getAttribute('primary'),
                    //该元素的索引是否已经添加到了 this.selected中
                    isIn = id in this.selected;

                //操作this.selected 从里面添加或者删除
                isCheck
                    ? !isIn
                        && (this.selected[id] = id)
                    : isIn
                        && delete this.selected[id];
                this.findChild(li, isCheck);
                this.findParent(li, isCheck);
                //实现图层控制
                map.getLayer("dataLayer").setVisibleLayers(this.getAllSelect());
            }
        },
        findParent: function (child, b) {
            if (!child.parent)
                return;
            var parentLi = child.parent.li,
                id = parentLi.getAttribute('primary')
            isAll = true;
            if (b) {
                //看他父下面的所有li是不是都是选中状态的
                each(parentLi.child, function (i, o) {
                    if (!o.input.checked) {
                        isAll = false;
                        return false;
                    }
                });
                //如果都是选中状态
                //继续查找他的父
                if (isAll) {
                    child.parent.input.checked = true;
                    !(id in this.selected)
                        && (this.selected[id] = id);
                    this.findParent(parentLi, b);
                }
            } else {
                if (child.parent.input.checked) {
                    child.parent.input.checked = false;
                    id in this.selected
                            && delete this.selected[id];
                    this.findParent(parentLi, b);
                }
            }
        },
        /*
        查找子节点 将所有没有选中的字节点选中  并放到this.selected里面去
        或者取消所有的子节点
        */
        findChild: function (parent, b) {
            if (!parent.child)
                return;
            var self = this;
            b
                ? each(parent.child, function (i, o) {
                    if (!o.input.checked) {
                        o.input.checked = true;
                        !(o.data[primary] in self.selected)
                            && (self.selected[o.data[primary]] = o.data[primary]);
                        o.li.child
                            && self.findChild(o.li, b);
                    }
                })
                : each(parent.child, function (i, o) {
                    if (o.input.checked) {
                        o.input.checked = false;
                        o.data[primary] in self.selected
                            && delete self.selected[o.data[primary]];
                        o.li.child
                            && self.findChild(o.li, b);
                    }
                });
        },
        getAllSelect: function () {
            var arr = [-1],
                id;
            for (id in this.selected)
                arr.push(id);
            return arr;
        },
        openNode: function (id) {
            if (id in this.data) {
                var li = this.data[id].elem,
                    parent = li.parentNode,
                    img,
                    ico;
                while (parent.style.display === 'none') {
                    parent.style.display = '';
                    li = li.parent.li;
                    img = $q('span', li)[0];
                    ico = $q('span', li)[1];
                    img.className = img.className + '-open';
                    ico
                        && (ico.className = ico.className + '-open');
                    parent = li.parentNode;
                }
            }
        }
    }
})(document);