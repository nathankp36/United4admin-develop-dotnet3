using System;
using System.Collections.Generic;
using System.Text;

namespace United4Admin.WebApplication.Models
{
    public partial class CRMTransactionModel
    {
        public CRMTransactionModel()
        {
            Contact = new ContactModel();
            Order = new OrderModel();
            OrderProduct = new OrderProductModel();
            Preference = new HashSet<PreferenceModel>();
            PaymentMethod = new PaymentMethodModel();
            PaymentSchedule = new PaymentScheduleModel();
            Transaction = new TransactionModel();
            WvchildLink = new WVChildLinkModel();
            CRMMapping = new CRMMappingModel();
            CRMTransaction = new CRMTransactionsModel();
        }

        public virtual ContactModel Contact { get; set; }
        public virtual OrderModel Order { get; set; }
        public virtual OrderProductModel OrderProduct { get; set; }
        public virtual ICollection<PreferenceModel> Preference { get; set; }
        public virtual PaymentMethodModel PaymentMethod { get; set; }
        public virtual PaymentScheduleModel PaymentSchedule { get; set; }
        public virtual TransactionModel Transaction { get; set; }
        public virtual WVChildLinkModel WvchildLink { get; set; }
        public virtual CRMMappingModel CRMMapping { get; set; }
        public virtual CRMTransactionsModel CRMTransaction { get; set; }
    }
}
