using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Web.App_Code;

namespace Web.Frame
{
    public partial class MainDefault : PageBase
    {
        protected string UserName = string.Empty;
        protected string DepartName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserName = RequestSession.GetSessionUser().UserName.ToString();
            DepartName = RequestSession.GetSessionUser().DeptName.ToString();
        }
    }
}