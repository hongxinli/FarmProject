using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Frame
{
    public partial class MainSwitch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //0，普通用户；1，管理员；2，超级管理员
            string Menu_Type = CookieHelper.GetCookie("Menu_Type");
            if (Menu_Type == "0")
            {
                Response.Redirect("~/Frame/MainDefault.aspx");
            }
            else if (Menu_Type == "2")
            {
                Response.Redirect("~/Frame/MainIndex.aspx");
            }
            else
            {
                Response.Redirect("~/Frame/MainDefault.aspx");
            }
        }
    }
}