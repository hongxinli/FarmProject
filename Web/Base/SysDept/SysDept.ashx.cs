using Bll;
using Bll.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Base.SysDept
{
    /// <summary>
    /// SysDept 的摘要说明
    /// </summary>
    public class SysDept : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Action = context.Request["action"].Trim();               //提交动作
            string ParentId = context.Request["ParentId"];
            string key = context.Request["key"];//主键
            DepartmentService bll = new DepartmentService();
            string actionId = "SysDept";
            switch (Action)
            {
                case "delete":
                    if (bll.Delete(key))
                    {
                        string actStr = "该用户对-单位编码：[" + key + "]进行了删除操作。";
                        BaseService.WriteLogEvent(actStr, actionId);
                        context.Response.Write(1); //删除成功
                        context.Response.End();
                    }
                    else
                    {
                        context.Response.Write(2); //删除失败
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