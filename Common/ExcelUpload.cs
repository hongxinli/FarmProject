using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web; 
namespace Common
{
    public class ExcelUpload  
    {
        public DataTable ReadExcelToDataTable( string excelSavePath,string sheetName)
        {
            string oleDBString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + @excelSavePath + "; Extended Properties=\"Excel 8.0; HDR=YES; IMEX=1;\"";
            OleDbConnection conn = new OleDbConnection(oleDBString);
            conn.Open();
            OleDbCommand odCommand = new OleDbCommand("select * from ["+sheetName+"$]", conn);
            OleDbDataReader odrReader = odCommand.ExecuteReader();
            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = odCommand;
            conn.Close();
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            return dt;
        }
    }
}
