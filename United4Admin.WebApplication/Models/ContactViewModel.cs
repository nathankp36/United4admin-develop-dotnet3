using System;
using System.Collections.Generic;
using System.Text;

namespace United4Admin.WebApplication.Models
{
    public class ContactViewModel
    {
        public ContactViewModel()
        {
            Contact = new ContactModel();
            Order = new OrderModel();
            OrderProduct = new OrderProductModel();
            Preference = new HashSet<PreferenceModel>();
            PaymentMethod = new PaymentMethodModel();
            PaymentSchedule = new PaymentScheduleModel();
            Transaction = new TransactionModel();
            WvchildLink = new WVChildLinkModel();
            WVCountry = new WVCountryModel();
        }

        public virtual ContactModel Contact { get; set; }
        public virtual OrderModel Order { get; set; }
        public virtual OrderProductModel OrderProduct { get; set; }
        public virtual ICollection<PreferenceModel> Preference { get; set; }
        public virtual PaymentMethodModel PaymentMethod { get; set; }
        public virtual PaymentScheduleModel PaymentSchedule { get; set; }
        public virtual TransactionModel Transaction { get; set; }
        public virtual WVChildLinkModel WvchildLink { get; set; }
        public virtual WVCountryModel WVCountry { get; set; }
    }
}
