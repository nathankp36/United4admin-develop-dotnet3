using United4Admin.WebApplication.ViewModels;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface ISignupEventFactory : IApiClientFactory<SignUpEventVM>
    {
        Task<bool> CheckRegistrationExists(int id);
    }
}
