using System;
using System.Collections.Generic;

namespace United4Admin.WebApplication.Models
{
    public partial class PaymentScheduleModel
    {
        public string PaymentScheduleId { get; set; }
        public string TransactionCurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public int LastTransactionId { get; set; }
        public string ReceiptOnContactId { get; set; }
        public DateTime? FirstPaymentDate { get; set; }
        public DateTime? NextPaymentDate { get; set; }
        public decimal? NextPaymentAmount { get; set; }
        public int? Frequency { get; set; }
        public decimal? RecurringAmount { get; set; }
        public int? StateCode { get; set; }
        public string WvSalesOrderDetailId { get; set; }
        public string WvAccountNumber { get; set; }
        public int? WvPaymentRetryAttempts { get; set; }
        public string WvAccountHolder { get; set; }
        public string WvIban { get; set; }
        public string WvSortCode { get; set; }
        public string WvExternalProviderSubscriptionId { get; set; }
        public int WvPaymentProviderId { get; set; }
    
    }
}
