using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Common
{
    /// <summary>
    /// 客户端提示信息帮助类
    /// </summary>
    public class ShowMsgHelper
    {
        /// <summary>
        /// 默认成功提示
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void Alert(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');", message));
        }
        /// <summary>
        /// 默认成功提示，刷新父窗口函数关闭页面
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void AlertMsg(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');top.main.windowload();OpenClose();", message));
        }
        /// <summary>
        /// 默认成功提示，刷新父窗口函数关闭页面
        /// </summary>
        /// <param name="message"></param>
        /// <param name="pageIndex">当前页索引值</param>
        public static void AlertMsg(string message,string pageIndex)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');top.main.windowloadByIndex(" + pageIndex + ");OpenClose();", message));
        }
        public static void AlertWsTreeMsg(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');gettopurl();OpenClose();", message));
        }
        /// <summary>
        /// 默认成功提示，不刷新父窗口函数关闭页面 为计量站器具分配页面提供
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void AlertMsgForJlzQjfp(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');OpenClose();", message));
        }
        /// <summary>
        /// 默认成功提示，刷新父窗口函数关闭页面进行跳转 by wendy
        /// </summary>
        /// <param name="message"></param>
        public static void MyAlertMsg(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');top.main.target_right.windowload();closewindow();", message));
        }
        /// <summary>
        /// 默认成功提示，刷新父窗口函数关闭页面
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void ParmAlertMsg(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');top.main.target_right.windowload();OpenClose();", message));
        }
        /// <summary>
        ///默认成功提示， 返回父页面
        /// </summary>
        /// <param name="message"></param>
        public static void AlertMsgToParentPage(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');top.main.windowload();closewindow();", message));
        }
        public static void AlertMsgToParentPageNoRefresh(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');closewindow();", message));
        }
        /// <summary>
        /// 节能站计算模块添加数据时的提示
        /// </summary>
        /// <param name="message"></param>
        public static void AlertMsgForJnz(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');top.main.windowload();", message));
        }
        /// <summary>
        /// 计量站数据添加修改时不刷新页面只提示
        /// </summary>
        /// <param name="message"></param>
        public static void AlertMsgForJlzSjAdd(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');", message));
        }
        /// <summary>
        /// 外出作业票单独用
        /// </summary>
        /// <param name="message"></param>
        public static void AlertMsgToParentPageforwczyp(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');closewindow();", message));
        }
        public static void AlertToNextPage(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','2500','4');top.main.windowload();ToNextPage();", message));
        }
        /// <summary>
        /// 默认错误提示
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void Alert_Error(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','5000','5');", message));
        }

        public static void Alert_Success(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','5000','4');", message));
        }
        /// <summary>
        /// 默认警告提示
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void Alert_Wern(string message)
        {
            ExecuteScript(string.Format("showTipsMsg('{0}','3000','3');", message));
        }
        /// <summary>
        /// 提示警告信息
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void showFaceMsg(string message)
        {
            ExecuteScript(string.Format("showFaceMsg('{0}');", message));
        }
        /// <summary>
        /// 提示警告信息
        /// </summary>
        /// <param name="message">显示消息</param>
        public static void showWarningMsg(string message)
        {
            ExecuteScript(string.Format("showWarningMsg('{0}');", message));
        }

        /// <summary>
        /// 后台调用JS函数
        /// </summary>
        /// <param name="obj"></param>
        public static void ShowScript(string strobj)
        {
            Page p = HttpContext.Current.Handler as Page;
            p.ClientScript.RegisterStartupScript(p.ClientScript.GetType(), "myscript", "<script>" + strobj + "</script>");
        }
        public static void ExecuteScript(string scriptBody)
        {
            string scriptKey = "Somekey";
            Page p = HttpContext.Current.Handler as Page;
            p.ClientScript.RegisterStartupScript(typeof(string), scriptKey, scriptBody, true);
        }
    }
}
