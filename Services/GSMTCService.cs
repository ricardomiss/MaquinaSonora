using Windows.Media.Control;
namespace MaquinaSonora.Services
{
    public class GSMTCService : IMediaService
    {
        private GlobalSystemMediaTransportControlsSessionManager _manager;

        public async Task Initialize()
        {
            _manager = await GlobalSystemMediaTransportControlsSessionManager
                .RequestAsync();
            Console.WriteLine("GSMTCService initialized." + _manager.GetCurrentSession());
        }

    }
}
