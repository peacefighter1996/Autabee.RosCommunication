using Autabee.Communication.RosClient;
using Autabee.Communication.RosClient.XmlRpc;
using Microsoft.AspNetCore.Mvc;
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
    public class RosMasterController : ControllerBase
    {
        #region getTopicTypes
        [HttpGet("getTopicTypes/")]
        public Task<IActionResult> GetTopicTypes()
            => GetTopicTypes("wa_host", "http://localhost:11311/");
        [HttpPost("getTopicTypes/")]
        public Task<IActionResult> GetTopicTypes_Host(string host)
            => GetTopicTypes("wa_host", host);
        [HttpGet("getTopicTypes/{callerId}")]
        public Task<IActionResult> GetTopicTypes(string callerId)
            => GetTopicTypes(callerId, "http://localhost:11311/");
        [HttpPost("getTopicTypes/{callerId}")]
        public async Task<IActionResult> GetTopicTypes(string callerId, string host)
        {
            try
            {
                MethodResponse responce = await CallExecutor.Execute(host, CallBuilder.GetTopicTypes(callerId));
                var result = ResponseParser.GetTopics(responce);
                return result.Success ? Ok(result.Object) : BadRequest(result.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion getTopicTypes

        #region lookupNode
        [HttpGet("lookupNode/{node}")]
        public Task<IActionResult> lookupNode(string node)
            => lookupNode("wa_host", node, "http://localhost:11311/");
        [HttpPost("lookupNode/{node}")]
        public Task<IActionResult> lookupNode_Host(string node, string host)
            => lookupNode("wa_host", node, host);
        [HttpGet("lookupNode/{callerId}/{node}")]
        public Task<IActionResult> lookupNode(string callerId, string node)
            => lookupNode(callerId, node, "http://localhost:11311/");
        [HttpPost("lookupNode/{callerId}/{node}")]
        public async Task<IActionResult> lookupNode(string callerId, string node, string host)
        {
            node = Regex.Replace(node, @"%2F", "/");
            var result = await CallExecutor.Execute(host, () => CallBuilder.LookupNode(callerId, node), ResponseParser.LookupNode );
            return result.Success ? Ok(result.Object) : BadRequest(result.ToString());
        }
        #endregion lookupNode

        #region lookupService
        [HttpGet("lookupService/{service}")]
        public Task<IActionResult> lookupService(string service)
            => lookupService("wa_host", service, "http://localhost:11311/");
        [HttpPost("lookupService/{service}")]
        public Task<IActionResult> lookupService_Host(string service, string host)
            => lookupService("wa_host", service, host);
        [HttpGet("lookupService/{callerId}/{service}")]
        public Task<IActionResult> lookupService(string callerId, string service)
            => lookupService(callerId, service, "http://localhost:11311/");
        [HttpPost("lookupService/{callerId}/{service}")]
        public async Task<IActionResult> lookupService(string callerId, string service, string host)
        {
            service = Regex.Replace(service, @"%2F", "/");
            try
            {
                MethodResponse responce = await CallExecutor.Execute(host, CallBuilder.LookupService(callerId, service));
                var result = ResponseParser.LookupService(responce);
                return result.Success ? Ok(result.Object) : BadRequest(result.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion lookupService

        #region getSystemState
        [HttpGet("getSystemState/")]
        public Task<IActionResult> getSystemState()
            => getSystemState("wa_host", "http://localhost:11311/");
        [HttpPost("getSystemState/")]
        public Task<IActionResult> getSystemState_Host(string host)
            => getSystemState("wa_host", host);
        [HttpGet("getSystemState/{callerId}")]
        public Task<IActionResult> getSystemState(string callerId)
            => getSystemState(callerId, "http://localhost:11311/");
        [HttpPost("getSystemState/{callerId}")]
        public async Task<IActionResult> getSystemState(string callerId, string host)
        {
            try
            {
                MethodResponse responce = await CallExecutor.Execute(host, CallBuilder.GetSystemState(callerId));
                var result = ResponseParser.GetSystemState(responce);
                return result.Success ? Ok(result.Object) : BadRequest(result.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion getSystemState

        #region getUri
        [HttpGet("getUri/")]
        public Task<IActionResult> getUri()
            => getUri("wa_host", "http://localhost:11311/");
        [HttpPost("getUri/")]
        public Task<IActionResult> getUri_Host(string host)
            => getUri("wa_host", host);
        [HttpGet("getUri/{callerId}")]
        public Task<IActionResult> getUri(string callerId)
            => getUri(callerId, "http://localhost:11311/");
        [HttpPost("getUri/{callerId}")]
        public async Task<IActionResult> getUri(string callerId, string host)
        {
            try
            {
                MethodResponse responce = await CallExecutor.Execute(host, CallBuilder.GetUri(callerId));
                var result = ResponseParser.GetUri(responce);
                return result.Success ? Ok(result.Object) : BadRequest(result.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion getUri

        #region getPublishedTopics
        [HttpGet("getPublishedTopics/")]
        public Task<IActionResult> getPublishedTopics()
            => getPublishedTopics("wa_host", "http://localhost:11311/", "");

        [HttpPost("getPublishedTopics/")]
        public Task<IActionResult> getPublishedTopics_Host(string host)
            => getPublishedTopics("wa_host", host, "");

        [HttpGet("getPublishedTopics/{callerId}")]
        public Task<IActionResult> getPublishedTopics(string callerId)
            => getPublishedTopics(callerId, "http://localhost:11311/", "");

        [HttpPost("getPublishedTopics/{callerId}")]
        public async Task<IActionResult> getPublishedTopics(string callerId, string host, string subgraph)
        {
            try
            {
                MethodResponse responce = await CallExecutor.Execute(host, CallBuilder.GetPublishedTopics(callerId, subgraph));
                var result = ResponseParser.GetTopics(responce);
                return result.Success ? Ok(result.Object) : BadRequest(result.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion getPublishedTopics
    }
}
