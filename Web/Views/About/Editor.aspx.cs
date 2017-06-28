using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Views.About
{
    public partial class Editor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            bool IsOk = true;
            string content = AboutContent.Value;
            string path = Server.MapPath("content.txt");
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {

                //获得字节数组
                byte[] data = System.Text.Encoding.UTF8.GetBytes(content);
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();
            }
            string str = File.ReadAllText(Server.MapPath("~/Templets/About.html"));
            str = str.Replace("${content}", content);
            using (StreamWriter sw = File.CreateText(Server.MapPath("a.html")))
            {
                sw.Write(str);
            }
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