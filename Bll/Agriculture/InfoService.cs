using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDal.Agriculture;
using OracleDal.Agriculture;
using System.Data;
using Common;

namespace Bll.Agriculture
{
    public class InfoService
    {
        IInfo dal = new InfoRepository();


        public System.Data.DataTable DataTableByPage(int pageIndex, int pageSize, ref int count)
        {
            string strSql = "select * from a_info";
            DataTable dt = dal.DataTableByPage(pageSize, pageIndex, strSql, "", ref count, " top desc, CreateDate desc");
            return dt;
        }
        public IList<Model.Agriculture.A_Info> ListByPage(int pageIndex, int pageSize, string type, ref int count)
        {
            count = dal.Count();
            return dal.ListByPage(pageSize, pageIndex, "infotype='" + type + "'", " CreateDate desc");
        }
        public IList<Model.Agriculture.A_Info> newsList()
        {
            string strSql = "select * from(select ROW_NUMBER() over(partition by t.infotype order by t.createdate desc) num, t.id , t.infotitle,t.infocontent,t.infotype,t.createdate from a_info t) where num=1";
            var list = dal.List(strSql, "");
            return list;
        }
        public IList<Model.Agriculture.A_Info> BarsList(int pageIndex, int pageSize, ref int count)
        {
            count = dal.Count();
            return dal.ListByPage(pageSize, pageIndex, " deletemark=0 ", " top desc,createdate desc");
        }
        public void InitData(System.Web.UI.Page page, string _key)
        {
            Model.Agriculture.A_Info model = dal.Get("Id", _key);
            ControlBindHelper.SetWebControls(page, model);
        }
        public bool Submit_AddOrEdit(System.Web.UI.Page page, string _key)
        {
            Model.Agriculture.A_Info model = ControlBindHelper.GetWebControls<Model.Agriculture.A_Info>(page, new Model.Agriculture.A_Info());
            if (string.IsNullOrEmpty(_key))
            {
                model.Id = CommonHelper.GetGuid;
                model.CreateUserName = RequestCookie.GetCookieUser().UserName.ToString();
                model.CreateDate = DateTime.Now;
                model.DeleteMark = "0";
                return dal.AddModel(model) > 0 ? true : false;
            }
            else
            {
                model.CreateUserName = RequestCookie.GetCookieUser().UserName.ToString();
                model.CreateDate = DateTime.Now;
                model.DeleteMark = "0";
                return dal.UpateModel(model) > 0 ? true : false;
            }
        }

        public int Delete(string _key)
        {
            return dal.Delete("Id", _key);
        }

        public string GetInfoContent(string _key)
        {
            return dal.Get("Id", _key).InfoContent;
        }

        public int Up(string id)
        {
            return dal.ExecuteNonQuery(" update a_info set top=1 where id='" + id + "'");
        }
        public int Down(string id)
        {
            return dal.ExecuteNonQuery(" update a_info set top=0 where id='" + id + "'");
        }
        public void InitInfoType(System.Web.UI.HtmlControls.HtmlSelect InfoType, string _key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from base_code where domainname='A_INFO_TYPE' order by scode");
            DataTable dt = dal.Query(strSql.ToString()).Tables[0];

            ControlBindHelper.BindHtmlSelectFirstShow(dt, InfoType, "SName", "SName", _key);
        }
    }
}
