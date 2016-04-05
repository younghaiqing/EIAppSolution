using EIApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.BLL
{
    public class RoleService : BaseService<Role>
    {
        public override void SetCurrentRepository()
        {
            CurrentRepository = DAL.RepositoryFactory.RoleRepository;
        }
    }
}