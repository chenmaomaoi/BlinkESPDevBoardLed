using System.Device.Gpio;
using Iot.Device.Button;
using NFApp.DependencyAttribute;

namespace NFApp.Services
{
    [SingletonDependency]
    public class DeviceService
    {
        /// <summary>
        /// 板载指示灯
        /// </summary>
        /// <remarks>
        /// <para>在ESP32开发板中，是单色LED</para>
        /// <para>在ESP32 S2开发板中，是RGB LED</para>
        /// </remarks>
        public GpioPin Light;

        /// <summary>
        /// 板载按钮
        /// </summary>
        public GpioButton Button;

        public DeviceService()
        {
            Light = new GpioController().OpenPin(GPIOConfigs.OnBoardLigth);
            Button = new GpioButton(GPIOConfigs.OnBoardButton);
        }
    }
}
