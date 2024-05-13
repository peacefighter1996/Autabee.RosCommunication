using System.Xml;
using System.Xml.Serialization;

namespace Autabee.Communication.RosClient.XmlRpc
{
    [XmlRoot(ElementName = "params")]
    public class Params
    {

        [XmlElement(ElementName = "param")]
        public List<Param> Param { get; set; } = new List<Param>();
    }
}