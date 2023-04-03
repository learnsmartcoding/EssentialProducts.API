using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace LearnSmartCoding.EssentialProducts.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var name = typeof(Program).Assembly.GetName().Name;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", name)
                // available sinks: https://github.com/serilog/serilog/wiki/Provided-Sinks
                // Seq: https://datalust.co/seq
                // Seq with Docker: https://docs.datalust.co/docs/getting-started-with-docker
                .WriteTo.Seq(serverUrl: "https://localhost:5001")
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
