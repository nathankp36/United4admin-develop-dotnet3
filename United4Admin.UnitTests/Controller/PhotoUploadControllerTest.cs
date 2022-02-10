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
    public class PhotoUploadControllerTest
    {
        private PhotoUploadController _photoUploadController;
        //private Mock<ILogger<RegistrationsController>> _mockLogger;
        private MockRegistrationFactory mockRegistrationFactory;
        private ActionContext _actionContext;
        private MockPhotoUploadFactory mockPhotoUploadFactory;

        private ApiResponse _mockResponse;
        private int _SignUpVMListCount;
        private int _SignUpVMListMaxId;

        private List<SignUpVM> _SignUpVMList;
        private List<PermissionsVM> _PermissionsVMList;
        private List<ImageInfoVM> _imageInfoVMList;
        private ApiResponse _apiReponse;
        private Mock<IZipFileHelper> _zipFileHelper;
        private Mock<IImageStoreFactory> _imageActions;
        private Mock<ILogger<PhotoUploadController>> _mockLogger;
        public IFormFile ConvertedIFormFilePhoto { get; set; }
        [SetUp]
        public void SetUp()

        {
            //Arrange
            #region "Arrange"
            SignUpVM signUpVM = new SignUpVM
            {
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
                RevealEventId = 1,
                RegistrationEventName = "ChosenEvent1"
            };

            _imageInfoVMList = new List<ImageInfoVM>()
                {
                    new ImageInfoVM{ImageInfoId=1, BlobGUID="test1.jpg",ChosenSignUpId =1,ImageStatusId=3,UploadDateTime=DateTime.Now,ChosenSignUp=signUpVM},
                    new ImageInfoVM{ImageInfoId=2, BlobGUID="test2.jpg",ChosenSignUpId =2,ImageStatusId=3,UploadDateTime=DateTime.Now,ChosenSignUp=signUpVM},
                    new ImageInfoVM{ImageInfoId=3, BlobGUID="test3.jpg",ChosenSignUpId =3,ImageStatusId=3,UploadDateTime=DateTime.Now,ChosenSignUp=signUpVM},
                     new ImageInfoVM{ImageInfoId=4, BlobGUID="test3.jpg",ChosenSignUpId =4,ImageStatusId=3,UploadDateTime=DateTime.Now,ChosenSignUp=signUpVM}
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
                            RevealEventId = 1,
                            RegistrationEventName="ChosenEvent1"
                        },
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
                            RevealEventId = 2,
                            RegistrationEventName="ChosenEvent2"
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
                            RevealEventId = 3,
                            RegistrationEventName="ChosenEvent3"
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
                            RevealEventId = 4,
                            RegistrationEventName="ChosenEvent5"
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

            //Setup mockRegistrationFactory
            mockRegistrationFactory = new MockRegistrationFactory();

            ////setup for permission
            //var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //logger
            _mockLogger = new Mock<ILogger<PhotoUploadController>>();

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

        #region "Index Action"
        /// <summary>
        /// Can we Load all RevealEvents?
        /// </summary>
        [Test]
        public void ImageNotUpload_Index_Valid()
        {
            //Arrange
            // Loading all SignUpVM
            var mockRegistrationFactoryResult = new MockPhotoUploadFactory().MockLoad(_SignUpVMList);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup PhotoUploadController
            _photoUploadController = new PhotoUploadController(mockRegistrationFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoUploadController.Index("");
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
        /// If we have no records in RevealEvents?
        /// </summary>
        [Test]
        public void ImageNotUpload_Index_NoRecords()
        {

            //Arrange
            // Loading all SignUpVM
            var mockRegistrationFactoryResult = new MockRegistrationFactory().MockLoadList(null);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //Setup RegistrationsController
            _photoUploadController = new PhotoUploadController(mockRegistrationFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoUploadController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<SignUpVM>;

            //Get Actual Count
            _SignUpVMListCount = _SignUpVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult, null); //Test if returns equal count             

        }

        #endregion

        #region "Upload Action "

        /// <summary>
        /// while we Load aRegistrations By Id for edit action?
        /// </summary>
        [Test]
        public void Registration_UploadImage_Valid()
        {
            //Arrange
            //Get Actual Count
            _SignUpVMListCount = _SignUpVMList.Count;

            //Get Max ID from List
            _SignUpVMListMaxId = Convert.ToInt32(_SignUpVMList.Max(x => x.chosenSignUpId).ToString()) + 1;

            //Arrange
            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Success = true;
            apiResponse.Message = "Successfully Image Created";
            apiResponse.ResponseObject = "1.jpg";

            var mockImageStoreManagerResult = new MockPhotoUploadFactory().MockCreate(apiResponse, _imageInfoVMList, "1");

            _photoUploadController = new PhotoUploadController(mockImageStoreManagerResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoUploadController.Create(ConvertedIFormFilePhoto, 1);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var actionResponseMessage = actionResponse.RouteValues.Values as object[];

            //Get Expected Count
            var expectedCount = _SignUpVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns RedirectToActionResult response  
            Assert.AreEqual(apiResponse.Message, actionResponseMessage[0].ToString()); // Verify the message
        }

        /// <summary>
        /// When we Load aRegistrations By Id for edit action if returns http not found result?
        /// </summary>
        [Test]
        public void Registration_UploadImage_InValid()
        {
            //Arrange
            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Success = false;
            apiResponse.Message = "Failed to Created Image";
            apiResponse.ResponseObject = null;
            var mockImageStoreManagerResult = new MockPhotoUploadFactory().MockCreate(apiResponse, _imageInfoVMList, null);
            _photoUploadController = new PhotoUploadController(mockImageStoreManagerResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _photoUploadController.Create(ConvertedIFormFilePhoto, 5);
            var actionResponse = actionResult.Result as RedirectToActionResult;
            var actionResponseMessage = actionResponse.RouteValues.Values as object[];

            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns RedirectToActionResult response  
            Assert.AreEqual(apiResponse.Message, actionResponseMessage[0].ToString()); // Verify the message

        }
        #endregion
    }
}
