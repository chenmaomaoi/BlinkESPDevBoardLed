using System.Drawing;
using nanoFramework.Hosting;
using NFApp.Services.Extensions.DependencyAttribute;

namespace NFApp.Services
{
    [HostedDependency]
    public class ApplicationService : IHostedService
    {
        private readonly LEDBlinkService ledBlink;

        public ApplicationService(ButtonService buttonService, LEDBlinkService ledBlink)
        {
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
