using System;
using System.Collections.Generic;

namespace Kudu.Logging
{
    public class ConsoleLogger : ILogger
    {
        private static readonly Dictionary<LogLevel, ConsoleColor> Colors = new Dictionary<LogLevel, ConsoleColor>
        {
            {LogLevel.Debug, ConsoleColor.Yellow},
            {LogLevel.Normal, ConsoleColor.White},
            {LogLevel.Important, ConsoleColor.Blue}
        };

        private readonly string _name;

        public ConsoleLogger(string name)
        {
            _name = name;
        }

        public void Log(LogLevel level, object message)
        {
            Log(level, ColorFor(level), message);
        }

        public void Log(LogLevel level, ConsoleColor color, object message)
        {
            if (level < Logging.Log.Level)
            {
                return;
            }
            Console.ForegroundColor = color;
            Console.WriteLine("{0}: {1}", _name, message);
            Console.ResetColor();
        }

        private static ConsoleColor ColorFor(LogLevel level)
        {
            if (Colors.ContainsKey(level))
            {
                return Colors[level];
            }
            return ConsoleColor.White;
        }

        public void Debug(object message)
        {
            Log(LogLevel.Debug, message);
        }

        public void Normal(object message)
        {
            Log(LogLevel.Normal, message);
        }

        public void Important(object message)
        {
            Log(LogLevel.Important, message);
        }

        public void Error(object message)
        {
            Log(LogLevel.Important, ConsoleColor.Red, message);
        }
    }
}