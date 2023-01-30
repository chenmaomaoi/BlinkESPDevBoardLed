using nanoFramework.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using NFApp.Logging.Extensions;
using NFApp.Services.Extensions;

namespace NFApp
{
    public static class Program
    {
        public static IHost host;
        public static void Main()
        {
            try
            {
                host = Host.CreateDefaultBuilder()
                    .ConfigureServices(services =>
                    {
                        services.AddDebugLogging(LogLevel.Trace);
                        services.AddServices();
                    })
                    .Build();
                host.Run();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}