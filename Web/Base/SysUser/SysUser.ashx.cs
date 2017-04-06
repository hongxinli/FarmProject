using Bll.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Base.SysUser
{
    /// <summary>
    /// SysUser 的摘要说明
    /// </summary>
    public class SysUser : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Action = context.Request["action"].Trim();               //提交动作
            string ParentId = context.Request["ParentId"];
            string key = context.Request["key"];//主键
            UserInfoService bll = new UserInfoService();
            string moduleId = "SysUser";
            switch (Action)
            {
                case "delete":
                    if (bll.Delete(key))
                    {
                        #region 操作日志记录
                        string actStr = "该用户对-用户编号：[" + key + "]进行了删除操作。";
                        Bll.BaseService.WriteLogEvent(actStr, moduleId);
                        #endregion
                        context.Response.Write(1); //删除成功
                        context.Response.End();
                    }
                    else
                    {
                        context.Response.Write(2); //删除失败
                        context.Response.End();
                    }
                    break;
                case "accredit":
                    if (bll.Update(key, 0)) {
                        #region 操作日志记录
                        string actStr = "该用户对-用户编号：[" + key + "]进行了授权操作。";
                        Bll.BaseService.WriteLogEvent(actStr, moduleId);
                        #endregion
                        context.Response.Write(1); //授权成功
                        context.Response.End();
                    }
                    else
                    {
                        context.Response.Write(2); //授权失败
                        context.Response.End();
                    }
                    break;
                case "lock":
                    if (bll.Update(key, 1))
                    {
                        #region 操作日志记录
                        string actStr = "该用户对-用户编号：[" + key + "]进行了锁定操作。";
                        Bll.BaseService.WriteLogEvent(actStr, moduleId);
                        #endregion
                        context.Response.Write(1); //锁定成功
                        context.Response.End();
                    }
                    else
                    {
                        context.Response.Write(2); //锁定失败
                        context.Response.End();
                    }
                    break;
                default: break;
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