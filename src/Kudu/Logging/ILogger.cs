namespace Kudu.Logging
{
    public interface ILogger
    {
        void Log(LogLevel level, object message);
        void Debug(object message);
        void Normal(object message);
        void Important(object message);
        void Error(object message);
    }
}