using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United4Admin.WebApplication.Models;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IChildDataCacheForWebFactory
    {
        Task<ChildInternalDataPagedVM> GetAllChildren(int PageNumber, int PageSize);

        Task<ApiResponse> DeleteChildRecord(int Id);

        Task<ApiResponse> ChangeStatusSponsored(UpdateChildStatusViewModel updateChildStatusViewModel);

        Task<ChildInternalDataVM> Load(int Id);
    }
}