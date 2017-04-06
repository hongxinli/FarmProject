using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.Sys;
using Web.App_Code;
namespace Web.Base.SysModule
{
    public partial class AllotButton_Form : PageBase
    {
        ButtonService bll_button = new ButtonService();
        ModuleInfoService bll_module = new ModuleInfoService();
        public string ButtonList = string.Empty;
        public string selectedButtonList = string.Empty;
        public string _ParentId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            _ParentId = Request["key"];                  //主键
            InitButtonList();
        }
        /// <summary>
        /// 初始化按钮
        /// </summary>
        public void InitButtonList()
        {
            ButtonList = bll_button.GetButtonList();
            selectedButtonList = bll_module.GetModuleByButton(_ParentId);
        }
    }
}