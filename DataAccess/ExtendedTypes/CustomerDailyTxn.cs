using System;

namespace DataAccess.ExtendedTypes
{
    public class CustomerDailyTxn
    {
        public string CustomerName { get;  set; }
        // Collection Spot Id.
        public int? CSId { get; set; }
        public int TransactionId { get; set; }
        public int AmountReceived { get; set; }
        public int Balance { get; set; }
        public DateTime TxnDate { get; set; }
        public int CustomerId { get; set; }
        public int CustomerSeqId { get; set; }
        public int Interest { get; set; }
    }
}
