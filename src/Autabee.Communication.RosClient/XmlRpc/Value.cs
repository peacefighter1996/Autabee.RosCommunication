using System.Xml;
using System.Xml.Serialization;

namespace Autabee.Communication.RosClient.XmlRpc
{
    [XmlRoot(ElementName = "value")]
    public class Value
    {

        [XmlElement(ElementName = "int")]
        public int? Int { get; set; }

        [XmlElement(ElementName = "string")]
        public string String { get; set; }

        [XmlElement(ElementName = "array")]
        public Array Array { get; set; }
    }
}