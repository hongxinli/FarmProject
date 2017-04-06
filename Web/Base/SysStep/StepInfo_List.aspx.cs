using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.Sys;
using System.Data;
using Common;
using Web.App_Code;

namespace Web.Base.SysStep
{
    public partial class StepInfo_List : PageBase
    {
        public string _DeptId = string.Empty;
        StepInfoService bll = new StepInfoService();
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
            DataTable dt = bll.GetStepInfoPage(_DeptId, PageControl1.PageIndex, PageControl1.PageSize, ref count);
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
                Label lblIsDelete = e.Item.FindControl("lblIsDelete") as Label;
                if (lblIsDelete != null)
                {

                    string textDeleteMark = lblIsDelete.Text;
                    textDeleteMark = textDeleteMark.Replace("0", "<span style='color:Blue'>启用</span>");
                    textDeleteMark = textDeleteMark.Replace("1", "<span style='color:red'>停用</span>");
                    lblIsDelete.Text = textDeleteMark;
                }
            }
        }
    }
}