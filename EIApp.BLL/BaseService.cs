using EIApp.IBLL;
using EIApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.BLL
{
    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        public IDAL.IBaseRepository<T> CurrentRepository { get; set; }

        public BaseService()
        {
            SetCurrentRepository();
        }

        public abstract void SetCurrentRepository();  //子类必须实现

        public T AddEntity(T entity)
        {
            return CurrentRepository.AddEntity(entity);
        }

        public bool UpdateEntity(T entity)
        {
            return CurrentRepository.UpdateEntity(entity);
        }

        public bool DeleteEntity(T entity)
        {
            return CurrentRepository.DeleteEntity(entity);
        }

        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return CurrentRepository.LoadEntities(whereLambda);
        }

        public IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int total, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, bool isAsc, System.Linq.Expressions.Expression<Func<T, S>> orderByLambda)
        {
            return CurrentRepository.LoadPageEntities<S>(pageIndex, pageSize, out  total, whereLambda, isAsc, orderByLambda);
        }
    }
}