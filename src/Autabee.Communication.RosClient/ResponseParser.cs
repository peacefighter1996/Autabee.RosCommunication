using Autabee.Communication.RosClient.Dto;
using Autabee.Communication.RosClient.XmlRpc;
using Autabee.Utility.Messaging;
using Autabee.Utility.Messaging.Validation;

namespace Autabee.Communication.RosClient
{
    public static class ResponseParser
    {
        public static ValidationResult<RosTopic[]> GetTopics(MethodResponse message) => Parse(message, ParseTopic);

        private static RosTopic[] ParseTopic(MethodResponse msg)
        {
            var topics = new List<RosTopic>();
            foreach (var item in msg.Params.Param[0].Value.Array.Data.Value[2].Array.Data.Value)
            {
                topics.Add(new RosTopic(item.Array.Data.Value[0].String, item.Array.Data.Value[1].String));
            }
            return topics.ToArray();
        }

        public static ValidationResult<RosSystem> GetSystemState(MethodResponse message) => Parse(message, ParseSystemState);

        private static RosSystem ParseSystemState(MethodResponse msg)
        {
            var tmp = msg.Params.Param[0].Value.Array.Data.Value[2].Array.Data.Value;
            var publishers = ParseService(tmp[0], RosConnectionType.Publisher);
            var subscribers = ParseService(tmp[1], RosConnectionType.Subscriber);
            var services = ParseService(tmp[2], RosConnectionType.ServiceProvider);
            return new RosSystem(publishers, subscribers, services);
        }
        static RosConnection[] ParseService(Value value, RosConnectionType rosConnectionType)
        {
            RosConnection[] connections = new RosConnection[value.Array.Data.Value.Count];
            for (int i = 0; i < value.Array.Data.Value.Count; i++)
            {
                var nodes = new List<string>();
                var connection = value.Array.Data.Value[i];
                var nodesOrg = connection.Array.Data.Value[1].Array.Data.Value;


                for (int j = 0; j < nodesOrg.Count; j++)
                {
                    nodes.Add(nodesOrg[j].String);
                }
                connections[i] = new RosConnection(connection.Array.Data.Value[0].String, rosConnectionType, nodes);
            }

            return connections;
        }

        public static ValidationResult<string> LookupService(MethodResponse responce) => Parse(responce, ParseUri);
        public static ValidationResult<string> LookupNode(MethodResponse message) => Parse(message, ParseUri);
        public static ValidationResult<string> GetUri(MethodResponse message) => Parse(message, ParseUri);

        private static string ParseUri(MethodResponse msg)
        {
            return msg.Params.Param[0].Value.Array.Data.Value[2].String;
        }

        private static ValidationResult<T> Parse<T>(MethodResponse message, Func<MethodResponse, T> func)
        {
            try
            {
                if (message.Params.Param[0].Value.Array.Data.Value[0].Int == 1)
                {
                    return new ValidationResult<T>(Object: func(message));
                }
                else
                {
                    return new ValidationResult<T>(failInfo: new List<Message>() { new Message(MessageLevel.Fatal, message.Params.Param[0].Value.Array.Data.Value[1].String) });
                }

            }
            catch (Exception ex)
            {
                return new ValidationResult<T>(failInfo: new List<Message>() { new Message(MessageLevel.Fatal, ex.Message) });
            }
        }
    }
}