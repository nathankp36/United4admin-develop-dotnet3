using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.APIClient
{
    public static class CRMExtractApiUrls
    {
        public const string crmExtractApiService = "thddlservice/api/CRMDataExtract/ExtractCRMData";
    }
    public static class ChosenExtractApiUrls
    {
        public const string chosenExtractLoad = "thddlservice//api/Chosen/GetChosenTransaction/{salesOrderId}";
        public const string chosenExtractLoadList = "thddlservice//api/Chosen/GetChosenTransactionList";
        public const string chosenNewExtractLoadList = "thddlservice//api/Chosen/GetChosenNewTransactionList";
    }
    public static class CRMQuarantineApiUrls
    {
        public const string crmQuarantineApiService = "Orchestrationms/api/Orchestration/GetCRMTransactionQuarantineInfo";
        public const string crmQuarantineUpdateApiService = "Orchestrationms/api/Orchestration/UpdateCRMTransactionQuarantineInfo";
    }
    public static class AddOnDonationApiUrls
    {
        public const string AddOnDonationLoadList = "thddlservice/api/AddOnDonation/GetAddOnDonationList";
    }
    public static class SupporterTransactionApiUrls
    {
        public const string GetSupporterTransaction = "thddlservice/api/Supporter/GetSupporterTransaction/{salesOrderId}";
    }
}

