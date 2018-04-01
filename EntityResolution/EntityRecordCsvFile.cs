using CsvHelper;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace EntityResolution
{
    public interface IEnumerator<T>
    {
        IEnumerable<RecordType> GetEnumerable<RecordType>();
        int FileLine();
    }

    /// <summary>
    /// CSV file reader
    /// Wraps CsvHelper.
    /// </summary>
    public class EntityRecordCsvFile<EntityRecord> : IDisposable, IEnumerator<EntityRecord>
    {

        private readonly Config _config;
        private CsvReader _csvReader;
        private StreamReader _streamReader;
        //public ReadingContext Context { get { return csvReader.Context; } }

        public int FileLine() { return _csvReader.Context.RawRow; }

        public EntityRecordCsvFile(IOptions<Config> config)
        {
            _config = config.Value;
            _streamReader = new StreamReader(_config.File);
            _csvReader = new CsvReader(_streamReader);
            _csvReader.Configuration.RegisterClassMap<EntityRecordMap>(); // TODO dependency inject retrieve based on RecordType
            _csvReader.Configuration.BadDataFound = null; // Skip bad fields, fixes some errors
            _csvReader.Configuration.MissingFieldFound = null; // Skip missing fields, still get record
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    _csvReader.Dispose();
                    _streamReader.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion

        public IEnumerable<RecordType> GetEnumerable<RecordType>()
        {
            return _csvReader.GetRecords<RecordType>();
        }

    }
}