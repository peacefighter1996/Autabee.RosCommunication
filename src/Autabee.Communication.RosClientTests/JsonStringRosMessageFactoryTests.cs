using NUnit.Framework;
using Autabee.Communication.RosClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using System.Text.Json;

namespace Autabee.Communication.RosClient.Tests
{
    [TestFixture()]
    public class JsonStringRosMessageFactoryTests
    {
        JsonSerializerOptions Options { get; set; } = new JsonSerializerOptions
        {
            WriteIndented =false
        };
        [Test()]
        public void BuildDefaultTestStringIndented()
        {
            var factory = new JsonStringRosMessageFactory();
            var msg = factory.BuildDefault("std_msgs/String")();
            Assert.AreEqual("{\r\n  \"data\": \"\"\r\n}", msg);
        }
        [Test()]
        public void BuildDefaultTestString()
        {
            var factory = new JsonStringRosMessageFactory();
            factory.Options = Options;
            var msg = factory.BuildDefault("std_msgs/String")();
            Assert.AreEqual("{\"data\":\"\"}", msg);
        }
    }
}