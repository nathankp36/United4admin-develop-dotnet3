using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.APIClient
{
    public class MotivationApiUrls
    {
        public const string crmMappingApiServicePrefix = "crmmappingservice/api/";

        public static class MotivationApiUrl
        {
            public const string LoadList = crmMappingApiServicePrefix + "SimmaMapping/GetMotivationList";
            public const string Load = crmMappingApiServicePrefix + "SimmaMapping/GetMotivationListById/{campaignCode}";
            public const string Create = crmMappingApiServicePrefix + "SimmaMapping/CreateMotivation";
            public const string Update = crmMappingApiServicePrefix + "SimmaMapping/UpdateMotivation";
            public const string Delete = crmMappingApiServicePrefix + "SimmaMapping/DeleteMotivation/{campaignCode}";
        }
    }
}
