using System;

namespace ConsoleLog
{
    public class Program
    {
        private static void Main(string[] args)
        {
            LoggerUtils.Log4ILog log = new LoggerUtils.Log4Log4Net();//new LoggerUtils.Log4Nlog() ;
            log.WriteDebugLog("test");
            Console.WriteLine("日志记录完毕。");
            Console.Read();
        }
    }
}