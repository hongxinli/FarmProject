using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDal.Sys;
using OracleDal.Sys;
using System.Data;
using Common;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Collections;
using System.Web.SessionState;
namespace Bll.Sys
{
    public class ModuleInfoService : IRequiresSessionState
    {
        IModuleInfo dal = new ModuleInfoRepository();
        IModuleDept dal_ModuleDept = new ModuleDeptRepsitory();
        /// <summary>
        /// 模块树列表
        /// </summary>
        /// <returns></returns>
        public string GetMenuTreeTable()
        {
            StringBuilder TableTree_Menu = new StringBuilder();
            DataTable dt = dal.Query(@"select * from (select t.*,m.deptid,n.deptname from base_moduleinfo t left join base_moduledept m on t.moduleid=m.moduleid left join base_department n on m.deptid=n.deptid ) t where t.deletemark=0 and t.moduletype!=3 order by t.deptid, t.sortcode asc").Tables[0];
            DataRow[] drs = dt.Select(" parentid='-1'");
            int eRowIndex = 0;
            for (int i = 0; i < drs.Length; i++)
            {
                string trID = "node-" + eRowIndex.ToString();
                TableTree_Menu.Append("<tr id='" + trID + "'>");
                TableTree_Menu.Append("<td style='width: 300px;padding-left:20px;'><span class=\"folder\">" + drs[i]["ModuleName"] + "</span></td>");
                if (!string.IsNullOrEmpty(drs[i]["ModuleImg"].ToString()))
                    TableTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + drs[i]["ModuleImg"] + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                else
                    TableTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                TableTree_Menu.Append("<td style='width: 60px;text-align:center;'>" + this.GetMenu_Type(drs[i]["ModuleType"].ToString()) + "</td>");
                TableTree_Menu.Append("<td style='width: 60px;text-align:center;'>" + drs[i]["Target"] + "</td>");
                TableTree_Menu.Append("<td style='width: 60px;text-align:center;'>" + drs[i]["ModuleType"] + "-" + drs[i]["SortCode"] + "</td>");
                TableTree_Menu.Append("<td>" + drs[i]["NavigateUrl"] + "</td>");
                TableTree_Menu.Append("<td style='display:none'>" + drs[i]["ModuleId"] + "</td>");
                TableTree_Menu.Append("</tr>");
                //创建子节点
                TableTree_Menu.Append(GetTableTreeNode(drs[i]["ModuleId"].ToString(), dt, trID));
                eRowIndex++;
            }
            return TableTree_Menu.ToString();
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTableTreeNode(string parentID, DataTable dt, string parentTRID)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            DataView dv = new DataView(dt);
            dv.RowFilter = ("parentid='" + parentID + "'");
            int i = 1;
            foreach (DataRowView dr in dv)
            {
                string trID = parentTRID + "-" + i.ToString();
                sb_TreeNode.Append("<tr id='" + trID + "' class='child-of-" + parentTRID + "'>");
                sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + dr["ModuleName"]);
                if (!string.IsNullOrEmpty(dr["DeptName"].ToString()))
                {
                    sb_TreeNode.Append("（" + dr["DeptName"] + "）");
                }
                sb_TreeNode.Append("</span></td>");
                if (!string.IsNullOrEmpty(dr["ModuleImg"].ToString()))
                    sb_TreeNode.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + dr["ModuleImg"] + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                else
                    sb_TreeNode.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + this.GetMenu_Type(dr["ModuleType"].ToString()) + "</td>");
                sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + dr["Target"] + "</td>");
                sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + dr["ModuleType"] + "-" + dr["SortCode"] + "</td>");
                sb_TreeNode.Append("<td>" + dr["NavigateUrl"] + "</td>");
                sb_TreeNode.Append("<td style='display:none'>" + dr["ModuleId"] + "</td>");
                sb_TreeNode.Append("<td style='display:none'>" + dr["DeptId"] + "</td>");
                sb_TreeNode.Append("</tr>");
                //创建子节点
                sb_TreeNode.Append(GetTableTreeNode(dr["ModuleId"].ToString(), dt, trID));
                i++;
            }
            return sb_TreeNode.ToString();
        }
        /// <summary>
        /// 模块类型
        /// </summary>
        /// <param name="Menu_Type">类型</param>
        /// <returns></returns>
        public string GetMenu_Type(string Menu_Type)
        {
            if (Menu_Type == "0")
            {
                return "根节";
            }
            if (Menu_Type == "1")
            {
                return "父节";
            }
            else if (Menu_Type == "2")
            {
                return "子节";
            }
            else if (Menu_Type == "3")
            {
                return "按钮";
            }
            else
            {
                return "其他";
            }
        }
        /// <summary>
        /// 节点位置下拉框绑定
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="_key"></param>
        public void InitParentId(HtmlSelect ParentId, string ModuleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT ModuleId,
                            ModuleName ||' - '|| CASE ModuleType WHEN 0 THEN '根节' WHEN 1 THEN '父节' WHEN 2 THEN '子节' END AS ModuleName
                            FROM Base_ModuleInfo WHERE DeleteMark = 0 AND ModuleType != 3 ORDER BY ModuleType, SortCode ASC");
            DataSet ds = dal.Query(strSql.ToString());
            DataTable dt = ds.Tables[0];
            if (!string.IsNullOrEmpty(ModuleId))
            {
                if (DataTableHelper.IsExistRows(dt))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["ModuleId"].ToString() == ModuleId)
                            dt.Rows.RemoveAt(i);
                    }
                }
            }
            ControlBindHelper.BindHtmlSelect(dt, ParentId, "ModuleName", "ModuleId", "模块菜单 - 下拉选择");
        }
        /// <summary>
        /// 获取模块model
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public Model.Base_ModuleInfo GetModuleInfo(string ModuleId)
        {
            Model.Base_ModuleInfo model = dal.Get("ModuleId", ModuleId);
            return model;
        }
        /// <summary>
        /// 初始化页面数据
        /// </summary>
        /// <param name="page"></param>
        public void InitData(Page page, string ModuleId)
        {
            Model.Base_ModuleInfo model = GetModuleInfo(ModuleId);
            ControlBindHelper.SetWebControls(page, model);
        }

        public void InitDept(HtmlSelect sel_Dept, string key)
        {
            DepartmentService bll_DeptSer = new DepartmentService();
            DataTable dt = bll_DeptSer.GetList("select t.deptid,t.parentid,t.deptname from base_department t where t.deletemark='0' and t.parentid='101101'").Tables[0];
            ControlBindHelper.BindHtmlSelectFirst(dt, sel_Dept, "DeptName", "DeptId", key);
        }
        /// <summary>
        /// 表单提交：新增，修改
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ParentId"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public bool Submit_AddOrEdit(Page page, HtmlSelect ParentId, string ModuleId, string DeptId)
        {
            string moduleId = "SysModule";
            Model.Base_ModuleInfo model = new Model.Base_ModuleInfo();
            if (ParentId.Value == "")
            {
                model.ParentId = "0";
                model.ModuleType = 1;
            }
            else
            {
                model.ModuleType = 2;
            }
            model = ControlBindHelper.GetWebControls<Model.Base_ModuleInfo>(page, model);
            if (string.IsNullOrEmpty(ModuleId))
            {
                model.ModuleId = CommonHelper.GetGuid;
                model.Creator = RequestSession.GetSessionUser().UserId.ToString();
                model.CreateDate = DateTime.Now;
                #region 操作日志记录
                string actStr = "该用户对-模块名称：[" + model.ModuleName + "]进行了新增操作。";
                Bll.BaseService.WriteLogEvent(actStr, moduleId);
                #endregion
                List<string> list = new List<string>() { 
                "insert into base_moduleinfo(moduleid,parentid,modulename,moduletitle,moduleimg,moduletype,navigateurl,target,sortcode,creator,createdate)values('"+model.ModuleId+"','"+model.ParentId+"','"+model.ModuleName+"','"+model.ModuleTitle+"','"+model.ModuleImg+"','"+model.ModuleType+"','"+model.NavigateUrl+"','"+model.Target+"','"+model.SortCode+"','"+model.Creator+"',to_date('"+model.CreateDate+"','yyyy-mm-dd hh24:mi:ss'))",
                "insert into base_moduledept(id,moduleid,moduleparentid,deptid)values('"+model.ModuleId+"','"+model.ModuleId+"','"+model.ParentId+"','"+DeptId+"')"
                };
                return dal.ExecuteSqlTran(list) > 0 ? true : false;
                // string.IsNullOrEmpty(dal.Add(model)) ? false : true;
            }
            else
            {
                #region 操作日志记录
                string actStr = "该用户对-模块名称：[" + model.ModuleName + "]进行了修改操作。";
                Bll.BaseService.WriteLogEvent(actStr, moduleId);
                #endregion

                List<string> list = new List<string>() { 
                "update base_moduleinfo set parentid='"+model.ParentId+"',modulename='"+model.ModuleName+"',moduletitle='"+model.ModuleTitle+"',moduleimg='"+model.ModuleImg+"',moduletype='"+model.ModuleType+"',navigateurl='"+model.NavigateUrl+"',target='"+model.Target+"',sortcode='"+model.SortCode+"' where moduleid='"+ModuleId+"'"
                };
                if (dal_ModuleDept.Get("ModuleId", ModuleId) == null)
                {
                    list.Add("insert into base_moduledept(id,moduleid,moduleparentid,deptid)values('" + ModuleId + "','" + ModuleId + "','" + model.ParentId + "','" + DeptId + "')");
                }
                else
                {
                    list.Add("update base_moduledept set deptid='" + DeptId + "',moduleparentid='" + model.ParentId + "' where moduleid='" + ModuleId + "'");
                }
                return dal.ExecuteSqlTran(list) > 0 ? true : false;
                // return dal.Update(model, " ModuleId='" + ModuleId + "'") > 0 ? true : false;
            }

        }

        /// <summary>
        /// 根据菜单主键获取已有按钮
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public string GetModuleByButton(string ModuleId)
        {
            StringBuilder selectedButtonList = new StringBuilder();
            IList<Model.Base_ModuleInfo> list = dal.List(" ParentId='" + ModuleId + "' and DeleteMark=0 and ModuleType=3 order by SortCode asc");
            for (int i = 0; i < list.Count; i++)
            {
                selectedButtonList.Append("<div onclick='selectedButton(this)' ondblclick=\"removeButton('" + list[i].ModuleId + "')\" title='" + list[i].ModuleTitle + "' class=\"shortcuticons\"><img src=\"/Themes/Images/16/" + list[i].ModuleImg + "\" alt=\"\" /><br />" + list[i].ModuleName + "</div>");
            }
            return selectedButtonList.ToString();
        }
        /// <summary>
        /// 分配按钮
        /// </summary>
        /// <param name="pkVal">全中按钮ID</param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public int AddButton(string pkVal, string ParentId)
        {
            ButtonRepository _buttonRepository = new ButtonRepository();
            Model.Base_Button model_Button = _buttonRepository.Get("id", pkVal);
            Model.Base_ModuleInfo model_ModuleInfo = new Model.Base_ModuleInfo();
            model_ModuleInfo.ModuleId = CommonHelper.GetGuid;
            model_ModuleInfo.ParentId = ParentId;
            model_ModuleInfo.ModuleName = model_Button.ButtonName;
            model_ModuleInfo.ModuleTitle = model_Button.ButtonTitle;
            model_ModuleInfo.ModuleImg = model_Button.ButtonImg;
            model_ModuleInfo.ModuleType = 3;
            model_ModuleInfo.NavigateUrl = model_Button.ButtonCode;
            int i = CommonHelper.GetInt(dal.Max("SortCode", " ParentId='" + ParentId + "' and DeleteMark=0 and ModuleType=3"));
            model_ModuleInfo.SortCode = i + 1;
            model_ModuleInfo.Target = "Onclick";
            model_ModuleInfo.Creator = RequestSession.GetSessionUser().UserId.ToString();
            model_ModuleInfo.CreateDate = DateTime.Now;
            return string.IsNullOrEmpty(dal.Add(model_ModuleInfo)) ? 0 : 1;
        }
        /// <summary>
        /// 移除按钮
        /// </summary>
        /// <param name="pkVal"></param>
        /// <returns></returns>
        public int RemoveButton(string pkVal)
        {
            return dal.Delete("ModuleId", pkVal);
        }
        /// <summary>
        /// 模块信息 json
        /// </summary>
        /// <returns></returns>
        public string GetMenuHtml()
        {
            string roleid = RequestSession.GetSessionUser().RoleId.ToString();
            string sqlStr = "select * from  base_moduleinfo  where deleteMark=0 and target='Iframe' and moduletype in ('0','1','2') and  moduleid in(select m.moduleid from base_roleright m where m.rolesid='" + roleid + "') order by moduleType,sortcode";
            DataTable dt = dal.Query(sqlStr).Tables[0];
            return JsonHelper.DataTableToJson(dt, "Menu");
        }
        /// <summary>
        /// 模块信息    IList
        /// </summary>
        /// <returns></returns>
        public string GetAdminMenu()
        {
            string roleid = RequestSession.GetSessionUser().RoleId.ToString();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from (");
            strSql.Append("select t.moduleid,case when t.deptid is null then t.parentid else t.deptid end parentid,t.modulename,t.moduletitle,t.moduleimg,t.moduletype,t.navigateurl,t.target,t.sortcode,'0' menuType ");
            strSql.Append("from (select t.*,n.moduleparentid,n.deptid from base_moduleinfo t left join base_moduledept n on t.moduleid=n.moduleid where t.deleteMark=0 and t.target='Iframe' and t.moduletype in ('0','1','2')) t");
            strSql.Append(" where t.moduleid in(select m.moduleid from base_roleright m where m.rolesid='" + roleid + "') ");
            strSql.Append(" union all ");
            strSql.Append(" select m.deptid,n.moduleparentid,m.deptname,'' moduletitle,'' moduleimg,1 moduletype,'' navigateurl,'Iframe' target,0 sortcode,'1' menuType from base_department m ");
            strSql.Append(" right join base_moduledept n on m.deptid=n.deptid ");
            strSql.Append(") t order by t.moduletype,t.sortcode ");
            DataTable dt = dal.Query(strSql.ToString()).Tables[0];
            return JsonHelper.DataTableToJson(dt, "Menu");
        }

        public DataTable GetAdminMenuTable(string parentId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select n.deptid,m.deptname,T.* from base_moduleinfo t left join base_moduledept n on t.moduleid=n.moduleid left join base_department m on n.deptid=m.deptid where t.deleteMark=0 and t.target='Iframe' and t.moduletype in ('0','1','2') and t.parentid='" + parentId + "' order by n.deptid,t.sortcode");
            DataTable dt = dal.Query(strSql.ToString()).Tables[0];
            return dt;
        }

        public DataTable GetMenuDepartment()
        {
            string strSql = "select distinct m.deptid,m.deptname from base_department m right join base_moduledept n on m.deptid=n.deptid";
            DataTable dt = dal.Query(strSql).Tables[0];
            return dt;
        }
        /// <summary>
        /// 是否允许删除
        /// </summary>
        /// <param name="_ModuleId"></param>
        /// <returns></returns>
        public int IsAllowDelete(string _ModuleId)
        {
            return dal.Get("ModuleId", _ModuleId).AllowDelete;
        }

        public bool IsExists(string _ParentId)
        {
            return dal.Exists("ParentId", _ParentId);
        }

        public bool UpdateDeleteMark(string _ModuleId)
        {
            List<string> list = new List<string>()
            {
                "Delete from Base_RoleRight where ModuleId='"+_ModuleId+"'",
                "Delete from Base_ModuleInfo where ModuleId='"+_ModuleId+"'",
                "Delete from base_moduledept where ModuleId='"+_ModuleId+"'"
            };
            return dal.ExecuteSqlTran(list) > 0 ? true : false;
        }
    }
}
