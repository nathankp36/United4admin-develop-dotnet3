using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class RevealEventFactory : IRevealEventFactory
    {

        protected ApiClient apiClient;

        public RevealEventFactory()
        {
            this.apiClient = new ApiClient();
        }

        public RevealEventFactory(ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }

        public async Task<List<RevealEventVM>> LoadList()
        {
            try
            {
                List<RevealEventVM> RevealEventVMList = new List<RevealEventVM>();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.RevealEventK8ApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                RevealEventVMList = JsonConvert.DeserializeObject<List<RevealEventVM>>(Convert.ToString(response.ResponseObject));
                return RevealEventVMList;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<RevealEventVM> Load(int id)
        {
            try
            {
                RevealEventVM result = new RevealEventVM();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.RevealEventK8ApiUrl.Load.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                RevealEventVM revealEventVM = JsonConvert.DeserializeObject<RevealEventVM>(Convert.ToString(response.ResponseObject));
                revealEventVM.TypeOfRevealList = result.TypeOfRevealList;
                return revealEventVM;
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
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.RevealEventK8ApiUrl.SignUpExists.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                return Convert.ToBoolean(response.ResponseObject);
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<ApiResponse> Create(RevealEventVM RevealEventVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.RevealEventK8ApiUrl.Create);
                var response = await apiClient.PostAsync<RevealEventVM>(requestUrl, RevealEventVM);
                return response;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<ApiResponse> Update(RevealEventVM RevealEventVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.RevealEventK8ApiUrl.Update);
                var response = await apiClient.PutSync<RevealEventVM>(requestUrl, RevealEventVM);
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
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.RevealEventK8ApiUrl.Delete.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.DeleteAsync<RevealEventVM>(requestUrl);
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