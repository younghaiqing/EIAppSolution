using EIApp.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.DAL
{
    public static class RepositoryFactory
    {
        public static IUserInfoRepository UserInfoRepository
        {
            get
            {
                return new UserInfoRepository();
            }
        }
        public static IRoleRepository RoleRepository
        {
            get
            {
                return new RoleRepository();
            }
        }
    }
}