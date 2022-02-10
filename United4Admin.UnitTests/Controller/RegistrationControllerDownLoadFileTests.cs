using United4Admin.WebApplication.BLL;
using United4Admin.WebApplication.Controllers;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Azure.Storage.Blob;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using System.Threading.Tasks;
using United4Admin.WebApplication.ViewModels;
using System.Linq;
using NUnit.Framework.Constraints;
using Microsoft.Extensions.Logging;

namespace United4Admin.UnitTests.Controllers
{
    [TestFixture]
    class RegistrationControllerDownLoadFileTests
    {
        private RegistrationsController _controller;
        private Mock<IZipFileHelper> _zipFileHelper;
        private Mock<IImageStoreFactory> _imageActions;
        private Mock<IRegistrationFactory> _registrationFactory;
        private Mock<IPermissionFactory> _permissionFactory;
        private List<ImageInfoVM> _imageInfoVMList;      
        private string[] blobFileNames;
        private Mock<ILogger<RegistrationsController>> _mockLogger;
        [SetUp]
        public void SetUp()
        {
           
            _registrationFactory = new Mock<IRegistrationFactory>();
            _permissionFactory = new Mock<IPermissionFactory>();
            _imageActions = new Mock<IImageStoreFactory>();
            _zipFileHelper = new Mock<IZipFileHelper>();

            //logger
            _mockLogger = new Mock<ILogger<RegistrationsController>>();

            _imageInfoVMList = new List<ImageInfoVM>()
            {
                new ImageInfoVM{ BlobGUID="test1.jpg",ChosenSignUpId =1},
                new ImageInfoVM{ BlobGUID="test2.jpg",ChosenSignUpId =2},
                new ImageInfoVM{ BlobGUID="test3.jpg",ChosenSignUpId =3},
            };
            //string[] _filenames = new string[3] { "test1.jpg", "test2.jpg", "test3.jpg" };
             blobFileNames = _imageInfoVMList.Select(x => x.BlobGUID).ToArray();

            Uri test = new Uri("https://app.blob.core.windows.net/container/Accounts/Images/acc.jpg");
            Mock<CloudBlockBlob> mockBlob = new Mock<CloudBlockBlob>(test);
            _imageActions.Setup(ia => ia.GetBlobImageFile(It.IsAny<string>())).Returns(Task.Run(() => mockBlob.Object));

            _registrationFactory.Setup(sur => sur.GetImageNames(It.IsAny<SignUpExtractParameterVM>())).Returns(Task.Run(() => _imageInfoVMList));

            _controller = new RegistrationsController(_registrationFactory.Object, _imageActions.Object, _zipFileHelper.Object, _permissionFactory.Object,_mockLogger.Object);
          
         //   FakeHttpContext.SetFakeContext(_controller); //set up the fake context for memory stream           

        }

        [Test]
        public void DownloadPhotos_WhenCalled_GeneratesZipFile()
        {
            SignUpExtractParameterVM signupVMList = new SignUpExtractParameterVM();

            //act
            var result = _controller.DownloadPhotos(signupVMList);
            //verify below that the zipfilehelper method is called.
            _zipFileHelper.Verify(zfh => zfh.ZipUpFiles(
                 blobFileNames,
                _imageActions.Object));
        }      
    }
}
