using System;
using System.Collections.Generic;

namespace United4Admin.WebApplication.Models
{
    public partial class TransactionModel
    {
        public string TransactionId { get; set; }
        public string ReceiptOnContactId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string TransactionCurrencyId { get; set; }
        public string TransactionPaymentScheduleId { get; set; }
        public int StateCode { get; set; }
        public decimal Amount { get; set; }      
        public string WvPaymentProviderTransactionId { get; set; }
       
    }
}
