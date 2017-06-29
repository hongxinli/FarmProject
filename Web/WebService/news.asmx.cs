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
        /// 新闻列表（专栏）
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [WebMethod]
        public void topicList(int page, int count, string type)
        {
            Bll.Agriculture.InfoService _Service = new Bll.Agriculture.InfoService();
            int total = 0;
            var dt = _Service.ListByPage(page, count, type, ref total);
            var list = new List<Web.Dto.InfoDto>();
            foreach (var item in dt)
            {
                list.Add(new Dto.InfoDto() { id = item.Id, time = item.CreateDate.ToString("yyyy-MM-dd"), title = item.InfoTitle, type = item.InfoType, url = "/Views/Info/Info.html?key=" + item.Id });
            }
            var model = new Dto.pageData<Dto.InfoDto>() { totalRow = total, pageNumber = page, pageSize = count, list = list };
            var jsonModel = new Dto.jsonModelData<Dto.pageData<Dto.InfoDto>>() { status = true, details = model };
            var result = JsonConvert.SerializeObject(jsonModel);
            Common.ResponseHelper.Write(result);
        }
        /// <summary>
        /// 新闻列表（首页）
        /// </summary>
        [WebMethod]
        public void newsList()
        {
            Bll.Agriculture.InfoService _Service = new Bll.Agriculture.InfoService();
            var entitys = _Service.newsList();
            var list = new List<Web.Dto.InfoDto>();
            foreach (var item in entitys)
            {
                list.Add(new Dto.InfoDto() { id = item.Id, time = item.CreateDate.ToString("yyyy-MM-dd"), title = item.InfoTitle, type = item.InfoType, url = "/Views/Info/Info.html?key=" + item.Id });
            }
            var model = new Dto.jsonData<Web.Dto.InfoDto>() { status = true, details = list };
            var result = JsonConvert.SerializeObject(model);
            Common.ResponseHelper.Write(result);
        }
        /// <summary>
        /// 广告条列表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void bars()
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
                    img = item.InfoContent.Contains("img") ? Common.StringHelper.GetHtmlImageUrlList(item.InfoContent)[0] : "/Themes/Images/news.jpg"
                });
            }
            var jsonModel = new Dto.jsonModelData<List<Web.Dto.BarDto>>() { status = true, details = list };
            var result = JsonConvert.SerializeObject(jsonModel);
            Common.ResponseHelper.Write(result);
        }
        [WebMethod]
        public void type()
        {
            Bll.Sys.CodeService _Service = new Bll.Sys.CodeService();
            IList<Model.Base_Code> entitys = _Service.List(" domainname='A_INFO_TYPE' ");
            List<string> list = new List<string>();
            foreach (var item in entitys)
            {
                list.Add(item.SName);
            }
            var result = JsonConvert.SerializeObject(list);
            Common.ResponseHelper.Write(result);
        }
    }
}
