using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Bll.Sys;
using Common;

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
    }
}