using Bll.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.App_Code;

namespace Web.Base.SysRole
{
    public partial class Role_Info : PageBase
    {
        public string str_tableTree = string.Empty;
        public string str_allUserInfo = string.Empty;
        public string str_seleteUserInfo = string.Empty;
        public string strModuleHtml = string.Empty;
        public string strDeptHtml = string.Empty;
        RolesService bll = new RolesService();
        public string _deptId = string.Empty;
        public string _key = string.Empty;
        public string _Roles_Name = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            _deptId = Request["DeptId"];
            _key = Request["key"];
            _Roles_Name = Request["Roles_Name"] != null ? HttpUtility.UrlDecode(Request["Roles_Name"].ToString()) : "";
            if (!IsPostBack)
            {
                InitParentId();
                InitUserInfo();
                InitMenuInfo();
                InitDeptInfo();
                //if (!string.IsNullOrEmpty(_deptId))
                //{
                //    DeptId.Value = _deptId;
                //}
                if (!string.IsNullOrEmpty(_key))
                {
                    InitData();
                }
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            bll.InitData(this.Page, _key);
        }

        private void InitParentId()
        {
            DepartmentService bll_dept = new DepartmentService();
            bll_dept.InitParentId(DeptId, _deptId);
        }
        /// <summary>
        /// 所有成员
        /// </summary>
        private void InitUserInfo()
        {
            str_allUserInfo = bll.InitUserInfo();
            str_seleteUserInfo = bll.InitUserRole(_key);
        }
        /// <summary>
        /// 模块菜单
        /// </summary>
        private void InitMenuInfo()
        {
            strModuleHtml = bll.GetMenuTreeTable(_key);
        }
        /// <summary>
        /// 数据权限
        /// </summary>
        private void InitDeptInfo()
        {
            strDeptHtml = bll.InitDeptInfo(_key);
        }
    }
}