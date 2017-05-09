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
            DataTable dt = dal.DataTableByPage(pageSize, pageIndex, strSql, "", ref count, " CreateDate desc");
            return dt;
        }
        public IList<Model.Agriculture.A_Info> ListByPage(int pageIndex, int pageSize, ref int count)
        {
            count = dal.Count();
            return dal.ListByPage(pageSize, pageIndex, "", " CreateDate desc");
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

        public void InitInfoType(System.Web.UI.HtmlControls.HtmlSelect InfoType, string _key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from base_code where domainname='A_INFO_TYPE' order by scode");
            DataTable dt = dal.Query(strSql.ToString()).Tables[0];

            ControlBindHelper.BindHtmlSelectFirstShow(dt, InfoType, "SName", "SName", _key);
        }
    }
}
