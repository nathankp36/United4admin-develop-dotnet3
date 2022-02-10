using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Moq;
using Microsoft.AspNetCore.Http;

namespace United4Admin.UnitTests.Mocks.ApiClientFactory
{
    class MockCRMQuarantineFactory : Mock<ICRMQuarantineFactory>
    {
        public async Task<MockCRMQuarantineFactory> MockLoad(ApiResponse apiResponse, string result, CRMExtractParameterModelVM cRMQuarantineParameterModel)
        {
            Setup(x => x.CRMQuarantineDataExtract(cRMQuarantineParameterModel)).Returns(Task.Run(() => result));
            Setup(x => x.UpdateCRMTransaction()).Returns(Task.Run(() => apiResponse));
            return await Task.FromResult(this);
        }
    }
}
