using Bll.Agriculture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Views.Pest
{
    /// <summary>
    /// Pest 的摘要说明
    /// </summary>
    public class Pest : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            PestService _Service = new PestService();
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
                    string infoContent = _Service.GetPestContent(key);
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