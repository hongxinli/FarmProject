using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Bll.Sys;
using Common;
using System.Configuration;
using Common.Constant;

namespace Web
{
    public class Global : System.Web.HttpApplication
    {
        static LogService bll_log = new LogService();
        protected void Application_Start(object sender, EventArgs e)
        {
            // 计算人数
            Application.Lock();
            Application["CurrentUsers"] = 0;
            Application.UnLock();
           
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // 计算人数
            Application.Lock();
            Application["CurrentUsers"] = (int)Application["CurrentUsers"] + 1;
            Application.UnLock();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError().GetBaseException();
            string error = objErr.Message + "";
            Server.ClearError();
            Application["error"] = error;
            Response.Redirect("~/Error/ErrorPage.aspx");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["CurrentUsers"] = (int)Application["CurrentUsers"] - 1;
            Application.UnLock();
            // bll_log.SysLoginLog(RequestSession.GetSessionUser(), false);
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private bool Lincese()
        {
            var product = ConfigurationManager.AppSettings["Product"];
            var ticket = Tincher.Interop.License.GetInstance(System.Web.HttpContext.Current.Server.MapPath("/") + "/" + product + ".license");
            //缓存

            DataCache.SetCache(Naming.APP_LINCESE, ticket, Value.MAX_MINUTES_TO_CACHE);
            DataCache.SetCache(Naming.APP_LICENSE_KEY, ticket.MachineCode, Value.MAX_MINUTES_TO_CACHE);
            DataCache.SetCache(Naming.APP_LINCESE_ACCESS, ticket.Allow, Value.MAX_MINUTES_TO_CACHE);
            DataCache.SetCache(Naming.APP_LICENSE_PRODUCT, product, Value.MAX_MINUTES_TO_CACHE);
            if (ticket.Allow)
            {
                DataCache.SetCache(Naming.APP_LICENSE_CUSTOMER, ticket.Customer, Value.MAX_MINUTES_TO_CACHE);
            }
            return ticket.Allow;
        }
    }
}