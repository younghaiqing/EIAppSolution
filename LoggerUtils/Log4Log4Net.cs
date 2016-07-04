using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "LogConfig/log4net.config", Watch = true)]

namespace LoggerUtils
{
    public class Log4Log4Net : Log4ILog
    {
        private ILog logger = log4net.LogManager.GetLogger(typeof(Log4Log4Net));

        /// <summary>
        /// Debug Log
        /// </summary>
        /// <param name="msg"></param>
        public void WriteDebugLog(string msg)
        {
            logger.Debug(msg);
        }

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="msg"></param>
        public void WriteInfoLog(string msg)
        {
            logger.Info(msg);
        }

        /// <summary>
        ///  Error
        /// </summary>
        /// <param name="msg"></param>
        public void WriteErrorLog(string msg)
        {
            logger.Error(msg);
        }
    }
}