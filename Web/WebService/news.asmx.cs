using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Web.WebService
{
    /// <summary>
    /// news 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class news : System.Web.Services.WebService
    {
        /// <summary>
        /// 新闻列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [WebMethod]
        public string newsList(int page, int count)
        {
            Bll.Agriculture.InfoService _Service = new Bll.Agriculture.InfoService();
            int total = 0;
            var dt = _Service.ListByPage(page, count, ref total);
            var list = new List<Web.Dto.InfoDto>();
            foreach (var item in dt)
            {
                list.Add(new Dto.InfoDto() { id = item.Id, time = item.CreateDate.ToString("yyyy-mm-dd"), title = item.InfoTitle, type = item.InfoType, url = "/Views/Info/Info.html?key=" + item.Id });
            }
            var model = new Dto.pageData<Dto.InfoDto>() { totalRow = total, pageNumber = page, pageSize = count, list = list };
            var jsonModel = new Dto.jsonModelData<Dto.pageData<Dto.InfoDto>>() { status = true, details = model };
            var result = JsonConvert.SerializeObject(jsonModel);
            return result;
        }
        /// <summary>
        /// 广告条列表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string bars()
        {
            Bll.Agriculture.InfoService _Service = new Bll.Agriculture.InfoService();
            int total = 0;
            int pageIndex = 1;
            int pageSize = 5;
            var entitys = _Service.BarsList(pageIndex, pageSize, ref total);
            var list = new List<Web.Dto.BarDto>();
            foreach (var item in entitys)
            {
                list.Add(new Web.Dto.BarDto()
                {
                    id = item.Id,
                    title = item.InfoTitle,
                    url = "/Views/Info/Info.html?key=" + item.Id,
                    img = item.InfoContent.Contains("img") ? Common.StringHelper.GetHtmlImageUrlList(item.InfoContent)[0] : "/Themes/Scripts/kindeditor/attached/image/20170510/20170510134832_2344.jpg"
                });
            }
            var jsonModel = new Dto.jsonModelData<List<Web.Dto.BarDto>>() { status = true, details = list };
            var result = JsonConvert.SerializeObject(jsonModel);
            return result;
        }
    }
}
