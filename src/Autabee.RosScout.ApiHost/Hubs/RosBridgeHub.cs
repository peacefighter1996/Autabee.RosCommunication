using Autabee.Communication.RosClient;
using Autabee.Communication.RosClient.Dto;
using Autabee.Communication.RosClient.XmlRpc;
using Autabee.WasmHostApi.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using RosSharp.RosBridgeClient;
using std_msgs =RosSharp.RosBridgeClient.MessageTypes.Std;
using System.Collections;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;

namespace Autabee.RosScout.WasmHostApi.Hubs
{

    public class RosBridgeHub:Hub
    {
        RosBridge rosBridge;

        public RosBridgeHub(RosBridge bridge)
        {
            rosBridge = bridge;
        }

        public async void Subscribe(string hostName, string topic) 
            => rosBridge.Subscribe(Clients.Caller.ToString(),hostName, topic);
        public async void ActiveSubscribe(string hostName, string topic, string rosMsgName)
            => rosBridge.Subscribe(Clients.Caller.ToString(), hostName, topic, rosMsgName);

        public void Unsubscribe(string topic) => rosBridge.Unsubscribe(topic);


        public void Publish(string hostName, string topic, string message)
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(message, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
            Console.WriteLine(obj.GetType());
            if (obj.GetType().IsSubclassOf(typeof(Message)))
            {
                rosBridge.Publish(Clients.Caller.ToString(), hostName, topic, (Message)obj);
            }
            else
            {
                //rosBridge.Publish(Clients.Caller.ToString(), hostName, topic, message);
            }
        }
    }
}
