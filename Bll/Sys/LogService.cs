using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OracleDal.Sys;
using Model;
using Common;
using System.Data;
using IDal.Sys;

namespace Bll.Sys
{
    public class LogService
    {
        LoginLogRepository dal = new LoginLogRepository();
        Base_LoginLog model = new Base_LoginLog();
        UserDeptRepository dal_UserDept = new UserDeptRepository();
        Base_OperLog model_operlog = new Base_OperLog();
        IOperLog dal_OperLog = new OperLogRepository();
        IDepartment dalBaseDepartment = new DepartmentRepository();
        public void SysLoginLog(SessionUser _User, bool IsOnLine)
        {
            Base_UserDept model_UserDept = dal_UserDept.Get("UserId", _User.UserId);
            model.Id = _User.Id.ToString();
            model.UserId = _User.UserId.ToString();
            model.UserName = _User.UserName.ToString();
            model.IpAddress = RequestHelper.GetIP();
            model.Deptid = model_UserDept.DeptId;
            model.IsOnline = IsOnLine ? 1 : 0;
            if (IsOnLine)
            {
                model.LoginTime = DateTime.Now;
                dal.Add(model);
            }
            else
            {
                model.ExitTime = DateTime.Now;
                dal.Update(model, "Id='" + model.Id + "'");
            }
        }

        public DataTable GetSysLoginLogPage(string _UserId, string _StartTime, string _EndTime, int _PageIndex, int _PageSize, ref int count)
        {
            StringBuilder SqlStr = new StringBuilder();
            SqlStr.Append("select t.id,t.userid,t.username,t.ipaddress,t.logintime,t.deptid,m.deptname from base_loginlog t left join base_department m on t.deptid=m.deptid");
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append("1=1 ");
            if (!string.IsNullOrEmpty(_StartTime))
            {
                SqlWhere.Append(" and LoginTime >= to_date('" + _StartTime + "','yyyy-mm-dd hh24:mi:ss')");
            }
            if (!string.IsNullOrEmpty(_EndTime))
            {
                SqlWhere.Append(" and LoginTime <= to_date('" + _EndTime + "','yyyy-mm-dd hh24:mi:ss')");
            }
            if (!string.IsNullOrEmpty(_UserId))
            {
                SqlWhere.Append(" and UserId='" + _UserId + "'");
            }
            return dal.DataTableByPage(_PageSize, _PageIndex, SqlStr.ToString(), SqlWhere.ToString(), ref count, " logintime desc");
        }

        public void AddOperLog(Model.Base_OperLog model)
        {
            dal_OperLog.Add(model);
        }
        public DataTable GetOperLogPage(string _UserId, string _StartTime, string _EndTime, int _PageIndex, int _PageSize, ref int count)
        {
            StringBuilder SqlStr = new StringBuilder();
            SqlStr.Append(@"select t.id,t.UserId,t.UserName,t.UserIp,t.OperationTime,t.Operation,t.DeptId,m.deptname,t.modulename from Base_OperLog t
                            left join base_department m on t.deptid=m.deptid");
            StringBuilder SqlWhere = new StringBuilder();
            SqlWhere.Append("1=1 ");
            if (!string.IsNullOrEmpty(_StartTime))
            {
                SqlWhere.Append(" and OperationTime >= to_date('" + _StartTime + "','yyyy-mm-dd hh24:mi:ss')");
            }
            if (!string.IsNullOrEmpty(_EndTime))
            {
                SqlWhere.Append(" and OperationTime <= to_date('" + _EndTime + "','yyyy-mm-dd hh24:mi:ss')");
            }
            if (!string.IsNullOrEmpty(_UserId))
            {
                SqlWhere.Append(" and UserId='" + _UserId + "'");
            }
            return dal.DataTableByPage(_PageSize, _PageIndex, SqlStr.ToString(), SqlWhere.ToString(), ref count, "  OperationTime desc");
        }
        public string BindLogInfo()
        {
            int count = 0;
            StringBuilder Login_InfoHtml = new StringBuilder();
            string _UserId = RequestSession.GetSessionUser().UserId.ToString();
            string strSql = "select * from(select rownum num, t.* from (Select * from Base_Loginlog where UserId='" + _UserId + "'  order by logintime desc) t) where num<=2";
            DataTable dt = dal.Query(strSql).Tables[0];
            //d1是本月的第一天，d2本月的最后一天，
            DateTime now = DateTime.Now;
            DateTime d1 = new DateTime(now.Year, now.Month, 1);
            DateTime d2 = d1.AddMonths(1).AddDays(-1);
            count = dal.Count("logintime>=to_date('" + d1 + "','yyyy-mm-dd hh24:mi:ss') and logintime<=to_date('" + d2 + "','yyyy-mm-dd hh24:mi:ss') and  UserId='" + _UserId + "'");
            Login_InfoHtml.Append("本月登录总数：" + count + " 次 <br />");
            Login_InfoHtml.Append("本次登录IP：" + dt.Rows[0]["IpAddress"].ToString() + "<br />");
            Login_InfoHtml.Append("本次登录时间：" + dt.Rows[0]["LoginTime"].ToString() + "<br />");
            if (dt.Rows.Count != 1)
            {
                Login_InfoHtml.Append("上次登录IP：" + dt.Rows[1]["IpAddress"].ToString() + "<br />");
                Login_InfoHtml.Append("上次登录时间：" + dt.Rows[1]["LoginTime"].ToString() + "<br />");
            }
            else
            {
                Login_InfoHtml.Append("上次登录IP：127.0.0.1 <br />");
                Login_InfoHtml.Append("上次登录时间：1900-01-01 00:00:00<br />");
            }
            return Login_InfoHtml.ToString();
        }
    }
}
