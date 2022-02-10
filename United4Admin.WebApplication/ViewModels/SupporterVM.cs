using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class SupporterVM
    {
        public int ContactId { get; set; }
        [Display(Name = "Email")]
        public string EmailAddress1 { get; set; }
        public string MobilePhone { get; set; }
        public string Salutation { get; set; }
        [Display(Name = "Full Name")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address1Line1 { get; set; }
        public string Address1City { get; set; }
        public string Address1PostalCode { get; set; }
        public string Address1Country { get; set; }
        public string TaxId { get; set; }
        public bool DataProcessingConsent { get; set; }
        public bool MarketingCommsConsent { get; set; }
        public string RecurringToken { get; set; }
        public string ShopperReference { get; set; }

        public TransactionVM Transactions { get; set; }
    }
}
