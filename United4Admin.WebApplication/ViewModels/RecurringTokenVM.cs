using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace United4Admin.WebApplication.ViewModels
{
    public class RecurringTokenVM
    {
        public int AdyenTransactionId { get; set; }
        [Required]
        public int LastTransactionId { get; set; }
        [Required]
        [Display(Name = "Recurring Token")]
        public string RecurringToken { get; set; }
        public DateTime BillCycleStartDate { get; set; }
        public DateTime BillCycleNextDate { get; set; }
        public DateTime LastPaymentDate { get; set; }
        [Required]
        [Display(Name = "Last Payment Status")]
        public string LastPaymentStatus { get; set; }
        public int NoOfRetryAttempts { get; set; }
        [Required]
        public int ContactId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [Display(Name = "Shopper Reference")]
        public string ShopperReference { get; set; }
        [Required]
        [Display(Name = "Currency Code")]
        public string CurrencyCode { get; set; }
    }
}
