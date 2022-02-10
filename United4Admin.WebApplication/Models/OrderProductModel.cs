using System;
using System.Collections.Generic;

namespace United4Admin.WebApplication.Models
{
    public partial class OrderProductModel
    {
        public string SalesOrderId { get; set; }
        public string SalesOrderDetailId { get; set; }
        public int WvProductId { get; set; }
        public string ProductTypeCodedisplay { get; set; }
        public decimal? PricePerUnit { get; set; }
        public decimal? Quantity { get; set; }
        public string WvChildId { get; set; }
        public string WvTaxId { get; set; }
        public bool? WvTaxConsentOptIn { get; set; }      
    }
}
