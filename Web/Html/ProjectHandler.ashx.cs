using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bll.Agriculture;
using System.Text;
using Web.Dto;
using Newtonsoft.Json;

namespace Web.Html
{
    /// <summary>
    /// ProjectHandler 的摘要说明
    /// </summary>
    public class ProjectHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var town = context.Request["town"];
            var village = context.Request["village"];
            CropsEnvironmentService _Service = new CropsEnvironmentService();
            var list = _Service.List(town, village);

            var townArea = Math.Round(list.Where(t => t.town.Equals(town)).Sum(t => t.area), 2);
            var townAreaRice = Math.Round(list.Where(t => t.town.Equals(town) && t.rice.Equals("高度适宜")).Sum(t => t.area), 2);
            var townAreaRice1 = Math.Round(list.Where(t => t.town.Equals(town) && t.rice.Equals("适宜")).Sum(t => t.area), 2);
            var townAreaRice2 = Math.Round(list.Where(t => t.town.Equals(town) && t.rice.Equals("勉强适宜")).Sum(t => t.area), 2);
            var townAreaRice3 = Math.Round(list.Where(t => t.town.Equals(town) && t.rice.Equals("不适宜")).Sum(t => t.area), 2);
            var townAreaWheat = Math.Round(list.Where(t => t.town.Equals(town) && t.wheat.Equals("高度适宜")).Sum(t => t.area), 2);
            var townAreaWheat1 = Math.Round(list.Where(t => t.town.Equals(town) && t.wheat.Equals("适宜")).Sum(t => t.area), 2);
            var townAreaWheat2 = Math.Round(list.Where(t => t.town.Equals(town) && t.wheat.Equals("勉强适宜")).Sum(t => t.area), 2);
            var townAreaWheat3 = Math.Round(list.Where(t => t.town.Equals(town) && t.wheat.Equals("不适宜")).Sum(t => t.area), 2);
            var townAreaCotton = Math.Round(list.Where(t => t.town.Equals(town) && t.cotton.Equals("高度适宜")).Sum(t => t.area), 2);
            var townAreaCotton1 = Math.Round(list.Where(t => t.town.Equals(town) && t.cotton.Equals("适宜")).Sum(t => t.area), 2);
            var townAreaCotton2 = Math.Round(list.Where(t => t.town.Equals(town) && t.cotton.Equals("勉强适宜")).Sum(t => t.area), 2);
            var townAreaCotton3 = Math.Round(list.Where(t => t.town.Equals(town) && t.cotton.Equals("不适宜")).Sum(t => t.area), 2);
            var townAreaRape = Math.Round(list.Where(t => t.town.Equals(town) && t.rape.Equals("高度适宜")).Sum(t => t.area), 2);
            var townAreaRape1 = Math.Round(list.Where(t => t.town.Equals(town) && t.rape.Equals("适宜")).Sum(t => t.area), 2);
            var townAreaRape2 = Math.Round(list.Where(t => t.town.Equals(town) && t.rape.Equals("勉强适宜")).Sum(t => t.area), 2);
            var townAreaRape3 = Math.Round(list.Where(t => t.town.Equals(town) && t.rape.Equals("不适宜")).Sum(t => t.area), 2);
            var townArea0 = Math.Round(list.Where(t => t.rice.Equals("高度适宜") || t.wheat.Equals("高度适宜") || t.cotton.Equals("高度适宜") || t.rape.Equals("高度适宜")).Sum(t => t.area), 2);
            var townArea1 = Math.Round(list.Where(t => t.rice.Equals("适宜") || t.wheat.Equals("适宜") || t.cotton.Equals("适宜") || t.rape.Equals("适宜")).Sum(t => t.area), 2);
            var townArea2 = Math.Round(list.Where(t => t.rice.Equals("勉强适宜") || t.wheat.Equals("勉强适宜") || t.cotton.Equals("勉强适宜") || t.rape.Equals("勉强适宜")).Sum(t => t.area), 2);
            var townArea3 = Math.Round(list.Where(t => t.rice.Equals("不适宜") || t.wheat.Equals("不适宜") || t.cotton.Equals("不适宜") || t.rape.Equals("不适宜")).Sum(t => t.area), 2);

