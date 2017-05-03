using Bll.Sys;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.App_Code;

namespace Web.Base.SysUser
{
    public partial class User_Info : PageBase
    {
        string _key, _deptId;
        public string strDeptHtml = string.Empty;
        public string strRoleHtml = string.Empty;
        DepartmentService bll_dept = new DepartmentService();
        RolesService bll_roles = new RolesService();
        UserInfoService bll_userInfo = new UserInfoService();
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            _deptId = Request["DeptId"];
            InitDeptInfo();
            InitRolesInfo();
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(_key))
                {
                    InitData();
                }
                else
                {
                    Creator.Value = RequestCookie.GetCookieUser().UserName.ToString();
                    CreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            bll_userInfo.InitData(this.Page, _key, Item_Hidden.Value);
            UserPwd.Value = "*************";
        }
        /// <summary>
        /// 初始化单位树
        /// </summary>
        private void InitDeptInfo()
        {
            strDeptHtml = bll_userInfo.InitDeptInfo(_key);
        }
        /// <summary>
        /// 初始化角色树
        /// </summary>
        private void InitRolesInfo()
        {
            strRoleHtml = bll_userInfo.InitRoles(_key);
        }
    }
}