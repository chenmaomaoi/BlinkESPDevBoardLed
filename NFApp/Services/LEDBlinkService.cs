using System;
using System.Device.Gpio;
using System.Threading;
using NFApp.DependencyAttribute;

namespace NFApp.Services
{
    /// <summary>
    /// 板载LED闪烁服务
    /// </summary>
    [SingletonDependency]
    public class LEDBlinkService
    {
        /// <summary>
        /// LED灯控制引脚。高电平点亮
        /// </summary>
        private readonly GpioPin led;

        /// <summary>
        /// 停止信号
        /// </summary>
        private bool signalStop;

        /// <summary>
        /// 控制LED闪烁的线程
        /// </summary>
        private Thread executingThread;

        public LEDBlinkService(HardwareService hardware)
        {
            led = hardware.LED;
            signalStop = false;
        }

        /// <summary>
        /// 闪烁
        /// </summary>
        /// <param name="brigth">亮灯时间</param>
        /// <param name="goOut">灭灯时间</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public void StartBlinkAsync(int brigth = 50, int goOut = 950)
        {
            if (brigth < 0 || goOut < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(brigth)},{nameof(goOut)}");
            }

            if (executingThread != null)
            {
                StopBlink();
            }
            signalStop = false;

            executingThread = new Thread(() =>
            {
                int th_bright = brigth;
                int th_goOut = goOut;
                while (!signalStop)
                {
                    led.Write(PinValue.High);
                    Thread.Sleep(th_bright);
                    led.Write(PinValue.Low);
                    Thread.Sleep(th_goOut);
                }
            });
            executingThread.Start();
        }

        /// <summary>
        /// 停止闪烁
        /// </summary>
        public void StopBlink()
        {
            signalStop = true;
            while (executingThread.ThreadState != ThreadState.Stopped)
            {
                Thread.Sleep(50);
            }
        }
    }
}
