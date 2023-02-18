using nanoFramework.Hosting;
using System;
using System.Diagnostics;
using NFApp.Logging.Extensions;
using NFApp.Services.Extensions;

namespace NFApp
{
    public static class Program
    {
        public static void Main()
        {
            try
            {
                Host.CreateDefaultBuilder()
                    .ConfigureServices(services =>
                    {
                        services.AddLogging();
                        services.AddServices();
                    })
                    .Build()
                    .Run();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}