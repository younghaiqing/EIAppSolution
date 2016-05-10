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
    #region 所有DAL的基类实现基本的CURD(增删改查,分页)

    /// <summary>
    /// 所有DAL的基类实现基本的CURD(增删改查,分页)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SqlDAOBase<T> : SqlDAOBaseReport<T> where T : AbstractBaseModel, new()
    {
        #region CURD

        /// <summary>
        /// 模型插入
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
        /// 模型修改
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
    }

    #endregion 所有DAL的基类实现基本的CURD(增删改查,分页)
}