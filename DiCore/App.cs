using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DiCore
{

    public class App
    {
        private readonly Config _config;
        private readonly ILogger<App> _logger;
        private readonly IAppService _service;

        public App(
            IOptions<Config> config,
            ILogger<App> logger,
            IAppService service)
        {
            _config = config.Value;
            _logger = logger;
            _service = service;
        }

        public void Run()
        {
            var method = new StackTrace(false).GetFrame(0).GetMethod();
            _logger.LogInformation($"{method.DeclaringType.FullName}.{method.Name} {_config.Title}");
            _service.Run();
        }
    }
}
