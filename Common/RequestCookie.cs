using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Common
{
    public class RequestCookie
    {
        public RequestCookie() { }

        private static string COOKIE_USER = "COOKIE_USER";

        public static void AddCookieUser(SessionUser user)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = HttpUtility.UrlEncode(js.Serialize(user));
            CookieHelper.WriteCookie("currentUser", json);
        }

        public static void RemoveCookieUser()
        {
            CookieHelper.RemoveCookie("currentUser");
        }
        public static CookieUser GetCookieUser()
        {

            string json = HttpUtility.UrlDecode(CookieHelper.GetCookie("currentUser"));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<CookieUser>(json);
        }
    }
}
