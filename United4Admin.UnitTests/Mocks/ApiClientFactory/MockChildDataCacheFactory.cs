using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Moq;
using Newtonsoft.Json;

namespace United4Admin.UnitTests.Mocks.ApiClientFactory
{


    public class MockChildDataCacheFactory : Mock<IChildDataCacheFactory>
    {

        public async Task<MockChildDataCacheFactory> MockUploadChildIds(List<string> objList)
        {
            Setup(x => x.UploadChildIds(objList)).Verifiable();
            return await Task.FromResult(this);
        }

        public async Task<MockChildDataCacheFactory> MockUploadChildIdsThrowsException(List<string> objList)
        {
            Setup(x => x.UploadChildIds(objList)).Throws<Exception>();
            return await Task.FromResult(this);
        }

        public async Task<MockChildDataCacheFactory> MockGetProgressUpdateById(ApiResponse result, int id)
        {
            result.ResponseObject = JsonConvert.SerializeObject(result.ResponseObject);
            Setup(x => x.GetUpdateProgressById(id)).Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockChildDataCacheFactory> MockGetProgressUpdateByIdThrowsException(int id)
        {
            Setup(x => x.GetUpdateProgressById(id)).Throws<Exception>();
            return await Task.FromResult(this);
        }

        public async Task<MockChildDataCacheFactory> MockGetProgressUpdate(ApiResponse result)
        {
            result.ResponseObject = JsonConvert.SerializeObject(result.ResponseObject);
            Setup(x => x.GetUpdateProgress()).Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockChildDataCacheFactory> MockGetProgressUpdateThrowsException()
        {
            Setup(x => x.GetUpdateProgress()).Throws<Exception>();
            return await Task.FromResult(this);
        }

    }
}
