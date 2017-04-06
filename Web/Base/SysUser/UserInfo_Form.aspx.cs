using Common;
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
    public partial class UserInfo_Form : PageBase
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
                    Creator.Value = RequestSession.GetSessionUser().UserName.ToString();
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
        #region 保存事件
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            bool IsExits = bll_userInfo.IsExits(UserId.Value.Trim());
            if (!string.IsNullOrEmpty(_key)) //修改功能不需要判断是否存在
                IsExits = false;
            if (!IsExits)
            {
                bool IsOk = bll_userInfo.Submit_AddOrEdit(this.Page, _key);
                if (IsOk)
                {
                    string str = Item_Hidden.Value;
                    string _UserId = UserId.Value.Trim();
                    string oldUserId = "";
                    if (string.IsNullOrEmpty(_key))
                    {
                        oldUserId= UserId.Value.Trim();
                    }
                    else
                    {
                        oldUserId = _key;
                    }
                    bool IsAllto = bll_userInfo.add_ItemForm(str, _UserId, oldUserId);
                    if (IsAllto)
                    {
                        ShowMsgHelper.ParmAlertMsg("操作成功！");
                    }
                    else
                    {
                        ShowMsgHelper.Alert_Error("操作失败！");
                    }
                   // ShowMsgHelper.AlertMsg("操作成功！");
                }
                else
                {
                    ShowMsgHelper.Alert_Error("操作失败！");
                }
            }
            else
            {
                ShowMsgHelper.Alert_Error("该用户已经存在！");
            }
        }
        #endregion
    }
}