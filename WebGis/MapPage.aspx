<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MapPage.aspx.cs" Inherits="MapPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <title>新疆油田重油公司地理信息平台</title>
    <link type="text/css" rel="stylesheet" href="css/map.css" />
    <script type="text/javascript">var dojoConfig = { parseOnLoad: true, isDebug: true };</script>
    <script src="js/map.js"></script>
    <link href="js/liger/themes/blue/css/liger-all.css" rel="stylesheet" />
    <script src="js/liger/jquery-1.7.2.min.js"></script>
    <script src="js/liger/plugins/core.js"></script>
    <script src="js/liger/plugins/ligerGrid.js"></script>
    <script src="js/liger/plugins/ligerTab.js"></script>
    <script src="js/liger/plugins/ligerDrag.js"></script>

    <script type="text/javascript">
        var navtab = null;
        $(function () {
            //tab
            $("#tabContainer").ligerTab();
            navtab = $("#tabContainer").ligerGetTabManager();
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="container">
            <div id="header">
                <div id="logo">
                    <img src="images/map/logo.png" />
                </div>
                <div id="exit">
                    欢迎，管理员&nbsp;&nbsp;
                    <img src="images/map/exit.png" onclick="exitFun();" />
                </div>
            </div>
            <div id="tool">
                <ul>
                    <li>
                        <img src="images/map/tools/zoomin.png" title="放大" onclick="controlNav('zoomin');" id="zoomin" /></li>
                    <li>
                        <img src="images/map/tools/zoomout.png" title="缩小" onclick="controlNav('zoomout');" id="zoomout" /></li>
                    <li>
                        <img src="images/map/tools/zoomfull.png" title="全图" onclick="navigationBar('zoomfull');" id="zoomfull" /></li>
                    <li style="width: 15px;">
                        <img src="images/map/tools/tool_vertical.png" /></li>
                    <li>
                        <img src="images/map/tools/zoompan.png" title="平移" onclick="navigationBar('zoompan');" id="zoompan" /></li>
                    <li>
                        <img src="images/map/tools/zoomprev.png" title="后退" onclick="navigationBar('zoomprev');" id="zoomprev" /></li>
                    <li>
                        <img src="images/map/tools/zoomnext.png" title="前进" onclick="navigationBar('zoomnext');" id="zoomnext" /></li>
                    <li style="width: 15px;">
                        <img src="images/map/tools/tool_vertical.png" /></li>
                    <li>
                        <img src="images/map/tools/measure_coord.png" title="获取坐标" onclick="coordInfo();" id="measure_coord" /></li>
                    <li>
                        <img src="images/map/tools/measure_length.png" title="长度测量" onclick="measure_fun('length');" id="measure_length" /></li>
                    <li>
                        <img src="images/map/tools/measure_area.png" title="面积测量" onclick="measure_fun('area');" id="measure_area" /></li>
                    <li style="width: 15px;">
                        <img src="images/map/tools/tool_vertical.png" /></li>
                    <li>
                        <img src="images/map/tools/query_point.png" title="点查询" onclick="queryEvent('point');" id="query_point" /></li>
                    <li>
                        <img src="images/map/tools/query_line.png" title="线查询" onclick="queryEvent('line');" id="query_line" /></li>
                    <li>
                        <img src="images/map/tools/query_extent.png" title="矩形查询" onclick="queryEvent('extent');" id="query_extent" /></li>
                    <li>
                        <img src="images/map/tools/query_polygon.png" title="多边形查询" onclick="queryEvent('polygon');" id="query_polygon" /></li>
                    <li style="width: 15px;">
                        <img src="images/map/tools/tool_vertical.png" /></li>
                    <li>
                        <img src="images/map/tools/clear.png" title="清除" onclick="clearGraphs();" id="clear" /></li>
                    <li style="width: 15px;">
                        <img src="images/map/tools/tool_vertical.png" /></li>
                    <li>
                        <img src="images/map/tools/layer.png" title="图层控制" onclick="showControlLayer();" id="controlLayer" /></li>
                    <li style="width: 15px;">
                        <img src="images/map/tools/tool_vertical.png" /></li>
                    <li style="width: 250px;">
                        <input type="text" id="txt_Search" value="请输入关键字" onfocus="javascript:if(dojo.byId('txt_Search').value=='请输入关键字'){dojo.byId('txt_Search').value='';dojo.byId('txt_Search').style.Color='#998080';}else{dojo.byId('txt_Search').style.Color='#000000'}" style="width: 150px; height: 20px; border: 0px solid; color: #998080;" />
                        <input type="button" style="width: 50px; cursor: pointer;" id="btn_Search" onclick="QueryByProperty()" value="查询" />
                    </li>
                </ul>
            </div>
            <div id="myMapDiv"></div>
            <div id="scaleBarDiv" style="position: absolute; left: 50px; bottom: 58px;"></div>
            <div id="measures"></div>
            <!--几何图形查询结果-->
            <div id="dgQuery" dojotype="dojo.dnd.Moveable" style="position: absolute; left: 70px; top: 120px; z-index: 30; width: 450px; height: 307px; border: solid 1px blue; background-color: whitesmoke; visibility: hidden;">
                <div id="dgQueryTitle" style="background-image: url('images/map/searchdataTitle.png'); width: 450px; height: 37px; position: relative; left: 0px;">
                    <div id="dgQueryClose" onclick="closeDgQuery();" class="close" style="margin-right: 15px; margin-top: 12px; float: right; cursor: pointer;"></div>
                </div>
                <div id="tabContainer" style="width: 450px; height: 270px;" tabstrip="true">
                </div>
            </div>
            <!--几何图形查询详细信息-->
            <div id="query_result" dojotype="dojo.dnd.move.parentConstrainedMoveable" area="margin" within="true" handle="query_result_head">
                <div id="query_result_head">
                    <table style="width: 100%;">
                        <tr>
                            <td>详细信息</td>
                            <td style="text-align: right; padding-right: 10px;">
                                <img src="images/map/close.png" alt="关闭" onclick="closeQueryResult();" style="vertical-align: auto;" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="query_result_content" style="width: 300px;"></div>
            </div>
            <!--图层控制-->
            <div id="control_layer" dojotype="dojo.dnd.move.parentConstrainedMoveable" area="margin" within="true" handle="control_layer_head" class="funciton_div">
                <div id="control_layer_head" class="function_head">
                    <table style="width: 100%;">
                        <tr>
                            <td>图层控制</td>
                            <td style="text-align: right; padding-right: 10px;">
                                <img src="images/map/close.png" alt="关闭" onclick="closeControlLayer();" style="vertical-align: auto;" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="layer_control_content" class="function_content">
                    <div id="tt" style="max-height: 300px; overflow: auto;"></div>
                </div>
            </div>
            <!--关键字查询-->
            <div id="keywords_query" dojotype="dojo.dnd.move.parentConstrainedMoveable" area="margin" within="true" handle="keywords_query_head" class="funciton_div">
                <div id="keywords_query_head" class="function_head">
                    <table style="width: 100%;">
                        <tr>
                            <td>关键字查询</td>
                            <td style="text-align: right; padding-right: 10px;">
                                <img src="images/map/close.png" alt="关闭" onclick="closeKeywordsQuery();" style="vertical-align: auto;" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="keywords_query_content" class="function_content">
                    <table>
                        <tr>
                            <td style="text-align: left">关键字：</td>
                            <td>
                                <input type="text" id="txt_keywords" class="input" style="width: 200px;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <img src="images/map/btn_query.png" style="cursor: pointer;" onclick="keywordsQueryFun();" />
                            </td>
                        </tr>
                    </table>
                    <div id="keywords_query_result"></div>
                </div>
            </div>
            <div id="footer">
                <span id="coordinates" style="float: left; width:300px;"></span>    技术支持：山东天元信息技术股份有限公司&nbsp;&nbsp;0546-8302577
            </div>
        </div>
        <input type="hidden" id="hid_navId" />
        <input type="hidden" id="hid_layerName" />
    </form>
</body>
</html>
