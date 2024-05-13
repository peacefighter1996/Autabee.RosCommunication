using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RosSharp.RosBridgeClient;

namespace Autabee.RosScout.WasmHostApi.Hubs
{
    public interface IRosView
    {
        void SubscriptionUpdate(string topic, Message message);
    }

    public class RosService : BackgroundService, IRosView
    {
        private readonly ILogger<RosService> _logger;
        private readonly IHubContext<RosBridgeHub> _hubContext;
        private readonly RosBridge _rosBridge;
        private readonly Dictionary<string, RosSettings> _rosConnectors;

        public RosService(ILogger<RosService> logger, IHubContext<RosBridgeHub> hubContext, RosBridge rosBridge)
        {
            _logger = logger;
            _hubContext = hubContext;
            _rosBridge = rosBridge;

            _rosBridge.SubscriptionUpdate += SubscriptionUpdate;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public void SubscriptionUpdate(string topic, Message message)
        {
            _hubContext.Clients.All.SendAsync(nameof(SubscriptionUpdate), topic, JsonConvert.SerializeObject(message));
        }
    }
}
