namespace Autabee.Communication.RosClient.Dto
{
    public record RosSystem(RosConnection[] publishers, RosConnection[] subscribers, RosConnection[] services);


    public static class RosSystemExtension
    {
        public static IEnumerable<string> GetNodes(this RosSystem rosSystem)
        {
            Dictionary<string, int> nodes = new Dictionary<string,int>();
            foreach (var item in rosSystem.publishers)
            {
                foreach (var node in item.nodes)
                {
                    if (!nodes.ContainsKey(node))
                    {
                        nodes.Add(node, 0);
                    }
                }
            }
            foreach (var item in rosSystem.subscribers)
            {
                foreach (var node in item.nodes)
                {
                    if (!nodes.ContainsKey(node))
                    {
                        nodes.Add(node, 0);
                    }
                }
            }
            foreach (var item in rosSystem.services)
            {
                foreach (var node in item.nodes)
                {
                    if (!nodes.ContainsKey(node))
                    {
                        nodes.Add(node, 0);
                    }
                }
            }
            return nodes.Keys.ToArray();
        }
    }
}