using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyOrm
{
    #region 基础模型类型

    /// <summary>
    /// 基础模型类型
    /// </summary>
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

    #endregion 基础模型类型
}