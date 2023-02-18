using System;

namespace NFApp
{
    public class Logger
    {
        public static void Log(string message, string className = "", string methodName = "", LogLevel logLevel = LogLevel.Information, params object[] args)
        {
            string msg = $@"[{className}.{methodName}],{logLevel},{string.Format(message, args)}";

            //todo:使用SD卡记录日志时，不要cw
            Console.WriteLine(msg);
        }
    }

    public enum LogLevel : byte
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error
    }
}
