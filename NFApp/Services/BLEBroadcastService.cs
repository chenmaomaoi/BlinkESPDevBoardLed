using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using nanoFramework.Hosting;
using NFApp.DependencyAttribute;

namespace NFApp.Services
{
    /// <summary>
    /// 串口BLE广播服务
    /// </summary>
    [HostedDependencyAttribute]
    public class BLEBroadcastService : SchedulerService
    {
        private readonly HardwareService _hardware;
        private readonly ILogger _logger;

        public BLEBroadcastService(HardwareService hardwareDevices, ILogger logger)
            : base(TimeSpan.FromSeconds(5))
        {
            _hardware = hardwareDevices;
            _logger = logger;

            if (!_hardware.BLESerialPort.IsOpen)
            {
                _hardware.BLESerialPort.Open();
            }
        }

        protected override void ExecuteAsync()
        {
            if (_hardware.BLEState.Read() == PinValue.High)
            {
                _hardware.BLESerialPort.WriteLine(
                    _hardware.SHT30Sensor.Temperature.DegreesCelsius.ToString("f1"));
            }
            _logger.LogDebug(_hardware.SHT30Sensor.Temperature.DegreesCelsius.ToString("f1"));
        }
    }
}
