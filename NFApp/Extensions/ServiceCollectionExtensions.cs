using System;
using Microsoft.Extensions.Logging;
using nanoFramework.DependencyInjection;
using nanoFramework.Logging.Debug;
using nanoFramework.Logging;
using nanoFramework.Hosting;
using NFApp.DependencyAttribute;
using System.Reflection;

namespace NFApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException();

            //获取所有类型
            Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in allTypes)
            {
                if (type.IsClass)
                {
                    object[] attributes = type.GetCustomAttributes(false);

                    if (attributes.Length == 1)
                    {
                        if (attributes[0].ToString() == typeof(HostedDependencyAttribute).FullName)
                        {
                            services.AddHostedService(type);
                        }
                        else if (attributes[0].ToString() == typeof(SingletonDependencyAttribute).FullName)
                        {
                            services.AddSingleton(type);
                        }
                        else if (attributes[0].ToString() == typeof(TransientDependencyAttribute).FullName)
                        {
                            services.AddTransient(type);
                        }
                    }
                }
            }
            return services;
        }

        public static IServiceCollection AddDebugLogging(this IServiceCollection services, LogLevel level = LogLevel.Debug)
        {
            if (services == null) throw new ArgumentNullException();

            DebugLoggerFactory loggerFactory = new();
            LogDispatcher.LoggerFactory = loggerFactory;

            DebugLogger logger = (DebugLogger)loggerFactory.GetCurrentClassLogger();
            logger.MinLogLevel = level;

            // using TryAdd prevents duplicate logging objects if AddLogging() 
            // is added more then once to ConfigureServices
            services.TryAdd(new ServiceDescriptor(typeof(ILogger), logger));
            //services.TryAdd(new ServiceDescriptor(typeof(ILoggerFactory), loggerFactory));

            return services;
        }

        // 使用串口日志要先初始化串口
    }
}
