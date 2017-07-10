//控制工具栏选中状态

var navDrawToolbar = null;
function navigationBar(navId) {
    mapTool.deactivateControl();

    mapTool.initImage();

    navToolbar.activate(esri.toolbars.Navigation.PAN);
    switch (navId) {
        case "zoomfull":
            navToolbar.zoomToFullExtent();
            break;
        case "zoomprev":
            navToolbar.zoomToPrevExtent();
            break;
        case "zoomnext":
            navToolbar.zoomToNextExtent();
            break;
        case "zoompan":
            dojo.byId("zoompan").src = "images/map/tools/zoompan_hover.png";
            navToolbar.deactivate();
            navToolbar.activate(esri.toolbars.Navigation.PAN);
            break;
        default:
            navToolbar.deactivate();
            break;
    }
}
//地图范围改变时
function extentHistoryChangeHandler() {
    dojo.byId("zoom_backward").disabled = navToolbar.isFirstExtent();
    dojo.byId("zoom_forward").disabled = navToolbar.isLastExtent();
}
function controlNav(navId) {
    mapTool.deactivateControl();
    mapTool.initImage();
    dojo.byId(navId).src = "images/map/tools/" + navId + "_ON.png";
    dojo.byId("hid_navId").value = navId;
    // 放大缩小
    navDrawToolbar = new esri.toolbars.Draw(map, { tooltipOffset: 20, drawTime: 90, showTooltips: false });
    dojo.connect(navDrawToolbar, "onDrawEnd", controlExtent);
    navDrawToolbar.activate(esri.toolbars.Draw.EXTENT);
    map.setMapCursor("crosshair");
}

//自定义放大与缩小功能
function controlExtent(geometry) {
    var navId = dojo.byId("hid_navId").value;
    switch (navId) {
        case "zoomin":
            var newExtent = new esri.geometry.Extent(geometry.xmin, geometry.ymin, geometry.xmax, geometry.ymax, map.spatialReference);
            map.setExtent(newExtent);
            break;
        case "zoomout":
            var currentExtent = map.extent;
            var zoomBoxExtent = new esri.geometry.Extent(geometry.xmin, geometry.ymin, geometry.xmax, geometry.ymax, map.spatialReference);
            var zoomBoxCenter = zoomBoxExtent.getCenter();
            var whRatioCurrent = parseFloat(currentExtent.getWidth()) / parseFloat(currentExtent.getHeight());
            var whRatioZoomBox = parseFloat(zoomBoxExtent.getWidth()) / parseFloat(zoomBoxExtent.getHeight());
            var multiplier = null;
            if (whRatioZoomBox > whRatioCurrent) {
                multiplier = currentExtent.getWidth() / zoomBoxExtent.getWidth();
            } else {
                multiplier = currentExtent.getHeight() / zoomBoxExtent.getHeight();
            }
            var newWidthMapUnits = currentExtent.getWidth() * multiplier;
            var newHeightMapUnits = currentExtent.getHeight() * multiplier;
            var newPoint1 = new esri.geometry.Point(zoomBoxCenter.x - (newWidthMapUnits / 2), zoomBoxCenter.y - (newHeightMapUnits / 2));
            var newPoint2 = new esri.geometry.Point(zoomBoxCenter.x + (newWidthMapUnits / 2), zoomBoxCenter.y + (newHeightMapUnits / 2));
            var newEnv = null;
            if (whRatioZoomBox > whRatioCurrent) {
                newEnv = new esri.geometry.Extent(newPoint2.x, newPoint2.y, newPoint1.x, newPoint1.y, map.spatialReference);
            } else {
                newEnv = new esri.geometry.Extent(newPoint1.x, newPoint1.y, newPoint2.x, newPoint2.y, map.spatialReference);
            }
            if (newEnv != null) {
                map.setExtent(newEnv);
            }
            break;

    }
    dojo.byId(navId).src = "images/map/tools/" + navId + "_OFF.png";
}

//清除缓存图片操作
function clearGraphs() {
    map.infoWindow.hide();
    map.graphics.clear();
    mapTool.deactivateControl();
    mapTool.initImage();
}

