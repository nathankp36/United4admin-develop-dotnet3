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
    public class MockPhotoApprovalFactory : Mock<IRegistrationFactory>
    {
        public async Task<MockPhotoApprovalFactory> MockLoad(List<SignUpVM> result)
        {

            Setup(x => x.GetPhotoApproval()).Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockPhotoApprovalFactory> MockCreateReplace(ApiResponse result, List<ImageInfoVM> resultInfo, ImageInfoVM imageInfo, string Name)
        {
            SignUpExtractParameterVM signUp = new SignUpExtractParameterVM();
            Setup(x => x.GetImage(It.IsAny<int>())).Returns(Task.Run(() => imageInfo));
            if (result != null)
            {
                Setup(x => x.UploadImage(It.IsAny<IFormFile>(), It.IsAny<string>())).Returns(Task.Run(() => result));
                Setup(x => x.GetImageNames(signUp)).Returns(Task.Run(() => resultInfo));
                if (resultInfo != null)
                {
                    Setup(x => x.UpdateImage(It.IsAny<ImageInfoVM>())).Returns(Task.Run(() => result));
                }
            }
            return await Task.FromResult(this);
        }

        public async Task<MockPhotoApprovalFactory> MockApprove(ApiResponse result, ImageInfoVM imageInfo , string Name)
        {
            Setup(x => x.GetImage(It.IsAny<int>())).Returns(Task.Run(() => imageInfo));
            if (result != null)
            {
                Setup(x => x.UpdateImage(It.IsAny<ImageInfoVM>())).Returns(Task.Run(() => result));
            }
            return await Task.FromResult(this);
        }

        public async Task<MockPhotoApprovalFactory> MockCreateImageInfo(ApiResponse result, ImageInfoVM Name)
        {
            Setup(x => x.CreateImage(It.IsAny<ImageInfoVM>())).Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockPhotoApprovalFactory> MockUpdateImageInfo(ApiResponse result, ImageInfoVM Name)
        {
            Setup(x => x.UpdateImage(It.IsAny<ImageInfoVM>())).Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockPhotoApprovalFactory> MockDeleteImageInfo(ImageInfoVM result, ApiResponse delResult, ApiResponse updateResult, int id)
        {
            Setup(x => x.GetImage(It.IsAny<int>())).Returns(Task.Run(() => result));
            if (result != null)
            {
                Setup(x => x.DeleteImage(It.IsAny<int>())).Returns(Task.Run(() => delResult));
                if (delResult.Success)
                {
                    Setup(x => x.UpdateImage(It.IsAny<ImageInfoVM>())).Returns(Task.Run(() => updateResult));
                }
            }
            return await Task.FromResult(this); 
        }

        public async Task<MockPhotoApprovalFactory> MockViewImageInfo(ImageInfoVM result, int id)
        {
            Setup(x => x.GetImage(It.IsAny<int>())).Returns(Task.Run(() => result));

            return await Task.FromResult(this);
        }

        public SignUpVM MockFindById(IList<SignUpVM> SignUpVMModels, int id)
        {
            var result = SignUpVMModels.Where(x => x.chosenSignUpId == id).SingleOrDefault();
            return result;
        }
    }
}
