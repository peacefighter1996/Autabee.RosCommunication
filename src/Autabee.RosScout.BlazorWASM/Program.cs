using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Autabee.RosScout.BlazorWASM;
using MudBlazor.Services;
using Autabee.Communication.RosClient;
using MudBlazor;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System.Runtime.CompilerServices;
using Autabee.RosScout.Components;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Logging.AddProvider(new SerilogLoggerProvider(new Serilog.LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.Sink(InMemoryLog.GetSingleInstance())
                    .CreateLogger()));

        builder.Services.AddSingleton(o => InMemoryLog.GetSingleInstance());

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddSingleton(sp => new UserTheme()
        {
            Theme = "app",
            NavLinked = true
        });

        builder.Services.AddSingleton<RosScoutMemory>(s =>
        {
            var scoped = s.CreateScope();

            return new RosScoutMemory(
                scoped.ServiceProvider.GetRequiredService<HttpClient>(),
                scoped.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>(),
                scoped.ServiceProvider.GetRequiredService<ILogger<RosScoutMemory>>());
        });

        builder.Services.AddSingleton<JsonToRosMessageFactory>(s => new JsonToRosMessageFactory());
        builder.Services.AddSingleton<JsonStringRosMessageFactory>(s => new JsonStringRosMessageFactory());

        builder.Services.AddMudServices();
        var app = builder.Build();
        app.RunAsync();
    }
}

public class RosScoutMemory
{
    public RosProfile[] RosProfiles { get;set; } = new RosProfile[0];
    public Dictionary<string, RosSystem> RosSystems { get; set; } = new Dictionary<string, RosSystem>();
    public Dictionary<string, RosTopic[]> RosTopics { get; set; } = new Dictionary<string, RosTopic[]>();
    
    readonly HttpClient http;
    readonly NavigationManager navigationManager;
    readonly ILogger<RosScoutMemory> logger;

    public event EventHandler<RosProfile> ProfileDataUpdate;

    internal RosScoutMemory(HttpClient http, NavigationManager navigationManager, ILogger<RosScoutMemory> logger)
    {
        this.logger = logger;
        this.navigationManager = navigationManager;
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


public class InMemoryLog : ILogEventSink
{
    public List<LogMessage> Messages { get; set; } = new List<LogMessage>();
    public event EventHandler<LogMessage> MessageUpdate;
    public InMemoryLog()
    {
    }
    public void Emit(LogEvent logEvent)
    {
        var log = new LogMessage(logEvent);
        Messages.Add(log);
        MessageUpdate?.Invoke(this, log);
    }

    private static InMemoryLog iml;
    public static InMemoryLog GetSingleInstance()
    {
        if (iml == null)
        {
            iml = new InMemoryLog();
        }
        return iml;
    }
}



