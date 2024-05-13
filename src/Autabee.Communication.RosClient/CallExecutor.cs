using Autabee.Communication.RosClient.XmlRpc;
using Autabee.Utility.Messaging.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Autabee.Communication.RosClient
{
    public static class CallExecutor
    {
        public static async Task<MethodResponse> Execute(string host, MethodCall methodCall)
        {
            string xml = methodCall.ToCallXml();
            var http = new HttpClient();
            var content = new StringContent(xml.ToString(), Encoding.UTF8, "text/xml");
            var response = await http.PostAsync(host, content);
            var data = await response.Content.ReadAsStringAsync();
            var serializer2 = new XmlSerializer(typeof(MethodResponse));
            MethodResponse result;
            using (var reader = new StringReader(data))
            {
                result = (MethodResponse)serializer2.Deserialize(reader);
            }

            return result;
        }


        public static async Task<ValidationResult<T>> Execute<T>(string host, 
            Func<MethodCall> callGenerator, 
            Func<MethodResponse, ValidationResult<T>> callback)
        {
            try
            {
                MethodResponse responce = await Execute(host, callGenerator());
                var result = callback(responce);
                return result;
            }
            catch (Exception ex)
            {
                return new ValidationResult<T>(success:false,unSuccesfullText: ex.Message);
            }
        }
    }
}
