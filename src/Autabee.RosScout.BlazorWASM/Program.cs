using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Autabee.RosScout.BlazorWASM;
using MudBlazor.Services;
using Autabee.Communication.RosClient;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddScoped(sp => new UserTheme()
        {
            Theme = "app",
            NavLinked = true
        });

        builder.Services.AddMudServices();
        var app = builder.Build();
        app.RunAsync();
    }
}