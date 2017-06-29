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
var dataLayerUrl = "http://10.72.118.19:6080/arcgis/rest/services/ZYData/MapServer";
var baseLayerUrl = "http://10.72.118.19:6080/arcgis/rest/services/ZYPublic/MapServer";
var geometryServiceUrl = "http://10.72.118.19:6080/arcgis/rest/services/Utilities/Geometry/GeometryServer";
var legendUrl = "http://10.72.118.19:6080/arcgis/rest/services/ZYData/MapServer/legend?f=pjson";
var navToolbar = null;
var map, imgLayer, dataLayer, baselayer, geometryService;
function init() {
    map = new esri.Map("myMapDiv", { logo: false, nav: false });
    var scalebar = new esri.dijit.Scalebar({
        map: map,
        scalebarStyle: "line",
        scalebarUnit: "metric"
    }, dojo.byId("scaleBarDiv"));

    dataLayer = new esri.layers.ArcGISDynamicMapServiceLayer(dataLayerUrl, { id: "dataLayer" });
    map.addLayer(dataLayer);

    baselayer = new esri.layers.ArcGISDynamicMapServiceLayer(baseLayerUrl, { id: "baselayer" });
    map.addLayer(baselayer);

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

function initPage() {
    var obj = document.getElementById("container");
    //var leftObj = document.getElementById("left");
    var mapObj = dojo.byId("myMapDiv");
    height = window.document.documentElement.clientHeight;
    width = window.document.documentElement.clientWidth;
    obj.style.height = (height - 110) + "px";
    //leftObj.style.height = (height - 110 - 4) + "px";
    mapObj.style.height = (height - 110) + "px";
}
$(function () {
    initPage();
});
window.onresize = initPage;

