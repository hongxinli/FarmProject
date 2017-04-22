using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDal.Sys;
using OracleDal.Sys;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Common;
using System.Data;

namespace Bll.Sys
{
    public class UserInfoService
    {
        IUserInfo dal = new UserInfoRepository();
        IDepartment dal_dept = new DepartmentRepository();
        IUserRole dal_UserRole = new UserRoleRepository();
        IUserDept dal_UserDept = new UserDeptRepository();
        /// <summary>
        /// 获取用户分页数据
        /// </summary>
        /// <param name="searchWhere"></param>
        /// <param name="searchValue"></param>
        /// <param name="deptid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable GetUserInfoPage(string searchWhere, string searchValue, string deptid, int pageIndex, int pageSize, ref int count)
        {
            string strWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(searchValue))
            {
                strWhere = strWhere + " and " + searchWhere + "='" + searchValue + "'";
            }
            if (!string.IsNullOrEmpty(deptid))
            {

                strWhere = strWhere + " and DeptId in(" + deptid + ")";
            }
            //过滤数据权限
            string strSql = "select * from ( select t.*,m.DeptId from Base_UserInfo t left join Base_userDept m on t.UserId=m.UserId  ";
            if (!BaseService.IsAdmin())
                strSql = strSql + "where m.DeptId in (" + Bll.BaseService.ReturnAuthority() + ") ";
            strSql = strSql + ")";
            DataTable dt = dal.DataTableByPage(pageSize, pageIndex, strSql, strWhere, ref count, "");
            return dt;
        }

        /// <summary>
        /// 表单提交：新增，修改
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ParentId"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public bool Submit_AddOrEdit(Page page, string UserId)
        {
            Model.Base_UserInfo model = new Model.Base_UserInfo();
            model = ControlBindHelper.GetWebControls<Model.Base_UserInfo>(page, model);
            string moduleId = "SysUser";
            if (model.UserPwd != "*************")
            {
                model.UserPwd = Md5Helper.MD5(model.UserPwd, 32);
            }
            else
            {
                model.UserPwd = Md5Helper.MD5("0000", 32);
            }
            if (string.IsNullOrEmpty(UserId))
            {
                model.Creator = RequestSession.GetSessionUser().UserId.ToString();
                model.CreateDate = DateTime.Now;
                #region 操作日志记录
                string actStr = "该用户对-用户编号：[" + model.UserId + "]进行了新增操作。";
                Bll.BaseService.WriteLogEvent(actStr, moduleId);
                #endregion
                return string.IsNullOrEmpty(dal.Add(model)) ? false : true;
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" update Base_UserInfo set UserId='" + model.UserId + "', UserName='" + model.UserName + "',Phone='" + model.Phone + "',Email='" + model.Email + "',IsAdmin=" + model.IsAdmin + ", UserPwd='" + model.UserPwd + "',Remarks='" + model.Remarks + "',Am='" + model.Am + "',Theme='" + model.Theme + "'");//赵志鹏  2016年9月21日09:21:40  增加‘备注’项内容修改
                strSql.Append(" where UserId='" + UserId + "'");
                #region 操作日志记录
                string actStr = "该用户对-用户编号：[" + model.UserId + "]进行了修改操作。";
                Bll.BaseService.WriteLogEvent(actStr, moduleId);
                #endregion

                if (dal.ExecuteNonQuery(strSql.ToString()) > 0)
                {
                    List<string> list = new List<string>();
                    string sql1 = "update s_flowdata set nextuserid='" + model.UserId + "' where nextuserid='" + UserId +
                                  "'";
                    list.Add(sql1);
                    string sql2 = "update s_tranctproc set nextuserid='" + model.UserId + "' where nextuserid='" +
                                  UserId + "'";
                    list.Add(sql2);
                    string sql3 = " update s_tranctproc set stepuserid='" + model.UserId + "' where stepuserid='" +
                                  UserId + "'";
                    list.Add(sql3);
                    return dal.ExecuteSqlTran(list) > 0 ? true : false;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 初始化页面数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="DeptId"></param>
        public void InitData(Page page, string UserId, string hiddenValue)
        {
            Model.Base_UserInfo model = dal.Get("UserId", UserId);
            ControlBindHelper.SetWebControls(page, model);
        }
        #region 单位树
        /// <summary>
        /// 初始化单位树
        /// </summary>
        /// <param name="_UserId"></param>
        /// <returns></returns>
        public string InitDeptInfo(string _UserId)
        {
            StringBuilder strOrgHtml = new StringBuilder();
            IList<Model.Base_Department> list = dal_dept.List();
            string _deptId = string.Empty;
            if (!string.IsNullOrEmpty(_UserId))
                _deptId = dal_UserDept.List("UserId='" + _UserId + "'")[0].DeptId;
            IList<Model.Base_Department> AuthorityList = null;
            if (BaseService.IsAdmin())
            {
                AuthorityList = list;
            }
            else
            {
                AuthorityList = BaseService.ReturnRightData<Model.Base_Department>(list); ;
            }
            IList<Model.Base_Department> newList = new List<Model.Base_Department>();
            foreach (Model.Base_Department model in AuthorityList)
            {
                if (!newList.Contains(model, Bll.BaseService.PopupComparer.Default))
                    newList.Add(model);
                newList = BaseService.ReturnParentNode(model.ParentId, list, newList);
            }
            IList<Model.Base_Department> rootList = newList.Where(t => t.ParentId.Equals("-1")).ToList();
            if (newList.Count > 0)
            {
                foreach (Model.Base_Department model in rootList)
                {
                    strOrgHtml.Append("<li>");
                    strOrgHtml.Append("<div>");
                    // strOrgHtml.Append("<input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked(model.DeptId, _deptId) + " value=\"" + model.DeptId + "|所属单位" + "\" name=\"checkbox\" />");
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

        public string InitRoles(string _UserId)
        {
            IList<Model.Base_UserRole> list = new List<Model.Base_UserRole>();
            string _RolesId = string.Empty;
            if (!string.IsNullOrEmpty(_UserId))
                _RolesId = dal_UserRole.List("UserId='" + _UserId + "'").Count > 0 ? dal_UserRole.List("UserId='" + _UserId + "'")[0].RolesId : "";
            StringBuilder str_allRolesInfo = new StringBuilder();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT DeptId, DeptName, ParentId, '0' AS isRoles FROM Base_Department where DeleteMark='0'");
            if (!BaseService.IsAdmin())
                strSql.Append(" and DeptId in (" + BaseService.ReturnAuthority() + ")");
            strSql.Append("  UNION ALL ");
            strSql.Append("  SELECT RolesId,RolesName, DeptId, '1' AS isRoles FROM Base_Roles where DeleteMark='0' ");
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
                    str_allRolesInfo.Append(GetTreeNode(drv["DeptId"].ToString(), drv["DeptName"].ToString(), newDT, "1", _RolesId));
                    str_allRolesInfo.Append("</li>");
                }
            }
            return str_allRolesInfo.ToString();
        }
        int index_TreeNode = 0;
        private string GetTreeNode(string parentID, string parentName, DataTable dtNode, string status, string _RolesId)
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
                            sb_TreeNode.Append(GetTreeNode(drv["DeptId"].ToString(), drv["DeptName"].ToString(), dtNode, "2", _RolesId));
                            sb_TreeNode.Append("</li>");
                        }
                    }
                    else
                    {
                        if (status != "1")
                        {
                            sb_TreeNode.Append("<li>");
                            sb_TreeNode.Append("<div>");
                            sb_TreeNode.Append("<input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked(drv["DeptId"].ToString(), _RolesId) + " value=\"" + drv["DeptId"].ToString() + "|所属角色" + "\" name=\"checkbox\" />");
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
        #endregion
        /// <summary>
        /// 所属单位、所属角色新增、修改
        /// </summary>
        /// <param name="items"></param>
        /// <param name="_UserId"></param>
        /// <returns></returns>
        public bool add_ItemForm(string strs, string _UserId, string oldUserId)
        {
            List<string> list = new List<string>();
            list.Add(" delete from Base_UserDept where UserId='" + oldUserId + "' ");
            list.Add(" delete from Base_UserRole where UserId='" + oldUserId + "' ");
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
                        list.Add("insert into Base_UserDept (Id,DeptId,UserId)values('" + CommonHelper.GetGuid + "','" + key + "','" + _UserId + "')");
                    }
                    else if (type == "所属角色")
                    {
                        list.Add("insert into Base_UserRole (Id,RolesId,UserId)values('" + CommonHelper.GetGuid + "','" + key + "','" + _UserId + "')");
                    }
                }
            }
            return dal.ExecuteSqlTran(list) > 0 ? true : false;
        }
        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="_UserName"></param>
        /// <param name="_UserPwd"></param>
        /// <returns></returns>
        public Model.Base_UserInfo UserLogin(string _UserId, string _UserPwd)
        {
            return dal.CheckUserInfo(_UserId, Md5Helper.MD5(_UserPwd, 32).ToString());
        }

