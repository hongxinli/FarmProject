using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDal.Sys;
using OracleDal.Sys;

namespace Bll.Sys
{
    public class ButtonService
    {
        IButton dal = new ButtonRepository();
        /// <summary>
        /// 所有按钮信息
        /// </summary>
        /// <returns></returns>
        public string GetButtonList()
        {
            StringBuilder ButtonList = new StringBuilder();
            IList<Model.Base_Button> list = dal.List(" DeleteMark=0 order by SortCode asc");
            for (int i = 0; i < list.Count; i++)
            {
                ButtonList.Append("<div id=" + list[i].Id + " onclick='selectedButton(this)' title='" + list[i].ButtonName + "' class=\"shortcuticons\"><img src=\"/Themes/Images/16/" + list[i].ButtonImg + "\" alt=\"\" /><br />" + list[i].ButtonName + "</div>");
            }

            return ButtonList.ToString();
        }
    }
}
