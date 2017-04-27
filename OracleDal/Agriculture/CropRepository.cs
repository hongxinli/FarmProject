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
    public class CropRepository : BaseRepository<Model.Agriculture.A_Crop>, ICrop
    {
        public int AddModel(Model.Agriculture.A_Crop model)
        {
            OracleParameter[] param ={
                new OracleParameter(":Id",OracleType.VarChar),
                new OracleParameter(":CropName",OracleType.VarChar),
                new OracleParameter(":CropContent",OracleType.VarChar),
                new OracleParameter(":CropType",OracleType.VarChar),
                new OracleParameter(":DeleteMark",OracleType.VarChar),
                new OracleParameter(":CreateUserName",OracleType.VarChar),
                new OracleParameter(":CreateDate",OracleType.DateTime),
                new OracleParameter(":Remarks",OracleType.VarChar)
                                     };
            param[0].Value = model.Id;
            param[1].Value = model.CropName;
            param[2].Value = model.CropContent;
            param[3].Value = model.CropType;
            param[4].Value = model.DeleteMark;
            param[5].Value = model.CreateUserName;
            param[6].Value = model.CreateDate;
            param[7].Value = model.Remarks;
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into A_Crop(Id,CropName,CropContent,CropType,DeleteMark,CreateUserName,CreateDate,Remarks)");
            sb.Append("values(:Id,:CropName,:CropContent,:CropType,:DeleteMark,:CreateUserName,:CreateDate,:Remarks)");
            return OracleHelper.ExecuteNonQuery(OracleHelper.Conn, CommandType.Text, sb.ToString(), param);
        }

        public int UpateModel(Model.Agriculture.A_Crop model)
        {
            OracleParameter[] param ={
                new OracleParameter(":Id",model.Id),
                new OracleParameter(":CropName",model.CropName),
                new OracleParameter(":CropContent",model.CropContent),
                new OracleParameter(":CropType",model.CropType),
                new OracleParameter(":DeleteMark",model.DeleteMark),
                new OracleParameter(":CreateUserName",model.CreateUserName),
                new OracleParameter(":CreateDate",model.CreateDate),
                new OracleParameter(":Remarks",model.Remarks)
                                     };
            StringBuilder sb = new StringBuilder();
            sb.Append("update A_Crop set ");
            sb.Append("CropName=:CropName,");
            sb.Append("CropContent=:CropContent,");
            sb.Append("CropType=:CropType,");
            sb.Append("DeleteMark=:DeleteMark,");
            sb.Append("CreateUserName=:CreateUserName,");
            sb.Append("CreateDate=:CreateDate,");
            sb.Append("Remarks=:Remarks");
            sb.Append(" where Id=:Id");
            return OracleHelper.ExecuteNonQuery(OracleHelper.Conn, CommandType.Text, sb.ToString(), param);
        }
    }
}
