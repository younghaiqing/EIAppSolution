using EIApp.DAL;
using EIApp.IBLL;
using EIApp.IDAL;
using EIApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.BLL
{
    public class UserInfoService : BaseService<UserInfo>
    {
        public override void SetCurrentRepository()
        {
            CurrentRepository = DAL.RepositoryFactory.UserInfoRepository;
        }
    }
}