using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United4Admin.WebApplication.ViewModels;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IPaymentMethodFactory : IApiClientFactory<PaymentMethodVM>
    {
    }
}
