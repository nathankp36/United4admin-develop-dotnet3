using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace United4Admin.WebApplication.APIClient
{
    public class ImageStoreApiUrls
    {        
        public const string ImageStoreApiServicePrefix = "blobstorage/api/ImageStorageActions/";

        public const string GetBlobImageFile = ImageStoreApiServicePrefix + "GetBlobImageFile/{imageName}";
        public const string GetSecureUrlImage = ImageStoreApiServicePrefix + "GetSecureUrlImage/{imageName}";
        public const string GetBlobStorageDetails = ImageStoreApiServicePrefix + "GetBlobStorageDetails";
        public const string Delete = ImageStoreApiServicePrefix + "DeleteImage/{imageName}";
        public const string UploadImage = ImageStoreApiServicePrefix + "UploadImage?fileName={nameOfFile}";
        // public const string UploadImage = "api/ImageStorageActions/UploadImage?fileName={nameOfFile}";

        public const string ImageStoreApiK8sServicePrefix = "k8simagestore/api/ImageStorageActions/";

        public const string GetBlobImageFileK8s = ImageStoreApiK8sServicePrefix + "GetBlobImageFile/{imageName}";
        public const string GetSecureUrlImageK8s = ImageStoreApiK8sServicePrefix + "GetSecureUrlImage/{imageName}";
        public const string GetBlobStorageDetailsK8s = ImageStoreApiK8sServicePrefix + "GetBlobStorageDetails";
        public const string DeleteK8s = ImageStoreApiK8sServicePrefix + "DeleteImage/{imageName}";
        public const string UploadImageK8s = ImageStoreApiK8sServicePrefix + "UploadImage?fileName={nameOfFile}";

    }
}