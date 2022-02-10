using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class PledgeDesignationFactory : IPledgeDesignationFactory
    {
        protected ApiClient apiClient;

        public PledgeDesignationFactory()
        {
            this.apiClient = new ApiClient();
        }

        public PledgeDesignationFactory(ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }

        public async Task<List<PledgeDesignationVM>> LoadList()
        {
            try
            {
                List<PledgeDesignationVM> fallbackValuesVMList = new List<PledgeDesignationVM>();
                var requestUrl = apiClient.CreateRequestUri(PledgeDesignationApiUrls.PledgeDesignationApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                fallbackValuesVMList = JsonConvert.DeserializeObject<List<PledgeDesignationVM>>(Convert.ToString(response.ResponseObject));
                return fallbackValuesVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Load list by string value
        public async Task<PledgeDesignationVM> LoadListById(string wVformId)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(PledgeDesignationApiUrls.PledgeDesignationApiUrl.Load.Replace("{wVformId}", wVformId));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                PledgeDesignationVM PledgeDesignationVM = JsonConvert.DeserializeObject<PledgeDesignationVM>(Convert.ToString(response.ResponseObject));
                return PledgeDesignationVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<PledgeDesignationVM> Load(int id)
        {
            throw new NotSupportedException();
        }

        public async Task<ApiResponse> Create(PledgeDesignationVM PledgeDesignationVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(PledgeDesignationApiUrls.PledgeDesignationApiUrl.Create);
                var response = await apiClient.PostAsync<PledgeDesignationVM>(requestUrl, PledgeDesignationVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> Update(PledgeDesignationVM PledgeDesignationVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(PledgeDesignationApiUrls.PledgeDesignationApiUrl.Update);
                var response = await apiClient.PutSync<PledgeDesignationVM>(requestUrl, PledgeDesignationVM);
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
        public async Task<ApiResponse> DeleteById(string wVformId)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(PledgeDesignationApiUrls.PledgeDesignationApiUrl.Delete.Replace("{wVformId}", wVformId));
                var response = await apiClient.DeleteAsync<PledgeDesignationVM>(requestUrl);
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
