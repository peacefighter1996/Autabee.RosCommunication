using Autabee.Communication.RosClient;
using Autabee.Communication.RosClient.Dto;
using Autabee.Communication.RosClient.XmlRpc;
using Microsoft.AspNetCore.SignalR;
using RosSharp.RosBridgeClient;
using std_msgs =RosSharp.RosBridgeClient.MessageTypes.Std;
using System.Collections;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using RosSharp.RosBridgeClient.MessageTypes.Std;

namespace Autabee.RosScout.WasmHostApi.Hubs
{

    public class RosBridgeHub:Hub
    {
        RosBridge rosBridge;

        public RosBridgeHub(RosBridge bridge)
        {
            rosBridge = bridge;
        }

        public async Task<string> Subscribe(string hostName, string topic) 
            => await rosBridge.Subscribe(hostName, topic);
        public string ActiveSubscribe(string hostName, string topic, string rosMsgName)
            => rosBridge.Subscribe(hostName, topic, rosMsgName);

        public void UnSubscribe(string hostname, string topic) => rosBridge.Unsubscribe(hostname, topic);


        public void Publish(string hostName, string topic, string message)
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(message, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
            Console.WriteLine(obj.GetType());
            if (obj.GetType().IsSubclassOf(typeof(Message)))
            {
                rosBridge.Publish(hostName, topic, (Message)obj);
            }
            else
            {
                //rosBridge.Publish(Clients.Caller.ToString(), hostName, topic, message);
            }
        }

        public void ProfilePublish(RosProfilePublish msg)
        {
            rosBridge.Publish(msg);
        }
    }
}
