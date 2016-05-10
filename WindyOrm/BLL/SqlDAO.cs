using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyOrm.BLL
{
    public class SqlDAO<T> : SqlDAOBase<T> where T : AbstractBaseModel, new()
    {
        public override IDBHelper SetDBHelper()
        {
            var DBHelper = WindyOrm.DBHelper.GetInstance();
            //DBHelper.ConnString = @"Data Source=172.27.3.125;Initial Catalog=LMS;User Id=sa;Password=!Qazwsxedc;";
            return DBHelper;
        }
    }
}