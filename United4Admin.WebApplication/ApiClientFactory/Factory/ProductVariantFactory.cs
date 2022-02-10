using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class ProductVariantFactory : IProductVariantFactory
    {
        protected ApiClient apiClient;

        public ProductVariantFactory()
        {
            this.apiClient = new ApiClient();
        }

        public ProductVariantFactory(ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }

        public async Task<List<ProductVariantVM>> LoadList()
        {
            try
            {
                List<ProductVariantVM> ProductVariantVMList = new List<ProductVariantVM>();
                var requestUrl = apiClient.CreateRequestUri(ProductVariantApiUrls.ProductVariantApiUrl.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                ProductVariantVMList = JsonConvert.DeserializeObject<List<ProductVariantVM>>(Convert.ToString(response.ResponseObject));
                return ProductVariantVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Load list by string value
        public async Task<ProductVariantVM> LoadListById(string ddlProductTypeCode)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(ProductVariantApiUrls.ProductVariantApiUrl.Load.Replace("{ddlProductTypeCode}", Convert.ToString(ddlProductTypeCode)));
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                ProductVariantVM productVariantVM = JsonConvert.DeserializeObject<ProductVariantVM>(Convert.ToString(response.ResponseObject));
                return productVariantVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<ProductVariantVM> Load(int id)
        {
            throw new NotSupportedException();
        }

        public async Task<ApiResponse> Create(ProductVariantVM ProductVariantVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(ProductVariantApiUrls.ProductVariantApiUrl.Create);
                var response = await apiClient.PostAsync<ProductVariantVM>(requestUrl, ProductVariantVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> Update(ProductVariantVM ProductVariantVM)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(ProductVariantApiUrls.ProductVariantApiUrl.Update);
                var response = await apiClient.PutSync<ProductVariantVM>(requestUrl, ProductVariantVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delete list by string value
        public async Task<ApiResponse> DeleteById(string ddlProductTypeCode)
        {
            try
            {
                var requestUrl = apiClient.CreateRequestUri(ProductVariantApiUrls.ProductVariantApiUrl.Delete.Replace("{ddlProductTypeCode}", Convert.ToString(ddlProductTypeCode)));
                var response = await apiClient.DeleteAsync<ProductVariantVM>(requestUrl);
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

        public async Task<List<IncidentTypeVM>> GetIncidentTypes()
        {
            try
            {
                List<IncidentTypeVM> IncidentTypeVMList = new List<IncidentTypeVM>();
                var requestUrl = apiClient.CreateRequestUri(ProductVariantApiUrls.ProductVariantApiUrl.GetIncidentTypes);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                IncidentTypeVMList = JsonConvert.DeserializeObject<List<IncidentTypeVM>>(Convert.ToString(response.ResponseObject));
                return IncidentTypeVMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PledgeTypeVM>> GetPledgeTypes()
        {
            try
            {
                List<PledgeTypeVM> PledgeTypeVMList = new List<PledgeTypeVM>();
                var requestUrl = apiClient.CreateRequestUri(ProductVariantApiUrls.ProductVariantApiUrl.GetPledgeTypes);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                PledgeTypeVMList = JsonConvert.DeserializeObject<List<PledgeTypeVM>>(Convert.ToString(response.ResponseObject));
                return PledgeTypeVMList;
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
