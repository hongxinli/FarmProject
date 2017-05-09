using Bll.Sys;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Base.SysCode
{
    public partial class Code_List : System.Web.UI.Page
    {
        protected CodeService _Service = new CodeService();
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
                Image lb_img = e.Item.FindControl("lb_img") as Image;
                if (lb_img != null && !string.IsNullOrEmpty(lb_img.ImageUrl))
                {
                    lb_img.Visible = true;
                }
                else
                {
                    lb_img.Visible = false;
                }
            }
        }
    }
}