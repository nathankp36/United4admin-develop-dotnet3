using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class FallbackValuesFactory : IFallbackValuesFactory
    {
        protected ApiClient apiClient;

        public FallbackValuesFactory()
        {
            this.apiClient = new ApiClient();
        }

        public FallbackValuesFactory(ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }

        public async Task<List<FallbackValuesVM>> LoadList()
        {
            try
            {
                List<FallbackValuesVM> fallbackValuesVMList = new List<FallbackValuesVM>();
                var requestUrl = apiClient.CreateRequestUri(FallbackValuesApiUrls.FallbackValuesApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                fallbackValuesVMList = JsonConvert.DeserializeObject<List<FallbackValuesVM>>(Convert.ToString(response.ResponseObject));
                return fallbackValuesVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Load list by string value
        public async Task<FallbackValuesVM> LoadListById(string simmaField)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(FallbackValuesApiUrls.FallbackValuesApiUrl.Load.Replace("{simmaField}", simmaField));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                FallbackValuesVM FallbackValuesVM = JsonConvert.DeserializeObject<FallbackValuesVM>(Convert.ToString(response.ResponseObject));
                return FallbackValuesVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<FallbackValuesVM> Load(int id)
        {
            throw new NotSupportedException();
        }

        public async Task<ApiResponse> Create(FallbackValuesVM FallbackValuesVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(FallbackValuesApiUrls.FallbackValuesApiUrl.Create);
                var response = await apiClient.PostAsync<FallbackValuesVM>(requestUrl, FallbackValuesVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> Update(FallbackValuesVM FallbackValuesVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(FallbackValuesApiUrls.FallbackValuesApiUrl.Update);
                var response = await apiClient.PutSync<FallbackValuesVM>(requestUrl, FallbackValuesVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<ApiResponse> Delete(int id)
        {
            throw new NotSupportedException();
        }

        //Delete list by string value
        public async Task<ApiResponse> DeleteById(string simmaField)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(FallbackValuesApiUrls.FallbackValuesApiUrl.Delete.Replace("{simmaField}", simmaField));
                var response = await apiClient.DeleteAsync<FallbackValuesVM>(requestUrl);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<IList<WorkflowStatusVM>> GetWorkflowStatuses()
        {
            throw new NotSupportedException();
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
