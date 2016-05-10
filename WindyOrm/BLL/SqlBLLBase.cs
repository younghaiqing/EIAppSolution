using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyOrm.BLL
{
    public abstract class SqlBLLBase<T> where T : AbstractBaseModel, new()
    {
        private SqlDAO<T> SqlDao = null;

        public SqlBLLBase()
        {
            SqlDao = new SqlDAO<T>();
            SqlDao.SetDBHelper().ConnString = SetDBHelperConn();
        }

        public abstract string SetDBHelperConn();

        public int InsertBy(T model)
        {
            return SqlDao.InsertBy(model);
        }

        public int ModifyBy(T model)
        {
            return SqlDao.ModifyBy(model);
        }

        public int DeleteBy(T model)
        {
            return SqlDao.DeleteBy(model);
        }

        public List<T> SelectBy(T model)
        {
            return SqlDao.SelectBy(model).ToModelList<T>();
        }

        public List<T> SelectByPage(T model)
        {
            return SqlDao.SelectByPage(model).ToModelList<T>();
        }
    }
}