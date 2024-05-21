using Autabee.Communication.RosClient;
using Autabee.Communication.RosClient.Dto;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RosSharp.RosBridgeClient;

namespace Autabee.RosScout.WasmHostApi.Hubs
{
    public class RosBridgeService : BackgroundService
    {
        private readonly ILogger<RosService> _logger;
        private readonly IHubContext<RosBridgeHub> _hubContext;
        private readonly RosBridge _rosBridge;
        private readonly Dictionary<string, RosProfile> _rosConnectors;

        public RosBridgeService(ILogger<RosService> logger, IHubContext<RosBridgeHub> hubContext, RosBridge rosBridge)
        {
            _logger = logger;
            _hubContext = hubContext;
            _rosBridge = rosBridge;

            _rosBridge.SubscriptionMsgUpdate += SubscriptionMsgUpdate;
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
        public void SubscriptionMsgUpdate(SubscriptionValueRecord obj)
        {
            _hubContext.Clients.All.SendAsync(nameof(SubscriptionMsgUpdate), obj);
        }

        //public void SubscriptionUpdate(string topic, string data, string rosMsgType)
        //{
        //    _hubContext.Clients.All.SendAsync(nameof(SubscriptionUpdate), topic, data, rosMsgType);
        //}
    }
}
