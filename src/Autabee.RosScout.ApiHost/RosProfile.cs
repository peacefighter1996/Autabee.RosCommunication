namespace Autabee.RosScout.WasmHostApi
{
    public class RosSettings
    {
        public List<RosProfile> Profiles { get; set; } = new List<RosProfile>();
    }
    public class RosProfile
    {
        public string Version { get; set; } = "Noetic";
        public string Bridge { get; set; } = string.Empty;
        public string Master { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

    }
}
