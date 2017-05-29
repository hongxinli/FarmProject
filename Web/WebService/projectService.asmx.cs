using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Bll.Agriculture;
using Newtonsoft.Json;

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

        [WebMethod]
        public void LandLevel(string town, string village, string farmLevel)
        {
            FarmLandLevelService _Service = new FarmLandLevelService();
            var model = _Service.GetModel(town, village, farmLevel);
            var result = JsonConvert.SerializeObject(model);
            Common.ResponseHelper.Write(result);
        }
    }
}
