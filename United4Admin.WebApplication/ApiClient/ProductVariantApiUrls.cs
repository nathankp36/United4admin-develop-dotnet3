using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.APIClient
{
    public class ProductVariantApiUrls
    {
        public const string crmMappingApiServicePrefix = "crmmappingservice/api/";

        public static class ProductVariantApiUrl
        {
            public const string LoadList = crmMappingApiServicePrefix + "CRMMapping/GetProdcutList";
            public const string Load = crmMappingApiServicePrefix + "CRMMapping/GetProductListById/{ddlProductTypeCode}";
            public const string Create = crmMappingApiServicePrefix + "CRMMapping/CreateProduct";
            public const string Update = crmMappingApiServicePrefix + "CRMMapping/UpdateProduct";
            public const string Delete = crmMappingApiServicePrefix + "CRMMapping/DeleteProduct/{ddlProductTypeCode}";
            public const string GetIncidentTypes = crmMappingApiServicePrefix + "CRMMapping/GetIncidentTypeList";
            public const string GetPledgeTypes = crmMappingApiServicePrefix + "CRMMapping/GetPledgeTypeList";
        }
    }
}
