dojo.require("esri.map");
dojo.require("esri.dijit.Scalebar");
dojo.require("dojo.dnd.move");
dojo.require("dijit.layout.TabContainer");
dojo.require("dijit.layout.ContentPane")
dojo.require("esri.toolbars.navigation");
dojo.require("esri.toolbars.draw");
dojo.require("esri.tasks.LengthsParameters");
dojo.require("esri.tasks.AreasAndLengthsParameters");
dojo.require("esri.tasks.IdentifyParameters");
dojo.require("esri.tasks.IdentifyTask");
dojo.require("esri.tasks.FindTask");
dojo.require("esri.tasks.FindParameters");
var dataLayerUrl = "http://36.7.89.33:6081/arcgis/rest/services/贵池区土地分布汇总图/MapServer";
var baseLayerUrl = "http://36.7.89.33:6081/arcgis/rest/services/贵池区行政区划图/MapServer";
var geometryServiceUrl = "http://36.7.89.33:6081/arcgis/rest/services/Utilities/Geometry/GeometryServer";
var legendUrl = "http://36.7.89.33:6081/arcgis/rest/services/贵池区土地分布汇总图/MapServer/legend?f=pjson";
var navToolbar = null;
var map, imgLayer, dataLayer, baselayer, geometryService;
function init() {
    //初始化地图范围
   // initPage();
    //地图初始化
    map = new esri.Map("mapDiv", { logo: false, nav: false });
    //比例尺
    var scalebar = new esri.dijit.Scalebar({
        map: map,
        scalebarStyle: "line",
        scalebarUnit: "metric"
    }, dojo.byId("scaleBarDiv"));
    //data图层添加
    dataLayer = new esri.layers.ArcGISDynamicMapServiceLayer(dataLayerUrl, { id: "dataLayer" });
    map.addLayer(dataLayer);
    //base图层添加
    baselayer = new esri.layers.ArcGISDynamicMapServiceLayer(baseLayerUrl, { id: "baselayer" });
    map.addLayer(baselayer);
    //初始化geo服务
    geometryService = new esri.tasks.GeometryService(geometryServiceUrl);
    geometryService.on("lengths-complete", outputLength);
    geometryService.on("areas-and-lengths-complete", outputAreaAndLength);

    navToolbar = new esri.toolbars.Navigation(map, { tooltipOffset: 20, drawTime: 90, showTooltips: false });
    navToolbar.on("extent-history-change", extentHistoryChangeHandler);
    //给地图控件添加载入完成（onLoad）监听事件
    dojo.connect(map, "onLoad", function () {
        //给地图控件添加载鼠标移动监听事件
        dojo.connect(map, "onMouseMove", showCoordinates);
        //给地图控件添加载鼠标拖拽监听事件
        dojo.connect(map, "onMouseDrag", showCoordinates);
    });
}
dojo.addOnLoad(init);


