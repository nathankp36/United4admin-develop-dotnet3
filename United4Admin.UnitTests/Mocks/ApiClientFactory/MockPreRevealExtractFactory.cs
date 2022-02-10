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
   public class MockPreRevealExtractFactory : Mock<IRegistrationFactory>
    {
        public async Task<MockPreRevealExtractFactory> MockLoad(List<SignUpVM> result, CRMExtractParameterModelVM cRMExtractParameterModel)
        {
            Setup(x => x.GetPreRevealExtractData(cRMExtractParameterModel)).Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }
    }
}
