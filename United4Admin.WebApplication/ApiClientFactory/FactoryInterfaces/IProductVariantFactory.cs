using United4Admin.WebApplication.ApiClientFactory;
using United4Admin.WebApplication.ViewModels;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;
using System.Collections.Generic;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IProductVariantFactory:IApiClientFactory<ProductVariantVM>
    {
        Task<ProductVariantVM> LoadListById(string ddlSalutation);

        Task<ApiResponse> DeleteById(string ddlSalutation);

        Task<List<IncidentTypeVM>> GetIncidentTypes();

        Task<List<PledgeTypeVM>> GetPledgeTypes();
    }
}
