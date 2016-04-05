using EIApp.IDAL;
using EIApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.DAL
{
    /// <summary>
    ///  实现IDAL接口的抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private EIDbContext db = new EIDbContext();

        public T AddEntity(T entity)
        {
            db.Set<T>().Add(entity);
            db.SaveChanges();
            return entity;
        }

        public bool UpdateEntity(T entity)
        {
            DbEntityEntry entry = db.Entry<T>(entity);
            entry.State = System.Data.Entity.EntityState.Unchanged;
            return db.SaveChanges() > 0;
        }

        public bool DeleteEntity(T entity)
        {
            db.Set<T>().Attach(entity);
            db.Set<T>().Remove(entity);
            return db.SaveChanges() > 0;
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda);
        }

        public IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, S>> orderByLambda)
        {
            total = db.Set<T>().Count();
            return db.Set<T>().Where(whereLambda).OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}