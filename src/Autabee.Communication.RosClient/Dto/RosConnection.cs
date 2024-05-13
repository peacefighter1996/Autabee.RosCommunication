namespace Autabee.Communication.RosClient.Dto
{
    public record RosConnection(string Name, RosConnectionType Type, List<string> nodes);
}