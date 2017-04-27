using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bll.Agriculture;

namespace Web.Views.Crop
{
    /// <summary>
    /// Crop 的摘要说明
    /// </summary>
    public class Crop : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            CropService _Service = new CropService();
            context.Response.ContentType = "text/plain";
            var action = context.Request["action"];
            var key = context.Request["key"];
            switch (action)
            {
                case "delete":
                    int result = _Service.Delete(key);
                    context.Response.Write(result);
                    context.Response.End();
                    break;
                case "detials":
                    string infoContent = _Service.GetInfoContent(key);
                    context.Response.Write(infoContent);
                    context.Response.End();
                    break;
                default:
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}