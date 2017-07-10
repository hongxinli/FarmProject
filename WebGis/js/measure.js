var pointToolbar = null;
function coordInfo() {
    mapTool.deactivateControl();
    mapTool.initImage();
    dojo.byId("measure_coord").src = "images/map/tools/measure_coord_hover.png";

    pointToolbar = new esri.toolbars.Draw(map, { tooltipOffset: 20, drawTime: 90, showTooltips: false });
    dojo.connect(pointToolbar, "onDrawEnd", doCoord);
    pointToolbar.activate(esri.toolbars.Draw.POINT);
    map.setMapCursor("crosshair");
}

var closeEvent;
function doCoord(geometry) {
    var layer = map.getLayer("pointlayer");
    if (typeof (layer) != "undefined") {
        map.removeLayer(layer);
    }

    var pointlayer = new esri.layers.GraphicsLayer({ id: "pointlayer" });
    map.addLayer(pointlayer);

    var markerSymbol = new esri.symbol.PictureMarkerSymbol('images/map/point_coord.png', 10, 16);
    var point = new esri.geometry.Point(geometry.x, geometry.y, map.spatialReference);
    var gra = new esri.Graphic(point, markerSymbol);
    pointlayer.add(gra);

    var content = "X:" + geometry.x + "<br/>Y:" + geometry.y;
    map.infoWindow.resize(210, 200);
    map.infoWindow.setTitle("坐标信息：");
    map.infoWindow.setContent(content);
    map.infoWindow.show(point);
    closeEvent = dojo.connect(map.infoWindow, "onHide", clearLayer);
}

function clearLayer() {
    var layer = map.getLayer("pointlayer");
    if (typeof (layer) != "undefined") {
        map.removeLayer(layer);
    }
    dojo.disconnect(closeEvent);
}

var measureToolbar = null;
var count = 1;
var measurelayer;
function measure_fun(name) {
    mapTool.deactivateControl();
    mapTool.initImage();
    measureToolbar = new esri.toolbars.Draw(map, { tooltipOffset: 20, drawTime: 90, showTooltips: false });
    measureToolbar.on("draw-end", showMeasureResults);

    map.setMapCursor("url(images/map/measure.cur),auto");

    switch (name) {
        case "length":
            dojo.byId("measure_length").src = "images/map/tools/measure_length_hover.png";
            measureToolbar.activate(esri.toolbars.Draw.POLYLINE);
            break;
        case "area":
            dojo.byId("measure_area").src = "images/map/tools/measure_area_hover.png";
            measureToolbar.activate(esri.toolbars.Draw.POLYGON);
            break;
    }
}
function showMeasureResults(evt) {
    measurelayer = new esri.layers.GraphicsLayer({ id: "measurelayer" + count });
    map.addLayer(measurelayer);
    var geometry = evt.geometry;
    var symbol = null;
    switch (geometry.type) {
        case "polyline": {
            var length = geometry.paths[0].length;
            showPt = new esri.geometry.Point(geometry.paths[0][length - 1], map.spatialReference);
            var lengthParams = new esri.tasks.LengthsParameters();
            lengthParams.lengthUnit = esri.tasks.GeometryService.UNIT_KILOMETER;
            lengthParams.polylines = [geometry];
            geometryService.lengths(lengthParams);
            symbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([100, 100, 100]), 3);
            break;
        }
        case "polygon": {
            showPt = new esri.geometry.Point(geometry.rings[0][0], map.spatialReference);
            var areasAndLengthParams = new esri.tasks.AreasAndLengthsParameters();
            areasAndLengthParams.lengthUnit = esri.tasks.GeometryService.UNIT_KILOMETER;
            areasAndLengthParams.areaUnit = esri.tasks.GeometryService.UNIT_SQUARE_KILOMETERS;
            geometryService.simplify([geometry], function (simplifiedGeometries) {
                areasAndLengthParams.polygons = simplifiedGeometries;
                geometryService.areasAndLengths(areasAndLengthParams);
            });
            symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([100, 100, 100]), 2), new dojo.Color([255, 255, 0, 0.25]));
            break;
        }
    }
    var graphic = new esri.Graphic(geometry, symbol);
    measurelayer.add(graphic);
}

function outputLength(evtObj) {
    var result = evtObj.result;
    showmeasureInfo(showPt, result.lengths[0].toFixed(3), "千米", count);
};

function outputAreaAndLength(evtObj) {
    var result = evtObj.result;
    showmeasureInfo(showPt, result.areas[0].toFixed(3), "平方千米", count);
};

function showmeasureInfo(showPnt, data, unit, currCount) {
    count++;
    var measureDiv = $('<div id="measure' + currCount + '" class="measure"><div id="result' + currCount + '" class="result" style="padding:0"></div><div id="infoclose' + currCount + '" onclick="close_result(' + currCount + ')" class="infoclose"><img src="images/map/measure_close.png" title="关闭" style="cursor: pointer;" /></div></div>');
    var isShow = false;
    var screenPnt = map.toScreen(showPnt);
    measureDiv.css("left", screenPnt.x - 30 + "px");
    measureDiv.css("top", screenPnt.y + 20 + "px");
    measureDiv.css("position", "absolute");
    measureDiv.css("height", "25px");
    measureDiv.css("line-height", "25px");
    measureDiv.css("z-index", "1");
    measureDiv.css("display", "block");

    $("#measures").append(measureDiv);

    $("#result" + currCount).html("" + data + unit);

    map.on("pan-start", function () {
        measureDiv.css("display", "none");
    });

    map.on("pan-end", function (panend) {
        screenPnt = map.toScreen(showPnt);
        measureDiv.css("left", screenPnt.x - 30 + "px");
        measureDiv.css("top", screenPnt.y + 20 + "px");
        measureDiv.css("position", "absolute");
        measureDiv.css("height", "25px");
        measureDiv.css("line-height", "25px");
        measureDiv.css("display", "block");
    });
    map.on("zoom-start", function () {
        measureDiv.css("display", "none");
    });
    map.on("zoom-end", function () {
        screenPnt = map.toScreen(showPnt);
        measureDiv.css("left", screenPnt.x - 30 + "px");
        measureDiv.css("top", screenPnt.y + 20 + "px");
        measureDiv.css("position", "absolute");
        measureDiv.css("height", "25px");
        measureDiv.css("line-height", "25px");
        measureDiv.css("display", "block");
    });
};

function close_result(currCount) {
    var layer = map.getLayer("measurelayer" + currCount);
    if (layer != null) {
        map.removeLayer(layer);
    }
    $("#measure" + currCount).remove();
}
