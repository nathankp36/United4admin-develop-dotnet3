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
   public class MockCRMExtractFactory : Mock<ICRMExtractFactory>
    {
        public async Task<MockCRMExtractFactory> MockLoad(string[] result, CRMExtractParameterModelVM cRMExtractParameterModel)
        {
            Setup(x => x.CRMDataExtract(cRMExtractParameterModel)).Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }
    }
}
