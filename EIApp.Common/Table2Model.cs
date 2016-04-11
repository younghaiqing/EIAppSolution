using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.Common
{
    /// <summary>
    /// DataTable转成实体
    /// </summary>
    public static class Table2Model
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="table">需转换的数据table</param>
        /// <returns></returns>
        public static T ToModel<T>(this DataTable table) where T : new()
        {
            T entity = new T();

            foreach (DataRow row in table.Rows)
            {
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            //if (item.PropertyType.Equals(typeof(DateTime?)))
                            //{
                            //    item.SetValue(entity, (System.Nullable<DateTime>)DateTime.Parse(row[item.Name].ToString()), null);
                            //}
                            //else if (item.PropertyType.Equals(typeof(System.Boolean?)))
                            //{
                            //    item.SetValue(entity, (System.Nullable<Boolean>)System.Boolean.Parse(row[item.Name].ToString()), null);
                            //}
                            //else if (item.PropertyType.Equals(typeof(int?)))
                            //{
                            //    item.SetValue(entity, Convert.ToInt32(row[item.Name]), null);
                            //}
                            //else
                            //{
                            item.SetValue(entity, ChangeType(row[item.Name], item.PropertyType), null);
                            //}
                        }
                    }
                }
            }

            return entity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="table">需转换的数据table</param>
        /// <param name="ext">外延表重名需加的前缀</param>
        /// <returns></returns>
        public static T ToModel<T>(this DataTable table, string ext) where T : new()
        {
            T entity = new T();

            foreach (DataRow row in table.Rows)
            {
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(ext + item.Name))
                    {
                        if (DBNull.Value != row[ext + item.Name])
                        {
                            item.SetValue(entity, Convert.ChangeType(row[ext + item.Name], item.PropertyType), null);
                        }
                    }
                }
            }

            return entity;
        }

        public static List<T> ToModelList<T>(this DataTable table) where T : new()
        {
            List<T> entities = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            item.SetValue(entity, ChangeType(row[item.Name], item.PropertyType), null);
                        }
                    }
                }
                entities.Add(entity);
            }

            return entities;
        }

        #region 私有方法(Convert.ChangeType处理Nullable<>和非Nullable<>)

        /// <summary>
        /// 类型转换（包含Nullable<>和非Nullable<>转换）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        private static object ChangeType(object value, Type conversionType)
        {
            // Note: This if block was taken from Convert.ChangeType as is, and is needed here since we're
            // checking properties on conversionType below.
            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType");
            } // end if

            // If it's not a nullable type, just pass through the parameters to Convert.ChangeType

            if (conversionType.IsGenericType &&
              conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                } // end if

                // It's a nullable type, and not null, so that means it can be converted to its underlying type,
                // so overwrite the passed-in conversion type with this underlying type
                NullableConverter nullableConverter = new NullableConverter(conversionType);

                conversionType = nullableConverter.UnderlyingType;
            } // end if

            // Now that we've guaranteed conversionType is something Convert.ChangeType can handle (i.e. not a
            // nullable type), pass the call on to Convert.ChangeType
            return Convert.ChangeType(value, conversionType);
        }

        #endregion 私有方法(Convert.ChangeType处理Nullable<>和非Nullable<>)
    }
}