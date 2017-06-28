using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoginPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HttpCookie cookies = Request.Cookies["USER_COOKIE"];

            // 如果此Cookies存在且它里面有子键则进行读取
            if (cookies != null && cookies.HasKeys)
            {
                this.username.Value = cookies["Userid"];
                // 密码框赋值
                this.pwd.Attributes.Add("value", cookies["Pwd"]);

                // 并设置勾选记住密码
                this.saveCookie.Checked = true;
            }
        }
    }
    protected void Login_btn_Click(object sender, ImageClickEventArgs e)
    {
        SavePwdFun(this.saveCookie.Checked);
        Response.Redirect("MapPage.aspx");
    }

    protected void SavePwdFun(bool check)
    {
        HttpCookie cookie = new HttpCookie("USER_COOKIE");
        if (check)
        {
            // 设置用户、密码
            cookie.Values.Add("Userid", this.username.Value);
            cookie.Values.Add("Pwd", this.pwd.Text);

            // 令 Cookie 永不过期
            cookie.Expires = System.DateTime.Now.AddDays(7.0);

            // 保存用户的 Cookie
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        else  // 若记住密码未勾选则默认这次取消记住密码，则将原本存密码的Cookies生存期设为现在，则会自动销毁
        {
            if (Response.Cookies["USER_COOKIE"] != null)
                Response.Cookies["USER_COOKIE"].Expires = DateTime.Now;
        }
    }
}