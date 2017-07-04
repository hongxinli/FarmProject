//控制工具栏选中状态

var navDrawToolbar = null;
function navigationBar(navId) {
    deactivateControl();

    initImage();

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
    dojo.byId("zoomprev").disabled = navToolbar.isFirstExtent();
    dojo.byId("zoomnext").disabled = navToolbar.isLastExtent();
}
function controlNav(navId) {
    deactivateControl();
    initImage();
    dojo.byId(navId).src = "images/map/tools/" + navId + "_hover.png";
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
    dojo.byId(navId).src = "images/map/tools/" + navId + ".png";
}

//清除缓存图片操作
function clearGraphs() {
    map.infoWindow.hide();
    map.graphics.clear();
    deactivateControl();
    initImage();
}

// 设置图片为初始状态图片
function initImage() {
    var image = "zoompan,zoomin,zoomout,measure_coord,measure_length,measure_area,query_point,query_line,query_extent,query_polygon";
    var images = image.split(',');
    for (var i = 0; i < images.length; i++) {
        dojo.byId(images[i]).src = "images/map/tools/" + images[i] + ".png";
    }
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
// 每个工具执行之前，取消导航和取消所有绘制工具
function deactivateControl() {
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
    if (queryTool != null) {
        queryTool.deactivate();
    }
    map.setMapCursor("default");
}

function DeleteTab(id) {
    //var con = dijit.byId(id);
    //dojo.forEach(con.getChildren(), function (child) {
    //    if (!child.selected) {
    //        con.removeChild(child);
    //    }
    //});
    //con.destroyDescendants();
    //关闭所有的tab标签
    navtab.removeAll();
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