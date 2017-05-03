using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDal.Sys;
using OracleDal.Sys;
using Common;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;

namespace Bll.Sys
{
    public class DepartmentService
    {
        private string moduleId = "SysDept";
        private IDepartment dal = new DepartmentRepository();
        private IUserDept dal_UserDept = new UserDeptRepository();
        public Model.Base_UserDept GetModel(string _userId)
        {
            return dal_UserDept.Get("userid", _userId);
        }
        public Model.Base_Department GetDepartment(string deptId)
        {
            return dal.Get("DeptId", deptId);
        }

        #region 部门树列表
        /// <summary>
        /// 部门树列表
        /// </summary>
        /// <returns></returns>
        public string GetDeptTreeTable()
        {
            StringBuilder str_tableTree = new StringBuilder();
            IList<Model.Base_Department> list = dal.List(" DeleteMark=0 order by SortCode asc");
            IList<Model.Base_Department> newList = list.Where(t => t.ParentId.Equals("-1")).ToList();
            int eRowIndex = 0;
            foreach (Model.Base_Department model in newList)
            {
                string trID = "node-" + eRowIndex.ToString();
                str_tableTree.Append("<tr id='" + trID + "'>");
                str_tableTree.Append("<td style='width: 200px;padding-left:20px;'><span class=\"folder\">" + model.DeptName + "</span></td>");
                str_tableTree.Append("<td style='width: 100px;text-align: center;'>" + model.Dlevel + "</td>");
                str_tableTree.Append("<td style='width: 100px;text-align: center;'>" + model.SortCode + "</td>");
                str_tableTree.Append("<td style='width: 100px;text-align: center;'>" + model.Creator + "</td>");
                str_tableTree.Append("<td style='width: 120px;text-align: center;'>" + model.CreateDate + "</td>");
                str_tableTree.Append("<td >" + model.Remarks + "</td>");
                str_tableTree.Append("<td style='display:none'>" + model.DeptId + "</td>");
                str_tableTree.Append("</tr>");
                //创建子节点
                str_tableTree.Append(GetTableTreeNode(model.DeptId.ToString(), list, trID));
                eRowIndex++;
            }
            return str_tableTree.ToString();
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTableTreeNode(string parentID, IList<Model.Base_Department> list, string parentTRID)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            IList<Model.Base_Department> newList = list.Where(t => t.ParentId.Equals(parentID)).ToList();
            int i = 1;
            foreach (Model.Base_Department model in newList)
            {
                string trID = parentTRID + "-" + i.ToString();
                sb_TreeNode.Append("<tr id='" + trID + "' class='child-of-" + parentTRID + "'>");
                sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + model.DeptName + "</span></td>");
                sb_TreeNode.Append("<td style='width: 100px;text-align: center;'>" + model.Dlevel + "</td>");
                sb_TreeNode.Append("<td style='width: 100px;text-align: center;'>" + model.SortCode + "</td>");
                sb_TreeNode.Append("<td style='width: 100px;text-align: center;'>" + model.Creator + "</td>");
                sb_TreeNode.Append("<td style='width: 120px;text-align: center;'>" + model.CreateDate + "</td>");
                sb_TreeNode.Append("<td >" + model.Remarks + "</td>");
                sb_TreeNode.Append("<td style='display:none'>" + model.DeptId + "</td>");
                sb_TreeNode.Append("</tr>");
                //创建子节点
                sb_TreeNode.Append(GetTableTreeNode(model.DeptId.ToString(), list, trID));
                i++;
            }
            return sb_TreeNode.ToString();
        }

