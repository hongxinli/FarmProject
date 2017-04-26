using IDal.Agriculture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using Common;
using System.IO;

namespace OracleDal.Agriculture
{
    public class InfoRepository : BaseRepository<Model.Agriculture.A_Info>, IInfo
    {
        public int AddModel(Model.Agriculture.A_Info model)
        {
            OracleParameter[] param ={
                new OracleParameter(":Id",OracleType.VarChar),
                new OracleParameter(":InfoTitle",OracleType.VarChar),
                new OracleParameter(":InfoContent",OracleType.VarChar),
                new OracleParameter(":InfoType",OracleType.VarChar),
                new OracleParameter(":DeleteMark",OracleType.VarChar),
                new OracleParameter(":CreateUserName",OracleType.VarChar),
                new OracleParameter(":CreateDate",OracleType.DateTime)
                                     };
            param[0].Value = model.Id;
            param[1].Value = model.InfoTitle;
            param[2].Value = model.InfoContent;
            param[3].Value = model.InfoType;
            param[4].Value = model.DeleteMark;
            param[5].Value = model.CreateUserName;
            param[6].Value = model.CreateDate;
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into A_Info(Id,InfoTitle,InfoContent,InfoType,DeleteMark,CreateUserName,CreateDate)");
            sb.Append("values(:Id,:InfoTitle,:InfoContent,:InfoType,:DeleteMark,:CreateUserName,:CreateDate)");
            return OracleHelper.ExecuteNonQuery(OracleHelper.Conn, CommandType.Text, sb.ToString(), param);
        }

        public int UpateModel(Model.Agriculture.A_Info model)
        {
            OracleParameter[] param ={
                new OracleParameter(":Id",model.Id),
                new OracleParameter(":InfoTitle",model.InfoTitle),
                new OracleParameter(":InfoContent",model.InfoContent),
                new OracleParameter(":InfoType",model.InfoType),
                new OracleParameter(":DeleteMark",model.DeleteMark),
                new OracleParameter(":CreateUserName",model.CreateUserName),
                new OracleParameter(":CreateDate",model.CreateDate)
                                     };
            StringBuilder sb = new StringBuilder();
            sb.Append("update A_Info set ");
            sb.Append("InfoTitle=:InfoTitle,");
            sb.Append("InfoContent=:InfoContent,");
            sb.Append("InfoType=:InfoType,");
            sb.Append("DeleteMark=:DeleteMark,");
            sb.Append("CreateUserName=:CreateUserName,");
            sb.Append("CreateDate=:CreateDate");
            sb.Append(" where Id=:Id");
            return OracleHelper.ExecuteNonQuery(OracleHelper.Conn, CommandType.Text, sb.ToString(), param);
        }
    }
}
