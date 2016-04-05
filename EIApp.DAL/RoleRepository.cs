using EIApp.IDAL;
using EIApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.DAL
{
    /// <summary>
    /// 具体的DAL实现类
    /// 继承基础实现类和个性化接口
    /// </summary>
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
    }
}