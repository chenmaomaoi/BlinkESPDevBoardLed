using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Diagnostics;
using System.Text;
using Iot.Device.Bmxx80.ReadResult;
using Iot.Device.Common;
using Microsoft.Extensions.Logging;
using nanoFramework.Hosting;
using nanoFramework.Json;
using NFApp.DependencyAttribute;
using NFApp.Dtos;

namespace NFApp.Services
{
    /// <summary>
    /// 串口广播服务
    /// </summary>
    [HostedDependency]
    public class SerialPortBroadcastService : SchedulerService
    {
        private readonly HardwareService _hardware;
        private readonly ILogger _logger;

        public SerialPortBroadcastService(HardwareService hardwareDevices, ILogger logger)
            : base(TimeSpan.FromSeconds(1))
        {
            _hardware = hardwareDevices;
            _logger = logger;

            if (!_hardware.BroadCastSerialPort.IsOpen)
            {
                _hardware.BroadCastSerialPort.Open();
            }
        }

        protected override void ExecuteAsync()
        {
            if (_hardware.IsEnableSerialPortBroadcast.Read() == PinValue.High)
            {
                Bmp280ReadResult bmp280Value = _hardware.BMP280Sencer.Read();
                SencerValues result = new()
                {
                    SHT30Sencer = new SencerValues.SHT30()
                    {
                        Temperature = (float)_hardware.SHT30Sensor.Temperature.DegreesCelsius,
                        Humidity = (float)_hardware.SHT30Sensor.Humidity.Percent
                    },
                    BMP280Sencer = new SencerValues.BMP280()
                    {
                        Temperature = (float)bmp280Value.Temperature.DegreesCelsius,
                        Pressure = (float)bmp280Value.Pressure.Hectopascals,
                        Altitude = (float)WeatherHelper.CalculateAltitude(bmp280Value.Pressure, bmp280Value.Temperature).Meters
                    }
                };
                _hardware.BroadCastSerialPort.WriteLine(JsonConvert.SerializeObject(result));
                _logger.LogDebug(JsonConvert.SerializeObject(result));
            }
            _logger.LogDebug(_hardware.SHT30Sensor.Temperature.DegreesCelsius.ToString("f1 C"));
        }
    }
}
