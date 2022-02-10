using System;
using System.Collections.Generic;

namespace United4Admin.WebApplication.Models
{
    public partial class PreferenceModel
    {
        public string PreferenceId { get; set; }
        public string SalesOrderId { get; set; }
        public int? WvPreferenceTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
