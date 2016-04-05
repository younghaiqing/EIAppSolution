using EIApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.DAL
{
    public class EIDbContext : DbContext
    {
        public EIDbContext()
            : base(Common.Config.DBConnString["EIConString"].ToString())
        {
        }

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Role> Role { get; set; }
    }
}