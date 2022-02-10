using System;
using System.Collections.Generic;

namespace United4Admin.WebApplication.Models
{
    public partial class OrderModel
    {
        public string SalesOrderId { get; set; }
        public string ContactId { get; set; }
        public string WvCampaignCode { get; set; }
        public DateTime? DateFulfilled { get; set; }
        public bool? WvExtractStatus { get; set; }
        public DateTime? WvExtractDate { get; set; }   

    }
}
