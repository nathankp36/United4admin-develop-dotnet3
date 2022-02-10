using United4Admin.WebApplication.ApiClientFactory;
using United4Admin.WebApplication.ViewModels;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IMotivationFactory : IApiClientFactory<MotivationVM>
    {
        Task<MotivationVM> LoadListById(string CampaignCode);

        Task<ApiResponse> DeleteById(string CampaignCode);
    }
}
