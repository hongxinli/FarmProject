using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Bll.Sys;
using Newtonsoft.Json;

namespace Web.WebService
{
    /// <summary>
    /// splash 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class splash : System.Web.Services.WebService
    {

        [WebMethod]
        public void pic()
        {
            CodeService _Service = new CodeService();
            var list = _Service.List(" DOMAINNAME='A_CODE_WELCOME'");
            var entitys = new List<Dto.SplashDto>();
            foreach (var item in list)
            {
                entitys.Add(new Dto.SplashDto() { id = item.Id, url = item.ImageUrl });
            }
            var jsonData = new Dto.jsonData<Dto.SplashDto>() { status = true, details = entitys };
            var result = JsonConvert.SerializeObject(jsonData);
            Common.ResponseHelper.Write(result);
        }
    }
}
