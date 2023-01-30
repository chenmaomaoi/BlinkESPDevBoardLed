using System;
using System.Device.Gpio;
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
        private readonly HardwareService hardware;
        private readonly ILogger logger;

        public SerialPortBroadcastService(HardwareService hardwareDevices, ILoggerFactory loggerFactory)
            : base(TimeSpan.FromSeconds(1))
        {
            hardware = hardwareDevices;
            logger = loggerFactory.CreateLogger(nameof(ButtonService));

            if (!hardware.SerialPortCOM2.IsOpen)
            {
                hardware.SerialPortCOM2.Open();
            }
        }

        protected override void ExecuteAsync()
        {
            if (hardware.isConnectedBLESerialPortCOM2.Read() == PinValue.High)
            {
                Bmp280ReadResult bmp280Value = hardware.BMP280Sencer.Read();
                SencerValues result = new()
                {
                    SHT30Sencer = new SencerValues.SHT30()
                    {
                        Temperature = (float)hardware.SHT30Sensor.Temperature.DegreesCelsius,
                        Humidity = (float)hardware.SHT30Sensor.Humidity.Percent
                    },
                    BMP280Sencer = new SencerValues.BMP280()
                    {
                        Temperature = (float)bmp280Value.Temperature.DegreesCelsius,
                        Pressure = (float)bmp280Value.Pressure.Hectopascals,
                        Altitude = (float)WeatherHelper.CalculateAltitude(bmp280Value.Pressure, bmp280Value.Temperature).Meters
                    }
                };
                hardware.SerialPortCOM2.WriteLine(JsonConvert.SerializeObject(result));
            }
        }
    }
}
