using EIApp.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.DAL
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class,new()
    {
        public T AddEntity(T entity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEntity(T entity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteEntity(T entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> LoadEntities(Func<T, bool> whereLambda)
        {
            throw new NotImplementedException();
        }
    }
}