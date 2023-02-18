using System;
using Microsoft.Extensions.Logging;
using nanoFramework.DependencyInjection;
using nanoFramework.Logging.Debug;
using nanoFramework.Logging;

namespace NFApp.Logging.Extensions
{
    public static class ServiceCollectionExtensions
    {
        #region Logging

        /// <summary>
        /// Adds logging services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddLogging(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException();
            }

            DebugLoggerFactory loggerFactory = new();
            LogDispatcher.LoggerFactory = loggerFactory;
            LoggerExtensions.MessageFormatter = typeof(LoggerFormat).GetType().GetMethod("MessageFormatter");
            
            DebugLogger logger = (DebugLogger)loggerFactory.GetCurrentClassLogger();
            //过滤不生效
            //logger.MinLogLevel = level;

            // using TryAdd prevents duplicate logging objects if AddLogging() is added more then once
            services.TryAdd(new ServiceDescriptor(typeof(ILogger), logger));
            services.TryAdd(new ServiceDescriptor(typeof(ILoggerFactory), loggerFactory));

            return services;
        }
        #endregion
    }
}
