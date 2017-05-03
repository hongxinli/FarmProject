using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bll.Sys;
using System.Web.SessionState;
using System.Data;
using System.Text;
using Bll;

namespace Web.Frame
{
    /// <summary>
    /// Frame 的摘要说明
    /// </summary>
    public class Frame : IHttpHandler, IRequiresSessionState
    {
        Bll.Sys.ModuleInfoService bll_Module = new Bll.Sys.ModuleInfoService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Buffer = true;
            //context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            //context.Response.AddHeader("pragma", "no-cache");
            //context.Response.AddHeader("cache-control", "");
            //context.Response.CacheControl = "no-cache";

            string Action = context.Request["action"];                      //提交动作
            string _UserId = context.Request["_UserId"];          //账户
            string _UserPwd = context.Request["_UserPwd"];                    //密码
            switch (Action)
            {
                case "login":
                    #region 用户登录
                    UserInfoService bll = new UserInfoService();
                    LogService bll_log = new LogService();
                    Model.Base_UserInfo model_user = bll.UserLogin(_UserId, _UserPwd);
                    if (model_user != null)
                    {
                        if (model_user.IsState == 0)
                        {
                            DepartmentService bll_dept = new DepartmentService();
                            Model.Base_UserDept model = bll_dept.GetModel(_UserId);
                            SessionUser user = new SessionUser();
                            user.Id = CommonHelper.GetGuid;
                            user.UserId = model_user.UserId;
                            user.UserName = model_user.UserName;
                            user.UserPwd = model_user.UserPwd;
                            user.RoleId = bll.GetUserRoleId(_UserId);
                            user.DeptId = model.DeptId;
                            user.Theme = model_user.Theme;
                            user.IsAdmin = model_user.IsAdmin;
                            user.DeptName = bll_dept.GetDepartment(model.DeptId).DeptName;
                            RequestCookie.AddCookieUser(user);
                            bll_log.SysLoginLog(user, true);  //新增登录日志
                            CookieHelper.WriteCookie("Menu_Type", model_user.Theme); //记录当前用户的系统主题
                            context.Response.Write("4");//验证成功
                            context.Response.End();
                        }
                        else
                        {
                            context.Response.Write("2");//账户被锁,联系管理员！
                            context.Response.End();
                        }
                    }
                    else
                    {
                        context.Response.Write("1");//账户或者密码有错误！
                        context.Response.End();
                    }
                    #endregion
                    break;
                case "Menu":
                    context.Response.Write(bll_Module.GetMenuHtml());
                    context.Response.End();
                    break;
                case "MenuAdmin":
                    context.Response.Write(bll_Module.GetAdminMenu());
                    context.Response.End();
                    break;
                case "MenuChildren":
                    string _ParentId = context.Request["parentId"];
                    context.Response.Write(GetMenu(_ParentId));
                    context.Response.End();
                    break;
                case "logout":
                    RequestCookie.RemoveCookieUser();
                    break;
                default:
                    break;
            }
        }

        private string GetMenu(string _ParentId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = bll_Module.GetAdminMenuTable(_ParentId);
            DataTable dt_Department = bll_Module.GetMenuDepartment();
            DataRow[] drs = dt.Select("DEPTID is null");
            DataRow[] drss = dt.Select("DEPTID is not null");
            if (drs.Length > 0)
            {
                for (int i = 0; i < drs.Length; i++)
                {
                    sb.Append("<li>");
                    sb.Append("<div onclick=\"NavMenu('" + drs[i]["NAVIGATEURL"] + "','" + drs[i]["MODULENAME"] + "')\">");
                    if (string.IsNullOrEmpty(drs[i]["MODULEIMG"].ToString()))
                    {
                        sb.Append("<img width=\"16\" height=\"16\" src=\"/Themes/Images/32/286.png\" />");
                    }
                    else
                    {
                        sb.Append("<img width=\"16\" height=\"16\" src=\"/Themes/Images/32/" + drs[i]["MODULEIMG"] + "\" />");
                    }
                    sb.Append(drs[i]["MODULENAME"]);
                    sb.Append("</div></li>");
                }
            }
            if (drss.Length > 0)
            {
                string deptids = Bll.BaseService.ReturnAuthority();
                for (int i = 0; i < dt_Department.Rows.Count; i++)
                {
                    DataRow[] drsss = dt.Select("DEPTID ='" + dt_Department.Rows[i]["DEPTID"] + "' and DEPTID in (" + deptids + ")");
                    if (drsss.Length == 0 || drsss == null)
                        continue;
                    sb.Append("<li>");
                    sb.Append("<div>");
                    sb.Append(dt_Department.Rows[i]["DEPTNAME"]);
                    sb.Append("</div>");
                    sb.Append("<ul>");
                    sb.Append(GetMenuChildren(dt, dt_Department.Rows[i]["DEPTID"].ToString()));
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }
            }
            return sb.ToString();
        }

        private string GetMenuChildren(DataTable dt, string _deptId)
        {
            StringBuilder sb = new StringBuilder();
            DataRow[] drs = dt.Select("DEPTID='" + _deptId + "'");
            for (int i = 0; i < drs.Length; i++)
            {
                sb.Append("<li>");
                sb.Append("<div onclick=\"NavMenu('" + drs[i]["NAVIGATEURL"] + "','" + drs[i]["MODULENAME"] + "')\">");
                if (string.IsNullOrEmpty(drs[i]["MODULEIMG"].ToString()))
                {
                    sb.Append("<img width=\"16\" height=\"16\" src=\"/Themes/Images/32/286.png\" />");
                }
                else
                {
                    sb.Append("<img width=\"16\" height=\"16\" src=\"/Themes/Images/32/" + drs[i]["MODULEIMG"] + "\" />");
                }
                sb.Append(drs[i]["MODULENAME"]);
                sb.Append("</div></li>");
            }
            return sb.ToString();
        }
        public bool IsLogin(HttpContext context, string _UserId)
        {
            if (context.Session != null)
            {
                SessionUser CurrentUsers = context.Session["SESSION_USER"] as SessionUser;
                if (CurrentUsers.UserId.Equals(_UserId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}