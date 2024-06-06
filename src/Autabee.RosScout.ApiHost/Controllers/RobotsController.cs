using Autabee.Communication.RosClient;
using Autabee.Communication.RosClient.XmlRpc;
using Autabee.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
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
    public class RobotsController : ControllerBase
    {
        private readonly RosSettings _settings;
        private string rosSettingsFile = "rossettings.json";
        public RobotsController(RosSettings settings)
        {
            _settings = settings;
        }

        [HttpGet("getRosSettings/")]
        public IActionResult GetRobotList()
        {
            return Ok(_settings);
        }

        [HttpGet("getRobotSettings/{robotName}")]
        public IActionResult GetRobotSettings(string robotName)
        {
            var robot = _settings.Profiles.FirstOrDefault(r => r.Name == robotName);
            if (robot == null)
            {
                return NotFound();
            }
            return Ok(robot);
        }

        [HttpPost("addRobotSettings/")]
        public IActionResult AddRobotSettings( [FromBody] RosProfile settings)
        {
            if (_settings.Profiles.Any(r => r.Name == settings.Name))
            {
                return BadRequest("Robot already exists");
            }
            ValidationResult validation = ValidateRequest(settings);

            // check if all required fields are filled
            if (!validation.Success)
            {
                return BadRequest(validation.ToString());
            }

            RosSettings rosSettings = GetSettings();
            rosSettings.Profiles.Add(settings);
            WriteToFile(rosSettings);
            return Ok(validation.ToString());
        }

        [HttpPost("updateRobotSettings/")]
        public IActionResult UpdateRobotSettings([FromBody] RosProfile settings)
        {
            var robot = _settings.Profiles.FirstOrDefault(r => r.Name == settings.Name);
            if (robot == null)
            {
                return NotFound();
            }
            ValidationResult validation = ValidateRequest(settings);

            // check if all required fields are filled
            if (!validation.Success)
            {
                return BadRequest(validation.ToString());
            }

            RosSettings rosSettings = GetSettings();
            rosSettings.Profiles.RemoveAll(o => o.Name == settings.Name);
            rosSettings.Profiles.Add(settings);
            WriteToFile(rosSettings);
            return Ok(validation.ToString());
        }

        [HttpDelete("deleteRobotSettings/{robotName}")]
        public IActionResult DeleteRobot(string robotName)
        {
            RosSettings rosSettings = GetSettings();
            rosSettings.Profiles.RemoveAll(o => o.Name == robotName);
            WriteToFile(rosSettings);
            return Ok();
        }

        private void WriteToFile(RosSettings rosSettings)
        {
            string json = JsonConvert.SerializeObject(rosSettings, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(rosSettingsFile, json);
        }

        private RosSettings GetSettings()
        {
            RosSettings rosSettings;
            using (StreamReader r = new StreamReader(rosSettingsFile))
            {
                string json = r.ReadToEnd();
                rosSettings = JsonConvert.DeserializeObject<RosSettings>(json);

            }
            if (rosSettings == null)
            {
                rosSettings = new RosSettings();
            }
            return rosSettings;
        }

        private static ValidationResult ValidateRequest(RosProfile settings)
        {
            var validation = new Utility.ValidationResult();
            if (string.IsNullOrEmpty(settings.Master))
            {
                validation.AddResult(false, "Master is required");
            }
            if (string.IsNullOrEmpty(settings.Name))
            {
                validation.AddResult(false, "Name is required");
            }
            if (string.IsNullOrEmpty(settings.Version))
            {
                settings.Version = "Noetic";
                validation.AddResult(true, "Automated Version set to Neotic");
            }
            if (string.IsNullOrEmpty(settings.Bridge))
            {
                validation.AddResult(false, "Bridge is required");
            }

            return validation;
        }
    }
}
