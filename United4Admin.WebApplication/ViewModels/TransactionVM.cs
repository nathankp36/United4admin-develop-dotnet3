using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class TransactionVM
    {
        public int TransactionId { get; set; }
        public int PaymentMethodId { get; set; }
        public bool GiftAidOptIn { get; set; }
        [Display(Name = "Payment Reference")]
        public string ExternalPaymentId { get; set; }
        public string Iban { get; set; }
        public string AccountHolder { get; set; }
        public string SortCode { get; set; }
        public string AccountNumber { get; set; }
        public DateTime? ExtractDate { get; set; }
        public bool? Exported { get; set; }
        [Display(Name = "Status")]
        public string TransactionStatus { get; set; }
        public int ContactId { get; set; }
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        public SupporterVM Contact { get; set; }
        public ICollection<TransactionItemVM> TransactionItem { get; set; }

    }
}
