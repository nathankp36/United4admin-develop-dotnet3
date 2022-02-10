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
    public class PermissionFactory : IPermissionFactory
    {
        protected ApiClient apiClient;

        public PermissionFactory()
        {
            this.apiClient = new ApiClient();
        }

        public PermissionFactory(ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }

        public async Task<List<PermissionsVM>> LoadList()
        {
            try
            {
                List<PermissionsVM> PermissionsVMList = new List<PermissionsVM>();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.PermissionK8ApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                PermissionsVMList = JsonConvert.DeserializeObject<List<PermissionsVM>>(Convert.ToString(response.ResponseObject));
                return PermissionsVMList;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<PermissionsVM> Load(int id)
        {
            try
            {
                PermissionsVM PermissionsVM = new PermissionsVM();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.PermissionK8ApiUrl.Load.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                PermissionsVM = JsonConvert.DeserializeObject<PermissionsVM>(Convert.ToString(response.ResponseObject));
                return PermissionsVM;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<List<PermissionsVM>> GetAdministrators()
        {
            try
            {
                List<PermissionsVM> PermissionsVMList = new List<PermissionsVM>();
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.PermissionK8ApiUrl.Adminstrators);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                PermissionsVMList = JsonConvert.DeserializeObject<List<PermissionsVM>>(Convert.ToString(response.ResponseObject));
                return PermissionsVMList;
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
        public async Task<ApiResponse> Create(PermissionsVM PermissionsVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.PermissionK8ApiUrl.Create);
                var response = await apiClient.PostAsync<PermissionsVM>(requestUrl, PermissionsVM);
                return response;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public async Task<ApiResponse> Update(PermissionsVM PermissionsVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.PermissionK8ApiUrl.Update);
                var response = await apiClient.PutSync<PermissionsVM>(requestUrl, PermissionsVM);
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
                var requestUrl = apiClient.CreateRequestUri(EventServiceApiUrls.PermissionK8ApiUrl.Delete.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.DeleteAsync<PermissionsVM>(requestUrl);
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