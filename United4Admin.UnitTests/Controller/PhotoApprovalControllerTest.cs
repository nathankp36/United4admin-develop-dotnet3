using United4Admin.UnitTests.Mocks.ApiClientFactory;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.BLL;
using United4Admin.WebApplication.Controllers;
using United4Admin.WebApplication.ViewModels;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text;

namespace United4Admin.UnitTests.Controllers
{
    [TestFixture]
    public class PhotoApprovalControllerTest
    {
        private PhotoApprovalController _photoApprovalController;
        private MockRegistrationFactory mockRegistrationFactory;
        private ActionContext _actionContext;
        private MockPhotoApprovalFactory mockPhotoApprovalFactory;

        private ApiResponse _mockResponse;
        private int _SignUpVMListCount;
        private int _SignUpVMListMaxId;

        private List<SignUpVM> _SignUpVMList;
        private List<PermissionsVM> _PermissionsVMList;
        private List<ImageInfoVM> _imageInfoVMList;
        private ApiResponse _apiReponse;
        private Mock<IZipFileHelper> _zipFileHelper;
        private Mock<IImageStoreFactory> _imageActions;
        private Mock<ILogger<PhotoApprovalController>> _mockLogger;
        public IFormFile ConvertedIFormFilePhoto { get; set; }
        [SetUp]
        public void SetUp()

