using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyOrm
{
    /// <summary>
    /// DBHelper对SQL数据库进行操作
    /// </summary>
    /// DBHelper 定义成单例类
    public class DBHelper : IDBHelper
    {
        private DBHelper()
        {
        }

        private static DBHelper _instance = null;

        public static DBHelper GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DBHelper();
            }
            return _instance;
        }

        #region 获取数据库连接的地址

        private string _ConnStr;

        /// <summary>
        /// 在Web.config中设定一条connectionStrings
        /// </summary>
        public string ConnString
        {
            set { _ConnStr = value; }
        }

        #endregion 获取数据库连接的地址

        #region 对数据库进行 增、删、改 操作

        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, CommandType.Text, null);
        }

        public int ExecuteNonQuery(string sql, SqlParameter[] sqlParams)
        {
            return ExecuteNonQuery(sql, CommandType.Text, sqlParams);
        }

        /// <summary>
        /// ExecuteNonQuery操作，对数据库进行 增、删、改 操作
        /// <para>返回受影响的行数，返回-1表示数据库操作失败</para>
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组 </param>
        /// <returns>返回受影响的行数，返回-1表示数据库操作失败</returns>
        public int ExecuteNonQuery(string sql, CommandType commandType, SqlParameter[] sqlParams)
        {
            int count = 0;
            using (SqlConnection sqlConn = new SqlConnection(_ConnStr))
            {
                using (SqlCommand sqlComm = new SqlCommand(sql, sqlConn))
                {
                    sqlComm.CommandType = commandType;
                    if (sqlParams != null)
                    {
                        foreach (SqlParameter sqlParam in sqlParams)
                        {
                            sqlComm.Parameters.Add(sqlParam);
                        }
                    }

                    try
                    {
                        sqlConn.Open();
                        count = sqlComm.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return count;
        }

        #endregion 对数据库进行 增、删、改 操作

        #region 查询并返回一个DataSet类型结果

        public DataSet ExecuteDataSet(string sql)
        {
            return ExecuteDataSet(sql, CommandType.Text, null);
        }

        public DataSet ExecuteDataSet(string sql, SqlParameter[] parameters)
        {
            return ExecuteDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataSet类型结果
        /// <para>返回一个DataSet对象，如果无法连接数据库或查询的结果为空，则返回null</para>
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组 </param>
        /// <returns>返回一个DataSet对象，如果无法连接数据库或查询的结果为空，则返回null</returns>
        public DataSet ExecuteDataSet(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(_ConnStr))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = 120;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(ds);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return ds;
        }

        #endregion 查询并返回一个DataSet类型结果

        #region 查询并返回一个DataTabel类型结果

        public DataTable ExecuteDataTable(string sql)
        {
            return ExecuteDataTable(sql, CommandType.Text, null);
        }

        public DataTable ExecuteDataTable(string sql, SqlParameter[] parameters)
        {
            return ExecuteDataTable(sql, CommandType.Text, parameters);
        }

        public DataTable ExecuteDataTable(string sql, CommandType commandType)
        {
            return ExecuteDataTable(sql, commandType, null);
        }

        /// <summary>
        /// SqlDataAdapter的Fill方法执行一个查询，并返回一个DataTable类型结果
        /// <para>返回一个DataTabel对象，如果无法连接数据库或查询的结果为空，则返回null</para>
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组 </param>
        /// <returns>返回一个DataTabel对象，如果无法连接数据库或查询的结果为空，则返回null</returns>
        public DataTable ExecuteDataTable(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_ConnStr))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dt);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return dt;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="sqlCount">查询满足条件的总行数SQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="totalCount">传递总行数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, string sqlCount, SqlParameter[] parameters, ref int totalCount)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(_ConnStr))
            {
                using (SqlCommand command = new SqlCommand(sql + ";" + sqlCount, connection))
                {
                    command.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(ds);

                        if (ds.Tables.Count == 2 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            totalCount = (int)ds.Tables[1].Rows[0][0];
                        }
                        else
                        {
                            totalCount = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                throw new Exception("Failed to query to meet the conditions of the collection!");
            }
        }

        #endregion 查询并返回一个DataTabel类型结果

        #region 查询并返回一个SqlDataReader对象实例

        /// <summary>
        /// ExecuteReader执行一查询，返回一SqlDataReader对象实例
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <param name="parameters">参数数组 </param>
        /// <returns>返回一SqlDataReader对象实例</returns>
        public SqlDataReader ExecuteReader(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(_ConnStr))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return dataReader;
        }

        #endregion 查询并返回一个SqlDataReader对象实例

        #region 查询并返回查询结果的第一行第一列

        public Object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, CommandType.Text, null);
        }

        public Object ExecuteScalar(string sql, SqlParameter[] parameters)
        {
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// ExecuteScalar执行一查询，返回查询结果的第一行第一列
        /// </summary>
        /// <param name="sql">要执行的SQL语句 </param>
        /// <param name="commandType">要执行的查询类型（存储过程、SQL文本） </param>
        /// <returns>返回查询结果的第一行第一列</returns>
        public Object ExecuteScalar(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            object result = null;
            using (SqlConnection connection = new SqlConnection(_ConnStr))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        result = command.ExecuteScalar();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return result;
        }

        #endregion 查询并返回查询结果的第一行第一列

        #region 执行事务

        /// <summary>
        /// ExecuteTransaction执行一组SQL语句，如果有一条操作失败，所有的操作都回滚
        /// </summary>
        /// <param name="sqlList">要执行的SQL语句集合 </param>
        /// <param name="earlyTermination">是否提前回滚</param>
        /// <returns></returns>
        public bool ExecuteTransaction(string[] sqlList, bool earlyTermination)
        {
            return ExecuteTransaction(sqlList, null, earlyTermination);
        }

        /// <summary>
        /// ExecuteTransaction执行一组SQL语句，如果有一条操作失败，所有的操作都回滚
        /// </summary>
        /// <param name="sqlList">要执行的SQL语句集合 </param>
        /// <param name="parameters">参数数组 </param>
        /// <param name="earlyTermination">事务中有数据不满足要求是否提前终止事务 </param>
        /// <returns></returns>
        public bool ExecuteTransaction(string[] sqlList, SqlParameter[] parameters, bool earlyTermination)
        {
            using (SqlConnection connection = new SqlConnection(_ConnStr))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;  //为命令指定事务

                        if (parameters != null)
                        {
                            foreach (SqlParameter parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }

                        try
                        {
                            bool mark = true;//标记值，记录是否有操作失败的
                            foreach (string str in sqlList)
                            {
                                command.CommandText = str;
                                if (earlyTermination)
                                {
                                    //如果没有受影响的行就立即终止并提前回滚
                                    if (command.ExecuteNonQuery() <= 0)
                                    {
                                        mark = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    //不管有没有受影响的行都执行下去
                                    command.ExecuteNonQuery();
                                }
                            }

                            if (!mark)//如果执行失败，就回滚
                            {
                                transaction.Rollback(); //事务回滚
                                return false;
                            }
                            else
                            {
                                transaction.Commit();   //事务提交
                                return true;
                            }
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback(); //事务回滚

                            throw e;
                        }
                    }
                }
            }
        }

        #endregion 执行事务
    }
}