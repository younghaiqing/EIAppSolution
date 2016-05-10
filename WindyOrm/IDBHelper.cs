using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyOrm
{
    #region DBHelper接口

    /// <summary>
    /// DBHelper接口
    /// </summary>
    public interface IDBHelper
    {
        string ConnString { set; }

        DataTable ExecuteDataTable(string sql, SqlParameter[] parameters);

        int ExecuteNonQuery(string sql, SqlParameter[] sqlParams);

        Object ExecuteScalar(string sql, SqlParameter[] parameters);
    }

    #endregion DBHelper接口
}