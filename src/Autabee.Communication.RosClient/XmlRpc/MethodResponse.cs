using System.Xml;
using System.Xml.Serialization;

namespace Autabee.Communication.RosClient.XmlRpc
{
    [XmlRoot(ElementName = "methodResponse")]
    public class MethodResponse
    {

        [XmlElement(ElementName = "params")]
        public Params Params { get; set; } = new Params();
    }
}