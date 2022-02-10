using United4Admin.WebApplication.ApiClientFactory;
using United4Admin.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;
using Microsoft.AspNetCore.Http;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IAddOnDonationFactory : IDisposable
    {
        Task<List<AddOnDonationVM>> GetAddOnDonationData();
        Task<AddOnDonationVM> GetAddOnDonationBySalesOrderId(string salesOrderId);
        Task<List<AddOnDonationVM>> GetFieldDataExport(SignUpExtractParameterVM signUpVM);
        Task<List<ImageInfoVM>> GetImageNames(SignUpExtractParameterVM signUpVM);
    }
}
