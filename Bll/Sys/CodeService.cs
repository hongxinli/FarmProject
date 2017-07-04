using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OracleDal.Sys;
using IDal.Sys;
using System.Data;
using Common;

namespace Bll.Sys
{
    public class CodeService
    {
        ICode DAL = new CodeRepository();
        private static Model.Base_Code _oldEntity;
        public DataTable DataTableByPage(int pageIndex, int pageSize, ref int count)
        {

            string strSql = "select * from BASE_CODE";
            DataTable dt = DAL.DataTableByPage(pageSize, pageIndex, strSql, "", ref count, " domainname ");
            return dt;
        }
        public void InitData(System.Web.UI.Page page, string _key)
        {
             _oldEntity = DAL.Get("Id", _key);
             ControlBindHelper.SetWebControls(page, _oldEntity);
        }

        public bool Submit_AddOrEdit(System.Web.UI.Page page, string _key)
        {
            Model.Base_Code model = ControlBindHelper.GetWebControls<Model.Base_Code>(page, new Model.Base_Code());
            if (string.IsNullOrEmpty(_key))
            {
                model.Id = CommonHelper.GetGuid;
                model.CreateUserName = RequestCookie.GetCookieUser().UserName.ToString();
                model.CreateDate = DateTime.Now;
                return string.IsNullOrEmpty(DAL.Add(model)) ? false : true;
            }
            else
            {
                model.CreateUserName = RequestCookie.GetCookieUser().UserName.ToString();
                model.CreateDate = DateTime.Now;
                if (model.DomainName == "A_INFO_TYPE")
                {
                    Bll.Agriculture.InfoService _Ser = new Agriculture.InfoService();
                    string strSql = "update a_info set INFOTYPE='" + model.SName + "' where INFOTYPE='"+_oldEntity.SName+"'";
                    _Ser.Update(strSql);
                }
                return DAL.Update(model, " Id='" + _key + "'") > 0 ? true : false;
            }
        }

        public int Delete(string _key)
        {
            return DAL.Delete("Id", _key);
        }

        public IList<Model.Base_Code> List(string where)
        {
            return DAL.List(where);
        }
    }
}
