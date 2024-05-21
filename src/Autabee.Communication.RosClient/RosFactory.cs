using Autabee.Utility.Messaging.Validation;
using RosSharp.RosBridgeClient;
using System;
using System.Linq;
using System.Text.Json;
using std_msg = RosSharp.RosBridgeClient.MessageTypes.Std;


namespace Autabee.Communication.RosClient
{
    public class JsonRosFactory
    {
        static Message JsonDeserialize(string json, Type type)
        {
            return (Message)JsonSerializer.Deserialize(json, type);
        }

        public Dictionary<string, Func<string, Message>> Builder = new Dictionary<string, Func<string, Message>>()
        {
            { std_msg.Empty.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Empty)) },

            { std_msg.Bool.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Bool)) },
            { std_msg.Byte.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Byte)) },
            { std_msg.ByteMultiArray.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.ByteMultiArray)) },
            { std_msg.Char.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Char)) },
            { std_msg.String.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.String)) },
            { std_msg.ColorRGBA.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.ColorRGBA)) },
            { std_msg.Duration.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Duration)) },
            { std_msg.Time.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Time)) },

            { std_msg.Header.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Header)) },

            { std_msg.Int8.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Int8)) },
            { std_msg.Int16.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Int16)) },
            { std_msg.Int32.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Int32)) },
            { std_msg.Int64.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Int64)) },
            { std_msg.Int8MultiArray.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Int8MultiArray)) },
            { std_msg.Int16MultiArray.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Int16MultiArray)) },
            { std_msg.Int32MultiArray.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Int32MultiArray)) },
            { std_msg.Int64MultiArray.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Int64MultiArray)) },

            { std_msg.UInt8.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.UInt8))},
            { std_msg.UInt16.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.UInt16)) },
            { std_msg.UInt32.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.UInt32)) },
            { std_msg.UInt64.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.UInt64)) },
            { std_msg.UInt8MultiArray.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.UInt8MultiArray)) },
            { std_msg.UInt16MultiArray.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.UInt16MultiArray)) },
            { std_msg.UInt32MultiArray.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.UInt32MultiArray)) },
            { std_msg.UInt64MultiArray.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.UInt64MultiArray)) },

            { std_msg.Float32.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Float32)) },
            { std_msg.Float64.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Float64)) },
            { std_msg.Float32MultiArray.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Float32MultiArray)) },
            { std_msg.Float64MultiArray.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.Float64MultiArray)) },

            { std_msg.MultiArrayDimension.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.MultiArrayDimension)) },
            { std_msg.MultiArrayLayout.RosMessageName, (string json) => JsonDeserialize(json,typeof(std_msg.MultiArrayLayout)) },
        };

        public ValidationResult<Message> GetMessage(string rosName, string json)
        {
            if (Builder.TryGetValue(rosName, out var func))
            {
                return new ValidationResult<Message>(true, func(json));
            }

            return new ValidationResult<Message>(false, failInfo: new List<Utility.Messaging.Message>() {
                new Utility.Messaging.Message(Utility.Messaging.MessageLevel.Error, "Could not find ros msg type [{}] in factory [{}]",  rosName, nameof(JsonRosFactory) )
            });
        }
    }
}
