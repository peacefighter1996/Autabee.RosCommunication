using Autabee.Communication.RosClient.Msg;
using Autabee.Utility.Messaging.Validation;
using RosSharp.RosBridgeClient;
using System;
using System.Linq;
using System.Text.Json;
using std_msg = RosSharp.RosBridgeClient.MessageTypes.Std;


namespace Autabee.Communication.RosClient
{
    public class JsonToRosMessageFactory
    {
        public JsonToRosMessageFactory()
        {
            AddBuilders(new StdBuilders());
            AddBuilders(new GeometryBuilder());
        }
        public JsonToRosMessageFactory(IEnumerable<IMsgBuilders> msgBuilders): base()
        {
            foreach (var builder in msgBuilders)
            {
                AddBuilders(builder);
            }
        }

        private void AddBuilders(IMsgBuilders stdBuilders)
        {
            foreach (var item in stdBuilders.JsonDeserializer)
            {
                Builder.Add(item.Key, item.Value);
            }
        }

        private Dictionary<string, Func<string, Message>> Builder = new Dictionary<string, Func<string, Message>>();

        public ValidationResult<Message> GetMessage(string rosName, string json)
        {
            if (Builder.TryGetValue(rosName, out var func))
            {
                return new ValidationResult<Message>(true, func(json));
            }

            return new ValidationResult<Message>(false, failInfo: new List<Utility.Messaging.Message>() {
                new Utility.Messaging.Message(Utility.Messaging.MessageLevel.Error, "Could not find ros msg type [{}] in factory [{}]",  rosName, nameof(JsonToRosMessageFactory) )
            });
        }
    }

    public class JsonStringRosMessageFactory
    {
        public JsonSerializerOptions Options { get; set; } = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private Dictionary<string, Func<JsonSerializerOptions, string>> Builder = new Dictionary<string, Func<JsonSerializerOptions, string>>();

        public JsonStringRosMessageFactory()
        {
            AddBuilders(new StdBuilders());
            AddBuilders(new GeometryBuilder());
        }

        public JsonStringRosMessageFactory(IEnumerable<IMsgBuilders> msgBuilders) : base()
        {
            foreach (var builder in msgBuilders)
            {
                AddBuilders(builder);
            }
        }

        private void AddBuilders(IMsgBuilders stdBuilders)
        {
            foreach (var item in stdBuilders.DefaultJsonStringBuilder)
            {
                Builder.Add(item.Key, item.Value);
            }
        }

        public Func<string> BuildDefault(string rosMsgType) 
        { 
            if(Builder.TryGetValue(rosMsgType, out var func))
            {
                return () => func(Options);
            }
            throw new Exception($"Could not find ros msg type [{rosMsgType}] in factory [{nameof(JsonStringRosMessageFactory)}]");
        }

        public IEnumerable<string> GetKeys()
        {
            return Builder.Keys;
        }
    }
}
