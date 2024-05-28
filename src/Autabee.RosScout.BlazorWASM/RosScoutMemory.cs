using Autabee.Communication.RosClient;
using Autabee.Communication.RosClient.Dto;
using Newtonsoft.Json;
using System.Net.Http.Json;

public class RosScoutMemory
{
    public RosProfile[] RosProfiles { get; set; } = new RosProfile[0];
    public Dictionary<string, RosSystem> RosSystems { get; set; } = new Dictionary<string, RosSystem>();
    public Dictionary<string, RosTopic[]> RosTopics { get; set; } = new Dictionary<string, RosTopic[]>();

    readonly HttpClient http;
    readonly ILogger<RosScoutMemory> logger;

    public event EventHandler<RosProfile> ProfileDataUpdate;

    public RosScoutMemory(HttpClient http, ILogger<RosScoutMemory> logger)
    {
        this.logger = logger;
        this.http = http;

        GetRosProfiles();

        foreach (var profile in RosProfiles)
        {
            ScanSystemData(profile);
        }
    }

    public RosProfile? GetRosProfile(string name)
    {
        return RosProfiles.FirstOrDefault(r => r.Name == name);
    }

    public void ScanSystemData(string name)
    {
        var profile = GetRosProfile(name);
        if (profile == null)
        {
            return;
        }
        ScanSystemData(profile);
    }

    public async void ScanSystemData(RosProfile rosProfile)
    {
        try
        {
            if (rosProfile == null)
            {
                logger.LogInformation("Profile not found");
                return;
            }

            var postBody = rosProfile.Master;



            var httpResult = await http.PostAsJsonAsync("/api/RosMaster/getTopicTypes/rosScout", postBody);
            if (httpResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                logger.LogError($"Error getting topics {httpResult.StatusCode}");
                return;
            }

            var result = await httpResult.Content.ReadFromJsonAsync<List<RosTopic>>();

            logger.LogInformation(JsonConvert.SerializeObject(result));

            var topics = (result ?? new List<RosTopic>()).ToArray();
            if (RosTopics.ContainsKey(rosProfile.Name))
            {
                RosTopics[rosProfile.Name] = topics;
            }
            else
            {
                RosTopics.Add(rosProfile.Name, topics);
            }

            logger.LogInformation("Getting system");

            httpResult = await http.PostAsJsonAsync("/api/RosMaster/getSystemState/rosScout", postBody);
            var system = await httpResult.Content.ReadFromJsonAsync<RosSystem>();

            if (RosSystems.ContainsKey(rosProfile.Name))
            {
                RosSystems[rosProfile.Name] = system;
            }
            else
            {
                RosSystems.Add(rosProfile.Name, system);

            }


        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }

        ProfileDataUpdate?.Invoke(this, rosProfile);
    }

    public async Task<RosProfile[]> GetRosProfiles()
    {
        if (RosProfiles.Length == 0)
        {
            RosProfiles = (await http.GetFromJsonAsync<RosSettings>("api/Robots/getRosSettings")).Profiles.ToArray();

        }
        return RosProfiles;
    }

    public async Task<RosProfile[]> UpdateRosProfiles(RosProfile rosProfile)
    {
        var result = await http.PostAsJsonAsync("api/Robots/addRobotSettings", rosProfile);
        result.EnsureSuccessStatusCode();

        // replace old profile with new one
        RosProfiles[RosProfiles.ToList().FindIndex(r => r.Name == rosProfile.Name)] = rosProfile;
        return RosProfiles;
    }
}



