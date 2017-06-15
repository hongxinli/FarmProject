using Common;
using IDal.Agriculture;
using OracleDal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using OracleDal.Agriculture;
using System.Web.UI.HtmlControls;

namespace Bll.Agriculture
{
    public class PestService
    {
        IPest dal = new PestRepository();
        public System.Data.DataTable DataTableByPage(int pageIndex, int pageSize, ref int count)
        {
            string strSql = "select * from a_pest";
            DataTable dt = dal.DataTableByPage(pageSize, pageIndex, strSql, "", ref count, " CreateDate desc");
            return dt;
        }
        public IList<Model.Agriculture.A_Pest> ListByPage(int pageIndex, int pageSize,string typeid, ref int count)
        {
            count = dal.Count();
            return dal.ListByPage(pageSize, pageIndex," deleteMark=0 and croptype='"+typeid+"'"," croptype desc");
        }
        public void InitData(System.Web.UI.Page page, string _key)
        {
            Model.Agriculture.A_Pest model = dal.Get("Id", _key);
            ControlBindHelper.SetWebControls(page, model);
        }
        public bool Submit_AddOrEdit(System.Web.UI.Page page, string _key)
        {
            Model.Agriculture.A_Pest model = ControlBindHelper.GetWebControls<Model.Agriculture.A_Pest>(page, new Model.Agriculture.A_Pest());
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

        public string GetPestContent(string _key)
        {
            return dal.Get("Id", _key).PestContent;
        }
        public void InitCropType(HtmlSelect CropType, string _key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from base_code where domainname='A_CROP_TYPE' order by scode");
            DataTable dt = dal.Query(strSql.ToString()).Tables[0];

            ControlBindHelper.BindHtmlSelectFirstShow(dt, CropType, "SName", "SName", _key);
        }

    }
}
