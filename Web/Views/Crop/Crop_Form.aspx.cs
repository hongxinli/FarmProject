using Bll.Agriculture;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Views.Crop
{
    public partial class Crop_Form : System.Web.UI.Page
    {
        private CropService _Service = new CropService();
        protected string _key;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            if (!IsPostBack)
            {
                InitCropType();
                if (!string.IsNullOrEmpty(_key))
                {
                    InitData();
                }
            }
        }
        /// <summary>
        /// 农作物类别下拉框绑定
        /// </summary>
        private void InitCropType()
        {
            _Service.InitCropType(CropType, _key);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            _Service.InitData(this.Page, _key);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            bool IsOk = _Service.Submit_AddOrEdit(this.Page, _key);
            if (IsOk)
            {
                ShowMsgHelper.Alert("操作成功！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("操作失败！");
            }
        }
    }
}