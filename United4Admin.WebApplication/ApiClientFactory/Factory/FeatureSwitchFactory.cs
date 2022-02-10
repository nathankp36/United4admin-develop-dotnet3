using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.Models;
using United4Admin.WebApplication.ViewModels;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class FeatureSwitchFactory : IFeatureSwitchFactory
    {
        protected ApiClient apiclient;
        public FeatureSwitchFactory()
        {
            this.apiclient = new ApiClient();
        }
        public FeatureSwitchFactory(ApiClient apiClient)
        {
            this.apiclient = apiClient;
        }

        public async Task<bool> CheckFeatureSwitch(string featureName)
        {
            try
            {                
                var requestUrl = apiclient.CreateRequestUploadUri(FeatureSwitchApiUrls.FeatureSwitchAPIUrl + featureName);
                var response = await apiclient.GetAsync<ApiResponse>(requestUrl);
                var result = JsonConvert.DeserializeObject<FeatureModelAPIModel>(Convert.ToString(response.ResponseObject));
                if (result != null)
                    return result.Flag;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
