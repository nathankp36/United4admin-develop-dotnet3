using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.APIClient
{
    public class AdyenPaymentApiUrls
    {
        public const string adyenPaymentsApiServicePrefix = "ddltransactionservice/api/Transaction/";
        public const string LoadList = adyenPaymentsApiServicePrefix + "LoadList";
        public const string LoadTransaction = adyenPaymentsApiServicePrefix + "LoadTransaction/{contactId}";
        public const string Create = "aydenservice/api/AdyenRecurring/AdyenManualPayment";
    }
}
