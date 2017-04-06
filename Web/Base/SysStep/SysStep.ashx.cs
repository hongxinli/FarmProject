using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bll.Sys;

namespace Web.Base.SysStep
{
    /// <summary>
    /// SysStep 的摘要说明
    /// </summary>
    public class SysStep : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Action = context.Request["action"].Trim();               //提交动作
            string key = context.Request["key"];//主键
            StepInfoService bll = new StepInfoService();
            switch (Action)
            {
                case "delete":
                    if (bll.Delete(key))
                    {
                        string moduleId = "SysStep";
                        #region 操作日志记录
                        string actStr = "该用户对-步骤编号：[" + key + "]进行了删除操作。";
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