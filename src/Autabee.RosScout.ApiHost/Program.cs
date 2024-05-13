
using Autabee.RosScout.WasmHostApi;
using Autabee.RosScout.WasmHostApi.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;

namespace Autabee.WasmHostApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
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

            List<RosSettings> instances = new List<RosSettings>();
            config.GetSection("RosSettings").Bind( instances );

            builder.Services.AddSignalR();
            builder.Services.AddHostedService<RosService>();
            builder.Services.AddTransient<RosBridgeHub>();
            builder.Services.AddSingleton<RosBridge>();
            builder.Configuration.AddConfiguration(config);
            builder.Services.AddTransient(s => config.GetSection("RosSettings")
                                                     .Get<IEnumerable<RosSettings>>()
                                         );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseWebAssemblyDebugging();
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
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<RosBridgeHub>("/rosbridgehub");
            });


            app.Run();
        }
    }
}
