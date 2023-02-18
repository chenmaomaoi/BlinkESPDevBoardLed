using System;

namespace NFApp.Services
{
    public static class Logger
    {
        public static void Log(string message, string className = "", string methodName = "", LogLevel logLevel = LogLevel.Information, params object[] args)
        {
            string msg = $@"[{className}],{logLevel} {string.Format(message, args)}";

            Console.WriteLine(msg);
        }
    }

    public enum LogLevel
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error
    }
}
