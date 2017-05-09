﻿using Newtonsoft.Json;
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
    }
}