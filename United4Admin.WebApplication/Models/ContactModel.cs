using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace United4Admin.WebApplication.Models
{
    public partial class ContactModel
    {     
        public string ContactId { get; set; }
        public string EmailAddress1 { get; set; }
        public string MobilePhone { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address1Line1 { get; set; }
        public string Address1Line2 { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? GenderCode { get; set; }
        public string Address1City { get; set; }
        public string Address1PostalCode { get; set; }
        public string Address1Country { get; set; }
        public int? CustomerTypeCode { get; set; }
        public string WvSupporterId { get; set; }
        public DateTime? CreatedOn { get; set; }     
    }
}
