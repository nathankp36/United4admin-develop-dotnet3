using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace United4Admin.WebApplication.ViewModels
{
    public class AdyenTransactionVM
    {
        public int AdyenTransactionId { get; set; }
        public int LastTransactionId { get; set; }
        [Required]
        [Display(Name = "Recurring Token")]
        public string RecurringToken { get; set; }
        [Required]
        [Display(Name = "Bill Cycle StartDate")]
        public DateTime BillCycleStartDate { get; set; }
        [Display(Name = "Next Payment Date")]
        public DateTime BillCycleNextDate { get; set; }
        [Display(Name = "Last Payment Date")]
        public DateTime LastPaymentDate { get; set; }
        [Required]
        [Display(Name = "Last Payment Status")]
        public string LastPaymentStatus { get; set; }
        public int NoOfRetryAttempts { get; set; }
        [Display(Name = "ID")]
        public int ContactId { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
        [Required]
        [Display(Name = "Shopper Reference")]
        public string ShopperReference { get; set; }
        [Required]
        [Display(Name = "Currency Code")]
        public string CurrencyCode { get; set; }
        [NotMapped]
        public string Search { get; set; }
        [NotMapped]
        public string SearchDDL { get; set; }
        [NotMapped]
        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "From Date")]
        public DateTime SearchFromDate { get; set; } = DateTime.Today;
        [NotMapped]
        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "To Date")]
        public DateTime SearchToDate { get; set; } = DateTime.Today;

        public virtual TransactionVM Transaction { get; set; }
        public virtual SupporterVM Supporter { get; set; }
        [NotMapped]
        public virtual ICollection<TransactionVM> TransactionList { get; set; }
    }

    public enum DDLSearch
    {
        EmailId,
        Name,
        TransactionDate,
        DateRange,
    }
}
