using Autabee.Communication.RosClient.Dto;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Moveit;


namespace Autabee.Communication.RosClient
{


    public class RosConnector
    {
        private Dictionary<string, string> topicAdvetiser = new Dictionary<string, string>();
        private Dictionary<string, string> subscriptionMapping = new Dictionary<string, string>();
        private List<string> subscriptions = new List<string>();
        private List<string> advertisements = new List<string>();
        private List<string> services = new List<string>();
        
        private RosSocket rosSocket;
        public Uri Uri { get; private set; }
        public RosConnector(string uri)
        {
            Uri = new Uri(uri);
            rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(uri));
            try { 
            rosSocket.Connect();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        ~RosConnector()
        {
            subscriptions.ForEach(rosSocket.Unsubscribe);
            advertisements.ForEach(rosSocket.Unadvertise);
            services.ForEach(rosSocket.UnadvertiseService);
            rosSocket.Close();
        }

        public string Subscribe<T>(string topic, SubscriptionHandler<T> handeler) where T : RosSharp.RosBridgeClient.Message
        {
            string subscription = rosSocket.Subscribe(topic, handeler);
            subscriptions.Add(subscription);
            return subscription;
        }

        //public string Subscribe(RosTopic topic, SubscriptionHandler2 handeler) 
        //{
        //    if (TypeMapping.TryGetValue(topic.Type, out Type dataType))
        //    {
        //        string subscription = rosSocket.Subscribe(dataType, topic.Name, handeler);
        //        subscriptions.Add(subscription);
        //        return subscription;
        //    }
        //    return string.Empty;
        //}

        //publish
        public bool Publish<T>(string topic, T message) where T : RosSharp.RosBridgeClient.Message
        {
            if (string.IsNullOrEmpty(topic))
            {
                return false;
                // return new ValidationResult(false, "topic is null or empty");
            }
            if (advertisements.Contains(topic))
            {
                rosSocket.Publish(topic, message);
            }
            else if (topicAdvetiser.TryGetValue(topic, out string? advertiser))
            {
                if (string.IsNullOrEmpty(advertiser))
                {
                    return false;
                    // return new ValidationResult(false, "advertiser is null or empty, something went wrong during setup of the topic");
                }
                rosSocket.Publish(advertiser, message);
            }
            else
            {
                try
                {
                    advertiser = rosSocket.Advertise<T>(topic);
                    topicAdvetiser.Add(topic, advertiser);
                    rosSocket.Publish(advertiser, message);
                }
                catch (Exception e)
                {
                    return false;
                    // return new ValidationResult(false, e.Message);
                }
            }

            return true;
            // return new ValidationResult(true);
        }
    }

}