        /// <summary>
        /// 部门树
        /// </summary>
        /// <returns></returns>
        public string GetDeptTree()
        {
            StringBuilder strHtml = new StringBuilder();
            var list = dal.List("DeleteMark=0 order by DeptId, SortCode asc");
            IList<Model.Base_Department> AuthorityList = null;
            if (BaseService.IsAdmin())
            {
                AuthorityList = list;
            }
            else
            {
                AuthorityList = BaseService.ReturnRightData<Model.Base_Department>(list);
            }

            RolesService bll_Roles = new RolesService();
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
                    strHtml.Append("<li>");
                    strHtml.Append("<div>" + model.DeptName + "");
                    strHtml.Append("<span style='display:none'>" + model.DeptId + "</span></div>");
                    //创建子节点
                    strHtml.Append(GetTreeNode(model.DeptId, newList));
                    strHtml.Append("</li>");
                }
            }
            else
            {
                strHtml.Append("<li>");
                strHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
                strHtml.Append("</li>");
            }
            return strHtml.ToString();
        }

        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public string GetTreeNode(string parentId, IList<Model.Base_Department> list)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            IList<Model.Base_Department> newList = list.Where(t => t.ParentId.Equals(parentId)).ToList();
            if (newList.Count > 0)
            {
                sb_TreeNode.Append("<ul>");
                foreach (Model.Base_Department model in newList)
                {
                    sb_TreeNode.Append("<li>");
                    sb_TreeNode.Append("<div>" + model.DeptName + "");
                    sb_TreeNode.Append("<span style='display:none'>" + model.DeptId + "</span></div>");
                    //创建子节点
                    sb_TreeNode.Append(GetTreeNode(model.DeptId, list));
                    sb_TreeNode.Append("</li>");
                }
                sb_TreeNode.Append("</ul>");
            }
            return sb_TreeNode.ToString();
        }
        #endregion
        /// <summary>
        /// 初始化页面数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="DeptId"></param>
        public void InitData(Page page, string DeptId)
        {
            Model.Base_Department model = dal.Get("DeptId", DeptId);
            ControlBindHelper.SetWebControls(page, model);
        }

        public void InitParentId(HtmlSelect ParentId, string DeptId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT DeptId,
                            DeptName||' - '||CASE Dlevel WHEN 1 THEN '父节' WHEN 2 THEN '父节' ELSE  '子节' END AS DeptName
                            FROM Base_Department ");
            strSql.Append(" WHERE DeleteMark = 0 ");
            if (!BaseService.IsAdmin())
                strSql.Append(" and deptid in (" + BaseService.ReturnAuthority() + ")");
            strSql.Append(" ORDER BY DeptId, SortCode ASC ");
            DataTable dt = dal.Query(strSql.ToString()).Tables[0];

            ControlBindHelper.BindHtmlSelectFirstShow(dt, ParentId, "DeptName", "DeptId", DeptId);
        }

        /// <summary>
        /// 表单提交：新增，修改
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ParentId"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public bool Submit_AddOrEdit(Page page, HtmlSelect ParentId, string DeptId)
        {
            Model.Base_Department model = new Model.Base_Department();
            if (ParentId.Value == "")
            {
                model.ParentId = "-1";
            }
            model = ControlBindHelper.GetWebControls<Model.Base_Department>(page, model);
            if (string.IsNullOrEmpty(DeptId))
            {
                model.Creator = RequestCookie.GetCookieUser().UserId.ToString();
                model.CreateDate = DateTime.Now;
                string actStr = "该用户对-[" + model.DeptName + "]进行了新增操作。";
                BaseService.WriteLogEvent(actStr, moduleId);
                return string.IsNullOrEmpty(dal.Add(model)) ? false : true;
            }
            else
            {
                string actStr = "该用户对-[" + model.DeptName + "]进行了修改操作。";
                BaseService.WriteLogEvent(actStr, moduleId);
                return dal.Update(model, " DeptId='" + DeptId + "'") > 0 ? true : false;
            }

        }
        /// <summary>
        /// 获取所有的单位信息
        /// </summary>
        /// <returns></returns>
        public IList<Model.Base_Department> List()
        {
            return dal.List();
        }

        public bool Delete(string _DeptId)
        {
            List<string> list = new List<string>
            {
                "Delete from Base_RoleDept where DeptId='"+_DeptId+"'",
                "Delete from Base_UserDept where DeptId='"+_DeptId+"'",
                "Delete from Base_Department where DeptId='"+_DeptId+"'"
            };
            return dal.ExecuteSqlTran(list) > 0 ? true : false;
        }
        public Model.Base_Department GetModual(string DeptId)
        {
            Model.Base_Department model = dal.Get("DeptId", DeptId);
            return model;
        }
        public Model.Base_Department GetModualByName(string deptName)
        {
            Model.Base_Department model = dal.Get("DeptName", deptName);
            return model;
        }
        public DataSet GetList(string strsql)
        {
            return dal.Query(strsql);
        }
    }
}
