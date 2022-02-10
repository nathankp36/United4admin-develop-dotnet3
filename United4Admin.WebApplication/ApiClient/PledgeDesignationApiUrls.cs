using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.APIClient
{
    public class PledgeDesignationApiUrls
    {
        public const string crmMappingApiServicePrefix = "crmmappingservice/api/";

        public static class PledgeDesignationApiUrl
        {
            public const string LoadList = crmMappingApiServicePrefix + "SimmaMapping/GetPledgeDesignationList";
            public const string Load = crmMappingApiServicePrefix + "SimmaMapping/GetPledgeDesignationListById/{wVformId}";
            public const string Create = crmMappingApiServicePrefix + "SimmaMapping/CreatePledgeDesignation";
            public const string Update = crmMappingApiServicePrefix + "SimmaMapping/UpdatePledgeDesignation";
            public const string Delete = crmMappingApiServicePrefix + "SimmaMapping/DeletePledgeDesignation/{wVformId}";
        }
    }
}
