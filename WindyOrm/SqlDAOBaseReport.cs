using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WindyOrm
{
    #region 只包含查询及分页查询DAL

    /// <summary>
    /// 只包含查询及分页查询DAL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SqlDAOBaseReport<T> where T : AbstractBaseModel, new()
    {
        #region 构造函数及虚方法

        protected IDBHelper DBHelper;

        public SqlDAOBaseReport()
        {
            this.DBHelper = SetDBHelper();
            if (this.DBHelper == null)
            {
                throw new Exception("IDBHelper接口需要实例化,请确保实例化该类型");
            }
            //if(string.IsNullOrEmpty(this.DBHelper.ConnString))
            //{
            //    throw new Exception("IDBHelper接口需要实例化,请确保实例化该类型");
            //}
        }

        public abstract IDBHelper SetDBHelper();

        #endregion 构造函数及虚方法

        #region 查询方法

        /// <summary>
        /// 条件查询
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
        /// 分页查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        #endregion 查询方法

        #region 私有方法

        /// <summary>
        /// 获取自定义的TableName
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected string GetTableName(Type targetType)
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
        protected string GetTableKey(Type targetType)
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
        protected object DefaultForType(Type targetType)
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
        protected bool IsContainProperty(PropertyInfo pi)
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

    #endregion 只包含查询及分页查询DAL
}
