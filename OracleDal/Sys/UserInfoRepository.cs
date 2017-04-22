using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDal.Sys;
using Common;
using System.Data;
using System.Data.OracleClient;

namespace OracleDal.Sys
{
    public class UserInfoRepository : BaseRepository<Model.Base_UserInfo>, IUserInfo
    {
        public Model.Base_UserInfo CheckUserInfo(string _UserId, string _UserPwd)
        {
            string strSql = "select * from Base_UserInfo where UserId=:UserId and UserPwd=:UserPwd";
            OracleParameter[] param = {
                new OracleParameter(":UserId",OracleType.VarChar),
                new OracleParameter(":UserPwd",OracleType.VarChar)
            };
            param[0].Value = _UserId;
            param[1].Value = _UserPwd;
            DataTable dt = OracleHelper.Query(OracleHelper.Conn, strSql, param).Tables[0];

            if (dt.Rows.Count > 0)
            {

                Model.Base_UserInfo model = new Model.Base_UserInfo();
                model.UserId = dt.Rows[0]["UserId"].ToString();
                model.UserName = dt.Rows[0]["UserName"].ToString();
                model.UserPwd = dt.Rows[0]["UserPwd"].ToString();
                model.Theme = dt.Rows[0]["Theme"].ToString();
                model.IsAdmin = Convert.ToInt32(dt.Rows[0]["IsAdmin"]);
                model.IsState = Convert.ToInt32(dt.Rows[0]["IsState"]);
                return model;
            }
            else
            {
                return null;
            }

        }
    }
}
