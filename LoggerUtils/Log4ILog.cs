namespace LoggerUtils
{
    public interface Log4ILog
    {
        void WriteDebugLog(string msg);

        void WriteInfoLog(string msg);

        void WriteErrorLog(string msg);
    }
}