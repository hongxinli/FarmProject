using Common;
using IDal.Sys;
using OracleDal.Sys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
//using ASIMLib;

namespace Bll
{
    public static class BaseService
    {
        private readonly static IRoleDept dal_RoleDept = new RoleDeptRepository();
        private readonly static Sys.LogService dal_LogSer = new Sys.LogService();
        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="actStr">操作记录</param>
        /// <param name="moduleId">模块编号</param>
        public static void WriteLogEvent(string actStr, string moduleId)
        {
            CookieUser currentUser = RequestCookie.GetCookieUser();
            Model.Base_OperLog model = new Model.Base_OperLog();
            model.Id = CommonHelper.GetGuid;
            model.UserId = currentUser.UserId.ToString();
            model.UserName = currentUser.UserName.ToString();
            model.UserIp = RequestHelper.GetIP();
            model.OperationTime = DateTime.Now;
            model.Operation = actStr;
            model.DeptId = currentUser.DeptId.ToString();
            model.ModuleName = Common.ConfigHelper.GetNodeValue(moduleId);
            dal_LogSer.AddOperLog(model);
        }
        /// <summary>
        /// 获取权限数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> ReturnRightData<T>(IList<T> list)
        {
            string rolesid = RequestCookie.GetCookieUser().RoleId.ToString();
            IList<Model.Base_RoleDept> list_RoleDept = dal_RoleDept.List("RolesId='" + rolesid + "'");
            var newlist = (from e in list join o in list_RoleDept on e.GetType().GetProperty("DeptId").GetValue(e, null) equals o.DeptId select e).ToList();
            return newlist;
        }
        /// <summary>
        /// 获取权限数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable ReturnRightData(DataTable dt)
        {
            string rolesid = RequestCookie.GetCookieUser().RoleId.ToString();
            IList<Model.Base_RoleDept> list_RoleDept = dal_RoleDept.List("RolesId='" + rolesid + "'");
            string str = "";
            foreach (Model.Base_RoleDept model in list_RoleDept)
            {
                str = str + "'" + model.DeptId + "',";
            }
            str = str.Substring(0, str.Length - 1);
            DataView dv = new DataView();
            dv.Table = dt;
            dv.RowFilter = ("DEPTID in (" + str + ")");
            return dv.ToTable();
        }
        /// <summary>
        /// 过滤待审批数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable ReturnUserRightData(DataTable dt)
        {

            if (Convert.ToInt32(RequestCookie.GetCookieUser().IsAdmin) == 0)
            {
                DataView dv = new DataView();
                dv.Table = dt;
                dv.RowFilter = ("nextuserid='" + RequestCookie.GetCookieUser().UserId + "' or stepaction<>'待审批'");
                dt = dv.ToTable();
            }
            return dt;
        }
        /// <summary>
        /// 获取权限数据deptid '101101106','101101112'
        /// </summary>
        /// <returns></returns>
        public static string ReturnAuthority()
        {
            string rolesid = RequestCookie.GetCookieUser().RoleId.ToString();
            IList<Model.Base_RoleDept> list_RoleDept = dal_RoleDept.List("RolesId='" + rolesid + "'");
            string str = "";
            foreach (Model.Base_RoleDept model in list_RoleDept)
            {
                str = str + "'" + model.DeptId + "',";
            }
            str = str.Substring(0, str.Length - 1);
            return str;
        }

        public static DataTable ReturnParentNode(string ParentId, DataTable dt, DataTable newDT)
        {
            DataRow[] drs = dt.Select("DEPTID='" + ParentId + "'");
            if (drs.Length > 0 && newDT.Select("DEPTID='" + drs[0]["DEPTID"] + "'").Length == 0)
            {
                newDT.Rows.Add(drs[0].ItemArray);
                newDT = ReturnParentNode(drs[0]["PARENTID"].ToString(), dt, newDT);
            }
            return newDT;
        }
        public static IList<Model.Base_ModuleInfo> ReturnModuleTreeParentNode(string ParentId, IList<Model.Base_ModuleInfo> list, IList<Model.Base_ModuleInfo> newList)
        {
            Model.Base_ModuleInfo model = list.Where(t => t.ModuleId.Equals(ParentId)).ToList().Count > 0 ? list.Where(t => t.ModuleId.Equals(ParentId)).ToList()[0] : null;
            if (model != null && !newList.Contains(model, ModuleComparer.Default))
            {
                newList.Add(model);
                newList = ReturnModuleTreeParentNode(model.ParentId, list, newList);
            }
            return newList;
        }
        public static IList<Model.Base_ModuleInfo> ReturnModuleTreeChildrenNode(string ModuleId, IList<Model.Base_ModuleInfo> list, IList<Model.Base_ModuleInfo> newList)
        {
            IList<Model.Base_ModuleInfo> childrenList = list.Where(t => t.ParentId.Equals(ModuleId)).ToList();
            if (childrenList.Count > 0)
            {
                foreach (Model.Base_ModuleInfo model in childrenList)
                {
                    if (!newList.Contains(model, ModuleComparer.Default))
                    {
                        newList.Add(model);
                        newList = ReturnModuleTreeChildrenNode(model.ModuleId, list, newList);
                    }
                }
            }
            return newList;
        }
        public static IList<Model.Base_Department> ReturnParentNode(string ParentId, IList<Model.Base_Department> list, IList<Model.Base_Department> newList)
        {
            Model.Base_Department model = list.Where(t => t.DeptId.Equals(ParentId)).ToList().Count > 0 ? list.Where(t => t.DeptId.Equals(ParentId)).ToList()[0] : null;
            if (model != null && !newList.Contains(model, PopupComparer.Default))
            {
                newList.Add(model);
                newList = ReturnParentNode(model.ParentId, list, newList);
            }
            return newList;
        }

