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


        public void Unsubscribe(string topic) => rosBridge.Unsubscribe(topic);

        public void Publish(string hostName, string topic) 
            => rosBridge.Publish(Clients.Caller.ToString(), hostName, topic, new std_msgs.String() { data = "data" });
    }
}
