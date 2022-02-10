using United4Admin.WebApplication.ApiClientFactory;
using United4Admin.WebApplication.ViewModels;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IChoosingEventFactory : IApiClientFactory<ChoosingPartyVM>
    {
        Task<bool> CheckRegistrationExists(int id);
    }
}