        /// <summary>
        /// 描    述：弹出模型对象列表比较器(根据ID比较)
        /// 作    者：Coderli
        /// 创建日期：2016-10-14
        /// </summary>
        public class PopupComparer : IEqualityComparer<Model.Base_Department>
        {
            public static PopupComparer Default = new PopupComparer();
            #region IEqualityComparer<PopupModel> 成员
            public bool Equals(Model.Base_Department x, Model.Base_Department y)
            {
                return x.DeptId.Equals(y.DeptId);
            }
            public int GetHashCode(Model.Base_Department obj)
            {
                return obj.GetHashCode();
            }
            #endregion
        }

        public class ModuleComparer : IEqualityComparer<Model.Base_ModuleInfo>
        {
            public static ModuleComparer Default = new ModuleComparer();
            #region IEqualityComparer<PopupModel> 成员
            public bool Equals(Model.Base_ModuleInfo x, Model.Base_ModuleInfo y)
            {
                return x.ModuleId.Equals(y.ModuleId);
            }
            public int GetHashCode(Model.Base_ModuleInfo obj)
            {
                return obj.GetHashCode();
            }
            #endregion
        }

        /// <summary>
        /// 判断在线用户是否超级管理员
        /// </summary>
        /// <returns></returns>
        public static bool IsAdmin()
        {
            string isAdmin = RequestCookie.GetCookieUser().IsAdmin.ToString();
            switch (isAdmin)
            {
                case "0":
                    return false;
                case "1":
                    return false;
                case "2":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 发送AM提醒
        /// </summary>
        /// <param name="msgBody"></param>
        /// <param name="AmUserid"></param>
        public static void SendMsg(string sender, int count, string AmUserid)
        {
            //ASIMLib.IM im = new ASIMLib.IM();
            //string AmServer = System.Configuration.ConfigurationManager.AppSettings["Amserver"];
            //string AmSender = System.Configuration.ConfigurationManager.AppSettings["AmSender"];
            //string AmSender_Passwd = System.Configuration.ConfigurationManager.AppSettings["AmSender_password"];
            //im.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["AmPort"]);
            //im.Init(AmServer, AmSender, AmSender_Passwd);

            //String strBody = "";
            //strBody = "您有" + count + "条新的审核任务。";
            //strBody = strBody + "    发送人：" + sender;
            //strBody = strBody + "    发送时间:" + DateTime.Now.ToString("yyyy-MM-dd 24hh:mm:ss");


            ////设置消息的相关内容
            //ASIMLib.Msg msg = new ASIMLib.Msg();
            //msg.ContentType = "Text/Text";
            //msg.Subject = "吐哈油田监（检）测综合管理信息系统审批提醒";
            //msg.Body = strBody;
            ////消息发送
            //im.SendMsgEx(msg, AmUserid);
        }
        public static void SendMsg(string sender, string strBody, string AmUserid)
        {
            //ASIMLib.IM im = new ASIMLib.IM();
            //string AmServer = System.Configuration.ConfigurationManager.AppSettings["Amserver"];
            //string AmSender = System.Configuration.ConfigurationManager.AppSettings["AmSender"];
            //string AmSender_Passwd = System.Configuration.ConfigurationManager.AppSettings["AmSender_password"];
            //im.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["AmPort"]);
            //im.Init(AmServer, AmSender, AmSender_Passwd);

            ////设置消息的相关内容
            //ASIMLib.Msg msg = new ASIMLib.Msg();
            //msg.ContentType = "Text/Text";
            //msg.Subject = "吐哈油田监（检）测综合管理信息系统审批提醒";
            //msg.Body = strBody;
            ////消息发送
            //im.SendMsgEx(msg, AmUserid);
        }
    }
}
