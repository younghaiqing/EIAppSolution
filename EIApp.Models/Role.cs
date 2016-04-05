using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.Models
{
    [Table("Role")]
    /// <summary>
    /// 角色实体（定义成partial类方便以后扩展）
    /// </summary>
    public partial class Role : BaseEntity
    {
        public int ID { set; get; }
        public string RoleName { set; get; }
    }
}