using LumiSoft.Net.Mime;
using LumiSoft.Net.POP3.Client;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EIAppeEmail
{
    public class Pop3Helper
    {
        public Pop3Helper()
        {
        }

        public Pop3Helper(string pop3Server, string username, string password, int pop3Port = 110)
        {
            this.pop3Server = pop3Server;
            this.pop3Port = pop3Port;
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// POP3服务器
        /// </summary>
        public string pop3Server { set; get; }

        /// <summary>
        /// POP3服务器端口号
        /// </summary>
        public int pop3Port { set; get; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string username { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { set; get; }

        public List<Mime> GetEmailsByLumiSoft()
        {   //需要首先设置这些信息
            string pop3Server = this.pop3Server;//邮箱服务器 如："pop.sina.com.cn";或 "pop.tom.com" 好像sina的比较快
            int pop3Port = this.pop3Port;//端口号码   用"110"好使。最好看一下你的邮箱服务器用的是什么端口号
            bool pop3UseSsl = false;
            string username = this.username;//你的邮箱用户名
            string password = this.password;//你的邮箱密码
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

        /// <summary>
        /// Example showing:
        ///  - how to fetch all messages from a POP3 server
        /// </summary>
        /// <param name="hostname">Hostname of the server. For example: pop3.live.com</param>
        /// <param name="port">Host port to connect to. Normally: 110 for plain POP3, 995 for SSL POP3</param>
        /// <param name="useSsl">Whether or not to use SSL to connect to server</param>
        /// <param name="username">Username of the user on the server</param>
        /// <param name="password">Password of the user on the server</param>
        /// <returns>All Messages on the POP3 server</returns>
        public List<Message> FetchAllMessages()
        {
            string hostname = this.pop3Server;
            int port = this.pop3Port;
            bool useSsl = false;
            // The client disconnects from the server when being disposed
            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(hostname, port, useSsl);

                // Authenticate ourselves towards the server
                client.Authenticate(username, password);

                client.GetMessageInfos();
                // Get the number of messages in the inbox
                int messageCount = client.GetMessageCount();

                // We want to download all messages
                List<Message> allMessages = new List<Message>(messageCount);

                // Messages are numbered in the interval: [1, messageCount]
                // Ergo: message numbers are 1-based.
                // Most servers give the latest message the highest number
                for (int i = messageCount; i > 0; i--)
                {
                    allMessages.Add(client.GetMessage(i));
                }
                string test = allMessages[0].MessagePart.MessageParts[0].GetBodyAsText();
                // Now return the fetched messages
                return allMessages;
            }
        }
    }
}