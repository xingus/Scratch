using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DiSumOfMultiples
{
    public class App
    {
        private readonly Config _config;
        private readonly ILogger<App> _logger;
        private readonly ISumOfMultiplesFactory _sumOfMultiplesFactory;

        public App(
            IOptions<Config> config,
            ILogger<App> logger,
            ISumOfMultiplesFactory sumOfMultiplesFactory)
        {
            _config = config.Value;
            _logger = logger;
            _sumOfMultiplesFactory = sumOfMultiplesFactory;
        }

        public void Run()
        {
            var method = new StackTrace(false).GetFrame(0).GetMethod();
            _logger.LogInformation($"{method.DeclaringType.FullName}.{method.Name} {_config.Title}");
            foreach (var strategyName in new List<string> { "Triangle", "Iterate", "Recurse", "Linq", "Ioc" })
            {
                _logger.LogInformation($@"{strategyName}: {
                    _sumOfMultiplesFactory.GetSumOfMultiples(strategyName).Sum(1, 999, new List<int> { 3, 5 })}");
            }
            // Show we can inject a different predicate into SumOfEnumerableByPredicate
            _logger.LogInformation($@"different predicate = {
                new SumOfMultiplesIoc().SumOfEnumerableByPredicate(Enumerable.Range(1, 999)
                    , e => new List<int> { 1, 3, 5, 7 }.Exists(f => f == e % 10))}"); // change predicate: last digit is 1, 3, 5, or 7 
        }
    }
}
