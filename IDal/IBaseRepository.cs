using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IDal
{
    public interface IBaseRepository<T> where T : class,new()
    {
        bool Exists(object id);
        bool Exists(string field, object value);
        bool Exists(string tableName, string pkName, string pkValue);
        int Count();
        int CountSql(string sql);
        int Count(string whereStr);
        int? MaxId();
        int? Max(string field);
        int? Max(string field, string strWhere);
        T Get(object id);
        T Get(string field, object value);
        T Get(string strWhere);
        object GetFieldValue(string field, object value);
        object GetFieldValueByView(string field, object value, string tablename);
        string Add(T obj);
        int Add(List<T> obj);
        int Update(T obj, string whereStr);
        int Update<T>(string field, object value, string whereStr);
        int Delete(object id);
        int Delete(string field, object value);
        int Delete(string tableName, string pkName, string pkValue);
        int Delete(T obj);
        IList<T> List();
        IList<T> List(string whereStr);
        IList<T> List(string sqlStr, string whereStr);
        IList<T> ListByPage(int pageSize, int pageNo);
        IList<T> ListByPage(int pageSize, int pageNo, string whereStr);
        IList<T> ListByPage(int pageSize, int pageNo, string whereStr, string orderStr);
        IDataReader ListByPage(int pageSize, int pageNo, string whereStr, string orderStr, string sql);
        DataSet Query(string sqlStr);
        int ExecuteSqlTran(List<string> list);
        DataTable DataTableByPage(int pageSize, int pageNo, string sql, string whereStr, ref int count, string orderStr);
        int ExecuteNonQuery(string strSql);
        List<String> GetFields();
        object GetSingle(string strSql);
        DataTable GetDataTable(string sql, string whereStr, string orderStr);
    }
}
