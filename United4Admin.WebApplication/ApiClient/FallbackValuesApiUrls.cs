using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.APIClient
{
    public class FallbackValuesApiUrls
    {
        public const string crmMappingApiServicePrefix = "crmmappingservice/api/";

        public static class FallbackValuesApiUrl
        {
            public const string LoadList = crmMappingApiServicePrefix + "SimmaMapping/GetFallbackValueList";
            public const string Load = crmMappingApiServicePrefix + "SimmaMapping/GetfallbackValueListById/{simmaField}";
            public const string Create = crmMappingApiServicePrefix + "SimmaMapping​/CreatefallbackValue";
            public const string Update = crmMappingApiServicePrefix + "SimmaMapping/UpdatefallbackValue";
            public const string Delete = crmMappingApiServicePrefix + "SimmaMapping/DeletefallbackValue/{simmaField}";
        }
    }
}