//显示鼠标坐标
function showCoordinates(event) {
    var mp = event.mapPoint;
    var mp2 = event.screenPoint;
    dojo.byId("coordinates").innerHTML = "地理坐标X：" + changeTwoDecimal(mp.x) + ",地理坐标Y：" + changeTwoDecimal(mp.y);
}
function changeTwoDecimal(x) {
    var f_x = parseFloat(x);
    if (isNaN(f_x)) {
        alert('function:changeTwoDecimal->parameter error');
        return false;
    }
    var f_x = Math.round(x * 100) / 100;

    return f_x;
}

function unique(data) {
    data = data || [];
    var a = {};
    for (var i = 0; i < data.length; i++) {
        var v = data[i];
        if (typeof (a[v]) == 'undefined') {
            a[v] = 1;
        }
    };
    data.length = 0;
    for (var i in a) {
        data[data.length] = i;
    }
    return data;
}

// 退出系统
function exitFun() {
    if (confirm("您确定要退出系统吗？")) {
        window.location.href = "LoginPage.aspx";
    }
}

var cityNav = {
    settings: {
        version: "1.0.0.0",
        title: "行政区域导航",
        description: "行政区域导航面板信息获取、关闭、定位"
    },
    //获取安徽省行政区域内容
    showNavContainer: function () {
        $("#navContainer").empty();
        dojo.byId("navQuery").style.visibility = "visible";
        var queryTask = new esri.tasks.QueryTask(baseLayerUrl + "/0");
        var query = new esri.tasks.Query();
        query.where = "1=1";
        query.returnGeometry = false;
        queryTask.execute(query, function (results) {
            var htmlStr = "";
            for (var i = 1; i < results.features.length; i++) {
                var city = results.features[i - 1].attributes['city'];
                htmlStr = htmlStr + "<a onclick=cityNav.cityLocation('" + city + "')>" + city + "</a>";
                if (i % 15 == 0)
                    htmlStr = htmlStr + "<hr/>";
                $("#navContainer").html(htmlStr);
            }
        });
    },
    //城市定位
    cityLocation: function (msg) {
        var queryTask = new esri.tasks.QueryTask(baseLayerUrl + "/0");
        var query = new esri.tasks.Query();
        query.where = "city='" + msg + "'";
        query.returnGeometry = true;
        queryTask.execute(query, function (results) {
            map.graphics.clear();
            var symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASH, new dojo.Color([255, 0, 0]), 2), new dojo.Color([255, 255, 0, 0.25]));
            var graphic = results.features[0];
            graphic.setSymbol(symbol);
            map.graphics.add(graphic);
            map.setExtent(graphic.geometry.getExtent());
        });
    },
    //关闭面板
    closeNavContainer: function () {
        dojo.byId("navQuery").style.visibility = "hidden";
    }
}
var toolbar = null;
var mapTool = {
    settings: {
        version: "1.0.0.0",
        title: "地图工具栏",
        description: "地图工具栏包括放大、缩小、全图、图形化查询"
    },
    //初始化(还原)工具条图片
    initImage: function () {
        var image = "zoom_in,zoom_out,zoom_full_extent,zoom_pan,zoom_backward,zoom_forward,measure_location,measure_length,measure_area,identify_point,identify_line,identify_rect,identify_circle,identify_poly,eraser";
        var images = image.split(',');
        for (var i = 0; i < images.length; i++) {
            dojo.byId(images[i]).src = "images/map/tools/" + images[i] + "_OFF.png";
        }
    },
    //取消导航、绘制、测量等状态，将地图还原为浏览状态
    deactivateControl: function () {
        if (navToolbar != null) {
            navToolbar.deactivate();
        }
        if (navDrawToolbar != null) {
            navDrawToolbar.deactivate();
        }
        if (pointToolbar != null) {
            pointToolbar.deactivate();
        }
        if (measureToolbar != null) {
            measureToolbar.deactivate();
        }
        if (toolbar != null) {
            toolbar.deactivate();
        }
        map.setMapCursor("default");
    },
    //激活按钮
    activeButton: function (buttonName) {
        mapTool.deactivateControl();
        mapTool.initImage();

        toolbar = new esri.toolbars.Draw(map, { drawTime: 90, showTooltips: false });

        map.setMapCursor("crosshair");
        switch (buttonName) {
            case "point":
                dojo.byId("query_point").src = "images/map/tools/query_point_hover.png";
                toolbar.activate(esri.toolbars.Draw.POINT);
                toolbar.on("draw-end", mapTool.fertilizerDone);
                break;
            case "line":
                dojo.byId("query_line").src = "images/map/tools/query_line_hover.png";
                toolbar.activate(esri.toolbars.Draw.POLYLINE);
                toolbar.on("draw-end", mapTool.fertilizerDone);
                break;
            case "extent":
                dojo.byId("query_extent").src = "images/map/tools/query_extent_hover.png";
                toolbar.activate(esri.toolbars.Draw.EXTENT);
                toolbar.on("draw-end", mapTool.fertilizerDone);
                break;
            case "polygon":
                dojo.byId("query_polygon").src = "images/map/tools/query_polygon_hover.png";
                toolbar.activate(esri.toolbars.Draw.POLYGON);
                toolbar.on("draw-end", mapTool.fertilizerDone);
                break;
        }
    },
    //精准施肥
    fertilizerDone: function (evt) {
        var geometry = evt.geometry;
        //设置字体样式
        mapTool.setSymbol(geometry);
        //图形化查询方法
        mapTool.identifyTask(geometry);
    },
    //精准施肥面板展示
    fertilizerShow: function (idResults) {
        for (var i = 0; i < idResults.length; i++) {
            //$("#tabContainer").html(idResults[0].feature.attributes.FID);
        }
        dojo.byId("fertilizerContainer").style.visibility = "visible";
    },
    //精准施肥面板关闭
    fertilizerClose: function () {
        dojo.byId("fertilizerContainer").style.visibility = "hidden";
    },
    //设置字体样式
    setSymbol: function (geometry) {
        var symbol;
        switch (geometry.type) {
            case "polyline":
                symbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2);
                break;
            case "extent":
                symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2), new dojo.Color([255, 255, 0, 0.25]));
                break;
            case "polygon":
                symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2), new dojo.Color([255, 255, 0, 0.25]));
                break;
        }
        var graphic = new esri.Graphic(geometry, symbol);
        map.graphics.clear();
        map.graphics.add(graphic);
    },
    //图形化查询方法
    identifyTask: function (geometry) {
        var identifyParams = new esri.tasks.IdentifyParameters();
        identifyParams.tolerance = 3;
        identifyParams.geometry = geometry;
        identifyParams.mapExtent = map.extent;
        identifyParams.width = parseInt(map.width);
        identifyParams.height = parseInt(map.height);
        identifyParams.spatialReference = map.spatialReference;
        identifyParams.layerOption = esri.tasks.IdentifyParameters.LAYER_OPTION_VISIBLE;
        identifyParams.returnGeometry = true;
        identifyParams.layerIds = mapTool.getVisibleLayers();// 数据地图的显示图层
        var identifyTask = new esri.tasks.IdentifyTask(dataLayerUrl);
        identifyTask.execute(identifyParams, function (idResults) {
            if (idResults.length > 0) {
                for (var i = 0; i < idResults.length; i++) {
                    mapTool.setSymbol(idResults[i].feature.geometry);
                }
                mapTool.fertilizerShow(idResults);
            } else {
                alert("没有查到元素...");
            }
        });
    },
    //获取可视图层
    getVisibleLayers: function () {
        var visibleLayers = dataLayer.visibleLayers;
        var layerIds = [];
        var layers = dataLayer.layerInfos;
        for (var i = 0; i < layers.length; i++) {
            var info = layers[i];
            if (info.subLayerIds == null) {
                for (var j = 0; j < visibleLayers.length; j++) {
                    if (visibleLayers[j] == info.id) {
                        layerIds.push(info.id);
                    }
                }
            }
        }
        return layerIds;
    }
}
//常用方法
var common = {
    settings: {
        version: "1.0.0.0",
        title: "js常用方法",
        description: "json日期"
    },
    //json格式后日期(毫秒数)转成日期格式
    dateFormat: function (cellval, type) {
        var date = new Date(cellval);
        var year = date.getFullYear();
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var min = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var sec = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        var result;
        switch (type) {
            case "0":
                result = year + "-" + month + "-" + day;
                break;
            case "1":
                result = hours + ":" + min + ":" + sec;
                break;
            case "2":
                result = year + "-" + month + "-" + day + " " + hours + ":" + min + ":" + sec;
                break;
            case "3":
                result = hours + ":" + min;
                break;
            default: "不符合格式"

        }
        return result;
    }
}