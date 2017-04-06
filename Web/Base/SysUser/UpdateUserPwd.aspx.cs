using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.App_Code;
using Common;
using Bll.Sys;

namespace Web.Base.SysUser
{
    public partial class UpdateUserPwd : PageBase
    {
        public string _PasPwd = string.Empty;
        UserInfoService bll = new UserInfoService();
        protected void Page_Load(object sender, EventArgs e)
        {
            txtUserName.Value = RequestSession.GetSessionUser().UserId.ToString();
            _PasPwd = RequestSession.GetSessionUser().UserPwd.ToString();
        }
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            bool IsOk = bll.UpdatePwd(txtUserName.Value.Trim(), Md5Helper.MD5(txtUserPwd.Value, 32));
            if (IsOk)
            {
                Session.Abandon();  //取消当前会话
                Session.Clear();    //清除当前浏览器所以Session
                Response.Write("<script>alert('登陆修改成功,请重新登陆');top.location.href='/Index.htm'</script>");
            }
            else
            {
                errorMsg.InnerHtml = "修改登录密码失败";
            }
        }
    }
}