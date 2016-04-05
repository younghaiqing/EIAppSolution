using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.Models
{
    [Table("UserInfo")]
    public partial class UserInfo : BaseEntity
    {
        public int ID { set; get; }
        public string UName { set; get; }
        public string Pwd { set; get; }
    }
}