using Autabee.Communication.RosClient;
using Autabee.Communication.RosClient.Dto;
using RosSharp.RosBridgeClient;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Autabee.RosScout.WasmHostApi.Hubs
{
    



    public class RosBridge : IRosBridge
    {
        readonly RosSettings rosSettings;

        public List<string> logs = new List<string>();

        readonly Dictionary<string, RosSocket> rosSocket = new Dictionary<string, RosSocket>();
        public event RosBridgeSubscriptionHandler SubscriptionMsgUpdate;

        public List<string> DisconnectedSockets = new List<string>();
        readonly JsonToRosMessageFactory messageFactory;
        public ILogger<RosBridge> logger;


        public RosBridge(RosSettings rosSettings, JsonToRosMessageFactory messageFactory, ILogger<RosBridge> logger)
        {
            this.messageFactory = messageFactory;
            this.rosSettings = rosSettings;
            this.rosSocket = new Dictionary<string, RosSocket>();
            this.logger = logger;
            
            foreach (var item in this.rosSettings.Profiles)
            {
                var bridge = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(item.Bridge), false);

                bridge.protocol.OnClosed += 
                    (sender, e) => DisconnectedSockets.Add(item.Name);
                bridge.protocol.OnConnected += 
                    (sender, e) => DisconnectedSockets.Remove(item.Name);

                DisconnectedSockets.Add(item.Name);


                rosSocket.Add(item.Name, bridge);
            }
        }

        public async Task<string> Subscribe(string profile, string topic)
        {
            if (!rosSocket.TryGetValue(profile, out RosSocket socket))
            {
                return string.Empty;
            }

            var host = rosSettings.Profiles.FirstOrDefault(o => o.Name == profile);

            try
            {
                // check if topic exist
                var result = await CallExecutor.Execute(host.Master,
                                       () => CallBuilder.GetTopicTypes("autabee_api"),
                                                          ResponseParser.GetTopics);
                if (!result.Success)
                {
                    return string.Empty;
                }

                var topicDetail = result.Object.FirstOrDefault(o => o.Name == topic);
                if (topicDetail == null)
                {
                    return string.Empty;
                }

                var connection = socket.AddSubscription(topic, (string id) => new RosBridgeSubscription(topicDetail.Type, id, topicDetail.Name, profile, parseData, throttle_rate:1, queue_length:1));

                return connection;
            }
            catch (Exception ex)
            {
                return ex.Message;  
            }

        }
        public string Subscribe(string profile, string topic, string rosMsgName)
        {
            if (!rosSocket.TryGetValue(profile, out RosSocket socket))
            {
                return string.Empty;
            }

            var host = rosSettings.Profiles.FirstOrDefault(o => o.Name == profile);

            try
            { 
                var connection = socket.AddSubscription(topic, (string id) => new RosBridgeSubscription(rosMsgName,  id, topic, profile, parseData, throttle_rate: 1, queue_length: 1));

                return connection;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async void Publish(string hostName, string topic, Message msg)
        {
            if (!rosSocket.TryGetValue(hostName, out RosSocket socket))
            {
                return;
            }

            try
            {
                socket.Publish(topic, msg);
                return;
            }
            catch (System.Collections.Generic.KeyNotFoundException _)
            {
                try
                {
                    socket.Advertise(msg.GetType(), topic);
                    socket.Publish(topic, msg);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }

        }
        private void parseData(SubscriptionValueRecord msg)
        {
            SubscriptionMsgUpdate?.Invoke(msg);
        }

        public void Unsubscribe(string hostName, string topic)
        {
            if (!rosSocket.TryGetValue(hostName, out RosSocket socket))
            {
                return;
            }

            socket.Unsubscribe(topic);

        }
        public void Connect(string item)
        {
            rosSocket[item].Connect();
        }
        public void Publish(RosProfilePublish msg)
        {
            if (!rosSocket.TryGetValue(msg.profile, out RosSocket socket))
            {
                return;
            }

            try
            {
                var message = messageFactory.GetMessage(msg.topic.Type, msg.json);
                if (!message.Success)
                {
                    return;
                }

                Publish(msg.profile, msg.topic.Name, message.Object);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message);
                return;
            }
        }
    }
}