        public bool IsExits(string _UserId)
        {
            return dal.Exists("UserId", _UserId);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="_UserId"></param>
        /// <returns></returns>
        public bool Delete(string _UserId)
        {
            List<string> list = new List<string>
            {
                "Delete from Base_UserDept where UserId='"+_UserId+"'",
                "Delete from Base_UserRole where UserId='"+_UserId+"'",
                "Delete from Base_UserInfo where UserId='"+_UserId+"'"
            };
            return dal.ExecuteSqlTran(list) > 0 ? true : false;
        }
        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="_UserId"></param>
        /// <param name="_IsState"></param>
        /// <returns></returns>
        public bool Update(string _UserId, int _IsState)
        {
            return dal.Update<Model.Base_UserInfo>("IsState", _IsState, " UserId='" + _UserId + "'") > 0 ? true : false;
        }
        public bool UpdatePwd(string _UserId, string _Pwd)
        {
            return dal.Update<Model.Base_UserInfo>("UserPwd", _Pwd, " UserId='" + _UserId + "'") > 0 ? true : false;
        }

        public string GetUserRoleId(string _UserId)
        {
            UserRoleRepository bll_UserRole = new UserRoleRepository();
            return bll_UserRole.Get("UserId", _UserId).RolesId;
        }
        /// <summary>
        /// 获取AM账号
        /// </summary>
        /// <param name="_UserId"></param>
        /// <returns></returns>
        public string GetAmUserId(string _UserId)
        {
            Model.Base_UserInfo model = dal.Get("USERID", _UserId);
            return model == null ? "" : model.Am;
        }
    }
}
