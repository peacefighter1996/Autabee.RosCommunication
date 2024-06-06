using Autabee.Communication.RosClient;
using Autabee.Communication.RosClient.Dto;
using Autabee.Communication.RosClient.XmlRpc;
using Autabee.RosScout.WasmHostApi.Hubs;
using Autabee.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autabee.WasmHostApi.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
        private readonly RosSettings settings;
        readonly RosBridge rosBridge;
        public CommunicationController(RosSettings settings, RosBridge rosBridge)
        {
            this.rosBridge = rosBridge;
            this.settings = settings;

        }

        [HttpGet("getProfileConnectionState/{robotName}")]
        public async Task<IActionResult> GetConnectedState(string robotName)
        {
            if (!settings.Profiles.Any(r => r.Name == robotName))
            {
                return NotFound();
            }
            ProfileConnectionState profile = await GetConnectionState(robotName);
            return Ok(profile);
        }

        private async Task<ProfileConnectionState> GetConnectionState(string robotName)
        {
            
            var bridge = rosBridge.GetConnectionState(robotName);
            ProfileConnectionState profile = new ProfileConnectionState(robotName, false, bridge);
            var uri = new Uri(settings.Profiles.First(o => o.Name == robotName).Master);
            using (TcpClient tcpClient = new TcpClient())
            {
                IPAddress[] ip = new IPAddress[0];
                try
                {
                    ip = Dns.GetHostAddresses(uri.Host);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    profile = new ProfileConnectionState(robotName, false, bridge);
                }
                foreach (var item in ip)
                {
                    Console.WriteLine($"testing {item.AddressFamily} {item}");
                    try
                    {
                        var t = tcpClient.ConnectAsync(uri.Host, uri.Port);
                        await t.WaitAsync(TimeSpan.FromSeconds(3));
                        // Wait 100 ms for the task to complete.
                        profile = new ProfileConnectionState(robotName, true, bridge);
                        if (tcpClient.Connected)
                        {
                            tcpClient.Close();
                        }
                    }
                    catch (TimeoutException e)
                    {
                        profile = new ProfileConnectionState(robotName, false, bridge);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e.Message);
                        profile = new ProfileConnectionState(robotName, false, bridge);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        profile = new ProfileConnectionState(robotName, false, bridge);
                    }

                    if (profile.master)
                    {
                        break;
                    }
                }
                
            }
            
            return profile;
        }

        [HttpGet("getProfileConnectionStates/")]
        public async Task<IActionResult> GetBridgeConnected()
        {
            var connections = new ProfileConnectionState[settings.Profiles.Count];

            for (int i = 0; i < settings.Profiles.Count; i++)
            {
                connections[i] = await GetConnectionState(settings.Profiles[i].Name);
            }

            return Ok(connections);
        }



    }
}
