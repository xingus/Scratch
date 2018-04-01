using System;
using System.Text.RegularExpressions;

namespace DivComLedger
{
    public interface ILedgerRecord
    {
        void Parse();
        int Index { get; set; }
        string Text { get; set; }
        //
        bool Ok { get; set; }
        string TransactionType { get; set; } // TODO to enum
        string ItemDescription { get; set; }
        double ItemQuantity { get; set; }
        string ItemUnits { get; set; }
        string Currency { get; set; }
        decimal Price { get; set; }
    }

    public class LedgerRecord : ILedgerRecord
    {
        public int Index { get; set; }
        public string Text { get; set; }
        public string TransactionType { get; set; } // TODO to enum
        public string ItemDescription { get; set; }
        public double ItemQuantity { get; set; }
        public string ItemUnits { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
        public bool Ok { get; set; }

        //public LedgerRecord() { }

        public void Parse()
        {
            try
            {
                var match = Regex.Match(Text, @"(?<type>[PS])\s(?<description>.{29})\s*(?<unit1>[^0-9. ]*\s*)?(?<quantity>\d+[.]?\d*)\s*(?<unit2>[^0-9. ]*)\s*(?<currency1>[^0-9. ]*\s*)?(?<price>\d+[.]?\d*)\s*(?<currency2>[^0-9. ]*)");
                TransactionType = match.Groups["type"].Value;
                ItemDescription = match.Groups["description"].Value.Trim();
                ItemUnits = match.Groups["unit1"].Value.Trim();
                ItemQuantity = Double.Parse(match.Groups["quantity"].Value);
                if ("" == ItemUnits) ItemUnits = match.Groups["unit2"].Value;
                Currency = match.Groups["currency1"].Value.Trim();
                Price = Decimal.Parse(match.Groups["price"].Value);
                if ("" == Currency) Currency = match.Groups["currency2"].Value;
                Ok = true;
                //
            }
            catch (Exception e)
            {
                Ok = false;
                // TODO Msg = e... Index
            }
            return;

        }

        public override string ToString()
        {
            return $"{TransactionType} {ItemDescription,-30} {ItemQuantity+ItemUnits,-14} {Currency}{Price:F2}";
        }
    }
}