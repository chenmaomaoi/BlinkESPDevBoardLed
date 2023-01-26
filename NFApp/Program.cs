using nanoFramework.DependencyInjection;
using nanoFramework.Hosting;
using NFApp.Services;

namespace NFApp
{
    public class Program
    {
        public static void Main()
        {
            // 连接WiFi
            HardwareService.ConnectWifi();

            IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    // 硬件设备
                    services.AddSingleton(typeof(HardwareService));

                    // 板载LED闪烁
                    services.AddHostedService(typeof(LEDBlinkService));

                    // 蓝牙广播
                    services.AddHostedService(typeof(BLEBroadcastService));
                })
                .Build();

            host.Run();
        }
    }
}