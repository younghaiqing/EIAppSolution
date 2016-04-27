using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.Common
{
    /// <summary>
    /// 所有DAL的基类实现基本的CURD(增删改查)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlDataAccessBase<T> where T : class
    {
        public DataAccess.DBHelper db { get; set; }

        /// <summary>
        /// 更新模型条件查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable SelectBy(T model)
        {
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            var modelType = model.GetType();
            var modelProperties = modelType.GetProperties();//获取所有属性
            //获取属性的名称数组
            var PropertiesArray = modelProperties.Where(m => IsContainProperty(m)).Select(m => m.Name).ToArray();
            sql.AppendFormat("SELECT {0} FROM {1} WHERE 1=1 ", string.Join(",", PropertiesArray), GetTableName(modelType));

            foreach (var Property in modelProperties)
            {
                if (IsContainProperty(Property))
                {
                    if (Property.GetValue(model) != null && !Property.GetValue(model).Equals(DefaultForType(Property.PropertyType)))
                    {
                        sql.AppendFormat("AND {0} = @{0} ", Property.Name);
                        sqlParamList.Add(new SqlParameter(Property.Name, Property.GetValue(model)));
                    }
                }
            }
            DBHelper.ConnString = Common.Config.DBConnString["LMS"].ToString();

            return DBHelper.ExecuteDataTable(sql.ToString(), sqlParamList.ToArray());
        }

        /// <summary>
        /// 更新模型插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertBy(T model)
        {
            StringBuilder sql = new StringBuilder();     //拼接最后的SQL语句
            StringBuilder field = new StringBuilder();   //拼接字段名
            StringBuilder value = new StringBuilder();   //拼接字段对应的值

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            var modelType = model.GetType();
            var modelProperties = modelType.GetProperties();//获取所有属性
            //获取属性的名称数组
            var PropertiesArray = modelProperties.Where(m => IsContainProperty(m)).Select(m => m.Name).ToArray();

            foreach (var Property in modelProperties)
            {
                if (IsContainProperty(Property))
                {
                    if (Property.Name == GetTableKey(modelType))//如果是主键
                    {
                        continue;
                    }
                    if (Property.GetValue(model) != null
                        && !Property.GetValue(model).Equals(DefaultForType(Property.PropertyType)))
                    {
                        field.AppendFormat(" {0}, ", Property.Name);
                        value.AppendFormat(" @{0}, ", Property.Name);
                        sqlParamList.Add(new SqlParameter(Property.Name, Property.GetValue(model)));
                    }
                }
            }
            sql.AppendFormat("INSERT INTO {0} ({1}) VALUES({2}); ", GetTableName(modelType), field.ToString().Remove(field.ToString().LastIndexOf(","), 1), value.ToString().Remove(value.ToString().LastIndexOf(","), 1));
            DBHelper.ConnString = Common.Config.DBConnString["LMS"].ToString();

            return DBHelper.ExecuteNonQuery(sql.ToString(), sqlParamList.ToArray());
        }

        /// <summary>
        /// 更新模型修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int ModifyBy(T model)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder set = new StringBuilder();

            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            var modelType = model.GetType();
            var modelProperties = modelType.GetProperties();//获取所有属性
            //获取属性的名称数组
            var PropertiesArray = modelProperties.Where(m => IsContainProperty(m)).Select(m => m.Name).ToArray();

            foreach (var Property in modelProperties)
            {
                if (IsContainProperty(Property))
                {
                    if (Property.Name == GetTableKey(modelType))//如果是主键
                    {
                        sqlParamList.Add(new SqlParameter(Property.Name, Property.GetValue(model)));
                        continue;
                    }
                    if (Property.GetValue(model) != null && !Property.GetValue(model).Equals(DefaultForType(Property.PropertyType)))
                    {
                        set.AppendFormat(" {0} = @{0}, ", Property.Name);
                        sqlParamList.Add(new SqlParameter(Property.Name, Property.GetValue(model)));
                    }
                }
            }

            sql.AppendFormat("UPDATE {0} SET {1} ", GetTableName(modelType), set.ToString().Remove(set.ToString().LastIndexOf(","), 1))
               .Append("WHERE 1 = 1 ")
               .AppendFormat("AND {0} = @{0}", GetTableKey(modelType));
            //sqlParamList.Add(new SqlParameter(Property.Name, Property.GetValue(model)));
            DBHelper.ConnString = Common.Config.DBConnString["LMS"].ToString();

            return DBHelper.ExecuteNonQuery(sql.ToString(), sqlParamList.ToArray());
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int DeleteBy(T model)
        {
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            var modelType = model.GetType();
            var modelProperties = modelType.GetProperties();//获取所有属性
            //获取属性的名称数组
            var PropertiesArray = modelProperties.Where(m => IsContainProperty(m)).Select(m => m.Name).ToArray();
            sql.AppendFormat("DELETE  FROM {0} WHERE 1=1 ",GetTableName(modelType));

            foreach (var Property in modelProperties)
            {
                if (IsContainProperty(Property))
                {
                    if (Property.GetValue(model) != null && !Property.GetValue(model).Equals(DefaultForType(Property.PropertyType)))
                    {
                        sql.AppendFormat("AND {0} = @{0} ", Property.Name);
                        sqlParamList.Add(new SqlParameter(Property.Name, Property.GetValue(model)));
                    }
                }
            }
            DBHelper.ConnString = Common.Config.DBConnString["LMS"].ToString();

            return DBHelper.ExecuteNonQuery(sql.ToString(), sqlParamList.ToArray());
        }

        #region 私有方法

        /// <summary>
        /// 获取自定义的TableName
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private string GetTableName(Type targetType)
        {
            string tableName = string.Empty;
            tableName = targetType.Name;
            object[] objAttrs = targetType.GetCustomAttributes(typeof(TableNameAttribute), true);
            if (objAttrs.Length > 0)
            {
                TableNameAttribute attr = objAttrs[0] as TableNameAttribute;
                if (attr != null)
                {
                    if (!string.IsNullOrEmpty(attr.Name))
                    {
                        tableName = attr.Name;
                    }
                }
            }
            return tableName;
        }

        /// <summary>
        /// 获取Table的主键用于更新
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private string GetTableKey(Type targetType)
        {
            string tableKey = string.Empty;
            tableKey = "ID";
            object[] objAttrs = targetType.GetCustomAttributes(typeof(TableNameAttribute), true);
            if (objAttrs.Length > 0)
            {
                TableNameAttribute attr = objAttrs[0] as TableNameAttribute;
                if (attr != null)
                {
                    if (!string.IsNullOrEmpty(attr.TableKey))
                    {
                        tableKey = attr.TableKey;
                    }
                }
            }
            return tableKey;
        }

        /// <summary>
        /// 获取Type类型的默认值
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private object DefaultForType(Type targetType)
        {
            object DefaultValue = null;
            if (targetType == typeof(DateTime))//如果是DateTime，使用DateTime默认值
            {
                DefaultValue = default(DateTime);
            }
            //不是Boolean的值类型
            if (targetType.IsValueType && targetType != typeof(Boolean))
            {
                DefaultValue = Activator.CreateInstance(targetType);
            }
            return DefaultValue;
        }

        /// <summary>
        /// 判断属性是否包含NoPropertyContainAttribute
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        private bool IsContainProperty(PropertyInfo pi)
        {
            bool flag = true;
            var CustomAttributes = pi.GetCustomAttributes(true);
            foreach (var temp in CustomAttributes)
            {
                var NoPropertyContain = temp as NoPropertyContainAttribute;
                if (NoPropertyContain != null)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        #endregion 私有方法
    }

    #region 特性Attribute

    /// <summary>
    /// 该特性使用在属性上,表示该属性无需参与SQL查询
    /// </summary>
    public class NoPropertyContainAttribute : Attribute
    {
    }

    /// <summary>
    ///  该特性使用在模型上,表示表名及表的主键
    /// </summary>
    public class TableNameAttribute : Attribute
    {
        /// <summary>
        /// 表的自定义名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表的主键
        /// </summary>
        public string TableKey { get; set; }
    }

    #endregion 特性Attribute

    public interface IDBHelper
    {
        DataTable ExecuteDataTable(string sql, SqlParameter[] parameters);
    }
}