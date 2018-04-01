using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EntityResolution
{
    internal class Application
    {
        private readonly Config _config;
        private readonly ILogger<Application> _logger;
        private readonly IEntityRecord _entityRecord;
        private readonly IEnumerator<IEntityRecord> _entityRecordEnumerator;

        ConcurrentDictionary<int, EntityRecord> records = new ConcurrentDictionary<int, EntityRecord>();
        ConcurrentDictionary<string, ConcurrentBag<int>> states = new ConcurrentDictionary<string, ConcurrentBag<int>>();
        ConcurrentDictionary<string, ConcurrentBag<int>> nameGroups = new ConcurrentDictionary<string, ConcurrentBag<int>>();

        SortedDictionary<int,List<string>> rankNames = new SortedDictionary<int,List<string>>(Comparer<int>.Create((x, y) => y.CompareTo(x)));

        //ConcurrentDictionary<LinkKey, double> links = new ConcurrentDictionary<LinkKey, double>();

        //ConcurrentDictionary<int, double> ranks = new ConcurrentDictionary<int, double>();

        //ConcurrentDictionary<int, ConcurrentBag<int>> leaderGroups = new ConcurrentDictionary<int, ConcurrentBag<int>>(); // leader & followers


        public Application(
            IOptions<Config> config,
            ILogger<Application> logger,
            IEntityRecord entityRecord,
            IEnumerator<IEntityRecord> entityRecordEnumerator)
        {
            _config = config.Value;
            _logger = logger;
            _entityRecord = entityRecord;
            _entityRecordEnumerator = entityRecordEnumerator;
        }

        internal void Run()
        {
            _logger.LogDebug($"Start");
            IEnumerable<EntityRecord> inputRecords = _entityRecordEnumerator.GetEnumerable<EntityRecord>();
            _logger.LogDebug($"GetEnumerable");
            try
            {
                Parallel.ForEach(inputRecords,
                    //new ParallelOptions { MaxDegreeOfParallelism = 1 }, // better to single thread here
                    record => {
                        //if (_config.StateFilterSet.Contains(record.INS_BROKER_US_STATE))
                        {
                            ToNameGroups(record);
                        }
                    });
                _logger.LogDebug($"ToNameGroups");
                foreach ( var tokenGroup in nameGroups)
                {
                    rankNames.TryAdd(tokenGroup.Value.Count, new List<string>());
                    rankNames[tokenGroup.Value.Count].Add(tokenGroup.Key);
                }
                _logger.LogDebug($"ToRankTokens");
                foreach (var names in rankNames)
                {
                    foreach (var name in names.Value)
                    {

                    }
                }

                //Parallel.ForEach(states.Values, lineNumbers =>
                //{
                //    Parallel.ForEach(lineNumbers, lineNumber1 =>
                //    {
                //        Parallel.ForEach(lineNumbers, lineNumber2 =>
                //        {
                //            //_logger.LogDebug($"{lineNumber1} {lineNumber2}");
                //            if (lineNumber1 > lineNumber2) link(records[lineNumber1], records[lineNumber2]);
                //        });
                //    });
                //});
                //_logger.LogDebug($"link");
            }
            catch (AggregateException oXcptn)
            {
                _logger.LogCritical($"Line: {_entityRecordEnumerator.FileLine()} {oXcptn.Message}");
                foreach (var iXcptn in oXcptn.InnerExceptions)
                {
                    _logger.LogCritical($"Line: {_entityRecordEnumerator.FileLine()} {iXcptn.Message}");
                }
            }
            finally
            {
                _logger.LogDebug($"records: {records.Count}");
                _logger.LogDebug($"states: {states.Count}");
                foreach (var state in states)
                {
                    _logger.LogDebug($"{state.Key}: {state.Value.Count}");
                }
                _logger.LogDebug($"nameGroups: {nameGroups.Count}");
                //_logger.LogDebug($"links: {links.Count}");
                //_logger.LogDebug($"ranks: {ranks.Count}");
                foreach (var rankName in rankNames)
                {
                    _logger.LogDebug($"{rankName.Key}: {(1==rankName.Value.Count ? rankName.Value[0] : rankName.Value.Count.ToString())}");
                    foreach (var name in rankName.Value)
                    {
                        var uniqueStates = new HashSet<string>();
                        foreach (var lineNumber in nameGroups[name])
                        {
                            uniqueStates.Add(records[lineNumber].INS_BROKER_US_STATE);
                        }
                        if (1 < uniqueStates.Count) _logger.LogDebug($"{name}: {String.Join(",", uniqueStates)}");
                        var uniqueAddress = new HashSet<string>();
                        foreach (var lineNumber in nameGroups[name])
                        {
                            uniqueAddress.Add($"{records[lineNumber].INS_BROKER_US_STATE} {records[lineNumber].INS_BROKER_US_ADDRESS1}");
                        }
                        if (1 < uniqueAddress.Count) _logger.LogDebug($"{name}: {Environment.NewLine}{String.Join($"{Environment.NewLine}", uniqueAddress)}");
                    }
                }
                _logger.LogDebug($"Finish");
            }
        }

        private void ToNameGroups(EntityRecord record)
        {
            records.TryAdd(record.LineNumber, record);
            //
            if (nameGroups.TryAdd(record.INS_BROKER_NAME, new ConcurrentBag<int>()))
            {
                states.TryAdd(record.INS_BROKER_US_STATE, new ConcurrentBag<int>());
                states[record.INS_BROKER_US_STATE].Add(record.LineNumber);
            }
            nameGroups[record.INS_BROKER_NAME].Add(record.LineNumber);
            //
            // Can't link here because we miss some links, unless process is single threded.
            //Parallel.ForEach(states[record.INS_BROKER_US_STATE],
            //    lineNumber =>
            //    {
            //        if (record.LineNumber > lineNumber) link(record, records[lineNumber]);
            //    });
        }

        //private void link(EntityRecord record1, EntityRecord record2)
        //{
        //    //if (record1.Key > record2.Key) Console.WriteLine($"link {record1.Key} {record1.Key}");
        //    var nameProximity = Proximity(record1.INS_BROKER_NAME, record2.INS_BROKER_NAME);
        //    var linkKey = new LinkKey(record1.LineNumber, record2.LineNumber);
        //    if (_config.ProximityMinimum < nameProximity)
        //    {
        //        links.TryAdd(linkKey, nameProximity);
        //        //_logger.LogDebug($@"{
        //        //    linkKey}, {
        //        //    nameProximity}, {
        //        //    record1.INS_BROKER_NAME}, {
        //        //    record2.INS_BROKER_NAME}"); //, {
        //        //                                    //                               //record.INS_BROKER_US_ADDRESS1}, {
        //        //                                    //                               //record.INS_BROKER_US_ADDRESS2}");
        //    }
        //}

        private static double Proximity(string name1, string name2)
            {
                return JaroWinkler.Proximity(name1, name2);
            }
        }
    }