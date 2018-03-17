using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DiCore
{

    public interface IAppService
    {
        void Run();
    }

    class AppService : IAppService
    {
        private readonly ILogger<AppService> _logger;
        private readonly Config _config;

        public AppService(
            IOptions<Config> config,
            ILogger<AppService> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public void Run()
        {
            var method = new StackTrace(false).GetFrame(0).GetMethod();
            _logger.LogWarning($"{method.DeclaringType.FullName}.{method.Name} {_config.Title}");
        }
    }
}
