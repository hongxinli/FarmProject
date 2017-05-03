using Common;
using Common.Constant;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Web.App_Code
{
    /// <summary>
    /// 基类
    /// </summary>
    public class PageBase : Page
    {
        protected override void OnLoad(EventArgs e)
        {
           

            GetReferer(true);

            #region 当Session过期自动跳出登录画面
            if (RequestCookie.GetCookieUser() == null)
            {
                Session.Abandon();  //取消当前会话
                Session.Clear();
                Response.Redirect("/Index.htm");
            }
            #endregion

            #region 防止刷新重复提交
            if (null == Session["Token"])
            {
                WebHelper.SetToken();
            }
            #endregion

            #region URL权限验证,拒绝，不合法的请求
            URLPermission();
            #endregion

            base.OnLoad(e);
        }
        /// <summary>
        /// 获取HTTP请求的Referer
        /// </summary>
        /// <param name="ishost">Referer为空时是否返回Host（网站首页地址）</param>
        /// <returns>string</returns>
        public string GetReferer(bool ishost)
        {
            if (Request.UrlReferrer != null)
            {
                return Request.UrlReferrer.ToString();
            }
            else
            {
                if (ishost)
                {
                    return Request.Url.Scheme + "://" + Request.Url.Authority;
                }
                else
                {
                    return "";
                }
            }
        }

        #region URL权限验证,拒绝，不合法的请求
        /// <summary>
        /// URL权限验证,拒绝，不合法的请求
        /// </summary>
        public void URLPermission()
        {
            bool IsOK = false;
            //获取当前访问页面地址
            string requestPath = RequestHelper.GetScriptName;
            string[] filterUrl = { "/Frame/HomeIndex.aspx", "/Base/SysUser/UpdateUserPwd.aspx", "/Frame/MainDefault.aspx", "/Frame/MainIndex.aspx" };//过滤特别页面
            //对上传的文件的类型进行一个个匹对
            for (int i = 0; i < filterUrl.Length; i++)
            {
                if (requestPath == filterUrl[i])
                {
                    IsOK = true;
                    break;
                }
            }
            if (!IsOK)
            {
                string UserId = RequestCookie.GetCookieUser().UserId.ToString();//用户ID
                Bll.Sys.RolesService bll = new Bll.Sys.RolesService();
                bool IsOk = bll.GetPermission_URL(UserId, requestPath);
                if (!IsOk)
                {
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<div style='text-align: center; line-height: 300px;'>");
                    strHTML.Append("<font style=\"font-size: 13;font-weight: bold; color: red;\">权限不足</font></div>");
                    HttpContext.Current.Response.Write(strHTML.ToString());
                    HttpContext.Current.Response.End();
                }
            }

        }
        #endregion

        
    }
}