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
        builder.Services.AddScoped(sp => new UserTheme()
        {
            Theme = "app",
            NavLinked = true
        });

        builder.Services.AddSingleton<JsonToRosMessageFactory>(s => new JsonToRosMessageFactory());
        builder.Services.AddSingleton<JsonStringRosMessageFactory>(s => new JsonStringRosMessageFactory());

        builder.Services.AddMudServices();
        var app = builder.Build();
        app.RunAsync();
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



