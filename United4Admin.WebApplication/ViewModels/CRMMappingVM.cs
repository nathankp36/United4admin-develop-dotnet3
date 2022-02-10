using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class CRMMappingVM
    {
        public IList<SalutationVM> SalutationVM { get; set; }
        public IList<ProductVariantVM> ProductVariantVM { get; set; }
        public IList<PaymentMethodVM> PaymentMethodVM { get; set; }
    }
}
