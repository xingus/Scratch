using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiCoreConsole
{

    public class App
    {
        private readonly ITestService _testService;
        private readonly ILogger<App> _logger;
        private readonly AppConfig _config;

        public App(ITestService testService,
            IOptions<AppConfig> config,
            ILogger<App> logger)
        {
            _testService = testService;
            _logger = logger;
            _config = config.Value;
        }

        public void Run()
        {
            _logger.LogInformation($"This is a console application for |{_config.Title}|");
            _testService.Run();
            System.Console.ReadKey();
        }
    }
}
