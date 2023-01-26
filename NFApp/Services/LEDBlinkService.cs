using System.Threading;
using nanoFramework.Hosting;

namespace NFApp.Services
{
    /// <summary>
    /// 板载LED闪烁
    /// </summary>
    public class LEDBlinkService : BackgroundService
    {
        private readonly HardwareService _hardwareDevices;

        public LEDBlinkService(HardwareService hardwareDevices)
        {
            _hardwareDevices = hardwareDevices;
        }

        protected override void ExecuteAsync()
        {
            while (!CancellationRequested)
            {
                _hardwareDevices.LED.Toggle();
                Thread.Sleep(50);
                _hardwareDevices.LED.Toggle();

                Thread.Sleep(1_000);
            }
        }
    }
}
