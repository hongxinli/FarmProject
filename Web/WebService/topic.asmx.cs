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
    /// topic 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class topic : System.Web.Services.WebService
    {
        /// <summary>
        /// 获取专题图信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string type()
        {
            CaptionService _Service = new CaptionService();
            IList<Model.Agriculture.A_Caption> list = _Service.List();
            List<Web.Dto.CaptionDto> entitys = new List<Dto.CaptionDto>();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    entitys.Add(new Dto.CaptionDto()
                    {
                        id = list[i].Id,
                        name = list[i].CaptionName,
                        url = list[i].CaptionUrl
                    });
                }
                var jsonModel = new Dto.jsonData<Dto.CaptionDto>()
                {
                    status = true,
                    list = entitys
                };
                var result = JsonConvert.SerializeObject(jsonModel);
                return result;
            }
            else
            {
                var jsonModel = new Dto.jsonData<Dto.CaptionDto>()
                {
                    status = false,
                    list = null
                };
                var result = JsonConvert.SerializeObject(jsonModel);
                return result;
            }
        }
    }

}
