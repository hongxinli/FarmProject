using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Bll.Agriculture;
using Bll.Sys;
using Newtonsoft.Json;
using System.Text;

namespace Web.WebService
{
    /// <summary>
    /// knowledge 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class knowledge : System.Web.Services.WebService
    {
        /// <summary>
        /// 作物种类
        /// </summary>
        /// <param name="type"> 0 为请求作物， 1为请求虫害</param>
        /// <returns></returns>
        [WebMethod]
        public void crops(int type)
        {

            if (type == 0) //农作物
            {
                CodeService _codeService = new CodeService();
                var list = _codeService.List("domainname='A_CROP_TYPE'");
                List<Web.Dto.CropTypeDto> entitys = new List<Dto.CropTypeDto>();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        entitys.Add(new Dto.CropTypeDto() { id = item.Id, name = item.SName, img = item.ImageUrl });
                    }
                    var jsonModel = new Dto.jsonData<Dto.CropTypeDto>()
                    {
                        status = true,
                        details = entitys
                    };
                    var result = JsonConvert.SerializeObject(jsonModel);
                    Common.ResponseHelper.Write(result);
                }
            }
            else if (type == 1) //病虫害
            {

            }
        }
        /// <summary>
        /// 获取农作物信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <param name="search"></param>
        /// <param name="typeid"></param>
        /// <returns></returns>
        [WebMethod]
        public void breed(int page, int count, string search, string typeid)
        {
            CropService _Service = new CropService();
            StringBuilder sb = new StringBuilder();
            sb.Append(" 1=1 ");
            if (!string.IsNullOrEmpty(typeid))
            {
                sb.Append(" and croptype='" + typeid + "'");
            }
            if (!string.IsNullOrEmpty(search))
            {
                sb.Append(" and cropname like '%" + search + "%'");
            }
            int total = 0;
            var list = _Service.ListByPage(page, count, sb.ToString(), ref total);
            var cropList = new List<Web.Dto.CropDto>();
            foreach (var item in list)
            {
                cropList.Add(new Dto.CropDto() { id = item.Id, name = item.CropName, time = item.CreateDate.ToString("yyyy-mm-dd"), url = "/Views/Crop/Crop.html?key=" + item.Id });
            }
            var entity = new Dto.pageData<Web.Dto.CropDto>() { totalRow = total, pageNumber = page, pageSize = count, list = cropList };
            var jsonModel = new Dto.jsonModelData<Dto.pageData<Web.Dto.CropDto>>() { status = true, details = entity };
            var result = JsonConvert.SerializeObject(jsonModel);
            Common.ResponseHelper.Write(result);
        }
        /// <summary>
        /// 获取病虫害信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        [WebMethod]
        public void pest(int page, int count)
        {
            PestService _Service = new PestService();
            int total = 0;
            var list = _Service.ListByPage(page, count, ref total);
            var pestList = new List<Web.Dto.PestDto>();
            foreach (var item in list)
            {
                pestList.Add(new Dto.PestDto()
                {
                    id = item.Id,
                    name = item.PestName,
                    time = item.CreateDate.ToString("yyyy-mm-dd"),
                    url = "/Views/Pest/Pest.html?key=" + item.Id,
                    img = item.PestContent.Contains("img") ? Common.StringHelper.GetHtmlImageUrlList(item.PestContent)[0] : "/Themes/Images/news.jpg"
                });
            }
            var entity = new Dto.pageData<Web.Dto.PestDto>() { totalRow = total, pageNumber = page, pageSize = count, list = pestList };
            var jsonModel = new Dto.jsonModelData<Dto.pageData<Web.Dto.PestDto>>() { status = true, details = entity };
            var result = JsonConvert.SerializeObject(jsonModel);
            Common.ResponseHelper.Write(result);
        }
    }
}
