using EIApp.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.DAL
{
    /// <summary>
    /// 一次跟数据库交互的会话，封装了所有仓储的属性，根据DbSession可以拿到仓储的属性
    /// 代表应用程序跟数据库之间的一次会话，也是数据库访问层的统一入口
    /// </summary>
    public class DbSession : IDbSession
    {
        public IUserInfoRepository UserInfoRepository
        {
            get
            {
                return new UserInfoRepository();
            }
        }

        public IRoleRepository RoleRepository
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 代表：当前应用程序跟数据库的绘画内所有的实体的变化，更新会数据库
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            //调用EF上下文的SaveChanges方法
            return 0;
        }
    }
}