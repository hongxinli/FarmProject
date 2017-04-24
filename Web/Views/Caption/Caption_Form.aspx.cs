using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.App_Code;

namespace Web.Views.Caption
{
    public partial class Caption_Form : PageBase
    {
        Bll.Agriculture.CaptionService _Service = new Bll.Agriculture.CaptionService();
        protected string _key;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            if (!IsPostBack)
            {
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
            _Service.InitData(this.Page, _key);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            bool IsOk = _Service.Submit_AddOrEdit(this.Page,_key);
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