using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory;
using United4Admin.WebApplication.ViewModels;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface ICRMQuarantineFactory
    {
        Task<string> CRMQuarantineDataExtract(CRMExtractParameterModelVM CRMQuarantineParameter);
        Task<ApiResponse> UpdateCRMTransaction();
    }
}
