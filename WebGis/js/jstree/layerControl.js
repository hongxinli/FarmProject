var legendlayers;
function f_LayerControl() {
    dojo.xhrGet({
        url: "proxy.ashx?" + legendUrl,
        handleAs: "json",
        sync: false,
        load: function (responseText) {
            legendlayers = eval(responseText);
            //控制图层显示功能
            if (dataLayer.loaded) {
                controlGroupLayers();
            } else {
                dojo.connect(dataLayer, "onLoad", function () {
                    controlGroupLayers();
                });
            }
            initlayer();
        },
        error: function (response) {
            alert(response);
        }
    })
}
function controlGroupLayers() {
    var layername = arguments[0];
    var layers = map.getLayer("dataLayer").layerInfos;
    if (layers.length == 0)
        return;
    var visibleLayerId = [];
    var tree = [{ id: '-1', name: '图层与图例', child: [] }];
    for (var i = 0; i < layers.length; i++) {
        if (typeof (layername) != "undefined" && layername.length > 0) {
            if (layers[i].parentLayerId == "-1" && layers[i].name == layername) {
                tree[0].child.push({ id: layers[i].id, name: layers[i].name, child: getSubtree(layers, layers[i]) });
                visibleLayerId.push(getVisibleLayer(layers, layers[i]));
            }
        } else {
            if (layers[i].parentLayerId == "-1") {
                tree[0].child.push({ id: layers[i].id, name: layers[i].name, child: getSubtree(layers, layers[i]) });   
            }
        }
    }

    window.tt = new easyTree({
        container: $$('tt'),
        isInput: true,
        isIco: true,
        data: tree
    });
}

function initlayer() {
    var layers = map.getLayer("dataLayer").visibleLayers;
    layers.push(-1);
    $("#tt li").each(function (i) {
        var value = $(this).attr("primary");
        for (var i = 0; i < layers.length; i++) {
            if (layers[i] == value) {
                $(this).children('input').attr("checked", "true");
            }
        }
    })
}

//获取mxd节点树
function getSubtree(layers, layer) {
    var tree = [];
    for (var i in layer.subLayerIds) {
        var id = layer.subLayerIds[i];
        if (layers[id].subLayerIds == null) {
            for (var j = 0; j < legendlayers.layers.length; j++) {
                var legendlayerid = legendlayers.layers[j].layerId;
                if (id == legendlayerid) {
                    var contentType = legendlayers.layers[j].legend[0].contentType;
                    var imageData = legendlayers.layers[j].legend[0].imageData;
                    tree.push({
                        id: id,
                        name: layers[id].name,
                        checked: layers[id].defaultVisibility,
                        img: 'data:' + contentType + ';base64,' + imageData
                    });
                }
            }
        } else {
            tree.push({
                id: id,
                name: layers[id].name,
                child: getSubtree(layers, layers[layer.subLayerIds[i]])
            });
        }
    }
    return tree;
}

//获取可见图层id
function getVisibleLayer(layers, layer) {
    var _visibleLayers = [];
    for (var i in layer.subLayerIds) {
        var id = layer.subLayerIds[i];
        if (layers[id].subLayerIds == null) {
            if (layers[id].defaultVisibility)
                _visibleLayers.push(id);
        }
        else {
            var temp = getVisibleLayer(layers, layers[id]);
            if (temp.length > 0)
                _visibleLayers.push(temp);
        }
    }
    return _visibleLayers;
}

function showControlLayer() {
    mapTool.deactivateControl();
    mapTool.initImage();
    dojo.empty("tt");
    f_LayerControl();
    $("#control_layer").show();
}
function closeControlLayer() {
    $("#control_layer").hide();
}
