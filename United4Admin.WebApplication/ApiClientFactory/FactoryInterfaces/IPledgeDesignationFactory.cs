using United4Admin.WebApplication.ApiClientFactory;
using United4Admin.WebApplication.ViewModels;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IPledgeDesignationFactory : IApiClientFactory<PledgeDesignationVM>
    {
        Task<PledgeDesignationVM> LoadListById(string wVformId);

        Task<ApiResponse> DeleteById(string wVformId);
    }
}
