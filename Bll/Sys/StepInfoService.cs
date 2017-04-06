using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OracleDal.Sys;
using IDal.Sys;
using System.Data;
using Common;
using System.Web.UI;

namespace Bll.Sys
{
    public class StepInfoService
    {
        IStepInfo dal = new StepInfoRepository();
        IRoleDept dal_roleDept = new RoleDeptRepository();
        /// <summary>
        /// 获取步骤信息
        /// </summary>
        /// <param name="DeptId"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable GetStepInfoPage(string DeptId, int PageIndex, int PageSize, ref int count)
        {
            string strWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(DeptId))
            {

                strWhere = strWhere + " and DeptId in(" + DeptId + ")";
            }
            string strSql = "select * from ( select n.deptid, m.deptname, t.* from S_StepInfo t left join S_StepDept n on t.stepid=n.stepid left join base_department m on n.deptid=m.deptid where n.deptid in (" + BaseService.ReturnAuthority() + "))";
            DataTable dt = dal.DataTableByPage(PageSize, PageIndex, strSql, strWhere, ref count, "");
            return dt;
        }

        #region 单位树
        IDepartment dal_dept = new DepartmentRepository();
        IStepDept dal_StepDept = new StepDeptRepository();
        /// <summary>
        /// 初始化单位树
        /// </summary>
        /// <param name="_UserId"></param>
        /// <returns></returns>
        public string InitDeptInfo(string _StepId)
        {
            StringBuilder strOrgHtml = new StringBuilder();
            IList<Model.Base_Department> list_dept = dal_dept.List("DeleteMark=0");
            IList<Model.S_StepDept> list = new List<Model.S_StepDept>();
            string _deptId = string.Empty;
            if (!string.IsNullOrEmpty(_StepId))
                list = dal_StepDept.List("StepId='" + _StepId + "'");
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
            if (list_dept.Count > 0)
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

        #region 初始化角色树
        IStepRole dal_StepRole = new StepRoleRepository();
        public string InitRoles(string _StepId)
        {
            IList<Model.S_StepRole> list = dal_StepRole.List("StepId='" + _StepId + "'");
            // string _RolesId = string.Empty;
            //if (!string.IsNullOrEmpty(_StepId))
            //    list = ;
            //if (list != null && list.Count > 0)
            //    _RolesId = list[0].RoleId;
            StringBuilder str_allRolesInfo = new StringBuilder();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT DeptId, DeptName, ParentId, '0' AS isRoles FROM Base_Department where DeleteMark='0'");
            if (!BaseService.IsAdmin())
                strSql.Append(" and DeptId in (" + BaseService.ReturnAuthority() + ")");
            strSql.Append("  UNION ALL ");
            strSql.Append("SELECT RolesId,RolesName, DeptId, '1' AS isRoles FROM Base_Roles where DeleteMark='0'");
            if (!BaseService.IsAdmin())
                strSql.Append(" and DeptId in (" + BaseService.ReturnAuthority() + ")");

            DataTable AuthorityDT = dal.Query(strSql.ToString()).Tables[0];
            DataTable dt = dal_dept.Query("select DeptId, DeptName, ParentId,'0' AS isRoles from base_department").Tables[0];
            DataTable newDT = new DataTable();
            newDT.Columns.Add("DEPTID");
            newDT.Columns.Add("DEPTNAME");
            newDT.Columns.Add("PARENTID");
            newDT.Columns.Add("ISROLES");
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
        private string GetTreeNode(string parentID, string parentName, DataTable dtNode, string status, IList<Model.S_StepRole> list)
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
                    if (drv["isRoles"].ToString() == "0")//判断是否是角色 1：是角色
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
                            sb_TreeNode.Append("<input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetRoleTreeChecked(drv["DeptId"].ToString(), list) + " value=\"" + drv["DeptId"].ToString() + "|所属角色" + "\" name=\"checkbox\" />");
                            sb_TreeNode.Append("<img src=\"/Themes/Images/user_mature.png\" width=\"16\" height=\"16\" />" + drv["DeptName"].ToString() + "</div>");
                            sb_TreeNode.Append("</li>");
                        }
                    }
                }
                sb_TreeNode.Append("</ul>");
            }
            index_TreeNode++;
            return sb_TreeNode.ToString();
        }
        private string GetRoleTreeChecked(string _DeptId1, IList<Model.S_StepRole> list)
        {
            if (list.Where(t => t.RoleId.Equals(_DeptId1)).Count() > 0)
            {
                return "checked=\"checked\"";
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
        public void InitData(Page page, string StepId, string hiddenValue)
        {
            Model.S_StepInfo model = dal.Get("StepId", StepId);
            ControlBindHelper.SetWebControls(page, model);
        }

        /// <summary>
        /// 表单提交：新增，修改
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ParentId"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public bool Submit_AddOrEdit(Page page, string StepId, out string _StepId)
        {
            string moduleId = "SysStep";
            Model.S_StepInfo model = new Model.S_StepInfo();
            model = ControlBindHelper.GetWebControls<Model.S_StepInfo>(page, model);
            if (string.IsNullOrEmpty(StepId))
            {
                model.StepId = CommonHelper.GetGuid;
                model.Creator = RequestSession.GetSessionUser().UserName.ToString();
                model.CreateDate = DateTime.Now;
                _StepId = model.StepId;
                #region 操作日志记录
                string actStr = "该用户对-步骤名称：[" + model.StepName + "]进行了新增操作。";
                Bll.BaseService.WriteLogEvent(actStr, moduleId);
                #endregion
                return string.IsNullOrEmpty(dal.Add(model)) ? false : true;
            }
            else
            {
                #region 操作日志记录
                string actStr = "该用户对-步骤名称：[" + model.StepName + "]进行了修改操作。";
                Bll.BaseService.WriteLogEvent(actStr, moduleId);
                #endregion
                _StepId = StepId;
                return dal.Update(model, " StepId='" + StepId + "'") > 0 ? true : false;
            }
        }

        /// <summary>
        /// 所属单位、所属角色新增、修改
        /// </summary>
        /// <param name="items"></param>
        /// <param name="_UserId"></param>
        /// <returns></returns>
        public bool add_ItemForm(string strs, string StepId)
        {
            List<string> list = new List<string>();
            list.Add(" delete from S_StepDept where StepId='" + StepId + "' ");
            list.Add(" delete from S_StepRole where StepId='" + StepId + "' ");
            if (!string.IsNullOrEmpty(strs))
            {
                string[] items = strs.Split(',');
                foreach (var item in items)
                {
                    string[] str_item = item.Split('|');
                    string key = str_item[0];
                    string type = str_item[1];
                    if (type == "所属单位")
                    {
                        list.Add("insert into S_StepDept (Id,DeptId,StepId)values('" + CommonHelper.GetGuid + "','" + key + "','" + StepId + "')");
                    }
                    else if (type == "所属角色")
                    {
                        list.Add("insert into S_StepRole (Id,RoleId,StepId)values('" + CommonHelper.GetGuid + "','" + key + "','" + StepId + "')");
                    }
                }
            }
            return dal.ExecuteSqlTran(list) > 0 ? true : false;
        }

        public bool Delete(string StepId)
        {
            List<string> list = new List<string>
            {
                "Delete from S_StepInfo where stepid='"+StepId+"'",
                "Delete from S_StepDept where stepid='"+StepId+"'",
                "Delete from S_StepRole where stepid='"+StepId+"'",
                "Delete from s_flowstep where stepid='"+StepId+"'"
            };
            return dal.ExecuteSqlTran(list) > 0 ? true : false;
        }
    }
}
