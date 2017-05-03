using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Error
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestCookie.GetCookieUser() == null) {
                Label1.Text = "用户登录超时，请重新登录。";
            }
            else
            {
                Label1.Text = Application["error"].ToString();
            }
           
        }
    }
}