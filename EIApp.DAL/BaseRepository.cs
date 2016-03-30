using EIApp.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.DAL
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class,new()
    {
        private DbContext db = EFContextFactory.GetCurrentDbContext();

        public T AddEntity(T entity)
        {
            db.Entry<T>(entity).State = EntityState.Added;
            db.SaveChanges();
            return entity;
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