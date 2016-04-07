using Limilabs.Client.POP3;
using Limilabs.Mail;
using Limilabs.Mail.Headers;
using Limilabs.Mail.MIME;
using LumiSoft.Net.Mime;
using LumiSoft.Net.POP3.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIAppeEmail
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Pop3Helper pop3 = new Pop3Helper();
            //pop3.pop3Server = "172.27.3.94";
            //pop3.pop3Port = 110;
            //pop3.username = "A150020";
            //pop3.password = "147";
            //pop3.FetchAllMessages();

            EIApp.BLL.UserInfoService UBll = new EIApp.BLL.UserInfoService();
            var model = new EIApp.Models.UserInfo() { ID=1,UName="wwwwwwwwd",Pwd="11111" };
            UBll.AddEntity(model);
           // var models = UBll.LoadEntities(m => m.ID > 2);
            //var m = UBll.DeleteEntity(model);
            //foreach (var model in models)
            //{
            //    Console.WriteLine(model.ID);
            //}
            Console.ReadLine();
        }
    }
}