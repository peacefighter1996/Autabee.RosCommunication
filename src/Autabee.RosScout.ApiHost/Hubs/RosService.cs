using Autabee.Communication.RosClient;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RosSharp.RosBridgeClient;

namespace Autabee.RosScout.WasmHostApi.Hubs
{
    public class RosService : BackgroundService
    {
        private readonly ILogger<RosService> _logger;
        private readonly IHubContext<RosBridgeHub> _hubContext;
        private readonly RosBridge _rosBridge;
        private readonly Dictionary<string, RosProfile> _rosConnectors;

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
                // Reconnection logic
                foreach (var item in _rosBridge.DisconnectedSockets)
                {
                    _rosBridge.Connect(item);
                }
            }
        }

        public void SubscriptionUpdate(string topic, string data, string rosMsgType)
        {
            _hubContext.Clients.All.SendAsync(nameof(SubscriptionUpdate), topic, data, rosMsgType);
        }
    }
}
