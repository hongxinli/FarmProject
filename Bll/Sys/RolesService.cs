using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDal.Sys;
using OracleDal.Sys;
using Common;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Web.UI;

namespace Bll.Sys
{
    public class RolesService
    {
        IRoles dal = new RolesRepository();
        IUserRole dal_userRole = new UserRoleRepository();
        IUserInfo dal_user = new UserInfoRepository();
        IModuleInfo dal_module = new ModuleInfoRepository();
        IRoleRight dal_right = new RoleRightRepository();
        IDepartment dal_dept = new DepartmentRepository();
        IRoleDept dal_roleDept = new RoleDeptRepository();
        IRoleRight dal_RoleRight = new RoleRightRepository();
        IModuleDept dal_ModuleDept = new ModuleDeptRepsitory();
        public bool IsExistsUserRole(string userId)
        {
            return dal_userRole.Exists("userid", userId);
        }
        /// <summary>
        /// 获取角色信息列表
        /// </summary>
        /// <param name="DeptId"></param>
        /// <returns></returns>
        public string GetRoleList(string DeptId)
        {
            IList<Model.Base_Roles> list;
            StringBuilder str_table = new StringBuilder();
            if (!string.IsNullOrEmpty(DeptId))
            {
                list = dal.List(" DeptId in (" + DeptId + ")");
            }
            else
            {
                list = dal.List();
            }
            if (!BaseService.IsAdmin())
                list = BaseService.ReturnRightData<Model.Base_Roles>(list);
            int eRowIndex = 0;
            foreach (Model.Base_Roles model in list)
            {
                str_table.Append("<tr id='" + eRowIndex + "'>");
                str_table.Append("<td style='width: 180px;padding-left:20px;'><span class=\"folder\">" + model.RolesName + "</span></td>");
                str_table.Append("<td style='width: 60px;text-align:center;'>" + Get_Type(model.DeleteMark.ToString()) + "</td>");
                str_table.Append("<td style='width: 60px;text-align:center;'>" + model.SortCode + "</td>");
                str_table.Append("<td style='width: 120px;text-align:center'>" + Return_Type(model.AllowDelete.ToString()) + "</td>");
                str_table.Append("<td style='width: 120px;text-align:center'>" + Return_Type(model.AllowEdit.ToString()) + "</td>");
                str_table.Append("<td>" + model.Remarks + "</td>");
                str_table.Append("<td style='display:none'>" + model.RolesId + "</td>");
                str_table.Append("</tr>");
                eRowIndex++;
            }
            return str_table.ToString();
        }
        /// <summary>
        /// 角色树列表
        /// </summary>
        /// <returns></returns>
        public string GetRoleTreeTable()
        {
            IList<Model.Base_Roles> list = dal.List("DeleteMark=0 order by SortCode asc");
            IList<Model.Base_Roles> newList = list.Where(t => t.DeptId.Equals("-1")).ToList();
            int eRowIndex = 0;
            StringBuilder str_tableTree = new StringBuilder();
            foreach (Model.Base_Roles model in newList)
            {
                string trID = "node-" + eRowIndex.ToString();
                str_tableTree.Append("<tr id='" + trID + "'>");
                str_tableTree.Append("<td style='width: 180px;padding-left:20px;'><span class=\"folder\">" + model.RolesName + "</span></td>");
                str_tableTree.Append("<td style='width: 60px;text-align:center;'>" + Get_Type(model.DeleteMark.ToString()) + "</td>");
                str_tableTree.Append("<td style='width: 60px;text-align:center;'>" + model.SortCode + "</td>");
                str_tableTree.Append("<td style='width: 120px;text-align:center'>" + Return_Type(model.AllowDelete.ToString()) + "</td>");
                str_tableTree.Append("<td style='width: 120px;text-align:center'>" + Return_Type(model.AllowEdit.ToString()) + "</td>");
                str_tableTree.Append("<td>" + model.Remarks + "</td>");
                str_tableTree.Append("<td style='display:none'>" + model.RolesId + "</td>");
                str_tableTree.Append("</tr>");
                //创建子节点
                str_tableTree.Append(GetTableTreeNode(model.RolesId, list, trID));
                eRowIndex++;
            }
            return str_tableTree.ToString();
        }
        public string GetTableTreeNode(string parentId, IList<Model.Base_Roles> list, string parentTRID)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            IList<Model.Base_Roles> newList = list.Where(t => t.DeptId.Equals(parentId)).ToList();
            int i = 1;
            foreach (Model.Base_Roles model in newList)
            {
                string trID = parentTRID + "-" + i.ToString();
                sb_TreeNode.Append("<tr id='" + trID + "' class='child-of-" + parentTRID + "'>");
                sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + model.RolesName + "</span></td>");
                sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + Get_Type(model.DeleteMark.ToString()) + "</td>");
                sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + model.SortCode + "</td>");
                sb_TreeNode.Append("<td style='width: 120px;text-align:center'>" + Return_Type(model.AllowDelete.ToString()) + "</td>");
                sb_TreeNode.Append("<td style='width: 120px;text-align:center'>" + Return_Type(model.AllowEdit.ToString()) + "</td>");
                sb_TreeNode.Append("<td>" + model.Remarks + "</td>");
                sb_TreeNode.Append("<td style='display:none'>" + model.RolesId + "</td>");
                sb_TreeNode.Append("</tr>");
                //创建子节点
                sb_TreeNode.Append(GetTableTreeNode(model.RolesId, list, trID));
                i++;
            }
            return sb_TreeNode.ToString();
        }
        /// <summary>
        /// 角色状态
        /// </summary>
        /// <param name="Menu_Type">类型</param>
        /// <returns></returns>
        public string Get_Type(string Menu_Type)
        {
            if (Menu_Type == "0")
            {
                return "正常";
            }
            else if (Menu_Type == "1")
            {
                return "<span style='color:red'>停用</span>";
            }
            else
            {
                return "其他";
            }
        }
        /// <summary>
        /// 是否允许编辑、删除
        /// </summary>
        /// <param name="Menu_Type"></param>
        /// <returns></returns>
        public string Return_Type(string Menu_Type)
        {
            if (Menu_Type == "0")
            {
                return "允许";
            }
            else if (Menu_Type == "1")
            {
                return "<span style='color:red'>不允许</span>";
            }
            else
            {
                return "其他";
            }
        }

        public void InitParentId(HtmlSelect ParentId, string RolesId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT RolesId,
                            RolesName||' - '||CASE ParentId WHEN '-1' THEN '父节' WHEN '0' THEN '父节' WHEN '1' THEN '父节' ELSE  '子节' END AS RolesName
                            FROM Base_Roles WHERE DeleteMark = 0 ORDER BY RolesId, SortCode ASC");
            DataTable dt = dal.Query(strSql.ToString()).Tables[0];
            if (!string.IsNullOrEmpty(RolesId))
            {
                if (DataTableHelper.IsExistRows(dt))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["RolesId"].ToString() == RolesId)
                            dt.Rows.RemoveAt(i);
                    }
                }
            }
            ControlBindHelper.BindHtmlSelect(dt, ParentId, "RolesName", "RolesId", "角色信息 - 父节");
        }

        #region 初始化角色成员


        /// <summary>
        /// 所有成员
        /// </summary>
        /// <returns></returns>
        public string InitUserInfo()
        {
            StringBuilder str_allUserInfo = new StringBuilder();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT DeptId,DeptName,ParentId,'0' AS isUser FROM Base_Department ");
            strSql.Append(" where DeptId in (" + BaseService.ReturnAuthority() + ")");
            strSql.Append(" UNION ALL ");
            strSql.Append(" SELECT t.UserId ,t.UserId||'|'||t.UserName ,m.DeptId,'1' AS isUser FROM Base_UserInfo t left join Base_UserDept m on t.userid=m.userid  ");
            strSql.Append(" where m.DeptId in (" + BaseService.ReturnAuthority() + ")");
            DataTable AuthorityDT = dal.Query(strSql.ToString()).Tables[0];

            DataTable dt = dal_dept.Query("select DeptId, DeptName, ParentId,'0' AS isUser from base_department").Tables[0];

            DataTable newDT = new DataTable();
            newDT.Columns.Add("DEPTID");
            newDT.Columns.Add("DEPTNAME");
            newDT.Columns.Add("PARENTID");
            newDT.Columns.Add("ISUSER");
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
                DataTable GetNewData = DataTableHelper.GetNewDataTable(dt, "ParentId = '" + drv["DeptId"].ToString() + "'");
                if (DataTableHelper.IsExistRows(GetNewData))
                {
                    str_allUserInfo.Append("<li>");
                    str_allUserInfo.Append("<div>" + drv["DeptName"].ToString() + "</div>");
                    //创建子节点
                    str_allUserInfo.Append(GetTreeNode(drv["DeptId"].ToString(), drv["DeptName"].ToString(), newDT, "1"));
                    str_allUserInfo.Append("</li>");
                }
            }
            return str_allUserInfo.ToString();
        }
        int index_TreeNode = 0;
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTreeNode(string parentID, string parentName, DataTable dtNode, string status)
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
                    if (drv["isUser"].ToString() == "0")//判断是否是成员 1：是成员
                    {
                        if (DataTableHelper.IsExistRows(GetNewData))
                        {
                            sb_TreeNode.Append("<li>");
                            sb_TreeNode.Append("<div>" + drv["DeptName"] + "</div>");
                            //创建子节点
                            sb_TreeNode.Append(GetTreeNode(drv["DeptId"].ToString(), drv["DeptName"].ToString(), dtNode, "2"));
                            sb_TreeNode.Append("</li>");
                        }
                    }
                    else
                    {
                        if (status != "1")
                        {
                            sb_TreeNode.Append("<li>");
                            sb_TreeNode.Append("<div ondblclick=\"addUserInfo('" + drv["DeptName"] + "','" + drv["DeptId"] + "','" + parentName + "')\">");
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

        public string InitUserRole(string _rolesId)
        {
            StringBuilder str_seleteUserInfo = new StringBuilder();
            if (!string.IsNullOrEmpty(_rolesId))
            {
                string strSql = "select t.UserId, t.UserName,t1.DeptName from Base_Userinfo t left join Base_Userdept t3 on t.userid=t3.userid  left join Base_Department t1 on t3.deptid = t1.deptid left join Base_UserRole t2 on t.userid=t2.userid where t2.RolesId='" + _rolesId + "'";
                DataTable dt = dal.Query(strSql).Tables[0];
                if (DataTableHelper.IsExistRows(dt))
                {
                    foreach (DataRow drv in dt.Rows)
                    {
                        str_seleteUserInfo.Append("<tr ondblclick='$(this).remove()'><td>" + drv["UserId"] + "|" + drv["UserName"] + "</td><td>" + drv["DeptName"] + "</td><td  style='display:none'>" + drv["UserId"] + "|角色成员</td></tr>");
                    }
                }
            }
            return str_seleteUserInfo.ToString();
        }
        #endregion

        /// <summary>
        /// 表单提交：新增，修改
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ParentId"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public bool Submit_AddOrEdit(Page page, string roleId, out string _roleId)
        {
            Model.Base_Roles model = new Model.Base_Roles();
            string moduleId = "SysRole";
            model = ControlBindHelper.GetWebControls<Model.Base_Roles>(page, model);
            if (string.IsNullOrEmpty(roleId))
            {
                model.Creator = RequestSession.GetSessionUser().UserId.ToString();
                model.CreateDate = DateTime.Now;
                model.RolesId = CommonHelper.GetGuid;
                _roleId = model.RolesId;
                #region 操作日志记录
                string actStr = "该用户对-角色信息：[" + model.RolesName + "]进行了新增操作。";
                Bll.BaseService.WriteLogEvent(actStr, moduleId);
                #endregion
                return string.IsNullOrEmpty(dal.Add(model)) ? false : true;
            }
            else
            {
                #region 操作日志记录
                string actStr = "该用户对-角色信息：[" + model.RolesName + "]进行了修改操作。";
                Bll.BaseService.WriteLogEvent(actStr, moduleId);
                #endregion
                _roleId = roleId;
                return dal.Update(model, " RolesId='" + roleId + "'") > 0 ? true : false;
            }

        }
        /// <summary>
        /// 角色用户/模块权限/数据权限的新增、修改
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="rolesId"></param>
        /// <returns></returns>
        public bool add_ItemForm(string strs, string rolesId)
        {

            List<string> list = new List<string>();
            list.Add(" delete from Base_UserRole where RolesId='" + rolesId + "' ");
            list.Add(" delete from Base_RoleRight where RolesId='" + rolesId + "' ");
            list.Add(" delete from Base_RoleDept where RolesId='" + rolesId + "' ");
            if (!string.IsNullOrEmpty(strs))
            {
                string[] items = strs.Split(',');
                foreach (var item in items)
                {
                    string[] str_item = item.Split('|');
                    string key = str_item[0];
                    string type = str_item[1];
                    if (type == "角色成员")
                    {
                        list.Add("insert into Base_UserRole (Id,UserId,RolesId)values('" + CommonHelper.GetGuid + "','" + key + "','" + rolesId + "')");
                    }
                    else if (type == "模块权限")
                    {
                        list.Add("insert into Base_RoleRight (Id,ModuleId,RolesId)values('" + CommonHelper.GetGuid + "','" + key + "','" + rolesId + "')");
                    }
                    else if (type == "数据权限")
                    {
                        list.Add("insert into Base_RoleDept (Id,DeptId,RolesId)values('" + CommonHelper.GetGuid + "','" + key + "','" + rolesId + "')");
                    }
                }
            }
            return dal_userRole.ExecuteSqlTran(list) > 0 ? true : false;
        }
        /// <summary>
        /// 初始化角色信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="_rolesId"></param>
        public void InitData(Page page, string _rolesId)
        {
            Model.Base_Roles model = dal.Get("RolesId", _rolesId);
            ControlBindHelper.SetWebControls(page, model);
        }
        #region 导航菜单
        /// <summary>
        /// 菜单树列表
        /// </summary>
        public string GetMenuTreeTable(string _rolesId)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" deletemark=0 ");
            #region 过滤非超级管理员信息
            if (!BaseService.IsAdmin())
            {
                string moduleids = ConfigHelper.GetAppSettings("moduleId").ToString();
                string[] strs = moduleids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                string str = "'";
                for (int i = 0; i < strs.Length; i++)
                {
                    str = str + strs[i] + "','";
                }
                str = str.Substring(0, str.Length - 2);
                strWhere.Append(" and moduleid not in( " + str + ")");
            }
            #endregion
            strWhere.Append(" order by sortcode asc ");
            IList<Model.Base_ModuleInfo> list = dal_module.List(strWhere.ToString());
            IList<Model.Base_RoleRight> list_Rights = null;
            if (!string.IsNullOrEmpty(_rolesId))
                list_Rights = dal_right.List(" RolesId='" + _rolesId + "'");
            IList<Model.Base_ModuleInfo> list_Button = list.Where(t => t.ModuleType.Equals(3)).ToList();
            List<Model.Base_ModuleInfo> list_Menu = new List<Model.Base_ModuleInfo>();
            #region 过滤非超级管理员信息
            if (!BaseService.IsAdmin())
            {
                IList<Model.Base_RoleRight> rights = dal_right.List(" RolesId='" + RequestSession.GetSessionUser().RoleId.ToString() + "'");
                foreach (Model.Base_RoleRight model in rights)
                {
                    List<Model.Base_ModuleInfo> newList = list.Where(t => t.ModuleId.Equals(model.ModuleId) && t.ModuleType != 3).ToList();
                    if (newList.Count > 0)
                    {
                        list_Menu.Add(newList[0]);
                    }

                }
            }
            else
            {
                list_Menu = list.Where(t => t.ModuleType < 3).ToList();
            }
            #endregion

            list_Menu = list_Menu.OrderBy(t => t.SortCode).ToList();

            IList<Model.Base_ModuleInfo> rootList = list_Menu.Where(t => t.ParentId.Equals("-1")).ToList();
            int eRowIndex = 0;
            StringBuilder StrTree_Menu = new StringBuilder();
            foreach (Model.Base_ModuleInfo model in rootList)
            {
                string trID = "node-" + eRowIndex.ToString();
                StrTree_Menu.Append("<tr id='" + trID + "'>");
                StrTree_Menu.Append("<td style='width: 200px;padding-left:20px;'><span class=\"folder\">" + model.ModuleName + "</span></td>");
                if (!string.IsNullOrEmpty(model.ModuleImg))
                    StrTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + model.ModuleImg + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                else
                    StrTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                StrTree_Menu.Append("<td style=\"width: 23px; text-align: left;\"><input id='ckb" + trID + "' onclick=\"ckbTableValueObj(this.id);ckbTableValueChildren(this.id);\" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked(model.ModuleId, list_Rights) + "  value=\"" + model.ModuleId + "|模块权限" + "\" name=\"checkbox\" /></td>");
                StrTree_Menu.Append("<td>" + GetButton(model.ModuleId, list_Button, trID, list_Rights) + "</td>");
                StrTree_Menu.Append("</tr>");
                //创建子节点
                StrTree_Menu.Append(GetTableTreeNode(model.ModuleId, list_Menu, trID, list_Button, list_Rights));
                eRowIndex++;
            }
            return StrTree_Menu.ToString();
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="_ParentId">父节点主键</param>
        /// <param name="list_Menu"></param>
        /// <returns></returns>
        private string GetTableTreeNode(string _ParentId, IList<Model.Base_ModuleInfo> list_Menu, string parentTRID, IList<Model.Base_ModuleInfo> list_Button, IList<Model.Base_RoleRight> list_Rights)
        {
            StringBuilder sb_TreeNode = new StringBuilder();

            IList<Model.Base_ModuleInfo> newList = list_Menu.Where(t => t.ParentId.Equals(_ParentId)).ToList();
            int i = 1;
            foreach (Model.Base_ModuleInfo model in newList)
            {
                string trID = parentTRID + "-" + i.ToString();
                sb_TreeNode.Append("<tr id='" + trID + "' class='child-of-" + parentTRID + "'>");
                sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + model.ModuleName + "</span></td>");
                if (!string.IsNullOrEmpty(model.ModuleImg))
                    sb_TreeNode.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + model.ModuleImg + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                else
                    sb_TreeNode.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                sb_TreeNode.Append("<td style=\"width: 23px; text-align: left;\"><input id='ckb" + trID + "' onclick=\"ckbTableValueObj(this.id);ckbTableValueChildren(this.id);\" " + GetChecked(model.ModuleId, list_Rights) + " style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"" + model.ModuleId + "|模块权限" + "\" name=\"checkbox\" /></td>");
                sb_TreeNode.Append("<td>" + GetButton(model.ModuleId, list_Button, trID, list_Rights) + "</td>");
                sb_TreeNode.Append("</tr>");
                //创建子节点
                sb_TreeNode.Append(GetTableTreeNode(model.ModuleId, list_Menu, trID, list_Button, list_Rights));
                i++;
            }
            return sb_TreeNode.ToString();
        }
        /// <summary>
        /// 获取导航菜单所属按钮
        /// </summary>
        /// <param name="Menu_Type">类型</param>
        /// <returns></returns> 
        private string GetButton(string _ParentId, IList<Model.Base_ModuleInfo> list_Button, string parentTRID, IList<Model.Base_RoleRight> list_Rights)
        {
            StringBuilder ButtonHtml = new StringBuilder(); ;
            IList<Model.Base_ModuleInfo> newList = list_Button.Where(t => t.ParentId.Equals(_ParentId)).ToList();
            int i = 1;
            foreach (Model.Base_ModuleInfo model in newList)
            {
                string trID = parentTRID + "--" + i.ToString();
                ButtonHtml.Append("<lable><input id='ckb" + trID + "' " + GetChecked(model.ModuleId, list_Rights) + " style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"" + model.ModuleId + "|模块权限" + "\" name=\"checkbox\" />");
                ButtonHtml.Append("" + model.ModuleName + "</lable>&nbsp;&nbsp;&nbsp;&nbsp;");
                i++;
            }
            return ButtonHtml.ToString();
        }
        #endregion
        /// <summary>
        /// 数据权限
        /// </summary>
        /// <param name="_RolesId"></param>
        /// <returns></returns>
        public string InitDeptInfo(string _RolesId)
        {
            StringBuilder strOrgHtml = new StringBuilder();
            IList<Model.Base_Department> list_dept = dal_dept.List("DeleteMark=0"); ;
            IList<Model.Base_RoleDept> list_roleDept = null;
            if (!string.IsNullOrEmpty(_RolesId))
                list_roleDept = dal_roleDept.List(" RolesId='" + _RolesId + "'");
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
            int eRowIndex = 0;
            if (newList.Count > 0)
            {
                foreach (Model.Base_Department model in rootList)
                {
                    string trID = "node-" + eRowIndex.ToString();
                    strOrgHtml.Append("<li>");
                    strOrgHtml.Append("<div>");
                    strOrgHtml.Append(model.DeptName + "</div>");
                    //创建子节点
                    strOrgHtml.Append(GetTreeNodeDept(model.DeptId, trID, newList, list_roleDept));
                    strOrgHtml.Append("</li>");
                    eRowIndex++;
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
        private string GetTreeNodeDept(string parentID, string parentTRID, IList<Model.Base_Department> list_dept, IList<Model.Base_RoleDept> list_roleDept)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            IList<Model.Base_Department> newList = list_dept.Where(t => t.ParentId.Equals(parentID)).ToList();
            if (newList.Count > 0)
            {
                int i = 1;
                sb_TreeNode.Append("<ul>");
                foreach (Model.Base_Department model in newList)
                {

                    string trID = parentTRID + "-" + i.ToString();
                    sb_TreeNode.Append("<li>");
                    sb_TreeNode.Append("<div class='treeview-file'>");
                    if (list_dept.Where(t => t.ParentId.Equals(model.DeptId)).ToList().Count == 0)
                        sb_TreeNode.Append("<input id='ckdr" + trID + "' style='vertical-align: middle;margin-bottom:2px;'  onclick=\"ckbValueChildren(this.id);\"  type=\"checkbox\" " + GetChecked(model.DeptId, list_roleDept) + " value=\"" + model.DeptId + "|数据权限" + "\" name=\"checkbox\" />");
                    sb_TreeNode.Append(model.DeptName + "</div>");
                    //创建子节点
                    sb_TreeNode.Append(GetTreeNodeDept(model.DeptId, trID, list_dept, list_roleDept));
                    sb_TreeNode.Append("</li>");
                    i++;
                }
                sb_TreeNode.Append("</ul>");
            }
            return sb_TreeNode.ToString();
        }
        /// <summary>
        /// 验证权限是否存在
        /// </summary>
        /// <param name="_ModuleId">权限主键</param>
        /// <param name="list_Rights">加载所属角色权限</param>
        /// <returns></returns>
        private string GetChecked(string _ModuleId, IList<Model.Base_RoleRight> list_Rights)
        {
            if (list_Rights == null)
                return "";
            IList<Model.Base_RoleRight> list = list_Rights.Where(t => t.ModuleId.Equals(_ModuleId)).ToList();
            if (list.Count > 0)
            {
                return "checked=\"checked\"";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 验证权限是否存在
        /// </summary>
        /// <param name="pkName">对象主键</param>
        /// <param name="Obj_id">对象主键值</param>
        /// <param name="dt">数据源</param>
        /// <returns></returns>
        public string GetChecked(string _DeptId, IList<Model.Base_RoleDept> list_RoleDept)
        {
            if (list_RoleDept == null)
                return "";
            IList<Model.Base_RoleDept> list = list_RoleDept.Where(t => t.DeptId.Equals(_DeptId)).ToList();
            if (list.Count > 0)
            {
                return "checked=\"checked\"";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// URL权限验证,拒绝，不合法的请求
        /// </summary>
        /// <param name="_UserId"></param>
        /// <param name="_RequestPath"></param>
        /// <returns></returns>
        public bool GetPermission_URL(string _UserId, string _RequestPath)
        {
            string strSql = @"select distinct t.navigateurl from Base_ModuleInfo t
                              left join Base_RoleRight t1 on t.ModuleId=t1.ModuleId
                              left join Base_UserRole t2 on t1.RolesId=t2.RolesId
                              where t.ModuleType!=3 ";
            if (!BaseService.IsAdmin())
                strSql = strSql + "and t2.UserId='" + _UserId + "'";
            DataTable dt = dal.Query(strSql).Tables[0];
            DataView dv = new DataView(dt);
            dv.RowFilter = "NavigateUrl = '" + _RequestPath + "'";
            return dv.Count > 0 ? true : false;
        }
        #region 权限显示按钮

        public string GetButtonHtml(string _UserId)
        {
            string strSql = @"select distinct t.ModuleId,t.ParentId,t.ModuleName,t.ModuleTitle,t.ModuleImg,t.ModuleType,t.NavigateUrl,t.SortCode from Base_ModuleInfo t
                            left join Base_RoleRight t1 on t.ModuleId=t1.ModuleId
                            left join Base_UserRole t2 on t1.RolesId=t2.RolesId
                            where t.DeleteMark=0 ";
            if (!BaseService.IsAdmin())
                strSql = strSql + " and t2.UserId='" + _UserId + "'";
            strSql = strSql + "  order by t.moduletype,  t.sortcode";
            DataTable dt = dal.Query(strSql).Tables[0];
            string URL = RequestHelper.GetScriptName;
            dt = DataTableHelper.GetNewDataTable(dt, "ParentId='" + GetMenuByNavigateUrl(URL, dt) + "' AND ModuleType = 3");
            StringBuilder sb_Button = new StringBuilder();
            if (DataTableHelper.IsExistRows(dt))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb_Button.Append("<a title=\"" + dr["ModuleTitle"].ToString() + "\" onclick=\"" + dr["NavigateUrl"].ToString() + ";\" class=\"button green\">");
                    sb_Button.Append("<span class=\"icon-botton\" style=\"background: url('/Themes/images/16/" + dr["ModuleImg"].ToString() + "') no-repeat scroll 0px 4px;\"></span>");
                    sb_Button.Append(dr["ModuleName"].ToString());
                    sb_Button.Append("</a>");
                }
            }
            else
            {
                sb_Button.Append("<a class=\"button green\">");
                sb_Button.Append("无操作按钮");
                sb_Button.Append("</a>");
            }
            return sb_Button.ToString();
        }
        /// <summary>
        /// 根据菜单URL获取主键
        /// </summary>
        /// <param name="NavigateUrl">菜单路径</param>
        /// <returns>返回主键</returns>
        public string GetMenuByNavigateUrl(string NavigateUrl, DataTable dt_Menu)
        {
            DataTable dt = DataTableHelper.GetNewDataTable(dt_Menu, "NavigateUrl='" + NavigateUrl + "'");
            string _ModuleId = string.Empty;
            if (dt.Rows.Count > 0)
            {
                _ModuleId = dt.Rows[0]["ModuleId"].ToString();
            }
            else
            {
                _ModuleId = "";
            }

            return _ModuleId;
        }
        #endregion
        /// <summary>
        /// 根据ModuleId判断RoleRight表是否存在数据
        /// </summary>
        /// <param name="_ModuleId"></param>
        /// <returns></returns>
        public bool IsExitsRoleRight(string _ModuleId)
        {
            return dal_RoleRight.Exists("ModuleId", _ModuleId);
        }
        /// <summary>
        /// 是否允许删除
        /// </summary>
        /// <param name="_RolesId"></param>
        /// <returns></returns>
        public bool IsAllowDelete(string _RolesId)
        {
            return dal.Get("RolesId", _RolesId).AllowDelete == 0 ? true : false;
        }
        /// <summary>
        /// 是否允许编辑
        /// </summary>
        /// <param name="_RolesId"></param>
        /// <returns></returns>
        public int IsAllowEdit(string _RolesId)
        {
            return dal.Get("RolesId", _RolesId).AllowEdit;
        }

        public bool UpdateDeleteMark(string _RolesId)
        {
            List<string> list = new List<string>
            {
                "Delete from Base_RoleDept where RolesId='"+_RolesId+"'",
                "Delete from Base_UserRole where RolesId='"+_RolesId+"'",
                "Delete from Base_RoleRight where RolesId='"+_RolesId+"'",
                "Update Base_Roles set DeleteMark=1 where RolesId='"+_RolesId+"'",
            };
            return dal.Update<Model.Base_Roles>("DeleteMark", 1, "RolesId='" + _RolesId + "'") > 0 ? true : false;
        }

        public IList<Model.Base_RoleDept> GetListRoleDept(string roleId)
        {
            return dal_roleDept.List("RolesId='" + roleId + "'");
        }
    }
}
