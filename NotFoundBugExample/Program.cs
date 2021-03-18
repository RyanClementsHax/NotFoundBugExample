using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace NotFoundBugExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(serverOptions =>
                        {

                            serverOptions.ListenLocalhost(5003, listenOptions =>
                            {
                                listenOptions.UseHttps();
                                listenOptions.UseConnectionLogging();
                            });
                        })
                        .UseStartup<Startup>();
                });
    }
}
