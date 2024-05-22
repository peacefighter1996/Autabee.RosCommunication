using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RosSharp.RosBridgeClient;
using std_msg = RosSharp.RosBridgeClient.MessageTypes.Std;
using geometry_msg = RosSharp.RosBridgeClient.MessageTypes.Geometry;


namespace Autabee.Communication.RosClient.Msg
{
    public class StdBuilders : IMsgBuilders
    {
        public Dictionary<string, Func<string, Message>> JsonDeserializer => new Dictionary<string, Func<string, Message>>()
        {
            { std_msg.Empty.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(std_msg.Empty)) },

            { std_msg.Bool.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(std_msg.Bool)) },
            { std_msg.Byte.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Byte)) },
            { std_msg.ByteMultiArray.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.ByteMultiArray)) },
            { std_msg.Char.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Char)) },
            { std_msg.String.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.String)) },
            { std_msg.ColorRGBA.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.ColorRGBA)) },
            { std_msg.Duration.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Duration)) },
            { std_msg.Time.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Time)) },

            { std_msg.Header.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Header)) },

            { std_msg.Int8.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Int8)) },
            { std_msg.Int16.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Int16)) },
            { std_msg.Int32.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Int32)) },
            { std_msg.Int64.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Int64)) },
            { std_msg.Int8MultiArray.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Int8MultiArray)) },
            { std_msg.Int16MultiArray.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Int16MultiArray)) },
            { std_msg.Int32MultiArray.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Int32MultiArray)) },
            { std_msg.Int64MultiArray.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Int64MultiArray)) },

            { std_msg.UInt8.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.UInt8))},
            { std_msg.UInt16.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.UInt16)) },
            { std_msg.UInt32.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.UInt32)) },
            { std_msg.UInt64.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.UInt64)) },
            { std_msg.UInt8MultiArray.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.UInt8MultiArray)) },
            { std_msg.UInt16MultiArray.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.UInt16MultiArray)) },
            { std_msg.UInt32MultiArray.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.UInt32MultiArray)) },
            { std_msg.UInt64MultiArray.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.UInt64MultiArray)) },

            { std_msg.Float32.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Float32)) },
            { std_msg.Float64.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Float64)) },
            { std_msg.Float32MultiArray.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Float32MultiArray)) },
            { std_msg.Float64MultiArray.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.Float64MultiArray)) },

            { std_msg.MultiArrayDimension.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.MultiArrayDimension)) },
            { std_msg.MultiArrayLayout.RosMessageName, (string json) => Serialization.JsonDeserialize(json, typeof(std_msg.MultiArrayLayout)) },
        };

        public Dictionary<string, Func<JsonSerializerOptions, string>> DefaultJsonStringBuilder => new Dictionary<string, Func<JsonSerializerOptions,string>>()
        {
            { std_msg.Empty.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Empty), o)) },
            { std_msg.Bool.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Bool), o)) },
            { std_msg.Byte.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Byte), o)) },
            { std_msg.ByteMultiArray.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.ByteMultiArray), o)) },
            { std_msg.Char.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Char), o)) },
            { std_msg.String.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.String), o)) },
            { std_msg.ColorRGBA.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.ColorRGBA), o)) },
            { std_msg.Duration.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Duration), o)) },
            { std_msg.Time.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Time), o)) },
            { std_msg.Header.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Header), o)) },
            { std_msg.Int8.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Int8), o)) },
            { std_msg.Int16.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Int16), o)) },
            { std_msg.Int32.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Int32), o)) },
            { std_msg.Int64.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Int64), o)) },
            { std_msg.Int8MultiArray.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Int8MultiArray), o)) },
            { std_msg.Int16MultiArray.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Int16MultiArray), o)) },
            { std_msg.Int32MultiArray.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Int32MultiArray), o)) },
            { std_msg.Int64MultiArray.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Int64MultiArray), o)) },
            { std_msg.UInt8.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.UInt8), o)) },
            { std_msg.UInt16.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.UInt16), o)) },
            { std_msg.UInt32.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.UInt32), o)) },
            { std_msg.UInt64.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.UInt64), o)) },
            { std_msg.UInt8MultiArray.RosMessageName,((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.UInt8MultiArray), o)) },
            { std_msg.UInt16MultiArray.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.UInt16MultiArray), o)) },
            { std_msg.UInt32MultiArray.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.UInt32MultiArray), o)) },
            { std_msg.UInt64MultiArray.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.UInt64MultiArray), o)) },
            { std_msg.Float32.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Float32), o)) },
            { std_msg.Float64.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Float64), o)) },
            { std_msg.Float32MultiArray.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Float32MultiArray), o)) },
            { std_msg.Float64MultiArray.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.Float64MultiArray), o)) },
            { std_msg.MultiArrayDimension.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.MultiArrayDimension), o)) },
            { std_msg.MultiArrayLayout.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(std_msg.MultiArrayLayout), o)) },

        };
    }


    public class GeometryBuilder : IMsgBuilders
    {
        public Dictionary<string, Func<string, Message>> JsonDeserializer => new Dictionary<string, Func<string, Message>>()
        {
            { geometry_msg.Accel.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.Accel)) },
            { geometry_msg.AccelStamped.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof (geometry_msg.AccelStamped)) },
            { geometry_msg.Point.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.Point)) },
            { geometry_msg.Pose.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.Pose)) },
            { geometry_msg.Pose2D.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.Pose2D)) },
            { geometry_msg.PoseStamped.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.PoseStamped)) },
            { geometry_msg.PoseWithCovariance.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.PoseWithCovariance)) },
            { geometry_msg.PoseWithCovarianceStamped.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.PoseWithCovarianceStamped)) },
            { geometry_msg.Quaternion.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.Quaternion)) },
            { geometry_msg.QuaternionStamped.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.QuaternionStamped)) },
            { geometry_msg.Twist.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.Twist)) },
            { geometry_msg.TwistStamped.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.TwistStamped)) },
            { geometry_msg.TwistWithCovariance.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.TwistWithCovariance)) },
            { geometry_msg.TwistWithCovarianceStamped.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.TwistWithCovarianceStamped)) },
            { geometry_msg.Vector3.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.Vector3)) },
            { geometry_msg.Wrench.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.Wrench)) },
            { geometry_msg.WrenchStamped.RosMessageName, (string json) => Serialization.JsonDeserialize(json,typeof(geometry_msg.WrenchStamped)) },
        };

    public Dictionary<string, Func<JsonSerializerOptions, string>> DefaultJsonStringBuilder => new Dictionary<string, Func<JsonSerializerOptions, string>>()
        {
            {geometry_msg.Accel.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.Accel), o)) },
            {geometry_msg.AccelStamped.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.AccelStamped), o)) },
            {geometry_msg.Point.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.Point), o)) },
            {geometry_msg.Pose.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.Pose), o)) },
            {geometry_msg.Pose2D.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.Pose2D), o)) },
            {geometry_msg.PoseStamped.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.PoseStamped), o)) },
            {geometry_msg.PoseWithCovariance.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.PoseWithCovariance), o)) },
            {geometry_msg.PoseWithCovarianceStamped.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.PoseWithCovarianceStamped), o)) },
            {geometry_msg.Quaternion.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.Quaternion), o)) },
            {geometry_msg.QuaternionStamped.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.QuaternionStamped), o)) },
            {geometry_msg.Twist.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.Twist), o)) },
            {geometry_msg.TwistStamped.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.TwistStamped), o)) },
            {geometry_msg.TwistWithCovariance.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.TwistWithCovariance), o)) },
            {geometry_msg.TwistWithCovarianceStamped.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.TwistWithCovarianceStamped), o)) },
            {geometry_msg.Vector3.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.Vector3), o)) },
            {geometry_msg.Wrench.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.Wrench), o)) },
            {geometry_msg.WrenchStamped.RosMessageName, ((JsonSerializerOptions o) => Serialization.DefaultStringSerialization(typeof(geometry_msg.WrenchStamped), o)) }
        };
    }
}
