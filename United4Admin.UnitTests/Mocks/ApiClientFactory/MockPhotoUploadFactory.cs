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
   public class MockPhotoUploadFactory : Mock<IRegistrationFactory>
    {
        public async Task<MockPhotoUploadFactory> MockLoad(List<SignUpVM> result)
        {

            Setup(x => x.GetImageNotUploadData()).Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockPhotoUploadFactory> MockCreate(ApiResponse result, List<ImageInfoVM> resultInfo, string Name)
        {
            ImageInfoVM imageInfo = new ImageInfoVM()
            {
                BlobGUID = Convert.ToString("1.jpg"),
                ChosenSignUpId = 1,
                UploadDateTime = DateTime.Now
            };

            Setup(x => x.UploadImage(It.IsAny<IFormFile>(), It.IsAny<string>())).Returns(Task.Run(() => result));
            Setup(x => x.GetImageNames(It.IsAny<SignUpExtractParameterVM>())).Returns(Task.Run(() => resultInfo));
            if (resultInfo == null)
            {
                Setup(x => x.CreateImage(It.IsAny<ImageInfoVM>())).Returns(Task.Run(() => result));
            }
            else
            {
                Setup(x => x.UpdateImage(It.IsAny<ImageInfoVM>())).Returns(Task.Run(() => result));
            }
            return await Task.FromResult(this);
        }

        public async Task<MockPhotoUploadFactory> MockCreateImageInfo(ApiResponse result, ImageInfoVM Name)
        {
            Setup(x => x.CreateImage(It.IsAny<ImageInfoVM>())).Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public SignUpVM MockFindById(IList<SignUpVM> SignUpVMModels, int id)
        {
            var result = SignUpVMModels.Where(x => x.chosenSignUpId == id).SingleOrDefault();
            return result;
        }
    }
}
