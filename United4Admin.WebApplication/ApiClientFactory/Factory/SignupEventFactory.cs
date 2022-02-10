using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class SignupEventFactory : ISignupEventFactory
    {

        protected ApiClient apiClient;

        public SignupEventFactory()
        {
            this.apiClient = new ApiClient();
        }

        public SignupEventFactory(ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }

        public async Task<List<SignUpEventVM>> LoadList()
        {
            try
            {
                List<SignUpEventVM> SignUpEventVMList = new List<SignUpEventVM>();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.SignUpEventK8ApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                SignUpEventVMList = JsonConvert.DeserializeObject<List<SignUpEventVM>>(Convert.ToString(response.ResponseObject));
                return SignUpEventVMList;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<SignUpEventVM> Load(int id)
        {
            try
            {
                SignUpEventVM result = new SignUpEventVM();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.SignUpEventK8ApiUrl.Load.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                SignUpEventVM SignUpEventVM = JsonConvert.DeserializeObject<SignUpEventVM>(Convert.ToString(response.ResponseObject));
                SignUpEventVM.TypeOfRegistrationList = result.TypeOfRegistrationList;
                return SignUpEventVM;
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
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.SignUpEventK8ApiUrl.SignUpExists.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                return Convert.ToBoolean(response.ResponseObject);
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<ApiResponse> Create(SignUpEventVM SignUpEventVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.SignUpEventK8ApiUrl.Create);
                var response = await apiClient.PostAsync<SignUpEventVM>(requestUrl, SignUpEventVM);
                return response;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<ApiResponse> Update(SignUpEventVM SignUpEventVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.SignUpEventK8ApiUrl.Update);
                var response = await apiClient.PutSync<SignUpEventVM>(requestUrl, SignUpEventVM);
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
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.SignUpEventK8ApiUrl.Delete.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.DeleteAsync<SignUpEventVM>(requestUrl);
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