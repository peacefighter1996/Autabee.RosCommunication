using RosSharp.RosBridgeClient;
using System.Text.Json;

namespace Autabee.Communication.RosClient.Msg
{

    public static class Serialization
    {
        public static Message JsonDeserialize(string json, Type type)
        {
            return (Message)JsonSerializer.Deserialize(json, type);
        }

        public static string DefaultStringSerialization(Type type, JsonSerializerOptions options)
        {
            var message = Activator.CreateInstance(type);
            return JsonSerializer.Serialize(message, options);
        }
    }
    public interface IMsgBuilders
    {
        Dictionary<string, Func<string, Message>> JsonDeserializer { get; }
        Dictionary<string, Func<JsonSerializerOptions, string>> DefaultJsonStringBuilder { get; }
    }
}
