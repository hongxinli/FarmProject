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
    public partial class Step_Info : PageBase
    {
        string _key, _deptId;
        public string strDeptHtml = string.Empty;
        public string strRoleHtml = string.Empty;
        StepInfoService bll = new StepInfoService();
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
            }
        }
        /// <summary>
        /// 初始化单位树
        /// </summary>
        private void InitDeptInfo()
        {
            strDeptHtml = bll.InitDeptInfo(_key);
        }
        /// <summary>
        /// 初始化角色树
        /// </summary>
        private void InitRolesInfo()
        {
            strRoleHtml = bll.InitRoles(_key);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            bll.InitData(this.Page, _key, Item_Hidden.Value);
        }

    }
}