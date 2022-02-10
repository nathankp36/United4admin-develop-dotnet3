using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class PaymentMethodFactory : IPaymentMethodFactory
    {
        protected ApiClient apiClient;

        public PaymentMethodFactory()
        {
            this.apiClient = new ApiClient();
        }

        public PaymentMethodFactory(ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }

        public async Task<List<PaymentMethodVM>> LoadList()
        {
            try
            {
                List<PaymentMethodVM> PaymentMethodVMList = new List<PaymentMethodVM>();
                var requestUrl = apiClient.CreateRequestUri(PaymentMethodApiUrls.PaymentMethodApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                PaymentMethodVMList = JsonConvert.DeserializeObject<List<PaymentMethodVM>>(Convert.ToString(response.ResponseObject));
                return PaymentMethodVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PaymentMethodVM> Load(int id)
        {
            try
            {
                PaymentMethodVM result = new PaymentMethodVM();
                var requestUrl = apiClient.CreateRequestUri(PaymentMethodApiUrls.PaymentMethodApiUrl.Load.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                PaymentMethodVM PaymentMethodVM = JsonConvert.DeserializeObject<PaymentMethodVM>(Convert.ToString(response.ResponseObject));
                return PaymentMethodVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> Create(PaymentMethodVM PaymentMethodVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(PaymentMethodApiUrls.PaymentMethodApiUrl.Create);
                var response = await apiClient.PostAsync<PaymentMethodVM>(requestUrl, PaymentMethodVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> Update(PaymentMethodVM PaymentMethodVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(PaymentMethodApiUrls.PaymentMethodApiUrl.Update);
                var response = await apiClient.PutSync<PaymentMethodVM>(requestUrl, PaymentMethodVM);
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
                var requestUrl = apiClient.CreateRequestUri(PaymentMethodApiUrls.PaymentMethodApiUrl.Delete.Replace("{id}", Convert.ToString(id)));
                var response = await apiClient.DeleteAsync<PaymentMethodVM>(requestUrl);
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
