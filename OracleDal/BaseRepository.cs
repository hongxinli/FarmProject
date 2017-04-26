using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDal;
using Common;
using System.Data;
using System.Reflection;

namespace OracleDal
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class,new()
    {

        /// <summary>
        ///     对象是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(object id)
        {
            return Exists("id", id);
        }

        /// <summary>
        ///     通过字段查询对象是否存在
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Exists(string field, object value)
        {
            var sqlStr = "select 1 from " + GetTableName<T>() + " where " + field + "='" + value + "'";
            return OracleHelper.Exists(OracleHelper.Conn, sqlStr);
        }
        /// <summary>
        /// 查询对象是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pkName"></param>
        /// <param name="pkValue"></param>
        /// <returns></returns>
        public bool Exists(string tableName, string pkName, string pkValue)
        {
            var sqlStr = "select 1 from " + tableName + " where " + pkName + "='" + pkValue + "'";
            return OracleHelper.Exists(OracleHelper.Conn, sqlStr);
        }
        /// <summary>
        ///     总记录数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Count(null);
        }

        public int Count(string whereStr)
        {
            var sqlStr = "select count(1) from " + GetTableName<T>() + " where " + whereStr;
            var count = OracleHelper.ExecuteScalar(OracleHelper.Conn, CommandType.Text, sqlStr);
            return int.Parse(count.ToString());
        }
        public int CountSql(string sql)
        {
            var sqlStr = "select count(1) from(" + sql + ")";
            var count = OracleHelper.ExecuteScalar(OracleHelper.Conn, CommandType.Text, sqlStr);
            return int.Parse(count.ToString());
        }
        /// <summary>
        ///     获取ID最大值
        /// </summary>
        /// <returns></returns>
        public int? MaxId()
        {
            return Max("id");
        }

        /// <summary>
        ///     获取字段的最大值
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public int? Max(string field)
        {
            var sqlStr = "select max(" + field + ") from " + GetTableName<T>();
            var maxValue = OracleHelper.ExecuteScalar(OracleHelper.Conn, CommandType.Text, sqlStr);
            if (DBNull.Value == maxValue)
            {
                return null;
            }
            return (int)maxValue;
        }
        /// <summary>
        ///     获取字段的最大值
        /// </summary>
        /// <param name="field"></param>
        /// /// <param name="strWhere"></param>
        /// <returns></returns>
        public int? Max(string field, string strWhere)
        {
            var sqlStr = "select max(" + field + ") from " + GetTableName<T>() + " where " + strWhere;
            var maxValue = OracleHelper.ExecuteScalar(OracleHelper.Conn, CommandType.Text, sqlStr);
            if (DBNull.Value == maxValue)
            {
                return null;
            }
            return int.Parse(maxValue.ToString());
        }
        /// <summary>
        ///     通过ID获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(object id)
        {
            return Get("id", id);
        }

        /// <summary>
        ///     通过指定字段条件获取对象
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值，不能是日期、blob等类型</param>
        /// <returns></returns>
        public T Get(string field, object value)
        {
            var a = value.GetType().Name;
            var sqlStr = "select " + GetTableFields() + " from " + GetTableName<T>() + " where " + field + "='" + value +
                         "'";
            var list = FillList<T>(OracleHelper.ExecuteReader(OracleHelper.Conn, CommandType.Text, sqlStr));
            if (list.Count > 0)
            {
                return list[0];
            }
            return default(T);
        }
        /// <summary>
        /// 通过指定条件获取对象
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public T Get(string strWhere)
        {
            var sqlStr = "select " + GetTableFields() + " from " + GetTableName<T>() + " where " + strWhere;
            var list = FillList<T>(OracleHelper.ExecuteReader(OracleHelper.Conn, CommandType.Text, sqlStr));
            if (list.Count > 0)
            {
                return list[0];
            }
            return default(T);
        }
        /// <summary>
        ///     通过指定字段条件获取第一条记录的指定字段值
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="where">where 条件 不带where </param>
        /// <returns></returns>
        public object GetFieldValue(string field, object where)
        {
            var sqlStr = "select " + field + " from " + GetTableName<T>() + " where " + where + "";
            return OracleHelper.ExecuteScalar(OracleHelper.Conn, CommandType.Text, sqlStr);
        }
        /// <summary>
        /// 根据表名获取字段值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public object GetFieldValueByView(string field, object where, string tablename)
        {
            var sqlStr = "select " + field + " from " + tablename + " where " + where + "";
            return OracleHelper.ExecuteScalar(OracleHelper.Conn, CommandType.Text, sqlStr);
        }
        /// <summary>
        ///     添加对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Add(T obj)
        {
            var id = Guid.NewGuid().ToString();
            var infos = GetPropertyInfos<T>();
            var insertStr = "insert into " + GetTableName<T>() + " ";
            var fields = "";
            var values = "";

            foreach (var info in infos)
            {
                if (info.Name.ToUpper().Equals("ID"))
                {
                    #region 程序生成id  Oracle数据库用

                    fields += info.Name;
                    values += "'" + info.GetValue(obj, null) + "'";
                    fields += ",";
                    values += ",";

                    #endregion

                    continue;
                }
                fields += info.Name;

                if (info.PropertyType.Name.ToUpper() == "DATETIME")
                {
                    //Oracle 日期格式方式
                    values += "to_date('" + info.GetValue(obj, null) + "','yyyy-mm-dd hh24:mi:ss')";
                    //Sql server日期格式处理
                    //values += "'" + info.GetValue(obj) + "'";
                }
                else if (info.PropertyType.Name.ToUpper() == "BOOLEAN")
                {
                    values += "'" + Convert.ToInt16(info.GetValue(obj, null)) + "'";
                }
                else if (info.PropertyType.Name.ToUpper() == "NULLABLE`1")
                {
                    string fullName = info.PropertyType.FullName.ToUpper();
                    if (fullName.Contains("BOOLEAN"))
                    {
                        if (null == info.GetValue(obj, null))
                        {
                            values += "'0'";
                        }
                        else
                        {
                            values += "'" + Convert.ToInt16(info.GetValue(obj, null)) + "'";
                        }

                    }
                    else if (fullName.Contains("INT"))
                    {
                        if (null == info.GetValue(obj, null))
                        {
                            values += "'0'";
                        }
                        else
                        {
                            values += "'" + info.GetValue(obj, null) + "'";
                        }
                    }
                    else if (fullName.Contains("DOUBLE"))
                    {
                        if (null == info.GetValue(obj, null))
                        {
                            values += "'0'";
                        }
                        else
                        {
                            values += "'" + info.GetValue(obj, null) + "'";
                        }
                    }
                }
                else
                {
                    values += "'" + info.GetValue(obj, null) + "'";
                }
                fields += ",";
                values += ",";
            }
            fields = StringHelper.Trim(fields);
            values = StringHelper.Trim(values);
            insertStr += "(" + fields + ") values (" + values + ")";
            try
            {
                object count = OracleHelper.ExecuteNonQuery(OracleHelper.Conn, CommandType.Text, insertStr);
                if ((int)count == 1)
                {
                    return id;
                }
            }
            catch (Exception)
            {

            }

            return null;
        }
        /// <summary>
        /// 插入实例类集合
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Add(List<T> obj)
        {
            List<string> sqlList = new List<string>();
            for (int i = 0; i < obj.Count; i++)
            {
                var infos = GetPropertyInfos<T>();
                var insertStr = "insert into " + GetTableName<T>() + " ";
                var fields = "";
                var values = "";
                foreach (var info in infos)
                {
                    fields += info.Name;
                    values += "'" + info.GetValue(obj[i], null) + "'";
                    fields += ",";
                    values += ",";
                }
                fields = StringHelper.Trim(fields);
                values = StringHelper.Trim(values);
                insertStr += "(" + fields + ") values (" + values + ")";
                sqlList.Add(insertStr);
            }
            return OracleHelper.ExecuteSqlTran(OracleHelper.Conn, sqlList);
        }
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="whereStr"> where key='value'</param>
        /// <returns></returns>
        public int Update(T obj, string whereStr)
        {
            var infos = GetPropertyInfos<T>();
            var updateStr = "update " + GetTableName<T>() + " set ";
            var setStr = "";
            foreach (var info in infos)
            {
                //if (info.Name.ToUpper().Equals("ID"))
                //{
                //    whereStr = " where id='" + info.GetValue(obj, null) + "'";
                //    continue;
                //}
                if (info.PropertyType.Name.ToUpper() == "DATETIME")
                {
                    var dt = (DateTime)info.GetValue(obj, null);
                    var dtValidate = new DateTime(2000, 1, 1);
                    if (dt < dtValidate)
                    {
                        //小于2000年的无效时间
                        continue;
                    }
                    //Oracle 日期格式方式
                    setStr += info.Name + "= to_date('" + info.GetValue(obj, null) + "','yyyy-mm-dd hh24:mi:ss')";
                    //Sql server日期格式处理
                    //setStr += info.Name + "='" + info.GetValue(obj) + "'";
                }
                else if (info.PropertyType.Name.ToUpper() == "BOOLEAN")
                {
                    var b = (bool)info.GetValue(obj, null);
                    if (!b)
                    {
                        continue;
                    }
                    setStr += info.Name + "='" + Convert.ToInt16(info.GetValue(obj, null)) + "'";
                }
                else if (info.PropertyType.Name.ToUpper() == "NULLABLE`1")
                {
                    string fullName = info.PropertyType.FullName.ToUpper();
                    if (fullName.Contains("BOOLEAN"))
                    {
                        if (null == info.GetValue(obj, null))
                        {
                            continue;
                        }
                        else
                        {
                            setStr += info.Name + "='" + Convert.ToInt16(info.GetValue(obj, null)) + "'";
                        }

                    }
                    else if (fullName.Contains("INT"))
                    {
                        if (null == info.GetValue(obj, null))
                        {
                            continue;
                        }
                        else
                        {
                            setStr += info.Name + "='" + info.GetValue(obj, null) + "'";
                        }
                    }
                    else if (fullName.Contains("DOUBLE"))
                    {
                        if (null == info.GetValue(obj, null))
                        {
                            continue;
                        }
                        else
                        {
                            setStr += info.Name + "='" + info.GetValue(obj, null) + "'";
                        }
                    }
                }
                else
                {
                    if (null == info.GetValue(obj, null))
                    {
                        continue;
                    }
                    setStr += info.Name + "='" + info.GetValue(obj, null) + "'";
                }
                setStr += ",";
            }
            setStr = StringHelper.Trim(setStr);
            updateStr += setStr + " where " + whereStr;
            object count = OracleHelper.ExecuteNonQuery(OracleHelper.Conn, CommandType.Text, updateStr);
            return (int)count;
        }

        public int Update<T>(string field, object value, string whereStr)
        {
            string updateStr = "update " + GetTableName<T>() + " set " + field + "='" + value + "' where " + whereStr;
            object count = OracleHelper.ExecuteNonQuery(OracleHelper.Conn, CommandType.Text, updateStr);
            return (int)count;
        }

        /// <summary>
        ///     根据ID删除对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(object id)
        {
            return Delete("id", id);
        }

        /// <summary>
        ///     根据字段删除对象
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Delete(string field, object value)
        {
            var sqlStr = "delete from " + GetTableName<T>() + " where " + field + "='" + value + "'";
            object count = OracleHelper.ExecuteNonQuery(OracleHelper.Conn, CommandType.Text, sqlStr);
            return (int)count;
        }
        /// <summary>
        /// 根据字段删除对象
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pkName"></param>
        /// <param name="pkValue"></param>
        /// <returns></returns>
        public int Delete(string tableName, string pkName, string pkValue)
        {
            var sqlStr = "delete from " + tableName + " where " + pkName + "='" + pkValue + "'";
            object count = OracleHelper.ExecuteNonQuery(OracleHelper.Conn, CommandType.Text, sqlStr);
            return (int)count;
        }
        /// <summary>
        ///     删除对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Delete(T obj)
        {
            var id = obj.GetType().InvokeMember("Id",
                BindingFlags.GetProperty, null, obj, null) as string;
            if (null != id)
            {
                return Delete(id);
            }
            //Id值无法获取返回错误-1
            return -1;
        }

        /// <summary>
        ///     所有数据列表
        /// </summary>
        /// <returns></returns>
        public IList<T> List()
        {
            return List(null);
        }

        /// <summary>
        ///     所有数据列表
        /// </summary>
        /// <param name="whereStr">where ....</param>
        /// <returns></returns>
        public IList<T> List(string whereStr)
        {
            var sqlStr = "select " + GetTableFields() + " from " + GetTableName<T>();
            if (!string.IsNullOrEmpty(whereStr))
                sqlStr = sqlStr + " where " + whereStr;
            return FillList<T>(OracleHelper.ExecuteReader(OracleHelper.Conn, CommandType.Text, sqlStr));
        }

        public IList<T> List(string sqlStr, string whereStr)
        {
            if (!string.IsNullOrEmpty(whereStr))
                sqlStr = sqlStr + " where " + whereStr;
            return FillList<T>(OracleHelper.ExecuteReader(OracleHelper.Conn, CommandType.Text, sqlStr));
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public DataSet Query(string sqlStr)
        {
            return OracleHelper.Query(OracleHelper.Conn, sqlStr);
        }
        /// <summary>
        ///     分页获取数据
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNo">页号</param>
        /// <returns></returns>
        public IList<T> ListByPage(int pageSize, int pageNo)
        {
            return ListByPage(pageSize, pageNo, null, null);
        }

        /// <summary>
        ///     分页获取数据
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNo">页号</param>
        /// <param name="whereStr">条件</param>
        /// <returns></returns>
        public IList<T> ListByPage(int pageSize, int pageNo, string whereStr)
        {
            return ListByPage(pageSize, pageNo, whereStr, null);
        }

        /// <summary>
        ///     分页获取数据，视图可能没有ID字段
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNo">页号</param>
        /// <param name="whereStr">条件（不需要加where）</param>
        /// <param name="orderStr">排序字段（中间有逗号和空格分隔），如id desc,name asc</param>
        /// <returns></returns>
        public IList<T> ListByPage(int pageSize, int pageNo, string whereStr, string orderStr)
        {
            if (string.IsNullOrWhiteSpace(whereStr))
            {
                whereStr = " 1=1 ";
            }

            if (string.IsNullOrWhiteSpace(orderStr))
            {
                orderStr = "";
            }
            else
            {
                orderStr = " order by " + orderStr;
            }
            var sqlStr = string.Format(@"select * from (
            select A.*,rownum rn from (SELECT * FROM ({0}) where ({4})  {3})  A 
             )
            where rn>{2} and rn<={1}",
                GetTableName<T>(), pageSize * pageNo, pageSize * (pageNo - 1), orderStr, whereStr);
            return FillList<T>(OracleHelper.ExecuteReader(OracleHelper.Conn, CommandType.Text, sqlStr));
        }
        public DataTable DataTableByPage(int pageSize, int pageNo, string sql, string whereStr, ref int count, string orderStr)
        {
            if (string.IsNullOrWhiteSpace(whereStr))
            {
                whereStr = " 1=1 ";
            }

            if (string.IsNullOrWhiteSpace(orderStr))
            {
                orderStr = "";
            }
            else
            {
                orderStr = " order by " + orderStr;
            }
            var sqlStr = string.Format(@"select * from (
            select A.*,rownum rn from (SELECT * FROM ({0}) where ({4})  {3})  A 
             )
            where rn>{2} and rn<={1}",
                sql, pageSize * pageNo, pageSize * (pageNo - 1), orderStr, whereStr);
            DataSet ds = OracleHelper.Query(OracleHelper.Conn, sqlStr);
            DataTable dt = ds.Tables[0];
            count = int.Parse(OracleHelper.ExecuteScalar(OracleHelper.Conn, CommandType.Text, "select count(1) from (" + sql + ") where (" + whereStr + ")").ToString());
            return dt;
        }
        /// <summary>
        ///     分页获取数据，视图可能没有ID字段
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNo">页号</param>
        /// <param name="whereStr">条件（不需要加where）</param>
        /// <param name="orderStr">排序字段（中间有逗号和空格分隔），如id desc,name asc</param>
        /// <param name="sql">查询条件</param>
        /// <returns></returns>
        public IDataReader ListByPage(int pageSize, int pageNo, string whereStr, string orderStr, string sql)
        {
            if (string.IsNullOrWhiteSpace(whereStr))
            {
                whereStr = " 1=1 ";
            }

            if (string.IsNullOrWhiteSpace(orderStr))
            {
                orderStr = "";
            }
            else
            {
                orderStr = " order by " + orderStr;
            }
            var sqlStr = string.Format(@"select * from (
            select A.*,rownum rn from (SELECT * FROM ({0}) where ({4})  {3})  A 
             )
            where rn>{2} and rn<={1}",
                sql, pageSize * pageNo, pageSize * (pageNo - 1), orderStr, whereStr);
            IDataReader dr = (IDataReader)OracleHelper.ExecuteReader(OracleHelper.Conn, CommandType.Text, sqlStr);
            return dr;
        }
        public IDataReader List(string whereStr, string orderStr, string sql)
        {
            if (string.IsNullOrWhiteSpace(whereStr))
            {
                whereStr = " 1=1 ";
            }

            if (string.IsNullOrWhiteSpace(orderStr))
            {
                orderStr = "";
            }
            else
            {
                orderStr = " order by " + orderStr;
            }
            var sqlStr = string.Format(@"SELECT * FROM ({0}) where {1}  {2} ",
                sql, whereStr, orderStr);
            IDataReader dr = (IDataReader)OracleHelper.ExecuteReader(OracleHelper.Conn, CommandType.Text, sqlStr);
            return dr;
        }
        /// <summary>
        ///     获取属性信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private PropertyInfo[] GetPropertyInfos<T>()
        {
            return typeof(T).GetProperties();
        }

        /// <summary>
        ///     获取表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private string GetTableName<T>()
        {
            return typeof(T).Name;
        }

        /// <summary>
        ///     获取表字段，用逗号分隔
        /// </summary>
        /// <returns></returns>
        private string GetTableFields()
        {
            var infos = GetPropertyInfos<T>();
            var fields = "";
            foreach (var info in infos)
            {
                fields += info.Name;
                fields += ",";
            }
            fields = StringHelper.Trim(fields);
            return fields;
        }
        /// <summary>
        ///     获取表字段
        /// </summary>
        /// <returns></returns>
        public List<String> GetFields()
        {
            var infos = GetPropertyInfos<T>();
            List<String> list = new List<string>();
            foreach (var info in infos)
            {
                list.Add(info.Name);
            }
            return list;
        }
        /// <summary>
        ///     填充 数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        private IList<T> FillList<T>(IDataReader reader)
        {
            IList<T> lst = new List<T>();

            while (reader.Read())
            {
                var RowInstance = Activator.CreateInstance<T>();

                foreach (var Property in typeof(T).GetProperties())
                {
                    try
                    {
                        var Ordinal = reader.GetOrdinal(Property.Name.ToUpper());
                        object v = reader.GetValue(Ordinal);
                        if (reader.GetValue(Ordinal) != DBNull.Value)
                        {
                            if (Property.PropertyType.Name.ToUpper() == "NULLABLE`1")
                            {
                                string fullName = Property.PropertyType.FullName.ToUpper();
                                if (fullName.Contains("BOOLEAN"))
                                {
                                    if (v.ToString().Equals("1"))
                                    {
                                        Property.SetValue(RowInstance,
                                            true, null);
                                    }
                                    else
                                    {
                                        Property.SetValue(RowInstance,
                                            false, null);
                                    }
                                }
                                else if (fullName.Contains("INT32"))
                                {
                                    Property.SetValue(RowInstance,
                                        v.ToString().ConvertTo<int?>(), null);
                                }
                                else if (fullName.Contains("INT64"))
                                {
                                    Property.SetValue(RowInstance,
                                        v.ToString().ConvertTo<Int64?>(), null);
                                }
                                else if (fullName.Contains("DOUBLE"))
                                {
                                    Property.SetValue(RowInstance,
                                        v.ToString().ConvertTo<double?>(), null);
                                }

                            }
                            else
                            {
                                Property.SetValue(RowInstance,
                                Convert.ChangeType(reader.GetValue(Ordinal),
                                    Property.PropertyType), null);
                            }


                        }
                    }
                    catch
                    {
                        break;
                    }
                }
                lst.Add(RowInstance);
            }
            reader.Close();
            return lst;
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int ExecuteSqlTran(List<string> list)
        {
            return OracleHelper.ExecuteSqlTran(OracleHelper.Conn, list);
        }

        public int ExecuteNonQuery(string strSql)
        {
            return OracleHelper.ExecuteNonQuery(OracleHelper.Conn, strSql);
        }

        public object GetSingle(string strSql)
        {
            return OracleHelper.GetSingle(OracleHelper.Conn, strSql);
        }
        /// <summary>
        /// 获取下拉需要的Datetable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="whereStr"></param>
        /// <param name="orderStr"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, string whereStr, string orderStr)
        {
            if (string.IsNullOrWhiteSpace(whereStr))
            {
                whereStr = " 1=1 ";
            }

            if (string.IsNullOrWhiteSpace(orderStr))
            {
                orderStr = "";
            }
            else
            {
                orderStr = " order by " + orderStr;
            }
            var sqlStr = string.Format(@"select * from (
            select A.*,rownum rn from (SELECT * FROM ({0}) where ({2})  {1})  A 
             ) ",
                sql, orderStr, whereStr);
            DataSet ds = OracleHelper.Query(OracleHelper.Conn, sqlStr);
            DataTable dt = ds.Tables[0];
            return dt;
        }
    }
}
