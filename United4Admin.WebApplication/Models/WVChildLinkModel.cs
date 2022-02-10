using System;
using System.Collections.Generic;

namespace United4Admin.WebApplication.Models
{
    public partial class WVChildLinkModel
    {
        public string WvChildId { get; set; }
        public string WvParentContactId { get; set; }
        public DateTime WvSponsorshipStartDate { get; set; }
        public DateTime? WvSponsorshipEndDate { get; set; }
        public bool? WvChildStatus { get; set; }
    }
}
