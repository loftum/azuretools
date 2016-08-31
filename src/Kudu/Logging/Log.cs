using System;
using Kudu.Extensions;

namespace Kudu.Logging
{
    public class Log
    {
        static Log()
        {
            Level = LogLevel.Normal;
            Factory = n => new ConsoleLogger(n);
        }

        public static Func<string, ILogger> Factory { get; set; }

        public static LogLevel Level { get; set; }

        public static ILogger For<T>()
        {
            return For(typeof(T));
        }

        public static ILogger For(Type type)
        {
            return Factory(type.GetFriendlyName());
        }

        public static ILogger For(object item)
        {
            return For(item.GetType());
        }
    }

    public enum LogLevel
    {
        Debug = 0,
        Normal = 10,
        Important = 20
    }
}