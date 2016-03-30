using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.IDAL
{
    public interface IDbSession
    {
        IDAL.IRoleRepository RoleRepository { get; }

        IDAL.IUserInfoRepository UserInfoRepository { get; }

        int SaveChanges();
    }
}