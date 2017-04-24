using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Web.App_Code;

namespace Web.Frame
{
    public partial class HomeIndex : PageBase
    {
        public StringBuilder sbHomeShortcouHtml = new StringBuilder();
        public String Login_InfoHtml = string.Empty;
        public string _UserName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _UserName = RequestSession.GetSessionUser().UserName.ToString();
            }
        }

    }
}