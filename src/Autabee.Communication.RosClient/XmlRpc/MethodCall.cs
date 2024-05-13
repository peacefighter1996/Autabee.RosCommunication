using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Autabee.Communication.RosClient.XmlRpc
{
    [XmlRoot(ElementName = "methodCall")]
    public class MethodCall
    {

        [XmlElement(ElementName = "methodName")]
        public string MethodName { get; set; }

        [XmlElement(ElementName = "params")]
        public Params Params { get; set; } = new Params();

        public string ToCallXml()
        {
            var xml = "";
            var serializer = new XmlSerializer(typeof(MethodCall));
            using (var sw = new StringWriter())
            {
                using (var wirter = XmlWriter.Create(sw, new XmlWriterSettings()
                {
                    Encoding = Encoding.UTF8,
                    OmitXmlDeclaration = true,
                }))
                {
                    serializer.Serialize(wirter, this);
                    xml = sw.ToString();
                }
            }

            // remove  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" from xml
            xml = xml.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);

            // remove all elements that contain nil="true" from xml
            xml = Regex.Replace(xml, @"<[^>]*xsi:nil=""true""[^>]*>", string.Empty);
            return xml;
        }
    }
}