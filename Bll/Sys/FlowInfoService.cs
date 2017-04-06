using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OracleDal.Sys;
using System.Data;
using IDal.Sys;
using Common;
using System.Web.UI;
using System.Web.SessionState;

namespace Bll.Sys
{
    public class FlowInfoService
    {
        IFlowInfo dal = new FlowInfoRepository();
        IRoleDept dal_roleDept = new RoleDeptRepository();
        public DataTable GetFlowInfoPage(string DeptId, int PageIndex, int PageSize, ref int count)
        {
            string strWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(DeptId))
            {

                strWhere = strWhere + " and DeptId in(" + DeptId + ")";
            }
            string strSql = "select * from (select n.deptid, m.deptname, t.* from s_flowinfo t left join s_flowDept n on t.flowid=n.flowid left join base_department m on n.deptid=m.deptid where n.deptid in (" + BaseService.ReturnAuthority() + "))";
            DataTable dt = dal.DataTableByPage(PageSize, PageIndex, strSql, strWhere, ref count, "");
            return dt;
        }


        #region 单位树
        IDepartment dal_dept = new DepartmentRepository();
        IFlowDept dal_FlowDept = new FlowDeptRepository();
        /// <summary>
        /// 初始化单位树
        /// </summary>
        /// <param name="_UserId"></param>
        /// <returns></returns>
        public string InitDeptInfo(string _FlowId)
        {
            StringBuilder strOrgHtml = new StringBuilder();
            IList<Model.Base_Department> list_dept = dal_dept.List("DeleteMark=0");
            IList<Model.S_FlowDept> list = new List<Model.S_FlowDept>();
            string _deptId = string.Empty;
            if (!string.IsNullOrEmpty(_FlowId))
                list = dal_FlowDept.List("FlowId='" + _FlowId + "'");
            if (list != null && list.Count > 0)
                _deptId = list[0].DeptId;
            IList<Model.Base_Department> newList = new List<Model.Base_Department>();
            if (!BaseService.IsAdmin())
            {
                IList<Model.Base_RoleDept> rights = dal_roleDept.List(" RolesId='" + RequestSession.GetSessionUser().RoleId + "'");
                foreach (Model.Base_RoleDept model in rights)
                {
                    newList = BaseService.ReturnParentNode(model.DeptId, list_dept, newList);
                }
            }
            else
            {
                newList = list_dept;
            }

            IList<Model.Base_Department> rootList = newList.Where(t => t.ParentId.Equals("-1")).ToList();
            if (newList.Count > 0)
            {
                foreach (Model.Base_Department model in rootList)
                {
                    strOrgHtml.Append("<li>");
                    strOrgHtml.Append("<div>");
                    strOrgHtml.Append(model.DeptName + "</div>");
                    //创建子节点
                    strOrgHtml.Append(GetTreeNodeDept(model.DeptId, newList, _deptId));
                    strOrgHtml.Append("</li>");
                }
            }
            else
            {
                strOrgHtml.Append("<li>");
                strOrgHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
                strOrgHtml.Append("</li>");
            }
            return strOrgHtml.ToString();
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="list_dept"></param>
        /// <param name="list_roleDept"></param>
        /// <returns></returns>
        private string GetTreeNodeDept(string parentID, IList<Model.Base_Department> list_dept, string _deptId)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            IList<Model.Base_Department> newList = list_dept.Where(t => t.ParentId.Equals(parentID)).ToList();
            if (newList.Count > 0)
            {
                sb_TreeNode.Append("<ul>");
                foreach (Model.Base_Department model in newList)
                {
                    sb_TreeNode.Append("<li>");
                    sb_TreeNode.Append("<div class='treeview-file'>");
                    if (list_dept.Where(t => t.ParentId.Equals(model.DeptId)).ToList().Count == 0)
                        sb_TreeNode.Append("<input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked(model.DeptId, _deptId) + " value=\"" + model.DeptId + "|所属单位" + "\" name=\"checkbox\" />");
                    sb_TreeNode.Append(model.DeptName + "</div>");
                    //创建子节点
                    sb_TreeNode.Append(GetTreeNodeDept(model.DeptId, list_dept, _deptId));
                    sb_TreeNode.Append("</li>");
                }
                sb_TreeNode.Append("</ul>");
            }
            return sb_TreeNode.ToString();
        }
        private string GetChecked(string _DeptId1, string _DeptId2)
        {
            if (_DeptId1.Equals(_DeptId2))
            {
                return "checked=\"checked\"";
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 初始化步骤树
        IFlowStep dal_FlowStep = new FlowStepRepository();
        public string InitStepInfo(string _FlowId)
        {
            IList<Model.S_FlowStep> list = new List<Model.S_FlowStep>();
            if (!string.IsNullOrEmpty(_FlowId))
                list = dal_FlowStep.List("FlowId='" + _FlowId + "'");
            StringBuilder str_allRolesInfo = new StringBuilder();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT DeptId, DeptName, ParentId, '0' AS isStep FROM Base_Department where DeleteMark='0'");
            if (!BaseService.IsAdmin())
                strSql.Append(" and DeptId in (" + BaseService.ReturnAuthority() + ")");
            strSql.Append("  UNION ALL ");
            strSql.Append("  SELECT m.StepId, m.StepName, n.DeptId, '1' AS isStep FROM S_StepInfo m left join S_StepDept n on m.StepId=n.StepId where DeleteMark = '0' ");
            if (!BaseService.IsAdmin())
                strSql.Append(" and DeptId in (" + BaseService.ReturnAuthority() + ")");
            DataTable AuthorityDT = dal.Query(strSql.ToString()).Tables[0];
            DataTable dt = dal_dept.Query("select DeptId, DeptName, ParentId,'0' AS isStep from base_department").Tables[0];
            DataTable newDT = new DataTable();
            newDT.Columns.Add("DEPTID");
            newDT.Columns.Add("DEPTNAME");
            newDT.Columns.Add("PARENTID");
            newDT.Columns.Add("ISSTEP");
            for (int i = 0; i < AuthorityDT.Rows.Count; i++)
            {
                if (newDT.Select("DEPTID='" + AuthorityDT.Rows[i]["DEPTID"].ToString() + "'").Length == 0)
                {
                    newDT.Rows.Add(AuthorityDT.Rows[i].ItemArray);
                    newDT = BaseService.ReturnParentNode(AuthorityDT.Rows[i]["PARENTID"].ToString(), dt, newDT);
                }
            }
            DataView dv = new DataView(newDT);
            dv.RowFilter = "ParentId = '-1'";
            foreach (DataRowView drv in dv)
            {
                DataTable GetNewData = DataTableHelper.GetNewDataTable(newDT, "ParentId = '" + drv["DeptId"].ToString() + "'");
                if (DataTableHelper.IsExistRows(GetNewData))
                {
                    str_allRolesInfo.Append("<li>");
                    str_allRolesInfo.Append("<div>" + drv["DeptName"].ToString() + "</div>");
                    //创建子节点
                    str_allRolesInfo.Append(GetTreeNode(drv["DeptId"].ToString(), drv["DeptName"].ToString(), newDT, "1", list));
                    str_allRolesInfo.Append("</li>");
                }
            }
            return str_allRolesInfo.ToString();
        }
        int index_TreeNode = 0;
        private string GetTreeNode(string parentID, string parentName, DataTable dtNode, string status, IList<Model.S_FlowStep> list)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            DataTable GetNewData = new DataTable();
            DataView dv = new DataView(dtNode);
            dv.RowFilter = "ParentId = '" + parentID + "'";
            if (dv.Count > 0)
            {
                if (index_TreeNode == 0)
                {
                    sb_TreeNode.Append("<ul>");
                }
                else
                {
                    sb_TreeNode.Append("<ul style='display: none'>");
                }
                foreach (DataRowView drv in dv)
                {
                    GetNewData = DataTableHelper.GetNewDataTable(dtNode, "ParentId = '" + drv["DeptId"].ToString() + "'");
                    if (drv["isStep"].ToString() == "0")//判断是否是步骤 1：是步骤
                    {
                        if (DataTableHelper.IsExistRows(GetNewData))
                        {
                            sb_TreeNode.Append("<li>");
                            sb_TreeNode.Append("<div>" + drv["DeptName"] + "</div>");
                            //创建子节点
                            sb_TreeNode.Append(GetTreeNode(drv["DeptId"].ToString(), drv["DeptName"].ToString(), dtNode, "2", list));
                            sb_TreeNode.Append("</li>");
                        }
                    }
                    else
                    {
                        if (status != "1")
                        {
                            sb_TreeNode.Append("<li>");
                            sb_TreeNode.Append("<div>");
                            sb_TreeNode.Append("<input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + SetStepChecked(drv["DeptId"].ToString(), list) + " value=\"" + drv["DeptId"].ToString() + "|所属步骤" + "\" name=\"checkbox\" />");
                            sb_TreeNode.Append("<img src=\"/Themes/Images/user_mature.png\" width=\"16\" height=\"16\" />" + drv["DeptName"].ToString());
                            sb_TreeNode.Append("&nbsp;&nbsp;<label style=\"font-size: 14px;\">(</label>");
                            sb_TreeNode.Append("<input type=\"text\" style=\"text-align:center;font-weight:600;color:red;height: 12px; width: 12px; border-bottom: 1px solid black; border-top: 0px; border-right: 0px; border-left: 0px;\" tags=\"" + drv["DeptId"].ToString() + "\" value=\"" + SetStepOrderNo(drv["DeptId"].ToString(), list) + "\" />");
                            sb_TreeNode.Append("<label style=\"font-size: 14px;\">)</label>" + "</div>");
                            sb_TreeNode.Append("</li>");
                        }
                    }
                }
                sb_TreeNode.Append("</ul>");
            }
            index_TreeNode++;
            return sb_TreeNode.ToString();
        }
        private string SetStepChecked(string _StepId, IList<Model.S_FlowStep> list)
        {
            list = list.Where(t => t.StepId.Equals(_StepId)).ToList();
            if (list.Count > 0)
            {
                return "checked=\"checked\"";
            }
            else
            {
                return "";
            }
        }
        private string SetStepOrderNo(string _StepId, IList<Model.S_FlowStep> list)
        {
            list = list.Where(t => t.StepId.Equals(_StepId)).ToList();
            if (list.Count > 0)
            {
                return list[0].SortCode.ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion

        /// <summary>
        /// 初始化页面数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="DeptId"></param>
        public void InitData(Page page, string FlowId, string hiddenValue)
        {
            Model.S_FlowInfo model = dal.Get("FlowId", FlowId);
            ControlBindHelper.SetWebControls(page, model);
            Model.S_FlowTable model_FlowTable = dal_flowTable.Get("FlowId", FlowId);
            ControlBindHelper.SetWebControls(page, model_FlowTable);
        }
        /// <summary>
        /// 表单提交：新增，修改
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ParentId"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public bool Submit_AddOrEdit(Page page, string FlowId, out string _FlowId)
        {
            string moduleId = "SysFlow";
            Model.S_FlowInfo model = new Model.S_FlowInfo();
            model = ControlBindHelper.GetWebControls<Model.S_FlowInfo>(page, model);
            Model.S_FlowTable model_FlowTable = new Model.S_FlowTable();
            model_FlowTable = ControlBindHelper.GetWebControls<Model.S_FlowTable>(page, model_FlowTable);
            if (string.IsNullOrEmpty(FlowId))
            {
                model.FlowId = CommonHelper.GetGuid;
                model.Creator = RequestSession.GetSessionUser().UserName.ToString();
                model.CreateDate = DateTime.Now;
                model.DeleteMark = 0;
                _FlowId = model.FlowId;

                model_FlowTable.Id = CommonHelper.GetGuid;
                model_FlowTable.FlowId = model.FlowId;
                #region 操作日志记录
                string actStr = "该用户对-流程Id：[" + _FlowId + "]进行了新增操作。";
                Bll.BaseService.WriteLogEvent(actStr, moduleId);
                #endregion
                return string.IsNullOrEmpty(dal.Add(model)) ? false : (string.IsNullOrEmpty(dal_flowTable.Add(model_FlowTable)) ? false : true);
            }
            else
            {
                _FlowId = FlowId;
                #region 操作日志记录
                string actStr = "该用户对-流程Id：[" + _FlowId + "]进行了修改操作。";
                Bll.BaseService.WriteLogEvent(actStr, moduleId);
                #endregion
                return dal.Update(model, " FlowId='" + FlowId + "'") > 0 ? (dal_flowTable.Update(model_FlowTable, " FlowId='" + FlowId + "'") > 0 ? true : false) : false;
            }
        }
        /// <summary>
        /// 所属单位、所属步骤新增、修改
        /// </summary>
        /// <param name="items"></param>
        /// <param name="_UserId"></param>
        /// <returns></returns>
        public bool add_ItemForm(string strs, string FlowId)
        {
            List<string> list = new List<string>();
            list.Add(" delete from S_FlowDept where FlowId='" + FlowId + "' ");
            list.Add(" delete from S_FlowStep where FlowId='" + FlowId + "' ");
            if (!string.IsNullOrEmpty(strs))
            {
                string[] arr = strs.Split('@');
                string[] items = arr[0].Split(',');
                foreach (var item in items)
                {
                    string[] str_item = item.Split('|');
                    string key = str_item[0];
                    string type = str_item[1];
                    if (type == "所属单位")
                    {
                        list.Add("insert into S_FlowDept (Id,DeptId,FlowId)values('" + CommonHelper.GetGuid + "','" + key + "','" + FlowId + "')");
                    }
                    else if (type == "所属步骤")
                    {
                        string[] items1 = arr[1].Substring(0, arr[1].Length - 1).Split(',');
                        foreach (var item1 in items1)
                        {
                            string[] str_item1 = item1.Split('|');
                            string key1 = str_item1[0];
                            string sortCode = str_item1[1];
                            if (key.Equals(key1))
                                list.Add("insert into S_FlowStep (Id,StepId,FlowId,SortCode)values('" + CommonHelper.GetGuid + "','" + key + "','" + FlowId + "'," + sortCode + ")");
                        }

                    }
                }
            }
            return dal.ExecuteSqlTran(list) > 0 ? true : false;
        }

        public bool Delete(string FlowId)
        {
            List<string> list = new List<string>
            {
                "Delete from S_FlowInfo where FlowId='"+FlowId+"'",
                "Delete from S_FlowDept where FlowId='"+FlowId+"'",
                "Delete from S_FlowStep where FlowId='"+FlowId+"'",
                "Delete from S_FlowTable where FlowId='"+FlowId+"'"
            };
            return dal.ExecuteSqlTran(list) > 0 ? true : false;
        }
        IStepInfo dal_stepInfo = new StepInfoRepository();
        /// <summary>
        /// 获取当前步骤名称
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
        public string CurrentStepName(string eventid)
        {
            string stepId = dal_flowData.Get("eventid", eventid).StepId;
            Model.S_StepInfo model = dal_stepInfo.Get("stepId", stepId);
            return model != null ? model.StepName : "";
        }
        /// <summary>
        /// 获取下一审核人
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
        public DataTable GetNextUser(string eventid)
        {
            string flowId = dal_flowData.Get("eventid", eventid).FlowId;
            string stepId = dal_flowData.Get("eventid", eventid).StepId;
            IList<Model.S_FlowStep> list = dal_FlowStep.List("flowId='" + flowId + "'");
            if (list.Where(t => t.StepId.Equals(stepId)).ToList().Count > 0)
            {
                int i = list.Where(t => t.StepId.Equals(stepId)).ToList()[0].SortCode;
                var newlist = list.Where(t => t.SortCode.Equals((i + 1))).ToList();
                if (newlist.Count > 0)
                {
                    stepId = newlist[0].StepId;
                    DataTable dt = GetUserByStepId(stepId);
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public string GetFirstUser(string eventid)
        {
            string strSql = "select t.eventid,t.stepuserid,t.stepusername,t.steptime from(select t.*,rownum from s_tranctproc t where t.eventid='" + eventid + "' order by t.steptime) t where rownum=1";
            DataTable dt = dal_tranctProc.Query(strSql).Tables[0];
            string result = dt.Rows[0]["stepusername"] + "," + dt.Rows[0]["stepuserid"] + "," + dt.Rows[0]["steptime"];
            return result;
        }
        /// <summary>
        /// 审批记录
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
        public string GetFlowRecord(string eventid)
        {
            string strSql = @"select t.eventid,m.stepname, t.stepusername,
                             (case  t.stepaction when 0 then '提交' when 1 then '未通过' when 2 then '通过' when 3 then '退回' when 4 then '否决' else '撤回' end)  stepaction,
                             t.stepnote,t.steptime,n.username 
                             from s_tranctproc t left join s_Stepinfo m on t.stepid=m.stepid
                             left join base_userinfo n on t.nextuserid=n.userid
                             where t.eventid='" + eventid + "'order by t.steptime ";
            DataTable dt = dal_tranctProc.Query(strSql).Tables[0];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr>");
                sb.Append("<td style=\"width: 80px; text-align: center;\">");
                sb.Append("第" + (i + 1) + "步");
                sb.Append("</td>");
                sb.Append("<td style=\"width: 100px; text-align: center;\">");
                sb.Append(dt.Rows[i]["stepname"]);
                sb.Append("</td>");
                sb.Append("<td style=\"width: 100px; text-align: center;\">");
                sb.Append(dt.Rows[i]["stepusername"]);
                sb.Append("</td>");
                sb.Append("<td style=\"width: 200px; text-align: center;\">");
                sb.Append(dt.Rows[i]["stepnote"]);
                sb.Append("</td>");
                if (dt.Rows[i]["stepaction"].ToString().Equals("未通过"))
                {
                    sb.Append("<td style=\"width: 80px; color:red; text-align: center;\">");
                }
                else
                {
                    sb.Append("<td style=\"width: 80px; text-align: center;\">");
                }
                sb.Append(dt.Rows[i]["stepaction"]);
                sb.Append("</td>");
                sb.Append("<td style=\"width: 150px; text-align: center;\">");
                sb.Append(dt.Rows[i]["steptime"]);
                sb.Append("</td>");
                sb.Append("<td style=\" text-align: center;\">");
                sb.Append(dt.Rows[i]["username"]);
                sb.Append("</td>");
                sb.Append("</tr>");
            }
            return sb.ToString();
        }
        IFlowData dal_flowData = new FlowDataRepository();
        IFlowTable dal_flowTable = new FlowTableRepository();
        IFlowStep dal_flowStep = new FlowStepRepository();
        /// <summary>
        /// 开启审批流程
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="eventid"></param>
        /// <returns></returns>
        public bool OpenFlow(string tabName, string eventid, string nextUserId)
        {
            string flowId = GetFlowTableModel(tabName).FlowId; //根据表名获取流程编号
            IList<Model.S_FlowStep> list = GetFlowStepList(flowId); //根据流程编号获取审批步骤
            string step1 = list.Where(t => t.SortCode.Equals(1)).ToList()[0].StepId;
            string step2 = list.Where(t => t.SortCode.Equals(2)).ToList()[0].StepId;
            string UserId = RequestSession.GetSessionUser().UserId.ToString();
            string UserName = RequestSession.GetSessionUser().UserName.ToString();
            string sql_FlowData = string.Empty;
            if (dal_flowData.Exists("EventId", eventid))
            {
                sql_FlowData = "update S_FlowData set stepid='" + step2 + "',StepAction=2,NextUserId='" + nextUserId + "' where eventid='" + eventid + "'";
            }
            else
            {
                sql_FlowData = "insert into S_FlowData(id,eventid,flowid,stepid,StepAction,NextUserId)values('" + CommonHelper.GetGuid + "','" + eventid + "','" + flowId + "','" + step2 + "',2,'" + nextUserId + "')";
            }
            List<string> sqlList = new List<string>
            {
                sql_FlowData,
                "insert into S_TranctProc(ProcId,EventId,StepId,StepUserId,StepUserName,StepNote,StepAction,StepTime,NextUserId)values('"+CommonHelper.GetGuid+"','"+eventid+"','"+step1+"','"+UserId+"','"+UserName+"','提交审核',0,to_date('"+DateTime.Now+"','yyyy-mm-dd hh24:mi:ss'),'"+nextUserId+"')"
            };
            return dal.ExecuteSqlTran(sqlList) > 0 ? true : false;
        }
        public bool OpenFlow(string tabName, string[] eventids, string nextUserId)
        {
            string flowId = GetFlowTableModel(tabName).FlowId; //根据表名获取流程编号
            IList<Model.S_FlowStep> list = GetFlowStepList(flowId); //根据流程编号获取审批步骤
            string step1 = list.Where(t => t.SortCode.Equals(1)).ToList()[0].StepId;
            string step2 = list.Where(t => t.SortCode.Equals(2)).ToList()[0].StepId;
            string UserId = RequestSession.GetSessionUser().UserId.ToString();
            string UserName = RequestSession.GetSessionUser().UserName.ToString();
            string sql_FlowData = string.Empty;
            List<string> sqlList = new List<string>();
            for (int i = 0; i < eventids.Length; i++)
            {
                string eventid = eventids[i];
                if (dal_flowData.Exists("EventId", eventid))
                {
                    sql_FlowData = "update S_FlowData set stepid='" + step2 + "',StepAction=2,NextUserId='" + nextUserId + "' where eventid='" + eventid + "'";
                }
                else
                {
                    sql_FlowData = "insert into S_FlowData(id,eventid,flowid,stepid,StepAction,NextUserId)values('" + CommonHelper.GetGuid + "','" + eventid + "','" + flowId + "','" + step2 + "',2,'" + nextUserId + "')";
                }
                string sql_TranctProc = "insert into S_TranctProc(ProcId,EventId,StepId,StepUserId,StepUserName,StepNote,StepAction,StepTime,NextUserId)values('" + CommonHelper.GetGuid + "','" + eventid + "','" + step1 + "','" + UserId + "','" + UserName + "','提交审核',0,to_date('" + DateTime.Now + "','yyyy-mm-dd hh24:mi:ss'),'" + nextUserId + "')";
                sqlList.Add(sql_FlowData);
                sqlList.Add(sql_TranctProc);
            }
            return dal.ExecuteSqlTran(sqlList) > 0 ? true : false;
        }
        /// <summary>
        ///  0 正常  1 请等待其他人审批  2  已审批，请勿重复审批  3  审核流程未开启  4  审批流程已经结束  5 该用户不具有审批权限
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
        public int IsAudit(string eventids)
        {
            string[] strs = eventids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int flag = 0;
            for (int i = 0; i < strs.Length; i++)
            {
                string eventid = strs[i];
                Model.S_FlowData model = dal_flowData.Get("EventId", eventid);
                string sqlStr = "SELECT * FROM (SELECT * FROM s_tranctproc T where  t.eventid='" + model.EventId + "' ORDER BY T.steptime DESC ) WHERE ROWNUM < 2";
                DataTable dt_TranctProc = dal_tranctProc.Query(sqlStr).Tables[0];
                if (model == null)
                {
                    flag = 3; //审核流程未开启。
                    break;
                }
                else if (string.IsNullOrEmpty(model.StepId))
                {
                    flag = 1; //1 请等待其他人审批 
                    break;
                }
                else if (model.StepId == "0")
                {
                    flag = 4;  //审批流程已经结束。
                    break;
                }
                else if (RequestSession.GetSessionUser().UserId.ToString() != model.NextUserId)
                {
                    flag = 5; //该用户不具有审批权限。
                    break;
                }
                else if (dt_TranctProc.Rows.Count > 0 && RequestSession.GetSessionUser().UserId.ToString() == dt_TranctProc.Rows[0]["STEPUSERID"].ToString())
                {
                    flag = 2; //已审批，请勿重复审批。
                    break;
                }
                else
                {
                    flag = 0;
                }
            }
            return flag;
        }
        public bool FlowAudit(string eventids, string StepAction, string StepNote, string NextUserId)
        {
            string[] strs = eventids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            List<string> sqlList = new List<string>();
            for (int i = 0; i < strs.Length; i++)
            {
                string eventid = strs[i];
                Model.S_FlowData model = dal_flowData.Get("EventId", eventid);
                IList<Model.S_FlowStep> list = GetFlowStepList(model.FlowId); //根据流程编号获取审批步骤
                string stepId1 = string.Empty;
                int stepAction = 0;
                if (StepAction == "2") //审核通过
                {
                    //判断是否还有下一步审核
                    int ii = list.Where(t => t.StepId.Equals(model.StepId)).ToList()[0].SortCode;
                    var list1 = list.Where(t => t.SortCode.Equals(ii + 1)).ToList();
                    if (list1.Count > 0)
                    {
                        stepId1 = list1[0].StepId;
                        stepAction = 2;
                    }
                    else
                    {
                        stepId1 = "0";
                        stepAction = 0;
                        NextUserId = "";
                    }
                }
                else if (StepAction == "1") //审核不通过,返回提交
                {
                    stepId1 = "";
                    NextUserId = "";
                    stepAction = 1;
                }
                string sql_FlowData = "update S_FlowData set stepid='" + stepId1 + "',StepAction=" + stepAction + ",NextUserId='" + NextUserId + "' where eventid='" + eventid + "'";
                string sql_TranctProc = "insert into S_TranctProc(ProcId,EventId,StepId,StepUserId,StepUserName,StepNote,StepAction,StepTime,NextUserId)values('" + CommonHelper.GetGuid + "','" + eventid + "','" + model.StepId + "','" + RequestSession.GetSessionUser().UserId.ToString() + "','" + RequestSession.GetSessionUser().UserName.ToString() + "','" + StepNote + "'," + StepAction + ",to_date('" + DateTime.Now + "','yyyy-mm-dd hh24:mi:ss'),'" + NextUserId + "')";
                sqlList.Add(sql_FlowData);
                sqlList.Add(sql_TranctProc);
            }
            return dal.ExecuteSqlTran(sqlList) > 0 ? true : false;
        }
        /// <summary>
        /// 判断是否已经开启流程
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
        public bool IsExistsFlowData(string eventid)
        {
            Model.S_FlowData model_flowData = dal_flowData.Get("eventid", eventid);
            if (model_flowData == null)
                return true;
            if (string.IsNullOrEmpty(model_flowData.StepId))
                return true;
            Model.S_FlowStep model_flowStep = dal_FlowStep.Get("FlowId='" + model_flowData.FlowId + "' and SortCode=1");
            return model_flowData.StepId.Equals(model_flowStep.StepId) ? true : false;

        }
        INextUser dal_nextUser = new NextUserRepository();
        /// <summary>
        /// 下一步处理人
        /// </summary>
        /// <param name="model"></param>
        public void InsertNextUser(Model.S_NextUser model)
        {
            dal_nextUser.Add(model);
        }
        /// <summary>
        /// 初始化第一步审核人员信息
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public DataTable InitUserInfoList(string tabName)
        {
            string flowId = GetFlowTableModel(tabName).FlowId; //根据表名获取流程编号
            IList<Model.S_FlowStep> list = GetFlowStepList(flowId); //根据流程编号获取审批步骤
            string stepId = list.Where(t => t.SortCode.Equals(2)).ToList()[0].StepId;
            DataTable dt = GetUserByStepId(stepId);
            return dt;
            /* StringBuilder sb = new StringBuilder();
             sb.Append("<table>");
             sb.Append("<tr>");
             sb.Append("<td style=\"height: 30px; line-height: 30px;\" >流转节点：</td>");
             sb.Append("<td>");
             sb.Append("<select id=\"sel\" runat=\"server\" style=\"border: 1px solid #A8A8A8; height: 22px; line-height: 20px; width: 100px;\">");
             sb.Append("<option selected=\"selected\" value=" + dt.Rows[0]["USERID"].ToString() + ">" + dt.Rows[0]["USERNAME"].ToString() + "</option> ");
             for (int i = 1; i < dt.Rows.Count; i++)
             {
                 sb.Append("<option value=" + dt.Rows[i]["USERID"].ToString() + ">" + dt.Rows[i]["USERNAME"].ToString() + "</option> ");
             }
             sb.Append("</select>");
             sb.Append("</td>");
             sb.Append("</tr>");
             sb.Append("<tr><td style=\"height: 30px; line-height: 30px;\">是否AM提醒：</td><td><input type=\"checkbox\" id=\"AM\" runat=\"server\" /></td></tr>");
             sb.Append("</table>");
             return sb.ToString();*/
        }
        IStepRole dal_stepRole = new StepRoleRepository();
        IUserRole dal_userRole = new UserRoleRepository();
        IUserInfo dal_userInfo = new UserInfoRepository();
        /// <summary>
        /// 根据步骤编号获取审核人员
        /// </summary>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public DataTable GetUserByStepId(string stepId)
        {
            IList<Model.S_StepRole> list = dal_stepRole.List("StepId='" + stepId + "'");
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select t.userid,t.username from base_userinfo t left join base_userrole m on t.userid=m.userid ");
            string strValue = " where t.userid!='" + RequestSession.GetSessionUser().UserId.ToString() + "' and m.rolesid in ( ";
            foreach (Model.S_StepRole model in list)
            {
                strValue = strValue + "'" + model.RoleId + "',";
            }
            strValue = strValue.Substring(0, strValue.Length - 1) + ")";
            strSql.Append(strValue);
            DataSet ds = dal_userInfo.Query(strSql.ToString());
            return ds.Tables[0];
        }
        ITranctProc dal_tranctProc = new TranctProcRepository();
        /// <summary>
        /// 审批流程记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertTranctProc(Model.S_TranctProc model)
        {
            return string.IsNullOrEmpty(dal_tranctProc.Add(model)) ? false : true;
        }
        /// <summary>
        /// 根据表名获取S_FlowTable信息
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public Model.S_FlowTable GetFlowTableModel(string tabName)
        {
            return dal_flowTable.Get("tableId", tabName);
        }
        /// <summary>
        /// 根据流程编号获取步骤信息
        /// </summary>
        /// <param name="_FlowId"></param>
        /// <returns></returns>
        public IList<Model.S_FlowStep> GetFlowStepList(string _FlowId)
        {
            return dal_flowStep.List(" FlowId='" + _FlowId + "'");
        }
        /// <summary>
        /// 判断数据是否审核通过
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsAudited(string key)
        {
            Model.S_FlowData model = dal_flowData.Get("EventId", key);
            if (model == null)
            {
                return false;
            }
            else if (model.StepId == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
