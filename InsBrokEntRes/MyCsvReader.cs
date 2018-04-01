using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;

namespace InsBrokEntRes
{
    /// <summary>
    /// CSV file reader
    /// Wraps CsvHelper.
    /// </summary>
    internal class MyCsvReader : IDisposable
    {
        private CsvReader csvReader;
        private StreamReader streamReader;
        //public ReadingContext Context { get { return csvReader.Context; } }
        public int FileLine { get { return csvReader.Context.RawRow; } }

        public MyCsvReader(string fileSpec)
        {
            this.streamReader = new StreamReader(fileSpec);
            this.csvReader = new CsvReader(streamReader);
            csvReader.Configuration.RegisterClassMap<InsBrokRecMap>();
            csvReader.Configuration.BadDataFound = null; // Skip bad fields, fixes some errors
            csvReader.Configuration.MissingFieldFound = null; // Skip missing fields, still get record
        }

        internal IEnumerable<T> Enumerate<T>()
        {
            return this.csvReader.GetRecords<T>();
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
                    this.csvReader.Dispose();
                    this.streamReader.Dispose();
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
    }
}