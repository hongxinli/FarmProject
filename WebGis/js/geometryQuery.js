var queryTool = null;
// 查询激活事件
function queryEvent(name) {
    deactivateControl();
    initImage();
    // 信息查询
    queryTool = new esri.toolbars.Draw(map, { drawTime: 90, showTooltips: false });
    queryTool.on("draw-end", doQuery);

    map.setMapCursor("crosshair");
    switch (name) {
        case "point":
            dojo.byId("query_point").src = "images/map/tools/query_point_hover.png";
            queryTool.activate(esri.toolbars.Draw.POINT);
            break;
        case "line":
            dojo.byId("query_line").src = "images/map/tools/query_line_hover.png";
            queryTool.activate(esri.toolbars.Draw.POLYLINE);
            break;
        case "extent":
            dojo.byId("query_extent").src = "images/map/tools/query_extent_hover.png";
            queryTool.activate(esri.toolbars.Draw.EXTENT);
            break;
        case "polygon":
            dojo.byId("query_polygon").src = "images/map/tools/query_polygon_hover.png";
            queryTool.activate(esri.toolbars.Draw.POLYGON);
            break;
    }
}
// 设置查询图形样式
function doQuery(evt) {
    var geometry = evt.geometry;
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
    queryInfo(geometry);
}

// 图形查询方法
function queryInfo(geometry) {
    var identifyParams = new esri.tasks.IdentifyParameters();
    identifyParams.tolerance = 3;
    identifyParams.geometry = geometry;
    identifyParams.mapExtent = map.extent;
    identifyParams.width = parseInt(map.width);
    identifyParams.height = parseInt(map.height);
    identifyParams.spatialReference = map.spatialReference;
    identifyParams.layerOption = esri.tasks.IdentifyParameters.LAYER_OPTION_VISIBLE;
    identifyParams.returnGeometry = true;
    identifyParams.layerIds = readVisibleLayers();// 数据地图的显示图层
    var identifyTask = new esri.tasks.IdentifyTask(dataLayerUrl);
    identifyTask.execute(identifyParams, function (idResults) {
        if (idResults.length > 0) {
            identifyTaskCallFuc(idResults);
        } else {
            alert("没有查到元素...");
        }
    });
}

function readVisibleLayers() {
    var visibleLayers = dataLayer.visibleLayers;
    var layerIds = [];
    var layers = dataLayer.layerInfos;
    for (var i = 0; i < layers.length; i++) {
        var info = layers[i];
        if (info.subLayerIds == null) {
            for (var j = 0; j < visibleLayers.length; j++) {
                if (visibleLayers[j] == info.id) {
                    if (info.name == "地理信息_WL_line" || info.name == "地理信息_WP_region") {
                        continue;
                    }
                    layerIds.push(info.id);
                }
            }
        }
    }
    return layerIds;
}

// 查询返回结果方法
function identifyTaskCallFuc(idResults) {
    var layerList = [];
    for (var i = 0; i < idResults.length; i++) {
        var graphic = idResults[i].feature;
        layerList.push(idResults[i].layerName);
        var symbol;
        switch (graphic.geometry.type) {
            case "point":
                symbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_CIRCLE, 10, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2), new dojo.Color("yellow"));
                break;
            case "polyline":
                symbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 255]), 3);
                break;
            case "polygon":
                symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASH, new dojo.Color([255, 0, 0]), 2), new dojo.Color([255, 255, 0, 0.25]));
                break;
        }
        graphic.setSymbol(symbol);
        map.graphics.add(graphic);
    }
    DeleteTab('tabContainer');//创建新的内容之前先删除原来的
    var results = unique(layerList);//去掉重复图层
    for (var i = 0; i < results.length; i++) {
        var count = GetCountByLayerName(results[i], idResults);
        var html = "<div style=\"margin-top:5px;\">共查询到<span style=\"color:red; font-weight:800;\">" + count + "</span>记录</div>";
        html += "<hr />";
        var v = 1;
        html += "<div style=\"height: 205px;overflow:scroll;\">";
        for (var j = 0; j < idResults.length; j++) {
            var currLayerName = idResults[j].layerName;
            if (currLayerName.toUpperCase() == results[i].toUpperCase()) {
                var id = idResults[j].feature.attributes["FID"];
                var str = id + "@" + idResults[j].layerId + "@" + currLayerName;
                html += "<div style=\"height: 25px;\">";
                html += "<span>";
                html += "&nbsp;" + (v++) + ".&nbsp;名称：" + idResults[j].value;
                html += "</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                html += "<a href='#' onclick='location_geometryQuery(\"" + str + "\",\"details\");'>详细信息</a> &nbsp;&nbsp;";
                html += "<a href='#' onclick='location_geometryQuery(\"" + str + "\",\"location\");'>定位</a> &nbsp;&nbsp;&nbsp;&nbsp;";
                html += "</div>";
            }
            else {
                continue;
            }
        }
        html += "</div>";
        navtab.addTabItem({ tabid: results[i], content: html, showClose: true });
        navtab.selectTabItem(results[i]);
    }
    dojo.byId("dgQuery").style.visibility = "visible";
    deactivateControl();
}

function GetCountByLayerName(layernames, idResults) {
    var count = 0;
    for (var i = 0; i < idResults.length; i++) {
        var currLayerName = idResults[i].layerName;
        if (currLayerName.toUpperCase() == layernames.toUpperCase()) {
            count++;
        }
    }
    return count;
}


