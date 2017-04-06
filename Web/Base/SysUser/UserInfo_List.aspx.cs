using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.Sys;
using Common;
using System.Data;
using Web.App_Code;

namespace Web.Base.SysUser
{
    public partial class UserInfo_List : PageBase
    {
        public string _DeptId = string.Empty;
        UserInfoService bll = new UserInfoService();
        protected void Page_Load(object sender, EventArgs e)
        {
            _DeptId = Request["key"];
            this.PageControl1.pageHandler += new EventHandler(pager_PageChanged);
        }
        /// <summary>
        /// 绑定数据，分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void pager_PageChanged(object sender, EventArgs e)
        {
            DataBindGrid();
        }
        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void DataBindGrid()
        {
            int count = 0;
            DataTable dt = bll.GetUserInfoPage(Searchwhere.Value, txt_Search.Value, _DeptId, PageControl1.PageIndex, PageControl1.PageSize, ref count);
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
            this.PageControl1.RecordCount = count;
        }

        /// <summary>
        /// 绑定后激发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblIsAdmin = e.Item.FindControl("lblIsAdmin") as Label;
                Label lblIsState = e.Item.FindControl("lblIsState") as Label;
                if (lblIsAdmin != null)
                {
                    string text = lblIsAdmin.Text;
                    text = text.Replace("0", "普通用户");
                    text = text.Replace("1", "管理员");
                    text = text.Replace("2", "超级管理员");
                    lblIsAdmin.Text = text;

                    string textDeleteMark = lblIsState.Text;
                    textDeleteMark = textDeleteMark.Replace("0", "<span style='color:Blue'>启用</span>");
                    textDeleteMark = textDeleteMark.Replace("1", "<span style='color:red'>停用</span>");
                    lblIsState.Text = textDeleteMark;
                }
            }
        }
        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            DataBindGrid();
            this.PageControl1.PageChecking();
        }
    }
}