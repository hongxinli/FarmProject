using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;

namespace Common
{
    /// <summary>
    /// Session 帮助类
    /// </summary>
    public class RequestSession : IRequiresSessionState
    {
        public RequestSession()
        {

        }
        private static string SESSION_USER = "SESSION_USER";
        public static void AddSessionUser(SessionUser user)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = HttpUtility.UrlEncode(js.Serialize(user));
            CookieHelper.WriteCookie("currentUser", json);
            //HttpContext rq = HttpContext.Current;
            //rq.Session[SESSION_USER] = user;
        }

        public static void RemoveSessionUser()
        {

            //HttpContext rq = HttpContext.Current;
            //rq.Session.Remove(SESSION_USER);
            CookieHelper.RemoveCookie("currentUser");
        }
        public static SessionUser GetSessionUser()
        {

            string json = HttpUtility.UrlDecode(CookieHelper.GetCookie("currentUser"));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<SessionUser>(json);
            //HttpContext rq = HttpContext.Current;
            //return (SessionUser)rq.Session[SESSION_USER];
        }
    }
}
