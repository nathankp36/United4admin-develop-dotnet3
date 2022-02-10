using United4Admin.WebApplication.ApiClientFactory;
using United4Admin.WebApplication.ViewModels;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface ISalutationFactory : IApiClientFactory<SalutationVM>
    {
        Task<SalutationVM> LoadListById(string ddlSalutation);

        Task<ApiResponse> DeleteById(string ddlSalutation);
    }
}
