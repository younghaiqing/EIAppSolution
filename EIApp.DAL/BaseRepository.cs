using EIApp.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }


        public IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, S>> orderByLambda)
        {
            throw new NotImplementedException();
        }
    }
}