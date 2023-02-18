using System;
using System.Drawing;
using Iot.Device.Button;
using Microsoft.Extensions.Logging;
using NFApp.Services.Extensions.DependencyAttribute;
using GC = nanoFramework.Runtime.Native.GC;

namespace NFApp.Services
{
    /// <summary>
    /// 板载按钮服务
    /// </summary>
    [SingletonDependency]
    public class ButtonService
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
            button.Press += button_Press;
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

            GC.Run(true);
        }
    }
}
