using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Collections;
using System.IO;

namespace Common
{
    public abstract class OracleHelper
    {

        // Read the connection strings from the configuration file
        public static readonly string Conn = ConfigHelper.GetAppSettings("Conn");
        //Create a hashtable for the parameter cached
        private static readonly Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        ///     Execute a database query which does not include a select
        /// </summary>
        /// <param name="connString">Connection string to database</param>
        /// <param name="cmdType">Command type either stored procedure or SQL</param>
        /// <param name="cmdText">Acutall SQL Command</param>
        /// <param name="commandParameters">Parameters to bind to the command</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText,
            params OracleParameter[] commandParameters)
        {
            // Create a new Oracle command
            var cmd = new OracleCommand();

            //Create a connection
            using (var connection = new OracleConnection(connectionString))
            {
                //Prepare the command
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);

                //Execute the command
                var val = cmd.ExecuteNonQuery();
                //connection.Close();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static DataSet Query(string connectionString, string SQLString, params OracleParameter[] cmdParms)
        {
            using (var connection = new OracleConnection(connectionString))
            {
                var cmd = new OracleCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (var da = new OracleDataAdapter(cmd))
                {
                    var ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (OracleException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    //finally
                    //{
                    //    if (connection.State != ConnectionState.Closed)
                    //    {
                    //        connection.Close();
                    //    }
                    //}
                    return ds;
                }
            }
        }

        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans,
            string cmdText, OracleParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text; //cmdType;
            if (cmdParms != null)
            {
                foreach (var parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput ||
                         parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        ///     执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string connectionString, string SQLString)
        {
            using (var connection = new OracleConnection(connectionString))
            {
                using (var cmd = new OracleCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        var obj = cmd.ExecuteScalar();
                        if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (OracleException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    //finally
                    //{
                    //    if (connection.State != ConnectionState.Closed)
                    //    {
                    //        connection.Close();
                    //    }
                    //}
                }
            }
        }

        public static bool Exists(string connectionString, string strOracle)
        {
            var obj = GetSingle(connectionString, strOracle);
            int cmdresult;
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns no resultset) against an existing database transaction
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid",
        ///     24));
        /// </remarks>
        /// <param name="trans">an existing database transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(OracleTransaction trans, CommandType cmdType, string cmdText,
            params OracleParameter[] commandParameters)
        {
            var cmd = new OracleCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            var val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns no resultset) against an existing database connection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new
        ///     OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(OracleConnection connection, CommandType cmdType, string cmdText,
            params OracleParameter[] commandParameters)
        {
            var cmd = new OracleCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            var val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        ///     Execute an OracleCommand (that returns no resultset) against an existing database connection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new
        ///     OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, string cmdText)
        {
            var cmd = new OracleCommand();
            var connection = new OracleConnection(connectionString);
            PrepareCommand(cmd, connection, null, CommandType.Text, cmdText, null);
            var val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        ///     Execute a select query that will return a result set
        /// </summary>
        /// <param name="connString">Connection string</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns></returns>
        public static OracleDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText,
            params OracleParameter[] commandParameters)
        {
            var cmd = new OracleCommand();
            var conn = new OracleConnection(connectionString);
            try
            {
                //Prepare the command to execute
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        ///     Execute an OracleCommand that returns the first column of the first record against the database specified in the
        ///     connection string
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid",
        ///     24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText,
            params OracleParameter[] commandParameters)
        {
            var cmd = new OracleCommand();

            using (var conn = new OracleConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                var val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        ///     Execute	a OracleCommand (that returns a 1x1 resultset)	against	the	specified SqlTransaction
        ///     using the provided parameters.
        /// </summary>
        /// <param name="transaction">A	valid SqlTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name	or PL/SQL command</param>
        /// <param name="commandParameters">An array of	OracleParamters used to execute the command</param>
        /// <returns>An	object containing the value	in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText,
            params OracleParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked	or commited, please	provide	an open	transaction.", "transaction");

            // Create a	command	and	prepare	it for execution
            var cmd = new OracleCommand();

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            // Execute the command & return	the	results
            var retval = cmd.ExecuteScalar();

            // Detach the SqlParameters	from the command object, so	they can be	used again
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        ///     Execute an OracleCommand that returns the first column of the first record against an existing database connection
        ///     using the provided parameters.
        /// </summary>
        /// <remarks>
        ///     e.g.:
        ///     Object obj = ExecuteScalar(conn, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(OracleConnection connectionString, CommandType cmdType, string cmdText,
            params OracleParameter[] commandParameters)
        {

            var cmd = new OracleCommand();


            PrepareCommand(cmd, connectionString, null, cmdType, cmdText, commandParameters);
            var val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        ///     Add a set of parameters to the cached
        /// </summary>
        /// <param name="cacheKey">Key value to look up the parameters</param>
        /// <param name="commandParameters">Actual parameters to cached</param>
        public static void CacheParameters(string cacheKey, params OracleParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        ///     Fetch parameters from the cache
        /// </summary>
        /// <param name="cacheKey">Key to look up the parameters</param>
        /// <returns></returns>
        public static OracleParameter[] GetCachedParameters(string cacheKey)
        {
            var cachedParms = (OracleParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            // If the parameters are in the cache
            var clonedParms = new OracleParameter[cachedParms.Length];

            // return a copy of the parameters
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (OracleParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        ///     Internal function to prepare a command for execution by the database
        /// </summary>
        /// <param name="cmd">Existing command object</param>
        /// <param name="conn">Database connection object</param>
        /// <param name="trans">Optional transaction object</param>
        /// <param name="cmdType">Command type, e.g. stored procedure</param>
        /// <param name="cmdText">Command test</param>
        /// <param name="commandParameters">Parameters for the command</param>
        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans,
            CommandType cmdType, string cmdText, OracleParameter[] commandParameters)
        {
            //Open the connection if required
            if (conn.State != ConnectionState.Open)
                conn.Open();

            //Set up the command
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            //Bind it to the transaction if it exists
            if (trans != null)
                cmd.Transaction = trans;

            // Bind the parameters passed in
            if (commandParameters != null)
            {
                foreach (var parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        ///     Converter to use boolean data type with Oracle
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string OraBit(bool value)
        {
            if (value)
                return "Y";
            return "N";
        }

        /// <summary>
        ///     Converter to use boolean data type with Oracle
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static bool OraBool(string value)
        {
            if (value.Equals("Y"))
                return true;
            return false;
        }

        ///// <summary>
        ///// 执行多条SQL语句，实现数据库事务。
        ///// </summary>
        ///// <param name="SQLStringList">多条SQL语句</param>		
        //public static bool ExecuteSqlTran(string conStr, List<CommandInfo> cmdList)
        //{
        //    using (OracleConnection conn = new OracleConnection(conStr))
        //    {
        //        conn.Open();
        //        OracleCommand cmd = new OracleCommand();
        //        cmd.Connection = conn;
        //        OracleTransaction tx = conn.BeginTransaction();
        //        cmd.Transaction = tx;
        //        try
        //        {
        //            foreach (CommandInfo c in cmdList)
        //            {
        //                if (!String.IsNullOrEmpty(c.CommandText))
        //                {
        //                    PrepareCommand(cmd, conn, tx, CommandType.Text, c.CommandText, (OracleParameter[])c.Parameters);
        //                    if (c.EffentNextType == EffentNextType.WhenHaveContine || c.EffentNextType == EffentNextType.WhenNoHaveContine)
        //                    {
        //                        if (c.CommandText.ToLower().IndexOf("count(") == -1)
        //                        {
        //                            tx.Rollback();
        //                            throw new Exception("Oracle:违背要求" + c.CommandText + "必须符合select count(..的格式");
        //                            //return false;
        //                        }

        //                        object obj = cmd.ExecuteScalar();
        //                        bool isHave = false;
        //                        if (obj == null && obj == DBNull.Value)
        //                        {
        //                            isHave = false;
        //                        }
        //                        isHave = Convert.ToInt32(obj) > 0;

        //                        if (c.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
        //                        {
        //                            tx.Rollback();
        //                            throw new Exception("Oracle:违背要求" + c.CommandText + "返回值必须大于0");
        //                            //return false;
        //                        }
        //                        if (c.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
        //                        {
        //                            tx.Rollback();
        //                            throw new Exception("Oracle:违背要求" + c.CommandText + "返回值必须等于0");
        //                            //eturn false;
        //                        }
        //                        continue;
        //                    }
        //                    int res = cmd.ExecuteNonQuery();
        //                    if (c.EffentNextType == EffentNextType.ExcuteEffectRows && res == 0)
        //                    {
        //                        tx.Rollback();
        //                        throw new Exception("Oracle:违背要求" + c.CommandText + "必须有影像行");
        //                        // return false;
        //                    }
        //                }
        //            }
        //            tx.Commit();
        //            return true;
        //        }
        //        catch (System.Data.OracleClient.OracleException E)
        //        {
        //            tx.Rollback();
        //            throw E;
        //        }
        //        finally
        //        {
        //            if (conn.State != ConnectionState.Closed)
        //            {
        //                conn.Close();
        //            }
        //        }
        //    }
        //}       
        /// <summary>
        ///     执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>
        public static int ExecuteSqlTran(string conStr, List<string> SQLStringList)
        {
            using (var conn = new OracleConnection(conStr))
            {
                int num = 0;
                conn.Open();
                var cmd = new OracleCommand();
                cmd.Connection = conn;
                var tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (var sql in SQLStringList)
                    {
                        if (!string.IsNullOrEmpty(sql))
                        {
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                            num++;
                        }
                    }
                    tx.Commit();
                }
                catch (OracleException E)
                {
                    tx.Rollback();
                    num = -1;
                    throw new Exception(E.Message);
                }
                //finally
                //{
                //    if (conn.State != ConnectionState.Closed)
                //    {
                //        conn.Close();
                //    }
                //}
                return num;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string conStr, string SQLString)
        {
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OracleDataAdapter command = new OracleDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.OracleClient.OracleException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        /// <summary>
        /// 读取大字段数据
        /// </summary>
        /// <param name="idData"></param>
        /// <param name="fileName"></param>
        /// <param name="id"></param>
        /// <param name="blob"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string ReadBlob(string idData, string fileName, string id, string blob, string tableName)
        {
            int actual = 0;
            string str = "";
            using (var conn = new OracleConnection(OracleHelper.Conn))
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                string strSql = "select " + blob + " from " + tableName + "  where  " + id + "= '" + idData + "'";
                cmd.CommandText = strSql;
                cmd.CommandType = CommandType.Text;
                try
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        OracleLob myOracleClob = reader.GetOracleLob(0);
                        using (StreamReader streamreader = new StreamReader(myOracleClob, Encoding.Unicode))
                        {
                            char[] cbuffer = new char[1048576];
                            while ((actual = streamreader.Read(cbuffer, 0, cbuffer.Length)) > 0)
                            {
                                str += new string(cbuffer, 0, actual);
                            }
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    // 释放占有资源
                    conn.Close();
                }
            }
            return str;
        }

    }
}
