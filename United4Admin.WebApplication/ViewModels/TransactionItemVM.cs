using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class TransactionItemVM
    {
        public int TransactionItemId { get; set; }
        public int? TransactionId { get; set; }
        public int ProductId { get; set; }
        public string ProductVariantId { get; set; }
        public string ChildId { get; set; }
        public int DonationFrequency { get; set; }
        public decimal DonationAmount { get; set; }
        public int Quantity { get; set; }
        public string CurrencyCode { get; set; }
    }
}
