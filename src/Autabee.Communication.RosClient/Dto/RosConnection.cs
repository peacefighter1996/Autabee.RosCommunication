namespace Autabee.Communication.RosClient.Dto
{
    public record RosConnection(string Name, RosConnectionType Type, List<string> nodes);
    public record RosProfileJsonMessagePublish(string json, string profile, RosTopic topic);
    public record ProfileConnectionState(string robotname, bool master, bool bridge);
    //public record RosProfileMessagePublish(Message Message, string profile, RosTopic topic);
}