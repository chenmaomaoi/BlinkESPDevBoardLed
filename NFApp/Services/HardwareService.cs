using System.Device.Gpio;
using System.Device.I2c;
using System.IO.Ports;
using Iot.Device.Bmxx80;
using Iot.Device.Button;
using Iot.Device.Sht3x;
using nanoFramework.Hardware.Esp32;
using NFApp.DependencyAttribute;

namespace NFApp.Services
{
    /// <summary>
    /// 硬件服务
    /// </summary>
    [SingletonDependency]
    public class HardwareService
    {
        /// <summary>
        /// 板载按钮
        /// </summary>
        public GpioButton Button;

        /// <summary>
        /// 板载LED灯
        /// </summary>
        public GpioPin LED;

        /// <summary>
        /// COM2-BLE串口
        /// </summary>
        public SerialPort SerialPortCOM2;

        /// <summary>
        /// BLE 设备连接状态
        /// </summary>
        public GpioPin isConnectedBLESerialPortCOM2;

        /// <summary>
        /// SHT30 温湿度传感器
        /// </summary>
        public Sht3x SHT30Sensor;

        /// <summary>
        /// BMP280 温度 气压传感器
        /// </summary>
        public Bmp280 BMP280Sencer;

        public HardwareService()
        {
            // I2C接口 busId=1
            Configuration.SetPinFunction(Gpio.IO21, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(Gpio.IO22, DeviceFunction.I2C1_CLOCK);

            // COM2
            Configuration.SetPinFunction(Gpio.IO16, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(Gpio.IO17, DeviceFunction.COM2_TX);
            SerialPortCOM2 = new("COM2", 115200);

            Button = new GpioButton(Gpio.IO00);
            GpioController gpioController = new GpioController();
            LED = gpioController.OpenPin(Gpio.IO02, PinMode.Output);
            isConnectedBLESerialPortCOM2 = gpioController.OpenPin(Gpio.IO23, PinMode.Input);
            SHT30Sensor = new(I2cDevice.Create(new(1, (byte)I2cAddress.AddrLow, I2cBusSpeed.FastMode)));
            BMP280Sencer = new(I2cDevice.Create(new(1, Bmx280Base.SecondaryI2cAddress, I2cBusSpeed.FastMode)))
            {
                TemperatureSampling = Sampling.UltraHighResolution,
                PressureSampling = Sampling.UltraHighResolution,
                FilterMode = Iot.Device.Bmxx80.FilteringMode.Bmx280FilteringMode.Off
            };
        }
    }
}
