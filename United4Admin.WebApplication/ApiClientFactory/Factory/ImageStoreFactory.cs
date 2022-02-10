using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class ImageStoreFactory : IImageStoreFactory
    {

        protected ApiClient apiClient;
        private readonly IFeatureSwitchFactory _featureSwitchFactory;

        public ImageStoreFactory()
        {
            this.apiClient = new ApiClient();
            this._featureSwitchFactory = new FeatureSwitchFactory();
        }

        public ImageStoreFactory(ApiClient _apiClient, IFeatureSwitchFactory featureSwitchFactory)
        {
            this.apiClient = _apiClient;
            this._featureSwitchFactory = featureSwitchFactory;
        }

        private bool kubernetes;
        public async Task<bool> FeatureSwitch()
        {
            try
            {
                bool result = await _featureSwitchFactory.CheckFeatureSwitch("name=ImageStoreMSInKubernetes");
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CloudBlockBlob> GetBlobImageFile(string imageName)
        {
            try
            {
                kubernetes = await FeatureSwitch();
                var requestUrl = apiClient.CreateRequestUri(ImageStoreApiUrls.GetBlobImageFile.Replace("{imageName}", Convert.ToString(imageName)));
                if (kubernetes)
                {
                    requestUrl = apiClient.CreateRequestUri(ImageStoreApiUrls.GetBlobImageFileK8s.Replace("{imageName}", Convert.ToString(imageName)));
                }
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                CloudBlockBlob cloudBlockBlob = JsonConvert.DeserializeObject<CloudBlockBlob>(Convert.ToString(response.ResponseObject));

                return cloudBlockBlob;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BlobStorageVM> GetBobStorageDetails()
        {
            try
            {
                kubernetes = await FeatureSwitch();
                var requestUrl = apiClient.CreateRequestUri(ImageStoreApiUrls.GetBlobStorageDetails);
                if (kubernetes)
                {
                    requestUrl = apiClient.CreateRequestUri(ImageStoreApiUrls.GetBlobStorageDetailsK8s);
                }
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                BlobStorageVM blobStorageVM = JsonConvert.DeserializeObject<BlobStorageVM>(Convert.ToString(response.ResponseObject));

                return blobStorageVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<object> GetSecureUrlImage(string imageName)
        {
            try
            {
                kubernetes = await FeatureSwitch();
                var requestUrl = apiClient.CreateRequestUri(ImageStoreApiUrls.GetSecureUrlImage.Replace("{imageName}", Convert.ToString(imageName)));
                if (kubernetes)
                {
                    requestUrl = apiClient.CreateRequestUri(ImageStoreApiUrls.GetSecureUrlImageK8s.Replace("{imageName}", Convert.ToString(imageName)));
                }
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                var result = response.ResponseObject;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ApiResponse> DeleteImage(string imageName)
        {
            try
            {
                kubernetes = await FeatureSwitch();
                var requestUrl = apiClient.CreateRequestUri(ImageStoreApiUrls.Delete.Replace("{imageName}", Convert.ToString(imageName)));
                if (kubernetes)
                {
                    requestUrl = apiClient.CreateRequestUri(ImageStoreApiUrls.DeleteK8s.Replace("{imageName}", Convert.ToString(imageName)));
                }
                var response = await apiClient.DeleteAsync<ViewModels.ImageInfoVM>(requestUrl);
                //return JsonConvert.DeserializeObject<ApiResponse>(Convert.ToString(response));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool disposed = false;

        /// <summary>
        ///Dispose the object used
        /// </summary>
        /// <param name=""></param>
        /// <returns>no values</returns>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.apiClient.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
