using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyOrm
{
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
}