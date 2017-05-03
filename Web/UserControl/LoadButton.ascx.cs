using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.Sys;

namespace Web.UserControl
{
    public partial class LoadButton : System.Web.UI.UserControl
    {
        public string sb_Button = string.Empty;
        RolesService bll = new RolesService();
        protected void Page_Load(object sender, EventArgs e)
        {
            string _UserId = RequestCookie.GetCookieUser().UserId.ToString();//用户ID
            sb_Button = bll.GetButtonHtml(_UserId);
        }
    }
}