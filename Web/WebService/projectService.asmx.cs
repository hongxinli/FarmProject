using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Bll.Agriculture;
using Newtonsoft.Json;
using Web.Dto;
using System.Text;
using System.Configuration;

namespace Web.WebService
{
    /// <summary>
    /// projectService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class projectService : System.Web.Services.WebService
    {
        /// <summary>
        /// 耕地地力等级图
        /// </summary>
        /// <param name="town"></param>
        /// <param name="village"></param>
        /// <param name="farmLevel"></param>
        [WebMethod]
        public void LandLevel(string town, string village, string farmLevel)
        {
            FarmLandLevelService _Service = new FarmLandLevelService();
            var model = _Service.GetModel(town, village, farmLevel);
            var result = JsonConvert.SerializeObject(model);
            Common.ResponseHelper.Write(result);
        }
        /// <summary>
        /// 作物适应性评价图
        /// </summary>
        /// <param name="town"></param>
        /// <param name="village"></param>
        [WebMethod]
        public void CropsEnvironment(string town, string village)
        {
            CropsEnvironmentService _Service = new CropsEnvironmentService();
            var list = _Service.List(town);
            var townArea = list.Where(t => t.town.Equals(town)).Sum(t => t.area).ToString();
            var townAreaRice = list.Where(t => t.town.Equals(town) && t.rice.Equals("高度适宜")).Sum(t => t.area).ToString();
            var townAreaRice1 = list.Where(t => t.town.Equals(town) && t.rice.Equals("适宜")).Sum(t => t.area).ToString();
            var townAreaRice2 = list.Where(t => t.town.Equals(town) && t.rice.Equals("勉强适宜")).Sum(t => t.area).ToString();
            var townAreaRice3 = list.Where(t => t.town.Equals(town) && t.rice.Equals("不适宜")).Sum(t => t.area).ToString();
            var townAreaWheat = list.Where(t => t.town.Equals(town) && t.wheat.Equals("高度适宜")).Sum(t => t.area).ToString();
            var townAreaWheat1 = list.Where(t => t.town.Equals(town) && t.wheat.Equals("适宜")).Sum(t => t.area).ToString();
            var townAreaWheat2 = list.Where(t => t.town.Equals(town) && t.wheat.Equals("勉强适宜")).Sum(t => t.area).ToString();
            var townAreaWheat3 = list.Where(t => t.town.Equals(town) && t.wheat.Equals("不适宜")).Sum(t => t.area).ToString();
            var townAreaCotton = list.Where(t => t.town.Equals(town) && t.cotton.Equals("高度适宜")).Sum(t => t.area).ToString();
            var townAreaCotton1 = list.Where(t => t.town.Equals(town) && t.cotton.Equals("适宜")).Sum(t => t.area).ToString();
            var townAreaCotton2 = list.Where(t => t.town.Equals(town) && t.cotton.Equals("勉强适宜")).Sum(t => t.area).ToString();
            var townAreaCotton3 = list.Where(t => t.town.Equals(town) && t.cotton.Equals("不适宜")).Sum(t => t.area).ToString();
            var townAreaRape = list.Where(t => t.town.Equals(town) && t.rape.Equals("高度适宜")).Sum(t => t.area).ToString();
            var townAreaRape1 = list.Where(t => t.town.Equals(town) && t.rape.Equals("适宜")).Sum(t => t.area).ToString();
            var townAreaRape2 = list.Where(t => t.town.Equals(town) && t.rape.Equals("勉强适宜")).Sum(t => t.area).ToString();
            var townAreaRape3 = list.Where(t => t.town.Equals(town) && t.rape.Equals("不适宜")).Sum(t => t.area).ToString();

            var villageArea = list.Where(t => t.village.Equals(village)).Sum(t => t.area).ToString();
            var villageAreaRice = list.Where(t => t.village.Equals(village) && t.rice.Equals("高度适宜")).Sum(t => t.area).ToString();
            var villageAreaRice1 = list.Where(t => t.village.Equals(village) && t.rice.Equals("适宜")).Sum(t => t.area).ToString();
            var villageAreaRice2 = list.Where(t => t.village.Equals(village) && t.rice.Equals("勉强适宜")).Sum(t => t.area).ToString();
            var villageAreaRice3 = list.Where(t => t.village.Equals(village) && t.rice.Equals("不适宜")).Sum(t => t.area).ToString();
            var villageAreaWheat = list.Where(t => t.village.Equals(village) && t.wheat.Equals("高度适宜")).Sum(t => t.area).ToString();
            var villageAreaWheat1 = list.Where(t => t.village.Equals(village) && t.wheat.Equals("适宜")).Sum(t => t.area).ToString();
            var villageAreaWheat2 = list.Where(t => t.village.Equals(village) && t.wheat.Equals("勉强适宜")).Sum(t => t.area).ToString();
            var villageAreaWheat3 = list.Where(t => t.village.Equals(village) && t.wheat.Equals("不适宜")).Sum(t => t.area).ToString();
            var villageAreaCotton = list.Where(t => t.village.Equals(village) && t.cotton.Equals("高度适宜")).Sum(t => t.area).ToString();
            var villageAreaCotton1 = list.Where(t => t.village.Equals(village) && t.cotton.Equals("适宜")).Sum(t => t.area).ToString();
            var villageAreaCotton2 = list.Where(t => t.village.Equals(village) && t.cotton.Equals("勉强适宜")).Sum(t => t.area).ToString();
            var villageAreaCotton3 = list.Where(t => t.village.Equals(village) && t.cotton.Equals("不适宜")).Sum(t => t.area).ToString();
            var villageAreaRape = list.Where(t => t.village.Equals(village) && t.rape.Equals("高度适宜")).Sum(t => t.area).ToString();
            var villageAreaRape1 = list.Where(t => t.village.Equals(village) && t.rape.Equals("适宜")).Sum(t => t.area).ToString();
            var villageAreaRape2 = list.Where(t => t.village.Equals(village) && t.rape.Equals("勉强适宜")).Sum(t => t.area).ToString();
            var villageAreaRape3 = list.Where(t => t.village.Equals(village) && t.rape.Equals("不适宜")).Sum(t => t.area).ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>");
            sb.Append("<p>全镇耕地面积为<label style=\"color:red;\">" + townArea + "</label>亩，</p>");
            sb.Append("<p>高度适宜种植水稻的耕地面积为<label style=\"color:red;\">" + townAreaRice + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaRice) / Convert.ToDecimal(townArea), 2) * 100 + "%</label>；</p>");
            sb.Append("<p>适宜种植水稻的耕地面积为<label style=\"color:red;\">" + townAreaRice1 + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaRice1) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>勉强适宜种植水稻的耕地面积为<label style=\"color:red;\">" + townAreaRice2 + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaRice2) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>不适宜种植水稻的耕地面积为<label style=\"color:red;\">" + townAreaRice3 + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaRice3) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");

            sb.Append("<p>高度适宜种植小麦的耕地面积为<label style=\"color:red;\">" + townAreaWheat + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaWheat) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>适宜种植小麦的耕地面积为<label style=\"color:red;\">" + townAreaWheat1 + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaWheat1) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>勉强适宜种植小麦的耕地面积为<label style=\"color:red;\">" + townAreaWheat2 + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaWheat2) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>不适宜种植小麦的耕地面积为<label style=\"color:red;\">" + townAreaWheat3 + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaWheat3) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");

            sb.Append("<p>高度适宜种植棉花的耕地面积为<label style=\"color:red;\">" + townAreaCotton + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaCotton) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>适宜种植棉花的耕地面积为<label style=\"color:red;\">" + townAreaCotton1 + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaCotton1) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>勉强适宜种植棉花的耕地面积为<label style=\"color:red;\">" + townAreaCotton2 + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaCotton2) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>不适宜种植棉花的耕地面积为<label style=\"color:red;\">" + townAreaCotton3 + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaCotton3) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");

            sb.Append("<p>高度适宜种植油菜的耕地面积为<label style=\"color:red;\">" + townAreaRape + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaRape) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>适宜种植油菜的耕地面积为<label style=\"color:red;\">" + townAreaRape1 + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaRape1) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>勉强适宜种植油菜的耕地面积为<label style=\"color:red;\">" + townAreaRape2 + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaRape2) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>不适宜种植油菜的耕地面积为<label style=\"color:red;\">" + townAreaRape3 + "</label>亩，占全镇耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(townAreaRape3) / Convert.ToDecimal(townArea), 4) * 100 + "%</label>；</p>");

            sb.Append("<p>全村耕地面积为<label style=\"color:red;\">" + villageArea + "</label>亩，");
            sb.Append("<p>高度适宜种植水稻的耕地面积为<label style=\"color:red;\">" + villageAreaRice + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaRice) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>适宜种植水稻的耕地面积为<label style=\"color:red;\">" + villageAreaRice1 + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaRice1) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>勉强适宜种植水稻的耕地面积为<label style=\"color:red;\">" + villageAreaRice2 + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaRice2) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>不适宜种植水稻的耕地面积为<label style=\"color:red;\">" + villageAreaRice3 + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaRice3) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");

            sb.Append("<p>高度适宜种植小麦的耕地面积为<label style=\"color:red;\">" + villageAreaWheat + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaWheat) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>适宜种植小麦的耕地面积为<label style=\"color:red;\">" + villageAreaWheat1 + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaWheat1) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>勉强适宜种植小麦的耕地面积为<label style=\"color:red;\">" + villageAreaWheat2 + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaWheat2) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>不适宜种植小麦的耕地面积为<label style=\"color:red;\">" + villageAreaWheat3 + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaWheat3) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");

            sb.Append("<p>高度适宜种植棉花的耕地面积为<label style=\"color:red;\">" + villageAreaCotton + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaCotton) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>适宜种植棉花的耕地面积为<label style=\"color:red;\">" + villageAreaCotton1 + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaCotton1) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>勉强适宜种植棉花的耕地面积为<label style=\"color:red;\">" + villageAreaCotton2 + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaCotton2) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>不适宜种植棉花的耕地面积为<label style=\"color:red;\">" + villageAreaCotton3 + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaCotton3) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");

            sb.Append("<p>高度适宜种植油菜的耕地面积为<label style=\"color:red;\">" + villageAreaRape + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaRape) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>适宜种植油菜的耕地面积为<label style=\"color:red;\">" + villageAreaRape1 + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaRape1) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>勉强适宜种植油菜的耕地面积为<label style=\"color:red;\">" + villageAreaRape2 + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaRape2) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("<p>不适宜种植油菜的耕地面积为<label style=\"color:red;\">" + villageAreaRape3 + "</label>亩，占全村耕地面积比例为<label style=\"color:red;\">" + Math.Round(Convert.ToDecimal(villageAreaRape3) / Convert.ToDecimal(villageArea), 4) * 100 + "%</label>；</p>");
            sb.Append("</div>");
            Common.ResponseHelper.Write(sb.ToString());
        }

        /// <summary>
        /// 0打开  1关闭
        /// </summary>
        [WebMethod]
        public void IsPay()
        {
            var result = ConfigurationManager.AppSettings["Switch"].ToString();
            Common.ResponseHelper.Write(result);
        }
    }
}
