using System;
using System.Drawing;
using Iot.Device.Button;
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
        private readonly GpioButton button;
        private readonly LEDBlinkService ledBlink;
        private bool isEnable;

        public ButtonService(DeviceService device, LEDBlinkService ledBlink)
        {
            button = device.Button;
            this.ledBlink = ledBlink;
            isEnable = true;
            button.Press += button_Press;
        }

        private void button_Press(object sender, EventArgs e)
        {
            Logger.Log("Button Press");
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
