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

namespace United4Admin.UnitTests.Controllers
    {
    [TestFixture]
    public class RegistrationsControllerTest
    {

        private RegistrationsController _registrationsController;
        //private Mock<ILogger<RegistrationsController>> _mockLogger;
        private MockRegistrationFactory mockRegistrationFactory;

        private SignUpVM _mockResponse;
        private int _SignUpVMListCount;
        private int _SignUpVMListMaxId;

        private List<SignUpVM> _SignUpVMList;
        private List<PermissionsVM> _PermissionsVMList;
        private List<ImageInfoVM> _imageInfoVMList;
        private ApiResponse _apiReponse;
        private Mock<IZipFileHelper> _zipFileHelper;
        private Mock<IImageStoreFactory> _imageActions;
        private Mock<ILogger<RegistrationsController>> _mockLogger;

        [SetUp]
        public void SetUp()

        {
            //Arrange
            #region "Arrange"

            _imageInfoVMList = new List<ImageInfoVM>()
                {
                    new ImageInfoVM{ImageInfoId=1, BlobGUID="test1.jpg",ChosenSignUpId =1},
                    new ImageInfoVM{ImageInfoId=2, BlobGUID="test2.jpg",ChosenSignUpId =2},
                    new ImageInfoVM{ImageInfoId=3, BlobGUID="test3.jpg",ChosenSignUpId =3},
                     new ImageInfoVM{ImageInfoId=4, BlobGUID="test3.jpg",ChosenSignUpId =4}
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
            _mockResponse = new SignUpVM();

            #endregion

            //Setup mockRegistrationFactory
            mockRegistrationFactory = new MockRegistrationFactory();

            ////setup for permission
            //var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //logger
            _mockLogger = new Mock<ILogger<RegistrationsController>>();

            // ApiResponse
            _apiReponse = new ApiResponse();

            _imageActions = new Mock<IImageStoreFactory>();
            _zipFileHelper = new Mock<IZipFileHelper>();
        }


            #region "Index Action"
            /// <summary>
            /// Can we Load all RevealEvents?
            /// </summary>
            [Test]
            public void Registration_Index_Valid()
            {
                //Arrange
                // Loading all SignUpVM
                var mockRegistrationFactoryResult = new MockRegistrationFactory().MockLoadList(_SignUpVMList);
                //setup for permission
                var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

                //Setup RegistrationsController
                _registrationsController = new RegistrationsController(mockRegistrationFactoryResult.Result.Object,
                    _imageActions.Object,_zipFileHelper.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

                //Act  
                var actionResult = _registrationsController.Index("");
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
        public void Registration_Index_NoRecords()
        {

            //Arrange
            // Loading all SignUpVM
            var mockRegistrationFactoryResult = new MockRegistrationFactory().MockLoadList(null);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //Setup RegistrationsController
            _registrationsController = new RegistrationsController(mockRegistrationFactoryResult.Result.Object,
              _imageActions.Object, _zipFileHelper.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _registrationsController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<SignUpVM>;

            //Get Actual Count
            _SignUpVMListCount = _SignUpVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult.Count, 0); //Test if returns equal count             

        }

        #endregion

        #region "View Image"
        /// <summary>
        ///Show image Action with Image
        /// </summary>
        [Test]
        public void Registration_ViewImage_WithImage()
        {
            // Get that SignUpVM for id : 2
            SignUpVM testSignUpVM = mockRegistrationFactory.MockFindById(_SignUpVMList, 2);

            ImageInfoVM testImageInfoVM = _imageInfoVMList.Where(x=>x.ImageInfoId == 2).FirstOrDefault();

            var mockRegistrationFactoryResult = new MockRegistrationFactory().MockGetImage(true,testImageInfoVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            object imgUrl = "TEst.jpeg";
            //setup for Image
            var mockImageFactory = new MockImageFactory().MockGetSecureUrlImage(true, imgUrl);

            _registrationsController = new RegistrationsController(mockRegistrationFactoryResult.Result.Object,
                         mockImageFactory.Result.Object, _zipFileHelper.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _registrationsController.ShowPhoto(2);
            var actionResponse = actionResult.Result as ViewResult;
            var _mockImgResponse = actionResponse.ViewData.Model as ImageInfoVM;
            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsNotNull(_mockImgResponse); // Test if null
            Assert.IsInstanceOf(typeof(ImageInfoVM), _mockImgResponse); //Test if returns ok response  
            Assert.IsNotNull(_mockImgResponse.ImageURL); //Test if returns ok response  
        }

        /// <summary>
        ///Show image Action without Image
        /// </summary>
        [Test]
        public void Registration_ViewImage_WithoutImage()
        {
            // Get that SignUpVM for id : 2
            SignUpVM testSignUpVM = mockRegistrationFactory.MockFindById(_SignUpVMList, 2);

            ImageInfoVM testImageInfoVM = _imageInfoVMList.Where(x => x.ImageInfoId == 2).FirstOrDefault();

            var mockRegistrationFactoryResult = new MockRegistrationFactory().MockGetImage(false, null);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            object imgUrl = "TEst.jpeg";
            //setup for Image
            var mockImageFactory = new MockImageFactory().MockGetSecureUrlImage(false, imgUrl);

            _registrationsController = new RegistrationsController(mockRegistrationFactoryResult.Result.Object,
                         mockImageFactory.Result.Object, _zipFileHelper.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _registrationsController.ShowPhoto(2);
            var actionResponse = actionResult.Result as ViewResult;
            var _mockImgResponse = actionResponse.ViewData.Model as ImageInfoVM;
            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsNotNull(_mockImgResponse); // Test if null
            Assert.IsInstanceOf(typeof(ImageInfoVM), _mockImgResponse); //Test if returns ok response  
            Assert.IsNull(_mockImgResponse.ImageURL); //Test if returns ok response  
        }
        #endregion

        #region "Download Action "

        ///// <summary>
        ///// when we download a Registrations  details using DownloadEchoData action
        ///// </summary>
        //[Test]
        //public void Registration_DownloadEchoData_WithData()
        //{
        //    //Arrange              
        //    var mockRegistrationFactoryResult = new MockRegistrationFactory().MockGetEchoData(true, _SignUpVMList);
        //    //setup for permission
        //    var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

        //    _registrationsController = new RegistrationsController(mockRegistrationFactoryResult.Result.Object,
        //                 _imageActions.Object, _zipFileHelper.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

        //    //Act  
        //    var actionResult = _registrationsController.DownloadECHOData();
        //    var actionResponse = actionResult.Result as FileContentResult;
        //    //Assert  
        //    Assert.IsInstanceOfType(actionResponse, typeof(FileContentResult)); //Test if returns ok response  
        //}

        ///// <summary>
        ///// when we download a Registrations  details using Field action
        ///// </summary>
        //[Test]
        //public void Registration_GetFieldData_WithData()
        //{
        //    //Arrange              
        //    var mockRegistrationFactoryResult = new MockRegistrationFactory().MockGetFieldData(true, _imageInfoVMList);
        //    //setup for permission
        //    var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

        //    _registrationsController = new RegistrationsController(mockRegistrationFactoryResult.Result.Object,
        //                 _imageActions.Object, _zipFileHelper.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

        //    //Act  
        //    var actionResult = _registrationsController.DownloadFieldData();
        //    var actionResponse = actionResult.Result as FileContentResult;
        //    //Assert  
        //    Assert.IsInstanceOfType(actionResponse, typeof(FileContentResult)); //Test if returns ok response  
        //}
        #endregion

        #region "Edit Action "

        /// <summary>
        /// while we Load aRegistrations By Id for edit action?
        /// </summary>
        [Test]
        public void Registration_EditLoad_Valid()
        {
            //Arrange

            // Get that SignUpVM for id : 2
            SignUpVM testSignUpVM = mockRegistrationFactory.MockFindById(_SignUpVMList, 2);

            var mockRegistrationFactoryResult = new MockRegistrationFactory().MockLoad(testSignUpVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _registrationsController = new RegistrationsController(mockRegistrationFactoryResult.Result.Object,
                    _imageActions.Object, _zipFileHelper.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);
            //Act  
            var actionResult = _registrationsController.Edit(2);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as SignUpVM;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsNotNull(_mockResponse); // Test if null
            Assert.IsInstanceOf(typeof(SignUpVM), _mockResponse); //Test if returns ok response   
        }

        /// <summary>
        /// When we Load aRegistrations By Id for edit action if returns http not found result?
        /// </summary>
        [Test]
        public void Registration_EditLoad_InValid()
        {
            //Arrange

            // Get that SignUpVM for id : 2
            SignUpVM testSignUpVM = mockRegistrationFactory.MockFindById(_SignUpVMList, 5);

            var mockRegistrationFactoryResult = new MockRegistrationFactory().MockLoad(testSignUpVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            _registrationsController = new RegistrationsController(mockRegistrationFactoryResult.Result.Object,
              _imageActions.Object, _zipFileHelper.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _registrationsController.Edit(5);
            var actionResponse = actionResult.Result as NotFoundResult;

            //Assert  
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResponse); //Test if returns ok response  

        }

            /// <summary>
            /// When we edit aRegistrations By Id using edit action if returns ok result?
            /// </summary>
            [Test]
            public void Registration_EditSave_Valid()
            {
                //Arrange            
                _apiReponse.Message = "Registrations updated successfully";
                _apiReponse.ResponseObject = (true) as object;
                _apiReponse.Success = true;

                // Get that SignUpVM for id : 2
                SignUpVM testSignUpVM = mockRegistrationFactory.MockFindById(_SignUpVMList, 2);

                // Change one of its properties
                testSignUpVM.FirstName = "Unit Test Data";
                //setup for permission
                var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

                var mockRegistrationFactoryResult = new MockRegistrationFactory().MockUpdate(_apiReponse, _SignUpVMList, testSignUpVM);
               
                _registrationsController = new RegistrationsController(mockRegistrationFactoryResult.Result.Object,
                  _imageActions.Object, _zipFileHelper.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

                //Act  
                var actionResult = _registrationsController.Edit(testSignUpVM);
                var actionResponse = actionResult.Result as RedirectToActionResult;


                //Assert  
                 Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
                Assert.AreEqual("Unit Test Data", mockRegistrationFactory.MockFindById(_SignUpVMList, 2).FirstName);
            }

          
            #endregion

            #region "Delete Action


            /// <summary>
            /// while we Load aRegistrations By Id for delete action?
            /// </summary>
            [Test]
            public void Registration_DeleteLoad()
            {
                //Arrange
                //Get Actual Count
                _SignUpVMListCount = _SignUpVMList.Count;
                // Get that SignUpVM for id : 2
                SignUpVM testSignUpVM = mockRegistrationFactory.MockFindById(_SignUpVMList, 4);

                var mockRegistrationFactoryResult = new MockRegistrationFactory().MockDeleteLoad(testSignUpVM, true, 4);
                //setup for permission
                var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

                _registrationsController = new RegistrationsController(mockRegistrationFactoryResult.Result.Object,
                  _imageActions.Object, _zipFileHelper.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

                //Act  
                var actionResult = _registrationsController.Delete(4);
                var actionResponse = actionResult.Result as ViewResult;


                //Assert  
                  Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

            }

            /// <summary>
            ///  we delete a SignUpVM if there is no registeration exists?
            /// </summary>
            [Test]
            public void Registration_DeleteConfirmed_Valid()
            {
                //Arrange            
                _apiReponse.Message = "Registrations deleted successfully";
                _apiReponse.ResponseObject = (true) as object;
                _apiReponse.Success = true;
                //Get Actual Count
                _SignUpVMListCount = _SignUpVMList.Count;
                // Get that SignUpVM for id : 2
                SignUpVM testSignUpVM = mockRegistrationFactory.MockFindById(_SignUpVMList, 4);

                var mockRegistrationFactoryResult = new MockRegistrationFactory().MockDelete(_apiReponse, _SignUpVMList, 4);
                //setup for permission
                var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

                _registrationsController = new RegistrationsController(mockRegistrationFactoryResult.Result.Object,
                  _imageActions.Object, _zipFileHelper.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

                //Act  
                var actionResult = _registrationsController.DeleteConfirmed(4);
                var actionResponse = actionResult.Result as RedirectToActionResult;

                //Assert  
                 Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
                Assert.AreNotEqual(_SignUpVMList.Count, _SignUpVMListCount); // Test count is not equal before and after delete.
            }


            #endregion
        }
    }

