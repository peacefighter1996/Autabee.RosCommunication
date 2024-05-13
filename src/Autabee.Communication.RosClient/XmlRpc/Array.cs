using System.Xml;
using System.Xml.Serialization;

namespace Autabee.Communication.RosClient.XmlRpc
{
    [XmlRoot(ElementName = "array")]
    public class Array
    {

        [XmlElement(ElementName = "data")]
        public Data Data { get; set; } = new Data();
    }
}