using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Base.SysCode
{
    /// <summary>
    /// Code 的摘要说明
    /// </summary>
    public class Code : IHttpHandler
    {
        Bll.Sys.CodeService _Service = new Bll.Sys.CodeService();
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