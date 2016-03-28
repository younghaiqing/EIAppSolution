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
            Pop3Helper pop3 = new Pop3Helper();
            pop3.pop3Server = "172.27.3.94";
            pop3.pop3Port = 110;
            pop3.username = "A150020";
            pop3.password = "147";
            pop3.FetchAllMessages();
        }

        public static List<Mime> GetEmails()
        {
            //需要首先设置这些信息
            string pop3Server = "172.27.3.94";// "pop3.163.com";    //邮箱服务器 如："pop.sina.com.cn";或 "pop.tom.com" 好像sina的比较快
            int pop3Port = 110;          //端口号码   用"110"好使。最好看一下你的邮箱服务器用的是什么端口号
            bool pop3UseSsl = false;
            string username = "A150020";//"wangwending513@163.com";        //你的邮箱用户名
            string password = "147";// "wu515632356?";      //你的邮箱密码
            List<string> gotEmailIds = new List<string>();

            List<Mime> result = new List<Mime>();
            using (POP3_Client pop3 = new POP3_Client())
            {
                try
                {
                    //与Pop3服务器建立连接
                    pop3.Connect(pop3Server, pop3Port, pop3UseSsl);
                    //验证身份
                    pop3.Login(username, password);
                    //获取邮件信息列表
                    POP3_ClientMessageCollection infos = pop3.Messages;

                    foreach (POP3_ClientMessage info in infos)
                    {
                        //每封Email会有一个在Pop3服务器范围内唯一的Id,检查这个Id是否存在就可以知道以前有没有接收过这封邮件
                        if (gotEmailIds.Contains(info.UID))
                            continue;

                        //获取这封邮件的内容
                        byte[] bytes = info.MessageToByte();
                        //记录这封邮件的Id
                        gotEmailIds.Add(info.UID);
                        //解析从Pop3服务器发送过来的邮件信息
                        Mime mime = Mime.Parse(bytes);
                        //mime.Attachments[0].DataToFile(@"c:\" + mime.Attachments[0].ContentDisposition_FileName);
                        result.Add(mime);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return result;
        }

        private static void NewMethod()
        {
            //Pop3 client = new Pop3();
            //client.ConnectSSL("pop3.163.com");
            //client.UseBestLogin("wangwending513@163.com", "wu515632356?");
            using (Pop3 pop3 = new Pop3())
            {
                pop3.Connect("pop3.163.com");    // use ConnectSSL for SSL
                pop3.Login("wangwending513@163.com", "wu515632356?");

                // Receive all messages
                MailBuilder builder = new MailBuilder();

                foreach (string uid in pop3.GetAll())
                {
                    IMail email = builder.CreateFromEml(
                        pop3.GetMessageByUID(uid)
                        );

                    // Subject
                    Console.WriteLine(email.Subject);

                    // From
                    foreach (MailBox m in email.From)
                    {
                        Console.WriteLine(m.Address);
                        Console.WriteLine(m.Name);
                    }

                    // Date
                    Console.WriteLine(email.Date);

                    // Text body of the message
                    Console.WriteLine(email.Text);

                    // Html body of the message
                    Console.WriteLine(email.Html);

                    // Custom header
                    Console.WriteLine(email.Document.Root.Headers["x-spam"]);

                    // Save all attachments to disk
                    foreach (MimeData mime in email.Attachments)
                    {
                        mime.Save(@"c:\" + mime.SafeFileName);
                    }
                }
                pop3.Close();
            }
        }
    }
}