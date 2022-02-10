using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Moq;

namespace United4Admin.UnitTests.Mocks.ApiClientFactory
{
    

    public class MockImageFactory : Mock<IImageStoreFactory>
    {
     
        public async Task<MockImageFactory> MockGetSecureUrlImage(bool result, object imageInfoVM)
        {

            if (result)
            {
                Setup(x => x.GetSecureUrlImage(It.IsAny<string>()))
                    .Returns(Task.Run(() => imageInfoVM));
            }
            else
            {
                Setup(x => x.GetSecureUrlImage(It.IsAny<string>()))
                    .Returns(Task.Run(() => new object()));
            }
            return await Task.FromResult(this);
        }

        public async Task<MockImageFactory> MockDeleteImage(ApiResponse result, string imageName)
        {
            Setup(x => x.DeleteImage(It.IsAny<string>())).Returns(Task.Run(() => result));
            return await Task.FromResult(this); ;
        }
    }
}
