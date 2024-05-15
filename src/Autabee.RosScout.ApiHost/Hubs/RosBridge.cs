using Autabee.Communication.RosClient;
using RosSharp.RosBridgeClient;

namespace Autabee.RosScout.WasmHostApi.Hubs
{
    public class RosBridge
    {
        readonly RosSettings rosSettings;

        readonly Dictionary<string, RosSocket> rosSocket = new Dictionary<string, RosSocket>();
        public event Action<string, string, string> SubscriptionUpdate;

        public List<string> DisconnectedSockets = new List<string>();

        public RosBridge(RosSettings rosSettings)
        {
            this.rosSettings = rosSettings;
            this.rosSocket = new Dictionary<string, RosSocket>();
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

        public async void Subscribe(string caller,string hostName, string topic)
        {
            if (!rosSocket.TryGetValue(hostName, out RosSocket socket))
            {
                return;
            }

            var host = rosSettings.Profiles.FirstOrDefault(o => o.Name == hostName);

            try
            {
                var result = await CallExecutor.Execute(host.Master,
                                       () => CallBuilder.GetTopicTypes(caller),
                                                          ResponseParser.GetTopics);
                if (!result.Success)
                {
                    return;
                }

                var topicTypes = result.Object.FirstOrDefault(o => o.Name == topic);
                if (topicTypes == null)
                {
                    return;
                }

                var connection = socket.Subscribe(topicTypes.Type, topic, parseData);

                return;
            }
            catch (Exception ex)
            {
                return;
            }

        }
        public void Subscribe(string? caller, string hostName, string topic, string rosMsgName)
        {
            if (!rosSocket.TryGetValue(hostName, out RosSocket socket))
            {
                return;
            }

            var host = rosSettings.Profiles.FirstOrDefault(o => o.Name == hostName);

            try
            { 

                var connection = socket.Subscribe(rosMsgName, topic, parseData);

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }


        public async void Publish(string caller, string hostName, string topic, Message msg)
        {
            if (!rosSocket.TryGetValue(hostName, out RosSocket socket))
            {
                return;
            }

            try
            {
                socket.Publish(topic, msg);


                //var connection = rosConnector.Subscribe(topicTypes, _SubscriptionUpdate);

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

        private void parseData(string topic, string data, string rosMsgType)
        {
            Console.WriteLine($"Topic: {topic}, Data: {data}, msg_type:{rosMsgType}");
            SubscriptionUpdate?.Invoke(topic, data, rosMsgType);
        }

        public void Unsubscribe(string topic)
        {
        }
        public void Connect(string item)
        {
            rosSocket[item].Connect();
        }
    }
}
