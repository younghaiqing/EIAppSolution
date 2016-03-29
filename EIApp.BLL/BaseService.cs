using EIApp.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.BLL
{
    public abstract class BaseService<T> : IBaseService<T> where T : class,new()
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

        public IQueryable<T> LoadEntities(Func<T, bool> whereLambda)
        {
            return CurrentRepository.LoadEntities(whereLambda);
        }
    }
}