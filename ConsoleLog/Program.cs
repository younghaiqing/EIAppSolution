using System;

namespace ConsoleLog
{
    public class Program
    {
        private static void Main(string[] args)
        {
            //输出一个日志到文件夹中
            LoggerUtils.Log4Nlog log = new LoggerUtils.Log4Nlog();
            log.WriteErrorLog("Error222");
            Console.ReadKey();
        }
    }
}