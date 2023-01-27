using nanoFramework.Hosting;
using Microsoft.Extensions.Logging;
using NFApp.Extensions;
using System;
using System.Diagnostics;

namespace NFApp
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                IHost host = Host.CreateDefaultBuilder()
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
                Debug.WriteLine($"message:{ex.Message}");
                Debug.WriteLine($"stack trace:{ex.StackTrace}");
            }
        }
    }
}