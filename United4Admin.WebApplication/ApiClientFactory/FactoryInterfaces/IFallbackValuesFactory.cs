using United4Admin.WebApplication.ApiClientFactory;
using United4Admin.WebApplication.ViewModels;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IFallbackValuesFactory : IApiClientFactory<FallbackValuesVM>
    {
        Task<FallbackValuesVM> LoadListById(string simmaField);

        Task<ApiResponse> DeleteById(string simmaField);
    }
}
