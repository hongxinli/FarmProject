using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.Sys;
using Web.App_Code;

namespace Web.Base.SysRole
{
    public partial class Role_List : PageBase
    {
        public string _DeptIds = string.Empty;
        public string _deptId = string.Empty;
        public string str_tableTree = string.Empty;
        RolesService bll = new RolesService();
        protected void Page_Load(object sender, EventArgs e)
        {
            _DeptIds = Request["key"];
            if (!string.IsNullOrEmpty(_DeptIds))
            {
                _deptId = _DeptIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0].Replace("'", "").Trim();
            }
            if (!IsPostBack)
            {
                GetTreeTable();
            }
        }

        private void GetTreeTable()
        {
            str_tableTree = bll.GetRoleList(_DeptIds);
        }
    }
}