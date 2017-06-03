using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Bll.Agriculture;
using Newtonsoft.Json;
using Web.Dto;

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
            CropsEnvironmentDto model = new CropsEnvironmentDto();
            model.townArea = list.Where(t=>t.town.Equals(town)).Sum(t => t.area).ToString();
            model.villageArea=list.Where(t=>t.village.Equals(village)).Sum(t => t.area).ToString();
            model.riceArea = list.Where(t => t.town.Equals(town) && t.rice.Equals("高度适宜")).Sum(t => t.area).ToString();
            model.riceArea1 = list.Where(t => t.town.Equals(town) && t.rice.Equals("适宜")).Sum(t => t.area).ToString();
            model.riceArea2 = list.Where(t => t.town.Equals(town) && t.rice.Equals("勉强适宜")).Sum(t => t.area).ToString();
            model.riceArea3 = list.Where(t => t.town.Equals(town) && t.rice.Equals("不适宜")).Sum(t => t.area).ToString();
            model.wheatArea = list.Where(t => t.town.Equals(town) && t.wheat.Equals("高度适宜")).Sum(t => t.area).ToString();
            model.wheatArea1 = list.Where(t => t.town.Equals(town) && t.wheat.Equals("适宜")).Sum(t => t.area).ToString();
            model.wheatArea2 = list.Where(t => t.town.Equals(town) && t.wheat.Equals("勉强适宜")).Sum(t => t.area).ToString();
            model.wheatArea3 = list.Where(t => t.town.Equals(town) && t.wheat.Equals("不适宜")).Sum(t => t.area).ToString();
            model.cottonArea = list.Where(t => t.town.Equals(town) && t.cotton.Equals("高度适宜")).Sum(t => t.area).ToString();
            model.cottonArea1 = list.Where(t => t.town.Equals(town) && t.cotton.Equals("适宜")).Sum(t => t.area).ToString();
            model.cottonArea2 = list.Where(t => t.town.Equals(town) && t.cotton.Equals("勉强适宜")).Sum(t => t.area).ToString();
            model.cottonArea3 = list.Where(t => t.town.Equals(town) && t.cotton.Equals("不适宜")).Sum(t => t.area).ToString();
            model.rapeArea = list.Where(t => t.town.Equals(town) && t.rape.Equals("高度适宜")).Sum(t => t.area).ToString();
            model.rapeArea1 = list.Where(t => t.town.Equals(town) && t.rape.Equals("适宜")).Sum(t => t.area).ToString();
            model.rapeArea2 = list.Where(t => t.town.Equals(town) && t.rape.Equals("勉强适宜")).Sum(t => t.area).ToString();
            model.rapeArea3 = list.Where(t => t.town.Equals(town) && t.rape.Equals("不适宜")).Sum(t => t.area).ToString();
            var result = JsonConvert.SerializeObject(model);
            Common.ResponseHelper.Write(result);
        }
    }
}
