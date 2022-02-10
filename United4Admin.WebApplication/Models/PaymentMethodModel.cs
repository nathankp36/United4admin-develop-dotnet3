using System;
using System.Collections.Generic;

namespace United4Admin.WebApplication.Models
{
    public partial class PaymentMethodModel
    {
        public string PaymentMethodId { get; set; }
        public int Type { get; set; }
        public string ContactId { get; set; }
        public string PaymentScheduleId { get; set; }
        public string Name { get; set; }
    }
}
