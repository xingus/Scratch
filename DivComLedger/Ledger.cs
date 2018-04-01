using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace DivComLedger
{
    public interface ILedger
    {
        void Enter(ILedgerRecord record);
    }

    public class Ledger : ILedger
    {
        private ConcurrentDictionary<String, int> _dictionary; // TODO int => LedgerEntry

        public Ledger()
        {
            _dictionary = new ConcurrentDictionary<String, int>();
        }

        public void Enter(ILedgerRecord record)
        {
            _dictionary.AddOrUpdate(record.ItemDescription, record.Index, (key, current) => record.Index + current);
        }
    }
}