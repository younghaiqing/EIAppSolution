using EIApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.IDAL
{
    /// <summary>
    /// 具体接口，继承基础接口。
    /// 可以在这个接口中定义特定的方法
    /// </summary>
    public interface IRoleRepository : IBaseRepository<Role>
    {
    }
}