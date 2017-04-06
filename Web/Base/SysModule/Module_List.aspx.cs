using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.App_Code;

namespace Web.Base.SysModule
{
    public partial class Module_List : PageBase
    {
        public string TableTree_Menu = string.Empty;
        Bll.Sys.ModuleInfoService moduleService = new Bll.Sys.ModuleInfoService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TableTree_Menu = moduleService.GetMenuTreeTable();
            }
        }
    }
}