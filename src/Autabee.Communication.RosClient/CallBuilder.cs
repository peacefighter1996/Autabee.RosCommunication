using Autabee.Communication.RosClient.Dto;
using Autabee.Communication.RosClient.XmlRpc;
using RosSharp.RosBridgeClient;
using System.Reflection.Metadata.Ecma335;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;

namespace Autabee.Communication.RosClient
{
    public static class CallBuilder
    {
        #region MASTER API
        public static MethodCall GetTopicTypes(string callerId)
        {
            MethodCall mc = new MethodCall { MethodName = "getTopicTypes" };
            mc.Params.Param.Add(new Param { Value = new Value() { String = callerId } });
            return mc;
        }

        public static MethodCall LookupNode(string callerId, string nodeName)
        {
            MethodCall mc = new MethodCall { MethodName = "lookupNode" };
            mc.Params.Param.Add(new Param { Value = new Value() { String = callerId } });
            mc.Params.Param.Add(new Param { Value = new Value() { String = nodeName } });
            return mc;
        }

        public static MethodCall LookupService(string callerId, string serviceName)
        {
            MethodCall mc = new MethodCall { MethodName = "lookupService" };
            mc.Params.Param.Add(new Param { Value = new Value() { String = callerId } });
            mc.Params.Param.Add(new Param { Value = new Value() { String = serviceName } });
            return mc;
        }

        public static MethodCall GetUri(string callerId)
        {
            MethodCall mc = new MethodCall { MethodName = "getUri" };
            mc.Params.Param.Add(new Param { Value = new Value() { String = callerId } });
            return mc;
        }

        public static MethodCall GetSystemState(string callerId)
        {
            MethodCall mc = new MethodCall { MethodName = "getSystemState" };
            mc.Params.Param.Add(new Param { Value = new Value() { String = callerId } });
            return mc;
        }
        public static MethodCall GetPublishedTopics(string callerId, string subgraph)
        {
            MethodCall mc = new MethodCall { MethodName = "getPublishedTopics" };
            mc.Params.Param.Add(new Param { Value = new Value() { String = callerId } });
            mc.Params.Param.Add(new Param { Value = new Value() { String = subgraph } });
            return mc;
        }
        #endregion MASTER API

        #region Parameter server API
        public static MethodCall GetParams(string callerId)
        {
            MethodCall mc = new MethodCall { MethodName = "getParamNames" };
            mc.Params.Param.Add(new Param { Value = new Value() { String = callerId } });
            return mc;
        }
        #endregion Parameter server API
    }
}