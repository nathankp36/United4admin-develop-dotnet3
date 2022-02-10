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
    public class ChildDataCacheFactory : IChildDataCacheFactory
    {

        protected ApiClient apiClient;
        private readonly IFeatureSwitchFactory _featureSwitchFactory;

        public ChildDataCacheFactory()
        {
            this.apiClient = new ApiClient();
            this._featureSwitchFactory = new FeatureSwitchFactory();
        }

        public ChildDataCacheFactory(ApiClient _apiClient, IFeatureSwitchFactory featureSwitchFactory)
        {
            this.apiClient = _apiClient;
            this._featureSwitchFactory = featureSwitchFactory;
        }
        private bool kubernetes;
        public async Task<bool> FeatureSwitch()
        {
            try
            {
                bool result = await _featureSwitchFactory.CheckFeatureSwitch("name=ChildDataCacheMSInKubernetes");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UploadChildIds(List<string> ChildIdList)
        {
            try
            {
                kubernetes = await FeatureSwitch();
                var requestUrl = apiClient.CreateRequestUri(ChildDataCacheApiUrls.UploadChildIds);
                if(kubernetes)
                {
                    requestUrl = apiClient.CreateRequestUri(ChildDataCacheApiUrls.UploadChildIdsK8s);
                }
                await apiClient.PostAsync<List<string>>(requestUrl, ChildIdList);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> GetUpdateProgress()
        {
            try
            {
                kubernetes = await FeatureSwitch();
                ProcessingUpdateVM processingUpdateVM = new ProcessingUpdateVM();
                var requestUrl = apiClient.CreateRequestUri(ChildDataCacheApiUrls.GetProgress);
                if(kubernetes)
                {
                    requestUrl = apiClient.CreateRequestUri(ChildDataCacheApiUrls.GetProgressK8s);
                }
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);                
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> GetUpdateProgressById(int Id)
        {
            try
            {
                kubernetes = await FeatureSwitch();
                ProcessingUpdateVM processingUpdateVM = new ProcessingUpdateVM();
                var requestUrl = apiClient.CreateRequestUri(ChildDataCacheApiUrls.GetProgressById, "Id=" + Id);
                if(kubernetes)
                {
                    requestUrl = apiClient.CreateRequestUri(ChildDataCacheApiUrls.GetProgressByIdK8s, "Id=" + Id);
                }
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                          
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
