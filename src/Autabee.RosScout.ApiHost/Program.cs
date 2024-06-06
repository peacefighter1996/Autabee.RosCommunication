using Autabee.Communication.RosClient;
using Autabee.RosScout.BlazorWASM;
using Autabee.RosScout.WasmHostApi.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;

namespace Autabee.WasmHostApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var rosConfig = new ConfigurationBuilder()
                .AddJsonFile("rossettings.json", optional: false, reloadOnChange: true)
                .Build();


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            builder.Services.AddSignalR();
            // setting up proxy service
            builder.Services.AddHostedService<RosBridgeService>();
            builder.Services.AddTransient<RosBridgeHub>();
            builder.Services.AddSingleton<RosBridge>();

            builder.Configuration.AddConfiguration(appConfig);
            builder.Configuration.AddConfiguration(rosConfig);
            builder.Services.AddTransient(s => rosConfig.Get<RosSettings>());

            builder.Services.AddTransient<JsonToRosMessageFactory>(s => new JsonToRosMessageFactory());
            builder.Services.AddTransient<JsonStringRosMessageFactory>(s=> new JsonStringRosMessageFactory());

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
               
            }
            
            app.UseCors("CorsPolicy");


            app.UseBlazorFrameworkFiles();
            app.MapFallbackToFile("index.html");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseWebSockets();

            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllers();

            app.MapHub<RosBridgeHub>("/hub/rosbridge");


            app.Run();
        }
    }
}
