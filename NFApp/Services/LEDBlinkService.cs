using System.Device.Gpio;
using System.Threading;
using Microsoft.Extensions.Logging;
using NFApp.DependencyAttribute;

namespace NFApp.Services
{
    /// <summary>
    /// 板载LED闪烁服务
    /// </summary>
    [SingletonDependency]
    public class LEDBlinkService
    {
        private readonly GpioPin _led;
        private readonly ILogger _logger;

        private bool _signalStop;
        private Thread _thread;

        public LEDBlinkService(HardwareService hardware, ILogger logger)
        {
            _led = hardware.LED;
            _logger = logger;
            _signalStop = false;

            StartBlinkAsync(1000, 1000);
        }

        /// <summary>
        /// 闪烁
        /// </summary>
        /// <param name="brigth">亮灯时间</param>
        /// <param name="goOut">灭灯时间</param>
        public void StartBlinkAsync(int brigth = 50, int goOut = 950)
        {
            if (_thread != null)
            {
                StopBlink();
            }
            _signalStop = false;

            _thread = new Thread(() =>
            {
                int th_bright = brigth;
                int th_goOut = goOut;
                while (!_signalStop)
                {
                    _led.Toggle();
                    Thread.Sleep(th_bright);
                    _led.Toggle();

                    Thread.Sleep(th_goOut);
                }
            });
            _thread.Start();
        }

        /// <summary>
        /// 停止闪烁
        /// </summary>
        public void StopBlink()
        {
            _signalStop = true;
            while (_thread.ThreadState != ThreadState.Stopped)
            {
                Thread.Sleep(50);
            }
        }
    }
}
