using System;
using Iot.Device.Button;
using Microsoft.Extensions.Logging;
using nanoFramework.Hosting;
using NFApp.DependencyAttribute;

namespace NFApp.Services
{
    /// <summary>
    /// 板载按钮服务
    /// </summary>
    [HostedDependency]
    public class ButtonService : IHostedService
    {
        private readonly ILogger logger;
        private readonly GpioButton button;
        private readonly LEDBlinkService ledBlink;
        private bool isEnable;
        public ButtonService(ILoggerFactory loggerFactory, HardwareService hardware, LEDBlinkService ledBlink)
        {
            logger = loggerFactory.CreateLogger(nameof(ButtonService));
            button = hardware.Button;
            this.ledBlink = ledBlink;
            isEnable = false;
        }

        public void Start()
        {
            button.Press += button_Press;
        }

        public void Stop()
        {
        }

        private void button_Press(object sender, EventArgs e)
        {
            isEnable = !isEnable;
            if (isEnable)
            {
                ledBlink.StartBlinkAsync();
            }
            else
            {
                ledBlink.StopBlink();
            }
        }
    }
}
