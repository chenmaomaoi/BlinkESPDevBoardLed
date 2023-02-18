using System;
using System.Drawing;
using Iot.Device.Button;
using Microsoft.Extensions.Logging;
using nanoFramework.Hosting;
using NFApp.Services.Extensions.DependencyAttribute;

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

        public ButtonService(ILoggerFactory loggerFactory, DeviceService device, LEDBlinkService ledBlink)
        {
            logger = loggerFactory.CreateLogger(nameof(ButtonService));
            button = device.Button;
            this.ledBlink = ledBlink;
            isEnable = true;
        }

        public void Start()
        {
#if DEV
            ledBlink.StartBlinkAsync();
#endif
#if S2_DEV
            ledBlink.StartBlinkAsync(Color.Red);
#endif

            button.Press += button_Press;
        }

        public void Stop()
        {
        }

        private void button_Press(object sender, EventArgs e)
        {
            logger.LogDebug("button press");
            isEnable = !isEnable;
            if (isEnable)
            {
#if DEV
                ledBlink.StartBlinkAsync();
#endif
#if S2_DEV
                ledBlink.StartBlinkAsync(Color.Green);
#endif
            }
            else
            {
                ledBlink.StopBlink();
            }
        }
    }
}