        {
            //Arrange
            #region "Arrange"

            _imageInfoVMList = new List<ImageInfoVM>()
                {
                    new ImageInfoVM{ImageInfoId=1, BlobGUID="test1.jpg",ChosenSignUpId =1,ImageStatusId=3},
                    new ImageInfoVM{ImageInfoId=2, BlobGUID="test2.jpg",ChosenSignUpId =2,ImageStatusId=3},
                    new ImageInfoVM{ImageInfoId=3, BlobGUID="test3.jpg",ChosenSignUpId =3,ImageStatusId=3},
                     new ImageInfoVM{ImageInfoId=4, BlobGUID="test3.jpg",ChosenSignUpId =4,ImageStatusId=3}
                };

            //Creating Dummy SignUpVM List
            _SignUpVMList = new List<SignUpVM>
                    {
                        new SignUpVM {
                            chosenSignUpId = 1,
                            Title = "",
                            FirstName = "Harry",
                            LastName = "Potter",
                            BuildingNumberName = "4",
                            StreetName = "Privet Drive",
                            AddressLine2 = "-",
                            TownCity = "Little Whinging",
                            Postcode = "WH23 4TH",
                            PhoneNumber = "02077582900",
                            EmailAddress = "theboywholived@nottinghamcity.gov.uk",
                            CorrectedBankAccountNumber = "12345678",
                            CorrectedBankSortCode = "123456",
                            DataConsent = true,
                            TaxConsent = false,
                            Post = true,
                            Email = false,
                            Phone = true,
                            SMS = false,
                            ChosenStatusId = 1,
                            SignUpEventId = 1,
                            ChoosingPartyId = 1,
                            RevealEventId = 1 },
                        new SignUpVM {
                            chosenSignUpId = 2,
                            Title = "",
                            FirstName = "Lyra",
                            LastName = "Belaqua",
                            BuildingNumberName = "1",
                            StreetName = "The Rooftops",
                            AddressLine2 = "Jordan College",
                            TownCity = "Oxford",
                            Postcode = "OX1 12JC",
                            PhoneNumber = "01865 123456",
                            EmailAddress = "lyra.silvertongue@gmail.com",
                            CorrectedBankAccountNumber = "98765432",
                            CorrectedBankSortCode = "112233",
                            DataConsent = true,
                            TaxConsent = true,
                            Post = false,
                            Email = false,
                            Phone = false,
                            SMS = false,
                            ChosenStatusId = 2,
                            SignUpEventId = 2,
                            ChoosingPartyId = 2,
                            RevealEventId = 2
                        },
                        new SignUpVM {
                            chosenSignUpId = 3,
                            Title = "",
                            FirstName = "Lucy",
                            LastName = "Pevensie",
                            BuildingNumberName = "Mr Tumnus' House",
                            StreetName = "Lampost Lane",
                            AddressLine2 = "Lantern Wood",
                            TownCity = "Narnia",
                            Postcode = "N43 6BS",
                            PhoneNumber = "0115 8471257",
                            EmailAddress = "lucy.pevensie@rolls-royce.com",
                            CorrectedBankAccountNumber = "55667788",
                            CorrectedBankSortCode = "000099",
                            DataConsent = true,
                            TaxConsent = true,
                            Post = true,
                            Email = true,
                            Phone = true,
                            SMS = true,
                            ChosenStatusId = 3,
                            SignUpEventId = 3,
                            ChoosingPartyId = 3,
                            RevealEventId = 3
                        },
                        new SignUpVM {
                            chosenSignUpId = 4,
                            Title = "",
                            FirstName = "Lucy test",
                            LastName = "Pevensietest",
                            BuildingNumberName = "Mr Tumnus' House",
                            StreetName = "Lampost Lane",
                            AddressLine2 = "Lantern Wood",
                            TownCity = "Narnia",
                            Postcode = "N43 6BS",
                            PhoneNumber = "0115 8471257",
                            EmailAddress = "lucy.pevensie@rolls-royce.com",
                            CorrectedBankAccountNumber = "55667788",
                            CorrectedBankSortCode = "000099",
                            DataConsent = true,
                            TaxConsent = true,
                            Post = true,
                            Email = true,
                            Phone = true,
                            SMS = true,
                            ChosenStatusId = 3,
                            SignUpEventId = 4,
                            ChoosingPartyId = 4,
                            RevealEventId = 4
                        }
                    };

            //Creating Dummy PermissionsVM List
            _PermissionsVMList = new List<PermissionsVM>
                    {
                        new PermissionsVM {   PermissionsId = 1,
                            WVEmail = "fred.bloggs@worldvision.org.uk",
                            Administrator = true,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = true,
                            DownloadFilesandImages = true},
                        new PermissionsVM {  PermissionsId = 2,
                            WVEmail = "minnie.mouse@worldvision.org.uk",
                            Administrator = false,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = false,
                            DownloadFilesandImages = false
                        },
                        new PermissionsVM {  PermissionsId = 3,
                            WVEmail = "Vidge.mouse@worldvision.org.uk",
                            Administrator = false,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = false,
                            DownloadFilesandImages = false
                        },
                        new PermissionsVM { PermissionsId = 4,
                            WVEmail = "Vidge.rockster@worldvision.org.uk",
                            Administrator = false,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = false,
                            DownloadFilesandImages = false}
                    };
            //logger
            //  _mockLogger = new Mock<ILogger<RegistrationsController>>();

            // Mock the API Response 
            _mockResponse = new ApiResponse();

            #endregion

            //Setup MockPhotoApprovalFactory
            mockPhotoApprovalFactory = new MockPhotoApprovalFactory();

            ////setup for permission
            //var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //logger
            _mockLogger = new Mock<ILogger<PhotoApprovalController>>();

            // ApiResponse
            _apiReponse = new ApiResponse();

            _imageActions = new Mock<IImageStoreFactory>();
            _zipFileHelper = new Mock<IZipFileHelper>();

            //Mock the file sent via Request.Forms.Files using the httpContext
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "test.jpg");
            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
        }

