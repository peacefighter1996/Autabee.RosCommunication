namespace Autabee.Communication.RosClient.Dto
{
    public record RosConnection(string Name, RosConnectionType Type, List<string> nodes);
    public record RosProfilePublish(string json, string profile, RosTopic topic);
}