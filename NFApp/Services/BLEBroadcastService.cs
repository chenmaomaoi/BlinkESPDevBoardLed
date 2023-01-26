using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Diagnostics;
using System.Text;
using nanoFramework.Hosting;

namespace NFApp.Services
{
    public class BLEBroadcastService : SchedulerService
    {
        private readonly HardwareService _hardwareDevices;

        public BLEBroadcastService(HardwareService hardwareDevices) : base(TimeSpan.FromSeconds(5))
        {
            _hardwareDevices = hardwareDevices;

            if (!_hardwareDevices.BLESerialPort.IsOpen)
            {
                _hardwareDevices.BLESerialPort.Open();
            }
        }

        protected override void ExecuteAsync()
        {
            if (_hardwareDevices.BLEState.Read() == PinValue.High)
            {
                _hardwareDevices.BLESerialPort.WriteLine(
                    _hardwareDevices.SHT30Sensor.Temperature.DegreesCelsius.ToString("f1"));
            }
            Debug.WriteLine(_hardwareDevices.SHT30Sensor.Temperature.DegreesCelsius.ToString("f1"));
        }
    }
}
