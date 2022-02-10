using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class ChoosingEventFactory : IChoosingEventFactory
    {

        protected ApiClient apiClient;

        public ChoosingEventFactory()
        {
            this.apiClient = new ApiClient();
        }

        public ChoosingEventFactory(ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }

        public async Task<List<ChoosingPartyVM>> LoadList()
        {   
            try
            {
                List<ChoosingPartyVM> choosingPartyVMList = new List<ChoosingPartyVM>();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.ChoosingEventK8ApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);                
                choosingPartyVMList = JsonConvert.DeserializeObject<List<ChoosingPartyVM>>(Convert.ToString(response.ResponseObject));
                return choosingPartyVMList;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<ChoosingPartyVM> Load(int id)
        {
            try
            {
                ChoosingPartyVM choosingPartyVM = new ChoosingPartyVM();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.ChoosingEventK8ApiUrl.Load.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                choosingPartyVM = JsonConvert.DeserializeObject<ChoosingPartyVM>(Convert.ToString(response.ResponseObject));
                return choosingPartyVM;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<bool> CheckRegistrationExists(int id)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.ChoosingEventK8ApiUrl.SignUpExists.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                return Convert.ToBoolean(response.ResponseObject);                
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<ApiResponse> Create(ChoosingPartyVM choosingPartyVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.ChoosingEventK8ApiUrl.Create);
                var response=  await apiClient.PostAsync<ChoosingPartyVM>(requestUrl, choosingPartyVM);
                return response;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<ApiResponse> Update(ChoosingPartyVM choosingPartyVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.ChoosingEventK8ApiUrl.Update);
                var response = await apiClient.PutSync<ChoosingPartyVM>(requestUrl, choosingPartyVM);
                return response;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.ChoosingEventK8ApiUrl.Delete.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.DeleteAsync<ChoosingPartyVM>(requestUrl);
                return response;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<IList<WorkflowStatusVM>> GetWorkflowStatuses()
        {
            try
            {
                List<WorkflowStatusVM> workFlowVMList = new List<WorkflowStatusVM>();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.GetWorkflowStatusListK8);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                workFlowVMList = JsonConvert.DeserializeObject<List<WorkflowStatusVM>>(Convert.ToString(response.ResponseObject));
                return workFlowVMList;
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