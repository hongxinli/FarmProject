<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapPage.aspx.cs" Inherits="WebGis.MapPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no" />
    <title>土壤养分监测与管理云平台</title>
    <link type="text/css" rel="stylesheet" href="css/map.css" />
    <script type="text/javascript">var dojoConfig = { parseOnLoad: true, isDebug: true };</script>
    <script src="js/map.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--地图-->
        <div id="mapDiv"></div>
        <!--比例尺-->
        <div id="scaleBarDiv" style="position: absolute; left: 50px; bottom: 58px;"></div>
    </form>
</body>
</html>
