using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.Sys;
using Common;
using Web.App_Code;

namespace Web.Base.SysDept
{
    public partial class Dept_Form : PageBase
    {
        string _key, _ParentId;
        DepartmentService bll = new DepartmentService();
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            _ParentId = Request["ParentId"];        //父节点
            if (!IsPostBack)
            {
                InitParentId();
                //if (!string.IsNullOrEmpty(_ParentId))
                //{
                //    ParentId.Value = _ParentId;
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
        /// <summary>
        /// 节点位置下拉框绑定
        /// </summary>
        private void InitParentId()
        {
            bll.InitParentId(ParentId, _key);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ParentId.Value))
            {
                ShowMsgHelper.Alert_Wern("请选择节点位置！");
                return;
            }
            bool IsOk = bll.Submit_AddOrEdit(this.Page, ParentId, _key);
            if (IsOk)
            {
                ShowMsgHelper.AlertMsg("操作成功！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("操作失败！");
            }
        }
    }
}