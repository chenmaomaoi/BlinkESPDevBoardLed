using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Wifi;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.ReadResult;
using Iot.Device.Button;
using Iot.Device.CharacterLcd;
using Iot.Device.Common;
using Iot.Device.Sht3x;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Networking;

namespace NFApp
{
    public class Program
    {
        public static GpioPin led;
        //public static GpioButton button;
        public static GpioPin BLEState;

        public static Sht3x sht30sensor;
        public static Bmp280 bmp280sencer;
        //public static Lcd1602 lcd1602;
        public static SerialPort BLESerialPort;

        /// <summary>
        /// 配置GPIO
        /// </summary>
        private static void ConfigGPIO()
        {
            // LED指示灯
            GpioController gpioController = new();
            led = gpioController.OpenPin(Gpio.IO02, PinMode.Output);

            // BLE状态指示
            BLEState = gpioController.OpenPin(Gpio.IO23, PinMode.Input);

            // BLE串口
            Configuration.SetPinFunction(Gpio.IO16, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(Gpio.IO17, DeviceFunction.COM2_TX);

            // I2c接口
            Configuration.SetPinFunction(Gpio.IO21, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(Gpio.IO22, DeviceFunction.I2C1_CLOCK);
        }

        /// <summary>
        /// 配置外设
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void ConfigPeripherals()
        {
            // SHT30
            sht30sensor = new(I2cDevice.Create(new(1, (byte)I2cAddress.AddrLow, I2cBusSpeed.FastMode)));

            // BMP280
            bmp280sencer = new(I2cDevice.Create(new(1, Bmp280.SecondaryI2cAddress, I2cBusSpeed.FastMode)))
            {
                TemperatureSampling = Sampling.UltraHighResolution,
                PressureSampling = Sampling.UltraHighResolution,
                FilterMode = Iot.Device.Bmxx80.FilteringMode.Bmx280FilteringMode.X8
            };

            // 配置串口
            BLESerialPort = new SerialPort("COM2", 115200);
            BLESerialPort.Open();
        }

        private static void LedFlash()
        {
            led.Toggle();
            Thread.Sleep(10);
            led.Toggle();
        }

        public static void ConnectWifi()
        {
            const string Ssid = "CMCC-49-301";
            const string Password = "13852981072";

            WifiAdapter wifi = WifiAdapter.FindAllAdapters()[0];
            wifi.Disconnect();

            var v = wifi.Connect(Ssid, WifiReconnectionKind.Manual, Password);
            Debug.WriteLine(v.ConnectionStatus.ToString());
            Debug.WriteLine(DateTime.UtcNow.AddHours(8).ToString());
        }

        public static void Main()
        {
            ConnectWifi();
            ConfigGPIO();
            LedFlash();
            ConfigPeripherals();

            StringBuilder sb = new();

            while (true)
            {
                LedFlash();

                Bmp280ReadResult bmp280value = bmp280sencer.Read();
                bmp280sencer.TryReadAltitude(out var altValue);

                sb.Clear();
                sb.Append($"{sht30sensor.Temperature.DegreesCelsius.ToString("f1")}C ");
                sb.Append($"{bmp280value.Temperature.DegreesCelsius.ToString("f1")}C ");
                sb.Append($"{sht30sensor.Humidity.Percent.ToString("f1")}% ");
                sb.Append($"{bmp280value.Pressure.Hectopascals.ToString("f2")}hPa ");
                sb.Append($"{altValue.Meters.ToString("f2")}m ");

                Debug.WriteLine(sb.ToString());
                if (BLEState.Read() == PinValue.High)
                {
                    BLESerialPort.WriteLine(DateTime.UtcNow.AddHours(8).ToString());
                    BLESerialPort.WriteLine(sb.ToString());
                }

                sht30sensor.Heater = true;
                Thread.Sleep(10_000);
            }
        }
    }
}