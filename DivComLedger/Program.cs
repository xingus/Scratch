using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.IO;

namespace DivComLedger
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
            // add services
            services.AddTransient<ILedger, Ledger>();
            services.AddTransient<IEnumerable<LedgerRecord>, LedgerRecordFile<LedgerRecord>>();
            // add application
            services.AddTransient<Application>();
            // return service provider
            return services.BuildServiceProvider();
        }
    }
}
