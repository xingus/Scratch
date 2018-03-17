using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace DiCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // make service provider, run application, wait for key press
            var services = new ServiceCollection();
            MakeServiceProvider(services).GetService<App>().Run();
            Console.ReadKey();
        }

        private static ServiceProvider MakeServiceProvider(ServiceCollection services)
        {
            // create service collection
            // add logging // TODO look into Serilog
            services.AddSingleton(new LoggerFactory()
                .AddConsole()
                .AddDebug());
            services.AddLogging();
            // add configuration
            var configurationJson = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config.json", optional: true, reloadOnChange: true) //, false)
                .Build();
            services.AddOptions();
            services.Configure<Config>(configurationJson.GetSection("Config"));
            // add services
            services.AddTransient<IAppService, AppService>();
            // add application
            services.AddTransient<App>();
            // return service provider
            return services.BuildServiceProvider();
        }
    }
}
