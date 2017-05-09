using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Common;
using IDal.Agriculture;
using OracleDal.Agriculture;
using System.Web.UI.HtmlControls;

namespace Bll.Agriculture
{
    public class CropService
    {
        ICrop dal = new CropRepository();
        public System.Data.DataTable DataTableByPage(int pageIndex, int pageSize, string strWhere, ref int count)
        {
            string strSql = "select * from a_crop";
            DataTable dt = dal.DataTableByPage(pageSize, pageIndex, strSql, strWhere, ref count, " CreateDate desc,CropType");
            return dt;
        }
        public IList<Model.Agriculture.A_Crop> ListByPage(int pageIndex, int pageSize, string strWhere, ref int count)
        {
            count = dal.Count(strWhere);
          return  dal.ListByPage(pageSize, pageIndex, strWhere, " CreateDate desc,CropType");
        }
        public void InitData(System.Web.UI.Page page, string _key)
        {
            Model.Agriculture.A_Crop model = dal.Get("Id", _key);
            ControlBindHelper.SetWebControls(page, model);
        }
        public bool Submit_AddOrEdit(System.Web.UI.Page page, string _key)
        {
            Model.Agriculture.A_Crop model = ControlBindHelper.GetWebControls<Model.Agriculture.A_Crop>(page, new Model.Agriculture.A_Crop());
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
            return dal.Get("Id", _key).CropContent;
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
