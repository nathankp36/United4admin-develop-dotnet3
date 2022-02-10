using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class SalutationFactory : ISalutationFactory
    {
        protected ApiClient apiClient;

        public SalutationFactory()
        {
            this.apiClient = new ApiClient();
        }

        public SalutationFactory(ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }

        public async Task<List<SalutationVM>> LoadList()
        {
            try
            {
                List<SalutationVM> SalutationVMList = new List<SalutationVM>();
                var requestUrl = apiClient.CreateRequestUri(SalutationApiUrls.SalutationApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                SalutationVMList = JsonConvert.DeserializeObject<List<SalutationVM>>(Convert.ToString(response.ResponseObject));
                return SalutationVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Load list by string value
        public async Task<SalutationVM> LoadListById(string ddlSalutation)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(SalutationApiUrls.SalutationApiUrl.Load.Replace("{ddlSalutation}", Convert.ToString(ddlSalutation)));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                SalutationVM salutationVM = JsonConvert.DeserializeObject<SalutationVM>(Convert.ToString(response.ResponseObject));
                return salutationVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<SalutationVM> Load(int id)
        {
            throw new NotSupportedException();
        }

        public async Task<ApiResponse> Create(SalutationVM SalutationVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(SalutationApiUrls.SalutationApiUrl.Create);
                var response = await apiClient.PostAsync<SalutationVM>(requestUrl, SalutationVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> Update(SalutationVM SalutationVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(SalutationApiUrls.SalutationApiUrl.Update);
                var response = await apiClient.PutSync<SalutationVM>(requestUrl, SalutationVM);
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
        public async Task<ApiResponse> DeleteById(string ddlSalutation)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(SalutationApiUrls.SalutationApiUrl.Delete.Replace("{ddlSalutation}", Convert.ToString(ddlSalutation)));
                var response = await apiClient.DeleteAsync<SalutationVM>(requestUrl);
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