            var villageArea = Math.Round(list.Where(t => t.village.Equals(village)).Sum(t => t.area), 2);
            var villageAreaRice = Math.Round(list.Where(t => t.village.Equals(village) && t.rice.Equals("高度适宜")).Sum(t => t.area), 2);
            var villageAreaRice1 = Math.Round(list.Where(t => t.village.Equals(village) && t.rice.Equals("适宜")).Sum(t => t.area), 2);
            var villageAreaRice2 = Math.Round(list.Where(t => t.village.Equals(village) && t.rice.Equals("勉强适宜")).Sum(t => t.area), 2);
            var villageAreaRice3 = Math.Round(list.Where(t => t.village.Equals(village) && t.rice.Equals("不适宜")).Sum(t => t.area), 2);
            var villageAreaWheat = Math.Round(list.Where(t => t.village.Equals(village) && t.wheat.Equals("高度适宜")).Sum(t => t.area), 2);
            var villageAreaWheat1 = Math.Round(list.Where(t => t.village.Equals(village) && t.wheat.Equals("适宜")).Sum(t => t.area), 2);
            var villageAreaWheat2 = Math.Round(list.Where(t => t.village.Equals(village) && t.wheat.Equals("勉强适宜")).Sum(t => t.area), 2);
            var villageAreaWheat3 = Math.Round(list.Where(t => t.village.Equals(village) && t.wheat.Equals("不适宜")).Sum(t => t.area), 2);
            var villageAreaCotton = Math.Round(list.Where(t => t.village.Equals(village) && t.cotton.Equals("高度适宜")).Sum(t => t.area), 2);
            var villageAreaCotton1 = Math.Round(list.Where(t => t.village.Equals(village) && t.cotton.Equals("适宜")).Sum(t => t.area), 2);
            var villageAreaCotton2 = Math.Round(list.Where(t => t.village.Equals(village) && t.cotton.Equals("勉强适宜")).Sum(t => t.area), 2);
            var villageAreaCotton3 = Math.Round(list.Where(t => t.village.Equals(village) && t.cotton.Equals("不适宜")).Sum(t => t.area), 2);
            var villageAreaRape = Math.Round(list.Where(t => t.village.Equals(village) && t.rape.Equals("高度适宜")).Sum(t => t.area), 2);
            var villageAreaRape1 = Math.Round(list.Where(t => t.village.Equals(village) && t.rape.Equals("适宜")).Sum(t => t.area), 2);
            var villageAreaRape2 = Math.Round(list.Where(t => t.village.Equals(village) && t.rape.Equals("勉强适宜")).Sum(t => t.area), 2);
            var villageAreaRape3 = Math.Round(list.Where(t => t.village.Equals(village) && t.rape.Equals("不适宜")).Sum(t => t.area), 2);
            var villageArea0 = Math.Round(list.Where(t => t.village.Equals(village)).Where(t => t.rice.Equals("高度适宜") || t.wheat.Equals("高度适宜") || t.cotton.Equals("高度适宜") || t.rape.Equals("高度适宜")).Sum(t => t.area), 2);
            var villageArea1 = Math.Round(list.Where(t => t.village.Equals(village)).Where(t => t.rice.Equals("适宜") || t.wheat.Equals("适宜") || t.cotton.Equals("适宜") || t.rape.Equals("适宜")).Sum(t => t.area), 2);
            var villageArea2 = Math.Round(list.Where(t => t.village.Equals(village)).Where(t => t.rice.Equals("勉强适宜") || t.wheat.Equals("勉强适宜") || t.cotton.Equals("勉强适宜") || t.rape.Equals("勉强适宜")).Sum(t => t.area), 2);
            var villageArea3 = Math.Round(list.Where(t => t.village.Equals(village)).Where(t => t.rice.Equals("不适宜") || t.wheat.Equals("不适宜") || t.cotton.Equals("不适宜") || t.rape.Equals("不适宜")).Sum(t => t.area), 2);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table >");
            sb.Append("<thead><tr><th colspan=\"5\">" + town + "农作物适应性数据报表(单位：亩)<br />注：" + town + "的耕地总面积为：" + townArea + "亩</th></tr>");
            sb.Append("<tr><td></td><td>高度适宜</td><td>适宜</td><td>勉强适宜</td><td>不适宜</td></tr></thead>");
            sb.Append("<tbody>");
            sb.Append("<tr><td>水稻</td><td>" + townAreaRice + "</td><td>" + townAreaRice1 + "</td><td>" + townAreaRice2 + "</td><td>" + townAreaRice3 + "</td></tr>");
            sb.Append("<tr><td>小麦</td><td>" + townAreaWheat + "</td><td>" + townAreaWheat1 + "</td><td>" + townAreaWheat2 + "</td><td>" + townAreaWheat3 + "</td></tr>");
            sb.Append("<tr><td>棉花</td><td>" + townAreaCotton + "</td><td>" + townAreaCotton1 + "</td><td>" + townAreaCotton2 + "</td><td>" + townAreaCotton3 + "</td></tr>");
            sb.Append("<tr><td>油菜</td><td>" + townAreaRape + "</td><td>" + townAreaRape1 + "</td><td>" + townAreaRape2 + "</td><td>" + townAreaRape3 + "</td></tr>");
            sb.Append("<tr><td>合计</td><td>" + townArea0 + "</td><td>" + townArea1 + "</td><td>" + townArea2 + "</td><td>" + townArea3 + "</td></tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("<hr style=\"width:100%;border:1px solid black;\" />");
            sb.Append("<table >");
            sb.Append("<thead><tr><th colspan=\"5\">" + village + "农作物适应性数据报表(单位：亩)<br />注：" + village + "的耕地总面积为：" + villageArea + "亩</th></tr>");
            sb.Append("<tr><td></td><td>高度适宜</td><td>适宜</td><td>勉强适宜</td><td>不适宜</td></tr></thead>");
            sb.Append("<tbody>");
            sb.Append("<tr><td>水稻</td><td>" + villageAreaRice + "</td><td>" + villageAreaRice1 + "</td><td>" + villageAreaRice2 + "</td><td>" + villageAreaRice3 + "</td></tr>");
            sb.Append("<tr><td>小麦</td><td>" + villageAreaWheat + "</td><td>" + villageAreaWheat1 + "</td><td>" + villageAreaWheat2 + "</td><td>" + villageAreaWheat3 + "</td></tr>");
            sb.Append("<tr><td>棉花</td><td>" + villageAreaCotton + "</td><td>" + villageAreaCotton1 + "</td><td>" + villageAreaCotton2 + "</td><td>" + villageAreaCotton3 + "</td></tr>");
            sb.Append("<tr><td>油菜</td><td>" + villageAreaRape + "</td><td>" + villageAreaRape1 + "</td><td>" + villageAreaRape2 + "</td><td>" + villageAreaRape3 + "</td></tr>");
            sb.Append("<tr><td>合计</td><td>" + villageArea0 + "</td><td>" + villageArea1 + "</td><td>" + villageArea2 + "</td><td>" + villageArea3 + "</td></tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");
            var model = new ChartData();
            model.labels = new List<string> { "水稻", "小麦", "棉花", "油菜" };
            model.datasets = new List<BarSets> { 
                new BarSets() { data = new List<decimal>() { townAreaRice, townAreaWheat, townAreaCotton, townAreaRape }, fillColor = "rgba(112,168,0,1)", strokeColor="rgba(112,168,0,1)" },
                new BarSets() { data = new List<decimal>() { townAreaRice1, townAreaWheat1, townAreaCotton1, townAreaRape1 }, fillColor = "rgba(115,223,255,1)", strokeColor="rgba(115,223,255,1)" },
                new BarSets() { data = new List<decimal>() { townAreaRice2, townAreaWheat2, townAreaCotton2, townAreaRape2 }, fillColor = "rgba(41,255,81,1)", strokeColor="rgba(41,255,81,1)" },
                new BarSets() { data = new List<decimal>() { townAreaRice3, townAreaWheat3, townAreaCotton3, townAreaRape3 }, fillColor = "rgba(104,104,104,1)", strokeColor="rgba(104,104,104,1)" },
            };
            sb.Append("||");
            var json = JsonConvert.SerializeObject(model);
            sb.Append(json);
            sb.Append("||");
            var villageModel = new ChartData();
            villageModel.labels = new List<string> { "水稻", "小麦", "棉花", "油菜" };
            villageModel.datasets = new List<BarSets> { 
                new BarSets() { data = new List<decimal>() { villageAreaRice, villageAreaWheat, villageAreaCotton, villageAreaRape }, fillColor = "rgba(112,168,0,1)", strokeColor="rgba(112,168,0,1)" },
                new BarSets() { data = new List<decimal>() { villageAreaRice1, villageAreaWheat1, villageAreaCotton1, villageAreaRape1 }, fillColor = "rgba(115,223,255,1)", strokeColor="rgba(115,223,255,1)" },
                new BarSets() { data = new List<decimal>() { villageAreaRice2, villageAreaWheat2, villageAreaCotton2, villageAreaRape2 }, fillColor = "rgba(41,255,81,1)", strokeColor="rgba(41,255,81,1)" },
                new BarSets() { data = new List<decimal>() { villageAreaRice3, villageAreaWheat3, villageAreaCotton3, villageAreaRape3 }, fillColor = "rgba(104,104,104,1)", strokeColor="rgba(104,104,104,1)" },
            };
            var villageJson = JsonConvert.SerializeObject(villageModel);
            sb.Append(villageJson);
            context.Response.Write(sb.ToString());
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}