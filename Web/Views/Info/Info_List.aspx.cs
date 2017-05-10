using Bll.Agriculture;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Views.Info
{
    public partial class Info_List : System.Web.UI.Page
    {
        protected InfoService _Service = new InfoService();
        protected void Page_Load(object sender, EventArgs e)
        {
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
        private void DataBindGrid()
        {
            int count = 0;
            DataTable dt = _Service.DataTableByPage(PageControl1.PageIndex, PageControl1.PageSize, ref count);
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
                Label lblIsState = e.Item.FindControl("lb_status") as Label;
                string mark = lblIsState.Text;
                mark = mark.Replace("0", "<span style='color:Blue'> </span>");
                mark = mark.Replace("1", "<span style='color:red'>置 顶</span>");
                lblIsState.Text = mark;
            }
        }
    }
}