using Autabee.Communication.RosClient;
using RosSharp.RosBridgeClient;

namespace Autabee.RosScout.WasmHostApi.Hubs
{
    public class RosBridge
    {
        readonly IEnumerable<RosSettings> rosSettings;

        readonly Dictionary<string, AutabeeRosSocket> rosSocket = new Dictionary<string, AutabeeRosSocket>();
        readonly Dictionary<string, RosConnector> rosConnectors = new Dictionary<string, RosConnector>();  

        public event Action<string, Message> SubscriptionUpdate;


        public RosBridge(IEnumerable<RosSettings> rosSettings)
        {
            this.rosSettings = rosSettings;
            this.rosSocket = new Dictionary<string, AutabeeRosSocket>();
            foreach (var item in this.rosSettings)
            {
                var bridge = new AutabeeRosSocket(item.Bridge);
                rosSocket.Add(item.Name, bridge);

                var bridge2 = new RosConnector(item.Bridge);
                rosConnectors.Add(item.Name, bridge2);
                //bridge.TypeMapping.Add(RosSharpTypeMappings.GetStdTypeMapping());
            }
        }

        public async void Subscribe(string caller,string hostName, string topic)
        {
            if (!rosSocket.TryGetValue(hostName, out AutabeeRosSocket rosConnector))
            {
                return;
            }

            var host = rosSettings.FirstOrDefault(o => o.Name == hostName);

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

                //var connection = rosConnector.Subscribe(topicTypes, _SubscriptionUpdate);

                return;
            }
            catch (Exception ex)
            {
                return;
            }

        }


        public async void Publish(string caller, string hostName, string topic, Message msg)
        {
            if (!rosSocket.TryGetValue(hostName, out AutabeeRosSocket rosConnector))
            {
                return;
            }

            try
            {
                var (id, data) = rosConnector.Publish(topic, msg, true);
                rosConnectors[hostName].Publish(topic, msg);


                //var connection = rosConnector.Subscribe(topicTypes, _SubscriptionUpdate);

                return;
            }
            catch (Exception ex)
            {
                return;
            }

        }

        private void _SubscriptionUpdate(string topic, Message data)
        {
            Console.WriteLine($"Topic: {topic} Data: {data}");
            SubscriptionUpdate?.Invoke(topic, data);
        }

        public void Unsubscribe(string topic)
        {
        }
    }
}
