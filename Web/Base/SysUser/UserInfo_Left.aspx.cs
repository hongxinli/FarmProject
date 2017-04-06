using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.Sys;
using Web.App_Code;

namespace Web.Base.SysUser
{
    public partial class UserInfo_Left : PageBase
    {
        public string strHtml = string.Empty;
        DepartmentService bll = new DepartmentService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                strHtml = bll.GetDeptTree();
            }
        }
    }
}