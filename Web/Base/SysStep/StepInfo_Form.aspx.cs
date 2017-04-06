using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.Sys;
using Common;
using Web.App_Code;

namespace Web.Base.SysStep
{
    public partial class StepInfo_Form : PageBase
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
                else
                {
                    Creator.Value = RequestSession.GetSessionUser().UserName.ToString();
                    CreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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

        #region 保存事件
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            string StepId = string.Empty;
            bool IsOk = bll.Submit_AddOrEdit(this.Page, _key, out StepId);
            if (IsOk)
            {
                string str = Item_Hidden.Value;
                bool IsAllto = bll.add_ItemForm(str, StepId);
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
        #endregion
    }
}