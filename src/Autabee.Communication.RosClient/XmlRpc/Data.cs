using System.Xml;
using System.Xml.Serialization;

namespace Autabee.Communication.RosClient.XmlRpc
{
    [XmlRoot(ElementName = "data")]
    public class Data
    {

        [XmlElement(ElementName = "value")]
        public List<Value> Value { get; set; } = new List<Value>();
    }
}