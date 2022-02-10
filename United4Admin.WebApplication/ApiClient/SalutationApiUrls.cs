using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.APIClient
{
    public class SalutationApiUrls
    {
        public const string crmMappingApiServicePrefix = "crmmappingservice/api/";

        public static class SalutationApiUrl
        {
            public const string LoadList = crmMappingApiServicePrefix + "CRMMapping/GetSalutationList";
            public const string Load = crmMappingApiServicePrefix + "CRMMapping/GetSalutationListById/{ddlSalutation}";
            public const string Create = crmMappingApiServicePrefix + "CRMMapping/CreateSalutation";
            public const string Update = crmMappingApiServicePrefix + "CRMMapping/UpdateSalutation";
            public const string Delete = crmMappingApiServicePrefix + "CRMMapping/DeleteSalutation/{ddlSalutation}";
        }
    }
}
