using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ViewModels;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IImageStoreFactory
    {
        Task<ApiResponse> DeleteImage(string imageName);
        Task<CloudBlockBlob> GetBlobImageFile(string imageName);
        Task<object> GetSecureUrlImage(string imageName);
        Task<BlobStorageVM> GetBobStorageDetails();
    }
}
