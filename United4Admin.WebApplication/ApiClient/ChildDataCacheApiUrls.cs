using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace United4Admin.WebApplication.APIClient
{
    public class ChildDataCacheApiUrls
    {
        public const string ChildDataCacheApiServicePrefix = "datacacheservice/api/ChildIdImport/";        

        public const string UploadChildIds = ChildDataCacheApiServicePrefix + "ImportChildId";
        public const string GetProgress = ChildDataCacheApiServicePrefix + "GetLatestProgressUpdate";
        public const string GetProgressById = ChildDataCacheApiServicePrefix + "GetProgressUpdateById";

        public const string ChildDataCacheApiK8sServicePrefix = "k8sdatacacheservice/api/ChildIdImport/";

        public const string UploadChildIdsK8s = ChildDataCacheApiK8sServicePrefix + "ImportChildId";
        public const string GetProgressK8s = ChildDataCacheApiK8sServicePrefix + "GetLatestProgressUpdate";
        public const string GetProgressByIdK8s = ChildDataCacheApiK8sServicePrefix + "GetProgressUpdateById";

    }
}