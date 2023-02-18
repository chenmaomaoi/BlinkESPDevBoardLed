using System;
using NFApp.Services.Extensions.DependencyAttribute;

namespace NFApp.Services
{
    public static class LoggerService
    {
        public static void Log(string? className, LogLevel logLevel, string message, params object[] args)
        {

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