        #region "Index and View Action"
        /// <summary>
        /// Can we Load all PhotoApproval?
        /// </summary>
        [Test]
        public void PhotoApproval_Index_Valid()
        {
            //Arrange
            // Loading all SignUpVM
            var mockPhotoApprovalFactoryResult = new MockPhotoApprovalFactory().MockLoad(_SignUpVMList);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup PhotoApprovalController
            _photoApprovalController = new PhotoApprovalController(mockPhotoApprovalFactoryResult.Result.Object,
                    _imageActions.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoApprovalController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<SignUpVM>;

            //Get Actual Count
            _SignUpVMListCount = _SignUpVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

            Assert.IsInstanceOf(typeof(List<SignUpVM>), dataResult); //Test if returns ok response               
            Assert.AreEqual(dataResult.Count, _SignUpVMListCount); //Test if returns equal count               
        }

        /// <summary>
        /// If we have no records in PhotoApproval?
        /// </summary>
        [Test]
        public void PhotoApproval_Index_NoRecords()
        {

            //Arrange
            // Loading all SignUpVM
            var mockPhotoApprovalFactoryResult = new MockPhotoApprovalFactory().MockLoad(null);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //Setup RegistrationsController
            _photoApprovalController = new PhotoApprovalController(mockPhotoApprovalFactoryResult.Result.Object,
                    _imageActions.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoApprovalController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<SignUpVM>;

            //Get Actual Count
            _SignUpVMListCount = _SignUpVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult, null); //Test if returns equal count             

        }

        /// <summary>
        /// Can we View Image of PhotoApproval?
        /// </summary>
        [Test]
        public void PhotoApproval_ViewImage_Valid()
        {
            //Arrange
            ImageInfoVM newImageInfoVM = new ImageInfoVM
            {
                ImageInfoId = 1,
                BlobGUID = "test1.jpg",
                UploadDateTime = DateTime.Now,
                ChosenSignUpId = 2,
                ImageStatusId = 3
            };
            bool imageUrlBool = true;
            string imgURL = "https://msapisandboxstorage.blob.core.windows.net/msapi-sandbox-photo-container/1.jpg?sv=2018-03-28&sr=b&sig=dJIW";
            // Loading all SignUpVM
            var mockPhotoApprovalFactoryResult = new MockPhotoApprovalFactory().MockViewImageInfo(newImageInfoVM, 2);
            var mockPhotoImageFactoryResult = new MockImageFactory().MockGetSecureUrlImage(imageUrlBool, imgURL);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup PhotoApprovalController
            _photoApprovalController = new PhotoApprovalController(mockPhotoApprovalFactoryResult.Result.Object,
                    mockPhotoImageFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoApprovalController.ShowPhoto(2);
            var actionResponse = actionResult.Result as JsonResult;
            var dataResult = actionResponse.Value as ImageInfoVM;

            //Assert  
            Assert.IsInstanceOf(typeof(JsonResult), actionResponse); //Test if returns ok response    

            Assert.IsInstanceOf(typeof(ImageInfoVM), dataResult); //Test if returns ok response        
            Assert.AreEqual(dataResult.ImageURL, imgURL); //Test if returns equal count             
        }

        /// <summary>
        /// If we have no records in PhotoApproval?
        /// </summary>
        [Test]
        public void PhotoApproval_ViewImage_NoRecords()
        {

            //Arrange
            ImageInfoVM newImageInfoVM = new ImageInfoVM
            {
                ImageInfoId = 1,
                BlobGUID = "test1.jpg",
                UploadDateTime = DateTime.Now,
                ChosenSignUpId = 2,
                ImageStatusId = 3
            };
            bool imageUrlBool = false;
            string imgURL = "https://msapisandboxstorage.blob.core.windows.net/msapi-sandbox-photo-container/1.jpg?sv=2018-03-28&sr=b&sig=dJIW";

            // Loading all SignUpVM
            var mockPhotoApprovalFactoryResult = new MockPhotoApprovalFactory().MockViewImageInfo(null, 2);

            //Load GetSecureUrlImage in MockImageFactory
            var mockPhotoImageFactoryResult = new MockImageFactory().MockGetSecureUrlImage(imageUrlBool, null);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //Setup RegistrationsController
            _photoApprovalController = new PhotoApprovalController(mockPhotoApprovalFactoryResult.Result.Object,
                    _imageActions.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoApprovalController.ShowPhoto(2);
            var actionResponse = actionResult.Result as JsonResult;
            var dataResult = actionResponse.Value as ImageInfoVM;

            //Assert  
            Assert.IsInstanceOf(typeof(JsonResult), actionResponse); //Test if returns ok response                        
            Assert.AreNotEqual(dataResult.ImageURL, imgURL); //Test if returns equal count             
        }
        #endregion

        #region "Delete,Replace and Approve Action "
        /// <summary>
        /// While we Delete PhotoApprove by Id?
        /// </summary>
        [Test]
        public void PhotoApprove_DeleteImage_Valid()
        {

            //Arrange
            ImageInfoVM newImageInfoVM = new ImageInfoVM
            {
                ImageInfoId = 1,
                BlobGUID = "test1.jpg",
                UploadDateTime = DateTime.Now,
                ChosenSignUpId = 1,
                ImageStatusId = 3
            };

            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Success = true;
            apiResponse.Message = "Successfully Image Deleted";
            apiResponse.ResponseObject = true;

            ApiResponse apiUpdResponse = new ApiResponse();
            apiUpdResponse.Success = true;
            apiUpdResponse.Message = "Successfully Image Updated";
            apiUpdResponse.ResponseObject = "1";

            //setup for DeleteImage
            var mockPhotoApprovalFactoryResult = new MockPhotoApprovalFactory().MockDeleteImageInfo(newImageInfoVM,apiResponse, apiUpdResponse, 1);

            //setup for Delete Image Factory
            var mockPhotoImageFactoryResult = new MockImageFactory().MockDeleteImage(apiResponse, newImageInfoVM.BlobGUID);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _photoApprovalController = new PhotoApprovalController(mockPhotoApprovalFactoryResult.Result.Object,
                    mockPhotoImageFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoApprovalController.Delete(1);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var actionResponseMessage = actionResponse.RouteValues.Values as object[];

            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns RedirectToActionResult response  
            Assert.AreEqual(apiResponse.Message, actionResponseMessage[0].ToString()); // Verify the message
        }

        /// <summary>
        /// While we Delete PhotoApprove by Id?
        /// </summary>
        [Test]
        public void PhotoApprove_DeleteImage_InValid()
        {
            //Arrange
            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Success = false;
            apiResponse.Message = "There is no image to delete";
            apiResponse.ResponseObject = null;

            ApiResponse apiUpdResponse = new ApiResponse();
            apiUpdResponse.Success = false;
            apiUpdResponse.Message = "Failed to Update Image";
            apiUpdResponse.ResponseObject = null;

            //setup for DeleteImage
            var mockPhotoApprovalFactoryResult = new MockPhotoApprovalFactory().MockDeleteImageInfo(null, apiResponse, apiUpdResponse, 5);

            //setup for Delete Image Factory
            var mockPhotoImageFactoryResult = new MockImageFactory().MockDeleteImage(null, "");

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _photoApprovalController = new PhotoApprovalController(mockPhotoApprovalFactoryResult.Result.Object,
                    mockPhotoImageFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoApprovalController.Delete(5);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var actionResponseMessage = actionResponse.RouteValues.Values as object[];

            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns RedirectToActionResult response  
            Assert.AreEqual(apiResponse.Message, actionResponseMessage[0].ToString()); // Verify the message
        }

        /// <summary>
        /// While we Replace PhotoApprove by Id?
        /// </summary>
        [Test]
        public void PhotoApprove_ReplaceImage_Valid()
        {
            //Arrange
            ImageInfoVM newImageInfoVM = new ImageInfoVM
            {
                ImageInfoId = 1,
                BlobGUID = "test1.jpg",
                UploadDateTime = DateTime.Now,
                ChosenSignUpId = 2,
                ImageStatusId = 3
            };

            //Arrange
            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Success = true;
            apiResponse.Message = "Successfully Image Updated";
            apiResponse.ResponseObject = "1.jpg";

            //setup for Replace Image
            var mockPhotoApprovalFactoryResult = new MockPhotoApprovalFactory().MockCreateReplace(apiResponse,_imageInfoVMList, newImageInfoVM, "1");

            //setup for Delete Image Factory
            var mockPhotoImageFactoryResult = new MockImageFactory().MockDeleteImage(apiResponse, newImageInfoVM.BlobGUID);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _photoApprovalController = new PhotoApprovalController(mockPhotoApprovalFactoryResult.Result.Object,
                    mockPhotoImageFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoApprovalController.Replace(ConvertedIFormFilePhoto, 1);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var actionResponseMessage = actionResponse.RouteValues.Values as object[];

            //Get Expected Count
            var expectedCount = _SignUpVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns RedirectToActionResult response  
            Assert.AreEqual(apiResponse.Message, actionResponseMessage[0].ToString()); // Verify the message
        }

        /// <summary>
        /// While we Replace PhotoApprove by Id?
        /// </summary>
        [Test]
        public void PhotoApprove_ReplaceImage_InValid()
        {
            //Arrange
            ImageInfoVM newImageInfoVM = new ImageInfoVM
            {
                ImageInfoId = 1,
                BlobGUID = "test1.jpg",
                UploadDateTime = DateTime.Now,
                ChosenSignUpId = 2,
                ImageStatusId = 3
            };

            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Success = false;
            apiResponse.Message = "Failed to Replace Image";
            apiResponse.ResponseObject = null;

            //setup for Replace Image
            var mockPhotoApprovalFactoryResult = new MockPhotoApprovalFactory().MockCreateReplace(apiResponse, null, newImageInfoVM, "1");

            //setup for Delete Image Factory
            var mockPhotoImageFactoryResult = new MockImageFactory().MockDeleteImage(apiResponse, null);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _photoApprovalController = new PhotoApprovalController(mockPhotoApprovalFactoryResult.Result.Object,
                    mockPhotoImageFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoApprovalController.Replace(ConvertedIFormFilePhoto, 5);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var actionResponseMessage = actionResponse.RouteValues.Values as object[];

            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns RedirectToActionResult response  
            Assert.AreEqual(apiResponse.Message, actionResponseMessage[0].ToString()); // Verify the message

        }

        /// <summary>
        /// While we Approve PhotoApprove by Id?
        /// </summary>
        [Test]
        public void PhotoApprove_ApproveImage_Valid()
        {
            //Arrange
            ImageInfoVM newImageInfoVM = new ImageInfoVM
            {
                ImageInfoId = 1,
                BlobGUID = "test1.jpg",
                UploadDateTime = DateTime.Now,
                ChosenSignUpId = 2,
                ImageStatusId = 3
            };

            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Success = true;
            apiResponse.Message = "Successfully Image Updated";
            apiResponse.ResponseObject = 1;

            //setup for to approve
            var mockImageStoreManagerResult = new MockPhotoApprovalFactory().MockApprove(apiResponse, newImageInfoVM, "test1.jpg");

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _photoApprovalController = new PhotoApprovalController(mockImageStoreManagerResult.Result.Object,
                    _imageActions.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoApprovalController.Approve(1);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var actionResponseMessage = actionResponse.RouteValues.Values as object[];

            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns RedirectToActionResult response  
            Assert.AreEqual(apiResponse.Message, actionResponseMessage[0].ToString()); // Verify the message
        }

        /// <summary>
        /// While we Approve PhotoApprove by Id?
        /// </summary>
        [Test]
        public void PhotoApprove_ApproveImage_InValid()
        {
            //Arrange
            ImageInfoVM newImageInfoVM = new ImageInfoVM
            {
                ImageInfoId = 1,
                BlobGUID = "test1.jpg",
                UploadDateTime = DateTime.Now,
                ChosenSignUpId = 2,
                ImageStatusId = 3
            };

            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Success = false;
            apiResponse.Message = "There is no image to approve";
            apiResponse.ResponseObject = null;
            var mockPhotoApprovalFactoryResult = new MockPhotoApprovalFactory().MockApprove(apiResponse, null, "test1.jpg");

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _photoApprovalController = new PhotoApprovalController(mockPhotoApprovalFactoryResult.Result.Object,
                    _imageActions.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoApprovalController.Approve(5);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var actionResponseMessage = actionResponse.RouteValues.Values as object[];

            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns RedirectToActionResult response  
            Assert.AreEqual(apiResponse.Message, actionResponseMessage[0].ToString()); // Verify the message

        }
        #endregion
    }
}
