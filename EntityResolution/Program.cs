using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;

namespace EntityResolution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // make service provider, run application, wait for key press
            var services = new ServiceCollection();
            MakeServiceProvider(services).GetService<Application>().Run();
            Console.ReadKey();
        }

        private static ServiceProvider MakeServiceProvider(ServiceCollection services)
        {
            // add Serilog
            Log.Logger = new LoggerConfiguration() // TODO use configuration
                .MinimumLevel.Debug()
                //.Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:w3}] {Message}{NewLine}{Exception}"
                                , theme: AnsiConsoleTheme.None)
                .CreateLogger();
            services.AddSingleton(new LoggerFactory()
                .AddSerilog());
            services.AddLogging();
            // add configuration
            var configurationJson = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config.json", optional: true, reloadOnChange: true) //, false)
                .Build();
            services.AddOptions();
            services.Configure<Config>(configurationJson.GetSection("Config"));
            // @"Data\F_SCH_A_PART1_2016_latest_byZip.csv"
            // add services
            services.AddTransient<IEntityRecord, EntityRecord>();
//            services.AddTransient<IEnumerator<IEntityRecord>, EntityRecordCsvFile<IEntityRecord>>();
//            services.AddTransient<IStringProximity, StringProximity>();
            // add application
            services.AddTransient<Application>();
            // return service provider
            return services.BuildServiceProvider();
        }
    }
}
