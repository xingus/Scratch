using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InsBrokEntRes
{
    class Program
    {

        static ConcurrentDictionary<int, InsBrokRec> records = new ConcurrentDictionary<int, InsBrokRec>();
        //static Dictionary<string, Dictionary<LinkKey, double>> linkByState = new Dictionary<string, Dictionary<LinkKey, double>>();
        //static Dictionary<string, Dictionary<int, double>> rankByState = new Dictionary<string, Dictionary<int, double>>();
        static ConcurrentDictionary<LinkKey, double> links = new ConcurrentDictionary<LinkKey, double>();
        static Dictionary<int, double> rank = new Dictionary<int, double>();

        static void Main(string[] args)
        {
            Ingest(@"Data\F_SCH_A_PART1_2016_latest_byZip.csv", FilterInsBrokRec, ProcInsBrokRec); // TODO args[1] for file spec
            Console.Read();
        }

        private static void Ingest(string fileSpec, Func<InsBrokRec, bool> filterInsBrokRec, Action<InsBrokRec> process) // TODO ApplyProcess method 
        {
            using (var csvReader = new MyCsvReader(fileSpec)) {
                IEnumerable<InsBrokRec> fileRecords = csvReader.Enumerate<InsBrokRec>();
                try
                {
                    Parallel.ForEach(fileRecords, record => { if (filterInsBrokRec(record)) process(record); });
                }
                catch (AggregateException oXcptn)
                {
                    Console.WriteLine($"Line: {csvReader.FileLine} {oXcptn.Message}");
                    foreach (var iXcptn in oXcptn.InnerExceptions)
                    {
                        Console.WriteLine($"Line: {csvReader.FileLine} {iXcptn.Message}");
                    }
                }
                Console.WriteLine($"{Environment.NewLine}process.");
                try
                {
                    Parallel.ForEach(records, record1 => {
                        Parallel.ForEach(records, record2 => { if (record1.Key < record2.Key) link(record1, record2); } ); // only need link in one direction
                    });
                }
                catch (AggregateException oXcptn)
                {
                    Console.WriteLine($"{oXcptn.Message}");
                    foreach (var iXcptn in oXcptn.InnerExceptions)
                    {
                        Console.WriteLine($"{iXcptn.Message}");
                    }
                }
                Console.WriteLine($"{Environment.NewLine}link.");
                Parallel.ForEach(links, link => { if (7777 > link.Key.Key1) Console.WriteLine($"{link.Key} {link.Value}"); });
                //Parallel.ForEach(links, link => { if (link.Key.Key1 > link.Key.Key2) Console.WriteLine($"{link.Key} {link.Value}"); });
                Console.WriteLine($"{Environment.NewLine}Done.");
            }
        }

        private static void link(KeyValuePair<int, InsBrokRec> record1, KeyValuePair<int, InsBrokRec> record2)
        {
            //if (record1.Key > record2.Key) Console.WriteLine($"link {record1.Key} {record1.Key}");
            double nameProximity = Proximity(record1.Value.INS_BROKER_NAME, record2.Value.INS_BROKER_NAME);
            if ( .99 < nameProximity ) links.TryAdd( new LinkKey(record1.Key, record2.Key), nameProximity);
            //Console.WriteLine($@"{
            //    record1.Key}, {
            //    //record.ACK_ID}, {
            //    //record.FORM_ID}, {
            //    //record.ROW_ORDER}, {
            //    //record.INS_BROKER_US_STATE}, {
            //    record1.Value.INS_BROKER_NAME}, {
            //    record2.Key}, {
            //    record2.Value.INS_BROKER_NAME}"); //, {
            //                               //record.INS_BROKER_US_ADDRESS1}, {
            //                               //record.INS_BROKER_US_ADDRESS2}");
        }

        private static double Proximity(string name1, string name2)
        {
            return JaroWinkler.Proximity(name1, name2);
        }

        private static HashSet<string> stateFilterSet = new HashSet<string> { "CT", "MA", "ME", "NH", "RI", "VT" };

        private static bool FilterInsBrokRec( InsBrokRec record)
        {
            return stateFilterSet.Contains(record.INS_BROKER_US_STATE)
                //&& null != record.INS_BROKER_US_ADDRESS1
                ;
        }

        private static void ProcInsBrokRec(InsBrokRec record)
        {
            records.TryAdd(record.LineNumber, record);
            //Console.WriteLine($@"{records.Count}, {
            //    record.LineNumber}, {
            //    record.ACK_ID}, {
            //    record.FORM_ID}, {
            //    record.ROW_ORDER}, {
            //    record.INS_BROKER_US_STATE}, {
            //    record.INS_BROKER_NAME}"); //, {
            //    //record.INS_BROKER_US_ADDRESS1}, {
            //    //record.INS_BROKER_US_ADDRESS2}");
        }

    }

    struct LinkKey : IEquatable<LinkKey>
    {
        readonly int key1;
        readonly int key2;

        public LinkKey(int n1, int n2)
        {
            key1 = n1 < n2 ? n1 : n2;
            key2 = n1 < n2 ? n2 : n1;
        }

        public int Key1 { get { return key1; } }
        public int Key2 { get { return key2; } }

        public override int GetHashCode()
        {
            return key1.GetHashCode() ^ key2.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((LinkKey)obj);
        }

        public bool Equals(LinkKey other)
        {
            return other.key1==key1 && other.key2==key2;
        }

        public override string ToString()
        {
            return $"{key1} : {key2}";
        }
    }

}
