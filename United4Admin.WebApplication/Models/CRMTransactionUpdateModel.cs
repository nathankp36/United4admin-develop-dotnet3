using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.Models
{
    public class CRMTransactionUpdateModel
    {
        public int Id { get; set; }
        public string SalesOrderDetailId { get; set; }
        public string CRMTransactionRef { get; set; }
        public string CRMSupporterId { get; set; }
        public DateTime? ProcessDate { get; set; }
        public string StatusReason { get; set; }
        public string StatusCode { get; set; }
        public string NoAttempt { get; set; }      
    }
}
