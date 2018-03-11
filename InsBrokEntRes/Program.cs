using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InsBrokEntRes
{
    class Program
    {
        static void Main(string[] args)
        {
            Ingest(@"Data\F_SCH_A_PART1_2016_latest_byZip.csv", FilterInsBrokRec, ProcInsBrokRec); // TODO args[0]
            //Console.Read();
        }

        private static IEnumerable<InsBrokRec> Enumerate(string fileSpec) // TODO class : IDisposable, w/ Enumerate method
        {
            //using (var streamReader = new StreamReader(fileSpec))
            //using (var csvReader = new CsvReader(streamReader))
            var streamReader = new StreamReader(fileSpec);
            var csvReader = new CsvReader(streamReader); // TODO in constructor
            {
                csvReader.Configuration.RegisterClassMap<InsBrokRecMap>();
                csvReader.Configuration.BadDataFound = null; // Skip bad fields, fixes some errors
                csvReader.Configuration.MissingFieldFound = null; // Skip missing fields, still get record
                return csvReader.GetRecords<InsBrokRec>();
            }
        }

        private static void Ingest(string fileSpec, Func<InsBrokRec, bool> FilterInsBrokRec, Action<InsBrokRec> process) // TODO ApplyProcess method 
        {
            //using (var streamReader = new StreamReader(fileSpec))
            //using (var csvReader = new CsvReader(streamReader))
            //{
            //    csvReader.Configuration.RegisterClassMap<InsBrokRecMap>();
            //    csvReader.Configuration.BadDataFound = null; // Skip bad fields, fixes some errors
            //    csvReader.Configuration.MissingFieldFound = null; // Skip missing fields, still get record
            //    IEnumerable<InsBrokRec> records = csvReader.GetRecords<InsBrokRec>();
                IEnumerable<InsBrokRec> records = Enumerate(fileSpec);
                try
                {
                    Parallel.ForEach(records, record => { if (FilterInsBrokRec(record)) process(record); });
                }
                catch (AggregateException oXcptn)
                {
                    //Console.WriteLine($"Line: {csvReader.Context.RawRow} {oXcptn.Message}");
                    foreach (var iXcptn in oXcptn.InnerExceptions)
                    {
                        //Console.WriteLine($"Line: {csvReader.Context.RawRow} {iXcptn.Message}");
                    }
                }
            //}
            // TODO dispose streamReader, csvReader
        }

        private static HashSet<string> stateFilterSet = new HashSet<string> { "ME", "NH", "VT" };
        private static bool FilterInsBrokRec( InsBrokRec record)
        {
            return stateFilterSet.Contains(record.INS_BROKER_US_STATE);
        }

        private static void ProcInsBrokRec(InsBrokRec record)
        {
            Console.WriteLine($"{record.LineNumber}, {record.ACK_ID}, {record.FORM_ID}, {record.ROW_ORDER}, {record.INS_BROKER_US_STATE}, {record.INS_BROKER_NAME}");
            records.Add(record.LineNumber, record);

        }

        static Dictionary<int, InsBrokRec> records = new Dictionary<int, InsBrokRec>();
        //static Dictionary<string, Dictionary<LinkKey, double>> linkByState = new Dictionary<string, Dictionary<LinkKey, double>>();
        //static Dictionary<string, Dictionary<int, double>> rankByState = new Dictionary<string, Dictionary<int, double>>();
        static Dictionary<LinkKey, double> linkByState = new Dictionary<LinkKey, double>();
        static Dictionary<int, double> rankByState = new Dictionary<int, double>();
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
