using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class MotivationFactory : IMotivationFactory
    {
        protected ApiClient apiClient;

        public MotivationFactory()
        {
            this.apiClient = new ApiClient();
        }

        public MotivationFactory(ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }

        public async Task<List<MotivationVM>> LoadList()
        {
            try
            {
                List<MotivationVM> motivationVMList = new List<MotivationVM>();
                var requestUrl = apiClient.CreateRequestUri(MotivationApiUrls.MotivationApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                motivationVMList = JsonConvert.DeserializeObject<List<MotivationVM>>(Convert.ToString(response.ResponseObject));
                return motivationVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Load list by string value
        public async Task<MotivationVM> LoadListById(string campaignCode)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(MotivationApiUrls.MotivationApiUrl.Load.Replace("{campaignCode}", campaignCode));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                MotivationVM MotivationVM = JsonConvert.DeserializeObject<MotivationVM>(Convert.ToString(response.ResponseObject));
                return MotivationVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<MotivationVM> Load(int id)
        {
            throw new NotSupportedException();
        }

        public async Task<ApiResponse> Create(MotivationVM MotivationVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(MotivationApiUrls.MotivationApiUrl.Create);
                var response = await apiClient.PostAsync<MotivationVM>(requestUrl, MotivationVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> Update(MotivationVM MotivationVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(MotivationApiUrls.MotivationApiUrl.Update);
                var response = await apiClient.PutSync<MotivationVM>(requestUrl, MotivationVM);
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
        public async Task<ApiResponse> DeleteById(string campaignCode)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(MotivationApiUrls.MotivationApiUrl.Delete.Replace("{campaignCode}", campaignCode));
                var response = await apiClient.DeleteAsync<MotivationVM>(requestUrl);
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
