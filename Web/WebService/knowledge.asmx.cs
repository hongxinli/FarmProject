﻿using System;
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
        public string crops(int type)
        {
            if (type == 0)
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
                    return result;
                }
                else
                {
                    var jsonModel = new Dto.jsonData<Dto.CropTypeDto>()
                    {
                        status = false,
                        details = null
                    };
                    var result = JsonConvert.SerializeObject(jsonModel);
                    return result;
                }
            }
            else
            {
                return "";
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
        public string breed(int page, int count, string search, string typeid)
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
            return result;
        }
    }
}