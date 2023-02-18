using nanoFramework.Hardware.Esp32;

namespace NFApp
{
    public static class GPIOConfigs
    {
        /// <summary>
        /// 板载按钮
        /// </summary>
        public const int OnBoardButton = Gpio.IO00;

#if DEV
        /// <summary>
        /// 板载LED灯
        /// </summary>
        public const int OnBoardLigth = Gpio.IO02;
#endif
#if S2_DEV
        /// <summary>
        /// 板载RGB灯
        /// </summary>
        public const int OnBoardLigth = Gpio.IO18;
#endif

    }
}
