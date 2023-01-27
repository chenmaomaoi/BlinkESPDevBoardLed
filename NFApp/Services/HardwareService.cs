using System;
using System.Device.Gpio;
using System.Device.I2c;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using Iot.Device.Bmxx80;
using Iot.Device.Button;
using Iot.Device.Sht3x;
using Microsoft.Extensions.Logging;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Networking;
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
        /// WiFi SSID
        /// </summary>
        private const string SSID = "CMCC-49-301";

        /// <summary>
        /// WIFI 密码
        /// </summary>
        private const string PASSWORD = "13852981072";

        private readonly ILogger _logger;

        /// <summary>
        /// 板载按钮
        /// </summary>
        public GpioButton Button;

        /// <summary>
        /// 板载LED灯
        /// </summary>
        public GpioPin LED;

        /// <summary>
        /// BLE连接状态指示信号
        /// </summary>
        /// <remarks> 高电平：已连接 </remarks>
        public GpioPin IsEnableSerialPortBroadcast;

        /// <summary>
        /// BLE串口 COM2
        /// </summary>
        public SerialPort BroadCastSerialPort;

        /// <summary>
        /// SHT30 温湿度传感器
        /// </summary>
        public Sht3x SHT30Sensor;

        /// <summary>
        /// BMP280 温度 气压传感器
        /// </summary>
        public Bmp280 BMP280Sencer;

        public HardwareService(ILogger logger)
        {
            _logger = logger;

            // I2C接口 busId=1
            Configuration.SetPinFunction(Gpio.IO21, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(Gpio.IO22, DeviceFunction.I2C1_CLOCK);

            // BLE串口 COM2
            Configuration.SetPinFunction(Gpio.IO16, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(Gpio.IO17, DeviceFunction.COM2_TX);
            BroadCastSerialPort = new("COM2", 115200);

            Button = new GpioButton(Gpio.IO00);
            GpioController _gpioController = new();
            LED = _gpioController.OpenPin(Gpio.IO02, PinMode.Output);
            IsEnableSerialPortBroadcast = _gpioController.OpenPin(Gpio.IO23, PinMode.Input);
            SHT30Sensor = new(I2cDevice.Create(new(1, (byte)I2cAddress.AddrLow, I2cBusSpeed.FastMode)));
            BMP280Sencer = new(I2cDevice.Create(new(1, Bmx280Base.SecondaryI2cAddress, I2cBusSpeed.FastMode)))
            {
                TemperatureSampling = Sampling.UltraHighResolution,
                PressureSampling = Sampling.UltraHighResolution,
                FilterMode = Iot.Device.Bmxx80.FilteringMode.Bmx280FilteringMode.X8
            };

            // 连接WiFi
            ConnectWifiAsync();
        }

        /// <summary>
        /// 连接WIFI
        /// </summary>
        public void ConnectWifi(string ssid = SSID, string password = PASSWORD)
        {
            if (!WifiNetworkHelper.ConnectDhcp(ssid, password, requiresDateTime: true))
            {
                // Something went wrong, you can get details with the ConnectionError property:
                Debug.WriteLine($"Can't connect to the network, error: {WifiNetworkHelper.Status}");
                if (WifiNetworkHelper.HelperException != null)
                {
                    _logger.LogInformation($"ex: {WifiNetworkHelper.HelperException}");
                }
            }
            else
            {
                _logger.LogInformation("WiFi Connected.");
            }

            _logger.LogInformation($"{DateTime.UtcNow.AddHours(8)}");
        }

        public void ConnectWifiAsync(string ssid = SSID, string password = PASSWORD)
        {
            new Thread(() => ConnectWifi(ssid, password)).Start();
        }
    }
}
