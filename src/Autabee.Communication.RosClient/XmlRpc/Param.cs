using System.Xml;
using System.Xml.Serialization;

namespace Autabee.Communication.RosClient.XmlRpc
{
    [XmlRoot(ElementName = "param")]
    public class Param
    {

        [XmlElement(ElementName = "value")]
        public Value Value { get; set; } = new Value();
    }
}