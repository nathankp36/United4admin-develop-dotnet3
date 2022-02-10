using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IChildDataCacheFactory
    {
        Task UploadChildIds(List<string> ChildIdList);
        Task<ApiResponse> GetUpdateProgress();

        Task<ApiResponse> GetUpdateProgressById(int Id);
    }
}