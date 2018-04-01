using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DivComLedger
{

    public class LedgerRecordFile<LedgerRecord> : IDisposable, IEnumerable<LedgerRecord> where LedgerRecord : ILedgerRecord, new()
    {

        private readonly Config _config;
        //private readonly StreamReader _streamReader;
        //private readonly ILedgerRecord _record;

        //public int FileLine() { return _csvReader.Context.RawRow; }

        public LedgerRecordFile(IOptions<Config> config) //, ILedgerRecord record)
        {
            _config = config.Value;
            //_record = record;
            //_streamReader = new StreamReader(_config.DataFile);
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
                    //_streamReader.Dispose();
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

        public IEnumerator<LedgerRecord> GetEnumerator()
        {
            return File.ReadLines(_config.DataFile)
                .Select((t, i) => {
                    var r = new LedgerRecord
                    {
                        Index = i,
                        Text = t
                    }; return r; })
                .GetEnumerator();
            ;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}