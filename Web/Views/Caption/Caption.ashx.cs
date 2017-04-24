using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bll.Agriculture;

namespace Web.Views.Caption
{
    /// <summary>
    /// Caption 的摘要说明
    /// </summary>
    public class Caption : IHttpHandler
    {
        CaptionService _Service = new CaptionService();
        public void ProcessRequest(HttpContext context)
        {
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