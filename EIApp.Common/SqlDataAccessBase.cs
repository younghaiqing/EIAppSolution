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
    public abstract class SqlDataAccessBase<T> where T : AbstractBaseModel
    {
        #region 构造函数及虚方法

        private IDBHelper DBHelper;

        public SqlDataAccessBase()
        {
            this.DBHelper = SetDBHelper();
            if (this.DBHelper == null)
            {
                throw new Exception("IDBHelper接口需要实例化,请确保实例化该类型");
            }
        }

        public abstract IDBHelper SetDBHelper();

        #endregion 构造函数及虚方法

        #region CURD

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
            if (string.IsNullOrEmpty(model.OrderByStrs))
            {
                model.OrderByStrs = GetTableKey(modelType);//如果排序栏位是空，则使用Key
            }
            if (string.IsNullOrEmpty(model.WhereStrs))
            {
                model.WhereStrs = " 1=1 ";//如果查询条件是空，则使用1=1
            }

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
            //查询条件
            sql.AppendFormat(" AND {0}", model.WhereStrs);
            //排序
            sql.AppendFormat("  ORDER BY {0}", model.OrderByStrs);
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
                if (IsContainProperty(Property))//需要包含的字段
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
            sql.AppendFormat("DELETE  FROM {0} WHERE 1=1 ", GetTableName(modelType));

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

            return DBHelper.ExecuteNonQuery(sql.ToString(), sqlParamList.ToArray());
        }

        #endregion CURD

        public DataTable SelectByPage(T model)
        {
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            var modelType = model.GetType();
            var modelProperties = modelType.GetProperties();//获取所有属性
            //获取属性的名称数组
            var PropertiesArray = modelProperties.Where(m => IsContainProperty(m)).Select(m => m.Name).ToArray();
            if (string.IsNullOrEmpty(model.OrderByStrs))
            {
                model.OrderByStrs = GetTableKey(modelType);//如果排序栏位是空，则使用Key
            }
            if (string.IsNullOrEmpty(model.WhereStrs))
            {
                model.WhereStrs = " 1=1 ";//如果查询条件是空，则使用1=1
            }

            string totalSql = @"SELECT count(*) FROM " + GetTableName(modelType) + " WHERE 1=1 AND " + model.WhereStrs;
            //获取总的行数
            var TotalCount = DBHelper.ExecuteScalar(totalSql, null);
            if (TotalCount != null)
            {
                model.TotalCount = Convert.ToInt32(TotalCount);
            }

            sql.AppendFormat("SELECT {0} FROM ", string.Join(",", PropertiesArray));

            sql.AppendFormat(@"(SELECT {0},ROW_NUMBER() OVER ( ORDER BY " + model.OrderByStrs + @") AS RowNumber

                                FROM {1} WHERE 1=1 AND {2}) as T WHERE 1=1  ", string.Join(",", PropertiesArray), GetTableName(modelType), model.WhereStrs);
            sql.AppendFormat(" AND RowNumber>{0} AND RowNumber<={1}", model.PageSize * (model.PageIndex - 1), model.PageSize * model.PageIndex);
            sql.AppendFormat(" ORDER BY " + model.OrderByStrs);

            return DBHelper.ExecuteDataTable(sql.ToString(), sqlParamList.ToArray());
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
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
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

    public abstract class AbstractBaseModel
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        [NoPropertyContain]
        public int PageIndex { set; get; }

        /// <summary>
        /// 每页总数
        /// </summary>
        [NoPropertyContain]
        public int PageSize { set; get; }

        /// <summary>
        /// 总的条码数
        /// </summary>
        [NoPropertyContain]
        public int TotalCount { set; get; }

        /// <summary>
        /// 排序使用的字段以逗号分隔
        /// </summary>
        [NoPropertyContain]
        public string OrderByStrs { set; get; }

        [NoPropertyContain]
        public string WhereStrs { set; get; }
    }
}