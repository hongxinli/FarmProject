using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.App_Code;
using Bll.Sys;
using Common;

namespace Web.Base.SysLog
{
    public partial class LoginList : PageBase
    {
        LogService bll = new LogService();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageControl1.pageHandler += new EventHandler(pager_PageChanged);
        }
        /// <summary>
        /// 自定义分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pager_PageChanged(object sender, EventArgs e)
        {
            DataBindGrid();
        }
        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void DataBindGrid()
        {
            int count = 0;
            DataTable dt = bll.GetSysLoginLogPage(txt_Search.Value.Trim(), BeginTime.Value.Trim(), endTime.Value.Trim(), PageControl1.PageIndex, PageControl1.PageSize, ref count);
            dt = Bll.BaseService.ReturnRightData(dt);
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
            this.PageControl1.RecordCount = Convert.ToInt32(dt.Rows.Count);
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