using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DivComLedger
{
    internal class Application
    {
        private readonly Config _config;
        private readonly ILogger<Application> _logger;
        private readonly ILedger _ledger;
        private readonly IEnumerable<LedgerRecord> _ledgerRecordEnumerator;

        public Application(
            IOptions<Config> config,
            ILogger<Application> logger,
            ILedger ledger,
            IEnumerable<LedgerRecord> ledgerRecordEnumerator)
        {
            _config = config.Value;
            _logger = logger;
            _ledger = ledger;
            _ledgerRecordEnumerator = ledgerRecordEnumerator;
        }

        internal void Run()
        {
            _logger.LogDebug($"Start");
            try
            {
                using (var client = new WebClient()) // TODO move to internet/file utility service, handle file already exists, et cetera
                {
                    //client.DownloadFile(_config.DataUrl, _config.DataFile);
                }
                _logger.LogDebug($"DownloadFile");
                Parallel.ForEach(_ledgerRecordEnumerator,
                    fileRecord =>
                    {
                        fileRecord.Parse();
                        _logger.LogDebug($"{fileRecord.ToString()}");
                        if (fileRecord.Ok) _ledger.Enter(fileRecord);
                    });
                _logger.LogDebug($"GetEnumerable");
                // TODO once we have enumerated records, produce final profit/loss report and cleaned input.
            }
            catch (AggregateException oXcptn)
            {
                _logger.LogCritical($"{oXcptn.Message}");
                foreach (var iXcptn in oXcptn.InnerExceptions)
                {
                    _logger.LogCritical($"{iXcptn.Message}");
                }
            }
            finally
            {
                _logger.LogDebug($"Finish");
            }
        }
    }
}