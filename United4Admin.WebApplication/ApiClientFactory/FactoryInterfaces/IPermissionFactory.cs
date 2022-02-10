using United4Admin.WebApplication.ApiClientFactory;
using United4Admin.WebApplication.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IPermissionFactory : IApiClientFactory<PermissionsVM>
    {
        Task<List<PermissionsVM>> GetAdministrators();
      
    }
}
