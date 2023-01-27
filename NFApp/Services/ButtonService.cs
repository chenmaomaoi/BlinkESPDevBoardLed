using System;
using System.Collections.Generic;
using System.Text;
using Iot.Device.Button;
using Microsoft.Extensions.Logging;
using nanoFramework.Hosting;
using NFApp.DependencyAttribute;
using UnitsNet.Units;

namespace NFApp.Services
{
    /// <summary>
    /// 板载按钮服务
    /// </summary>
    [SingletonDependencyAttribute]
    public class ButtonService
    {
        private readonly GpioButton _button;
        private readonly LEDBlinkService _ledBlink;
        private readonly ILogger _logger;
        private int times;

        public ButtonService(HardwareService hardware, LEDBlinkService ledBlink, ILogger logger)
        {
            _button = hardware.Button;
            _ledBlink = ledBlink;
            _logger = logger;

            times = 0;

            _button.Press += button_Press;
        }

        private void button_Press(object sender, EventArgs e)
        {
            times++;

            _ledBlink.StartBlinkAsync(times * 10, 1000);
        }
    }
}
