﻿using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.Agriculture;
using Web.App_Code;

namespace Web.Views.Caption
{
    public partial class Caption_List : PageBase
    {
        protected CaptionService _Service = new CaptionService();
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
    }
}