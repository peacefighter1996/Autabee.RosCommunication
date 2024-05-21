using RosSharp.RosBridgeClient;

namespace Autabee.Communication.RosClient
{
    public interface IRosBridge
    {
        public event RosBridgeSubscriptionHandler SubscriptionMsgUpdate;
    }


    public class RosSettings
    {
        public List<RosProfile> Profiles { get; set; } = new List<RosProfile>();
    }
    public class RosProfile
    {
        public string Version { get; set; } = "Noetic";
        public string Bridge { get; set; } = string.Empty;
        public string Master { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

    }

    public delegate void RosBridgeSubscriptionHandler(SubscriptionValueRecord record);
    public record SubscriptionValueRecord(string Id, string Topic, string Profile, string RosName, string Data);
    public class RosBridgeSubscription : Subscriber
    {

        public override string Id { get; }

        public override string Topic { get; }

        public override Type TopicType { get; }

        public override Subscription Subscription { get; }

        public string rosName { get; }

        private string Profile { get; }

        public RosBridgeSubscriptionHandler SubscriptionHandler { get; }

        public RosBridgeSubscription(string rosName, string id, string topic, string profile, RosBridgeSubscriptionHandler subscriptionHandler, int throttle_rate = 0, int queue_length = 1, int fragment_size = int.MaxValue, string compression = "none")
        {
            Id = id;
            Topic = topic;
            SubscriptionHandler = subscriptionHandler;
            this.rosName = rosName;
            this.Profile = profile;

            Subscription = new Subscription(id, topic, rosName, throttle_rate, queue_length, fragment_size, compression);

        }

        public override void Receive(string message, ISerializer serializer)
        {
            SubscriptionHandler?.Invoke(new SubscriptionValueRecord(Id, Topic, Profile, rosName, message));
        }
    }
}