//定位对象
function location_geometryQuery(str, flag) {
    try {
        // dojo.byId("query_result").style.visibility = "hidden";
        var layervalue = str.split('@')[0];
        var layerid = str.split('@')[1];
        var layerName = str.split('@')[2];
        dojo.byId("hid_layerName").value = layerName;
        if (layervalue != "undefined") {
            var queryTask = new esri.tasks.QueryTask(dataLayerUrl + "/" + layerid);
            var query = new esri.tasks.Query();
            //需要返回Geometry
            query.returnGeometry = true;
            //需要返回的字段
            query.outFields = ["*"];
            //查询条件
            query.where = "FID=" + layervalue + "";
            if (flag == "details") {
                queryTask.execute(query, ShowQueryResult);
            }
            if (flag == "location") {
                queryTask.execute(query, LocationPel);
            }
        } else {
            alert("未查到...");
        }
    } catch (e) {
        alert(e);
    }

}

var dataForGrid = [];
var fields = [];
//显示详细信息
function ShowQueryResult(queryResult) {
    try {
        dataForGrid = [];
        fields = queryResult.fields;
        map.graphics.clear();
        if (queryResult.features.length == 0) {
            alert("未查到相关数据！");
            return;
        }
        for (var i = 0; i < queryResult.features.length; i++) {
            var graphic = queryResult.features[i];
            dataForGrid.push(graphic.attributes);
            var geometry = graphic.geometry;
            //设置查询到的graphic的显示样式
            var symbol;
            switch (geometry.type) {
                case "point":
                    symbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_CIRCLE, 10, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2), new dojo.Color("yellow"));
                    break;
                case "multipoint":
                    symbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_CIRCLE, 10, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2), new dojo.Color("yellow"));
                    break;
                case "polyline":
                    symbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 255]), 3);
                    break;
                case "polygon":
                    symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASH, new dojo.Color([255, 0, 0]), 2), new dojo.Color([255, 255, 0, 0.25]));
                    break;
            }
            graphic.setSymbol(symbol);
            map.graphics.add(graphic);
        }
        var layerName = document.getElementById("hid_layerName").value;

        detailHandler();
    } catch (e) {
        alert(e);
    }
}
//定位
function LocationPel(queryResult) {
    try {
        dataForGrid = [];
        fields = queryResult.fields;
        map.graphics.clear();
        if (queryResult.features.length == 0) {
            alert("未查到相关数据！");
            return;
        }
        for (var i = 0; i < queryResult.features.length; i++) {
            var graphic = queryResult.features[i];
            dataForGrid.push(graphic.attributes);
            var geometry = graphic.geometry;
            //设置查询到的graphic的显示样式
            var symbol;
            switch (geometry.type) {
                case "point":
                    symbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_CIRCLE, 10, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2), new dojo.Color("yellow"));
                    break;
                case "multipoint":
                    symbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_CIRCLE, 10, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2), new dojo.Color("yellow"));
                    break;
                case "polyline":
                    symbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 255]), 3);
                    break;
                case "polygon":
                    symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_DASH, new dojo.Color([255, 0, 0]), 2), new dojo.Color([255, 255, 0, 0.25]));
                    break;
            }
            graphic.setSymbol(symbol);
            map.graphics.add(graphic);
            if (geometry.type != "point" && geometry.type != "multipoint") {
                var sExtent = geometry.getExtent();
                map.setExtent(sExtent.expand(5));
            } else {
                map.centerAt(geometry);
            }
        }
        var layerName = document.getElementById("hid_layerName").value;
    } catch (e) {
        alert(e);
    }
}

function detailHandler() {
    var html = "<table><tr><td width=\"30%\" style='font-weight:bold;'>字段名</td><td width=\"70%\" style='font-weight:bold;'>字段值</td></tr>";
    for (var i = 0; i < fields.length; i++) {
        html += "<tr>";
        html += "<td>";
        html += fields[i]["alias"];
        html += "&nbsp;</td>";
        html += "<td>";
        if (fields[i]["type"] == "esriFieldTypeDate") {
            html += ChangeDateFormat(dataForGrid[0][fields[i]["name"]], "0");
        } else {
            html += dataForGrid[0][fields[i]["name"]];
        }
        html += "&nbsp;</td>";
        html += "</tr>";
    }
    html += "</table>";
    dojo.byId("query_result_content").innerHTML = html;
    dojo.byId("query_result").style.visibility = "visible";
}
//将序列化成json格式后日期(毫秒数)转成日期格式 YYYY-MM-DD HH:MI:SS 
function ChangeDateFormat(cellval, type) {
    //var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
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
function closeDgQuery() {
    dojo.byId("dgQuery").style.visibility = "hidden";
    DeleteTab('tabContainer');
}

function closeQueryResult() {
    dojo.byId("query_result").style.visibility = "hidden";
}

function QueryByProperty() {
    var queryName = dojo.byId("txt_Search").value;
    // 实例化FindTask
    var findTaskObj = new esri.tasks.FindTask(dataLayerUrl);
    // FindTask的参数
    var findParams = new esri.tasks.FindParameters();
    // 返回Geometry
    findParams.returnGeometry = true;
    // 获取所有图层ID
    var infos = dataLayer.layerInfos;
    var layerIds = [];
    for (var i = 0; i < infos.length; i++) {
        var info = infos[i];
        layerIds.push(info.id);
    }
    findParams.layerIds = layerIds;
    // 查询字段
    var fields = "JH,GDMC";
    findParams.searchFields = fields.split(',');

    // 查询关键字
    findParams.searchText = queryName;
    // 执行查询
    findTaskObj.execute(findParams, identifyTaskCallFuc);
}