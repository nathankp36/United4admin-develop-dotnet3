using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace United4Admin.WebApplication.APIClient
{
    public class ChildDataCacheForWebApiUrls
    {
        public const string ChildDataCacheApiServicePrefix = "childcacheforwebservice/api/ChildCache/";        

        public const string DeleteChildRecord = ChildDataCacheApiServicePrefix + "DeleteChild";
        public const string GetAllRecords = ChildDataCacheApiServicePrefix + "GetAllInternal";
        public const string GetChildRecordById = ChildDataCacheApiServicePrefix + "GetChildByIdInternal";
        public const string UpdateChildSponsored = ChildDataCacheApiServicePrefix + "UpdateChildSponsored";

    }
}