﻿using CsvHelper.Configuration;

namespace InsBrokEntRes
{
    public class InsBrokRec
    {
        public string ACK_ID { get; set; }
        public string FORM_ID { get; set; }
        public string ROW_ORDER { get; set; }
        public string INS_BROKER_NAME { get; set; }
        public string INS_BROKER_US_ADDRESS1 { get; set; }
        public string INS_BROKER_US_ADDRESS2 { get; set; }
        public string INS_BROKER_US_CITY { get; set; }
        public string INS_BROKER_US_STATE { get; set; }
        public string INS_BROKER_US_ZIP { get; set; }
        public string INS_BROKER_FOREIGN_ADDRESS1 { get; set; }
        public string INS_BROKER_FOREIGN_ADDRESS2 { get; set; }
        public string INS_BROKER_FOREIGN_CITY { get; set; }
        public string INS_BROKER_FOREIGN_PROV_STATE { get; set; }
        public string INS_BROKER_FOREIGN_CNTRY { get; set; }
        public string INS_BROKER_FOREIGN_POSTAL_CD { get; set; }
        public string INS_BROKER_COMM_PD_AMT { get; set; }
        public string INS_BROKER_FEES_PD_AMT { get; set; }
        public string INS_BROKER_FEES_PD_TEXT { get; set; }
        public string INS_BROKER_CODE { get; set; }
        public int LineNumber { get; set; }
    }

    public sealed class InsBrokRecMap : ClassMap<InsBrokRec>
    {
        public InsBrokRecMap()
        {
            Map(m => m.ACK_ID);
            Map(m => m.FORM_ID);
            Map(m => m.ROW_ORDER);
            Map(m => m.INS_BROKER_NAME);
            Map(m => m.INS_BROKER_US_ADDRESS1);
            Map(m => m.INS_BROKER_US_ADDRESS2);
            Map(m => m.INS_BROKER_US_STATE);
            Map(m => m.LineNumber).ConvertUsing(row => row.Context.RawRow);
        }
    }
}