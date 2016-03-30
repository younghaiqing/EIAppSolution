using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.DAL
{
    public class EFContextFactory
    {
        ///// <summary>
        ///// 帮我们返回当前线程内的数据库上下文，如果当前线程内没有上下文，那么创建一个上下文，并保证
        ///// 上线问实例在线程内部是唯一的
        ///// </summary>
        ///// <returns></returns>
        //public static DbContext  GetCurrentDbContext()
        //{
        //    DbContext dbContext = CallContext.GetData("DbContext") as DbContext;
        //    if (dbContext==null)
        //    {
        //        dbContext = null;//; new DataModelContainer(); //如果不存在上下文的话，创建一个
        //        CallContext.SetData("DbContext", dbContext);
        //    }
        //    return dbContext;
        //}
    }
}