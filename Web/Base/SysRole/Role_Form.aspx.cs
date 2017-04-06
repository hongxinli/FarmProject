using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.Sys;
using Common;
using Web.App_Code;

namespace Web.Base.SysRole
{
    public partial class Role_Form : PageBase
    {
        public string str_tableTree = string.Empty;
        public string str_allUserInfo = string.Empty;
        public string str_seleteUserInfo = string.Empty;
        public string strModuleHtml = string.Empty;
        public string strDeptHtml = string.Empty;
        RolesService bll = new RolesService();
        public string _deptId = string.Empty;
        public string _key = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            _deptId = Request["DeptId"];
            _key = Request["key"];
            InitParentId();
            InitUserInfo();
            InitMenuInfo();
            InitDeptInfo();
            if (!IsPostBack)
            {
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
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            string _roleId = string.Empty;
            bool IsOk = bll.Submit_AddOrEdit(this.Page, _key, out _roleId);
            if (IsOk)
            {
                string str = Item_Hidden.Value;
                if (!string.IsNullOrEmpty(str))
                {
                    str = Item_Hidden.Value.Substring(0, Item_Hidden.Value.Length - 1);
                }
                bool IsAllto = bll.add_ItemForm(str, _roleId);
                if (IsAllto)
                {
                    ShowMsgHelper.ParmAlertMsg("操作成功！");
                }
                else
                {
                    ShowMsgHelper.Alert_Error("操作失败！");
                }
            }
            else
            {
                ShowMsgHelper.Alert_Error("操作失败！");
            }

        }
    }
}