using System;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Wifi;
using System.Diagnostics;
using System.IO.Ports;
using Iot.Device.Bmxx80;
using Iot.Device.Sht3x;
using nanoFramework.Hardware.Esp32;

namespace NFApp.Services
{
    public class HardwareService
    {
        /// <summary>
        /// WiFi SSID
        /// </summary>
        private const string SSID = "CMCC-49-301";

        /// <summary>
        /// WIFI 密码
        /// </summary>
        private const string PASSWORD = "13852981072";

        /// <summary>
        /// 板载按钮
        /// </summary>
        public GpioPin Button;

        /// <summary>
        /// 板载LED灯
        /// </summary>
        public GpioPin LED;

        /// <summary>
        /// BLE连接状态指示信号
        /// </summary>
        /// <remarks> 高电平：已连接 </remarks>
        public GpioPin BLEState;

        /// <summary>
        /// BLE串口 COM2
        /// </summary>
        public SerialPort BLESerialPort;

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

            // BLE串口 COM2
            Configuration.SetPinFunction(Gpio.IO16, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(Gpio.IO17, DeviceFunction.COM2_TX);
            BLESerialPort = new("COM2", 115200);

            GpioController _gpioController = new();
            Button = _gpioController.OpenPin(Gpio.IO00, PinMode.Input);
            LED = _gpioController.OpenPin(Gpio.IO02, PinMode.Output);
            BLEState = _gpioController.OpenPin(Gpio.IO23, PinMode.Input);
            SHT30Sensor = new(I2cDevice.Create(new(1, (byte)I2cAddress.AddrLow, I2cBusSpeed.FastMode)));
            BMP280Sencer = new(I2cDevice.Create(new(1, Bmx280Base.SecondaryI2cAddress, I2cBusSpeed.FastMode)))
            {
                TemperatureSampling = Sampling.UltraHighResolution,
                PressureSampling = Sampling.UltraHighResolution,
                FilterMode = Iot.Device.Bmxx80.FilteringMode.Bmx280FilteringMode.X8
            };
        }

        /// <summary>
        /// 连接WIFI
        /// </summary>
        public static void ConnectWifi(string ssid = SSID, string password = PASSWORD)
        {
            WifiAdapter wifi = WifiAdapter.FindAllAdapters()[0];
            wifi.Disconnect();

            WifiConnectionResult result = wifi.Connect(ssid, WifiReconnectionKind.Manual, password);
            Debug.WriteLine(result.ConnectionStatus.ToString());
            Debug.WriteLine(DateTime.UtcNow.AddHours(8).ToString());
        }
    }
}
