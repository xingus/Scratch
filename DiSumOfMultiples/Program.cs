using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiSumOfMultiples
{
    class Program
    {
        static void Main(string[] args)
        {

            // make service[s] provider, run application, wait for key press
            var services = new ServiceCollection();
            MakeServiceProvider(services).GetService<App>().Run();
            Console.ReadKey();
        }

        /// <summary>
        /// Add application and all it's dependencies to services
        /// </summary>
        /// <param name="services"></param>
        /// <returns>service provider</returns>
        private static ServiceProvider MakeServiceProvider(ServiceCollection services)
        {
            // add Serilog
            Log.Logger = new LoggerConfiguration() // TODO use configuration
                .MinimumLevel.Debug()
                //.Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:w3}] {Message}{NewLine}{Exception}"
                                , theme: ConsoleTheme.None)
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
            // add sum of multiple strategies
            services.AddSingleton( new List<ISumOfMultiples> { new SumOfMultiplesTriangle()
                                                             , new SumOfMultiplesIterate()
                                                             , new SumOfMultiplesRecurse()
                                                             , new SumOfMultiplesLinq()
                                                             , new SumOfMultiplesIoc() });
            // add strategy factory
            services.AddTransient<ISumOfMultiplesFactory, SumOfMultiplesFactory>();
            // add application
            services.AddTransient<App>();
            // return service provider
            return services.BuildServiceProvider();
        }
    }
}
