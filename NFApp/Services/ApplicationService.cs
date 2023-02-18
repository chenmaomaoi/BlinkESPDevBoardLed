using System;
using System.Drawing;
using Microsoft.Extensions.Logging;
using nanoFramework.Hosting;
using NFApp.Services.Extensions.DependencyAttribute;

namespace NFApp.Services
{
    [HostedDependency]
    public class ApplicationService : IHostedService
    {
        private readonly ILogger logger;
        private readonly LEDBlinkService ledBlink;

        public ApplicationService(ILoggerFactory loggerFactory, ButtonService buttonService, LEDBlinkService ledBlink)
        {
            logger = loggerFactory.CreateLogger(nameof(ButtonService));
            this.ledBlink = ledBlink;
        }

        public void Start()
        {
#if DEV
            ledBlink.StartBlinkAsync();
#endif
#if S2_DEV
            ledBlink.StartBlinkAsync(Color.Red);
#endif
        }

        public void Stop()
        {
        }
    }
}
