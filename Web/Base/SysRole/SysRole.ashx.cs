using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bll.Sys;
namespace Web.Base.SysRole
{
    /// <summary>
    /// SysRole 的摘要说明
    /// </summary>
    public class SysRole : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Action = context.Request["action"].Trim();               //提交动作
            string ParentId = context.Request["ParentId"];
            string key = context.Request["key"];//主键
            RolesService bll = new RolesService();
            switch (Action)
            {
                case "IsAllowEdit":
                    int i = bll.IsAllowEdit(key);
                    context.Response.Write(i);
                    context.Response.End();
                    break;
                case "delete":
                    #region 删除操作
                    if (bll.IsAllowDelete(key))
                    {
                        if (bll.UpdateDeleteMark(key))
                        {
                            string moduleId = "SysRole";
                            #region 操作日志记录
                            string actStr = "该用户对-角色编号：[" + key + "]进行了删除操作。";
                            Bll.BaseService.WriteLogEvent(actStr, moduleId);
                            #endregion
                            context.Response.Write(2); //删除成功
                            context.Response.End();
                        }
                        else
                        {
                            context.Response.Write(1); //删除失败
                            context.Response.End();
                        }
                    }
                    else
                    {
                        context.Response.Write(0); //不允许删除
                        context.Response.End();
                    }
                    #endregion
                    break;
                case "IsExists":
                    if (bll.IsExistsUserRole(key))
                    {
                        context.Response.Write(1); //存在
                        context.Response.End();
                    }
                    else
                    {
                        context.Response.Write(0); //不存在
                        context.Response.End();
                    }
                    break;
                default:
                    break;
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