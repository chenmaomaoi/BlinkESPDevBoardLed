﻿using System;
using System.Device.Gpio;
using System.Drawing;
using System.Threading;
using Iot.Device.Ws28xx.Esp32;
using NFApp.Services.Extensions.DependencyAttribute;

namespace NFApp.Services
{
    /// <summary>
    /// 板载LED闪烁服务
    /// </summary>
    [SingletonDependency]
    public class LEDBlinkService
    {

#if DEV
        /// <summary>
        /// LED灯
        /// </summary>
        private GpioPin Light;
#endif
#if S2_DEV
        /// <summary>
        /// RGB灯
        /// </summary>
        private Ws28xx rgbLight;
#endif
        /// <summary>
        /// 停止信号
        /// </summary>
        private bool signalStop;

        /// <summary>
        /// 控制LED闪烁的线程
        /// </summary>
        private Thread executingThread;

        public LEDBlinkService( DeviceService device)
        {
#if DEV
            new GpioController().SetPinMode(device.Light.PinNumber, PinMode.Output);
            Light = device.Light;
#endif
#if S2_DEV
            rgbLight = new Ws2812c(device.Light.PinNumber, 1);
#endif

            signalStop = false;
        }

        /// <summary>
        /// 闪烁
        /// </summary>
        /// <param name="brightColour">亮灯颜色</param>
        /// <param name="brigth">亮灯时间</param>
        /// <param name="goOut">灭灯时间</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public void StartBlinkAsync(
#if S2_DEV
            Color brightColour,
#endif
            int brigth = 50, int goOut = 950)
        {
            if (brigth < 0) throw new ArgumentOutOfRangeException(nameof(brigth));
            if (goOut < 0) throw new ArgumentOutOfRangeException(nameof(goOut));

            if (executingThread != null)
            {
                StopBlink();
            }
            signalStop = false;

            executingThread = new Thread(() =>
            {
                int th_bright = brigth;
                int th_goOut = goOut;
#if S2_DEV
                Color color = brightColour;
#endif
                while (!signalStop)
                {
#if DEV
                    Light.Write(PinValue.High);
                    Thread.Sleep(brigth);
                    Light.Write(PinValue.Low);
                    Thread.Sleep(goOut);
#endif
#if S2_DEV
                    rgbLight.Image.SetPixel(0, 0, color);
                    rgbLight.Update();
                    Thread.Sleep(brigth);
                    rgbLight.Image.Clear();
                    rgbLight.Update();
                    Thread.Sleep(goOut);
#endif
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
#if DEV
            Light.Write(PinValue.Low);
#endif
#if S2_DEV
            rgbLight.Image.Clear();
            rgbLight.Update();
#endif
        }
    }
}
