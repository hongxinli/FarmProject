using Bll.Agriculture;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Views.Crop
{
    public partial class Crop_List : System.Web.UI.Page
    {
        protected CropService _Service = new CropService();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageControl1.pageHandler += new EventHandler(pager_PageChanged);
            if (!IsPostBack)
            {
                InitCropType();
            }
        }
        /// <summary>
        /// 农作物类别下拉框绑定
        /// </summary>
        private void InitCropType()
        {
            _Service.InitCropType(CropType, "");
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
            StringBuilder sb = new StringBuilder();
            sb.Append(" CropType='" + CropType.Value + "' ");
            if (!string.IsNullOrEmpty(txt_Search.Value))
                sb.Append(" and CropName like '%" + txt_Search.Value + "%'");
            int count = 0;
            DataTable dt = _Service.DataTableByPage(PageControl1.PageIndex, PageControl1.PageSize, sb.ToString(), ref count);
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
            this.PageControl1.RecordCount = count;
        }
        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            PageControl1.PageIndex = 1;
            PageControl1.PageSize = 15;
            DataBindGrid();
            this.PageControl1.PageChecking();
        }
    }
}