using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Autabee.Utility.Messaging.Validation;
using Autabee.Utility.Messaging;
using System.Reflection.Emit;
namespace Autabee.Communication.RosClient.Dto
{
    public record RosTopic(string Name, string Type);
    public record RosService(string Name, string TypeIn, string TypeOut);
}