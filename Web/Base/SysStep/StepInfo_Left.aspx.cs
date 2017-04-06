using Bll.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.App_Code;

namespace Web.Base.SysStep
{
    public partial class StepInfo_Left : PageBase
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