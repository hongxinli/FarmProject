using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bll.Sys;
using System.Web.SessionState;

namespace Web.Base.SysModule
{
    /// <summary>
    /// SysModule 的摘要说明
    /// </summary>
    public class SysModule : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
            string Action = context.Request["action"].Trim();               //提交动作
            string ParentId = context.Request["ParentId"];
            string key = context.Request["key"];//主键
            ModuleInfoService bll = new ModuleInfoService();
            switch (Action)
            {
                case "addButton"://菜单添加按钮
                    context.Response.Write(bll.AddButton(key, ParentId));
                    context.Response.End();
                    break;
                case "removeButton"://菜单移除按钮
                    context.Response.Write(bll.RemoveButton(key));
                    context.Response.End();
                    break;
                case "delete":
                    int i = bll.IsAllowDelete(key);
                    if (i == 0)
                    {
                        context.Response.Write(0); //不允许删除
                        context.Response.End();
                    }
                    else
                    {
                        if (bll.IsExists(key))
                        {
                            context.Response.Write(1); //该数据已被关联，不允许删除
                            context.Response.End();
                        }
                        else
                        {
                            if (bll.UpdateDeleteMark(key))
                            {
                                string moduleId = "SysModule";
                                #region 操作日志记录
                                string actStr = "该用户对-模块编号：[" + key + "]进行了删除操作。";
                                Bll.BaseService.WriteLogEvent(actStr, moduleId);
                                #endregion
                                context.Response.Write(3); //删除成功
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.Write(2); //删除失败
                                context.Response.End();
                            }
                        }
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