using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.APIClient
{
    public class PaymentMethodApiUrls
    {
        public const string crmMappingApiServicePrefix = "crmmappingservice/api/";

        public static class PaymentMethodApiUrl
        {
            public const string LoadList = crmMappingApiServicePrefix + "CRMMapping/GetPaymentMethodList";
            public const string Load = crmMappingApiServicePrefix + "CRMMapping/GetPaymentMethodListById/{id}";
            public const string Create = crmMappingApiServicePrefix + "CRMMapping/CreatePaymentMethod";
            public const string Update = crmMappingApiServicePrefix + "CRMMapping/UpdatePaymentMethod";
            public const string Delete = crmMappingApiServicePrefix + "CRMMapping/DeletePaymentMethod/{id}";
        }
    }
}
