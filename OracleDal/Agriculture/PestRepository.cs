using Common;
using IDal.Agriculture;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace OracleDal.Agriculture
{
    public class PestRepository : BaseRepository<Model.Agriculture.A_Pest>, IPest
    {
        public int AddModel(Model.Agriculture.A_Pest model)
        {
            OracleParameter[] param ={
                new OracleParameter(":Id",OracleType.VarChar),
                new OracleParameter(":PestName",OracleType.VarChar),
                new OracleParameter(":PestContent",OracleType.VarChar),
                new OracleParameter(":CropType",OracleType.VarChar),
                new OracleParameter(":DeleteMark",OracleType.VarChar),
                new OracleParameter(":CreateUserName",OracleType.VarChar),
                new OracleParameter(":CreateDate",OracleType.DateTime)
                                     };
            param[0].Value = model.Id;
            param[1].Value = model.PestName;
            param[2].Value = model.PestContent;
            param[3].Value = model.CropType;
            param[4].Value = model.DeleteMark;
            param[5].Value = model.CreateUserName;
            param[6].Value = model.CreateDate;
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into A_Pest(Id,PestName,PestContent,CropType,DeleteMark,CreateUserName,CreateDate)");
            sb.Append("values(:Id,:PestName,:PestContent,:CropType,:DeleteMark,:CreateUserName,:CreateDate)");
            return OracleHelper.ExecuteNonQuery(OracleHelper.Conn, CommandType.Text, sb.ToString(), param);
        }

        public int UpateModel(Model.Agriculture.A_Pest model)
        {
            OracleParameter[] param ={
                new OracleParameter(":Id",model.Id),
                new OracleParameter(":PestName",model.PestName),
                new OracleParameter(":PestContent",model.PestContent),
                new OracleParameter(":CropType",model.CropType),
                new OracleParameter(":DeleteMark",model.DeleteMark),
                new OracleParameter(":CreateUserName",model.CreateUserName),
                new OracleParameter(":CreateDate",model.CreateDate)
                                     };
            StringBuilder sb = new StringBuilder();
            sb.Append("update A_Pest set ");
            sb.Append("PestName=:PestName,");
            sb.Append("PestContent=:PestContent,");
            sb.Append("CropType=:CropType,");
            sb.Append("DeleteMark=:DeleteMark,");
            sb.Append("CreateUserName=:CreateUserName,");
            sb.Append("CreateDate=:CreateDate");
            sb.Append(" where Id=:Id");
            return OracleHelper.ExecuteNonQuery(OracleHelper.Conn, CommandType.Text, sb.ToString(), param);
        }
    }
}
