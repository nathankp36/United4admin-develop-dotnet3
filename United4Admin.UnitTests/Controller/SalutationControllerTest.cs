using United4Admin.UnitTests.Mocks.ApiClientFactory;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.Controllers;
using United4Admin.WebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace United4Admin.UnitTests.Controller
{
    [TestFixture]
    public class SalutationControllerTest
    {
        private SalutationController _salutationController;
        private Mock<ILogger<SalutationController>> _mockLogger;
        private MockSalutationFactory mockSalutationFactory;

        private SalutationVM _mockResponse;
        private int _salutationVMListCount;
        private string _salutationVMListMaxId;

        private List<SalutationVM> _salutationVMList;
        private List<PermissionsVM> _permissionsVMList;
        private ApiResponse _apiReponse;

        [SetUp]
        public void TestInitialize()

        {
            //Arrange
            #region "Arrange"
            //Creating Dummy SalutationVM List
            _salutationVMList = new List<SalutationVM>
                    {
                        new SalutationVM {ddlSalutation = "TestMs",
                                                crmSalutation = "2"},
                        new SalutationVM {ddlSalutation = "TestMrs",
                                                crmSalutation = "2"},
                        new SalutationVM {ddlSalutation = "TestMs1",
                                                crmSalutation = "5"},
                        new SalutationVM {ddlSalutation = "TestMrs1",
                                                crmSalutation = "6"}
                    };

            //Creating Dummy PermissionsVM List
            _permissionsVMList = new List<PermissionsVM>
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
            //  _mockLogger = new Mock<ILogger<SalutationController>>();

            // Mock the API Response 
            _mockResponse = new SalutationVM();

            #endregion

            //Setup mockSalutationFactory
            mockSalutationFactory = new MockSalutationFactory();

            ////setup for permission
            //var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //logger
            _mockLogger = new Mock<ILogger<SalutationController>>();

            // ApiResponse
            _apiReponse = new ApiResponse();
        }

        #region "Create Action "

        /// <summary>
        /// while we Load a Salutation By Id for edit action?
        /// </summary>
        [Test]
        public void Salutation_CreateLoad_Valid()
        {
            //Arrange
            // Create a new SalutationModel, not I do not supply an id
            SalutationVM newSalutationVM = new SalutationVM
            {
                Create = true
            };

            var mockSalutationFactoryResult = new MockSalutationFactory().MockLoad(newSalutationVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            _salutationController = new SalutationController(mockSalutationFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _salutationController.Create(0);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as SalutationVM;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
        }

        ///// <summary>
        ///// Can we insert a new SalutationVM?
        ///// </summary>        
        [Test]
        public void Salutation_CreateSave_Valid()
        {
            //Arrange
            //Get Actual Count
            _salutationVMListCount = _salutationVMList.Count;

            //Get Max ID from List
            _salutationVMListMaxId = _salutationVMList.Max(x => x.ddlSalutation).ToString();

            // Create a new SalutationModel, not I do not supply an id
            SalutationVM newSalutationVM = new SalutationVM
            {
                ddlSalutation = "Test MS 3",
                crmSalutation = "6"
            };
            _apiReponse.Message = "Salutation created successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            var mockSalutationFactoryResult = new MockSalutationFactory().MockCreate(_apiReponse, _salutationVMList, newSalutationVM);
            _salutationController = new SalutationController(
                                      mockSalutationFactoryResult.Result.Object,
                                       mockPermissionFactoryResult.Result.Object, _mockLogger.Object);


            //ACT
            #region "ACT"

            // try saving our new SalutationVM
            var actionResult = _salutationController.Create(newSalutationVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            #endregion

            //Get Expected Count
            var expectedCount = _salutationVMList.Count;

            // Verify that our new SalutationVM has been created
            SalutationVM testSalutationVM = mockSalutationFactory.MockFindById(_salutationVMList, _salutationVMListMaxId);

            // Assert
            #region "Assert"            
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response    
            // Check both actual and expected count is same after insert        
            Assert.AreEqual(expectedCount, _salutationVMListCount + 1); // Verify the expected Number post-insert
            #endregion
        }

        /// <summary>
        /// Can we insert a new SalutationVM?
        /// </summary>
        [Test]
        public void Salutation_CreateSave_DuplicateData()
        {
            //Arrange
            //Get Actual Count
            _salutationVMListCount = _salutationVMList.Count;

            //Get Max ID from List
            _salutationVMListMaxId = _salutationVMList.Max(x => x.ddlSalutation).ToString();

            // Create a new SalutationModel, not I do not supply an id
            SalutationVM newSalutationVM = new SalutationVM
            {
                ddlSalutation = "TestMs",
                crmSalutation = "2"
            };
            _apiReponse.Message = "Failed to create Salutation";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);
            var mockSalutationFactoryResult = new MockSalutationFactory().MockCreate(_apiReponse, _salutationVMList, newSalutationVM);
            _salutationController = new SalutationController(
                                      mockSalutationFactoryResult.Result.Object,
                                       mockPermissionFactoryResult.Result.Object, _mockLogger.Object);


            //ACT
            #region "ACT"

            // try saving our new SalutationVM
            var actionResult = _salutationController.Create(newSalutationVM);
            var actionResponse = actionResult.Result as ViewResult;
            #endregion


            //Get Expected Count
            var expectedCount = _salutationVMList.Count;


            // Assert
            #region "Assert"            
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response      
            // Check both actual and expected count is same after insert        
            Assert.AreNotEqual(expectedCount, _salutationVMListCount + 1); // Verify the expected Number post-insert
            #endregion
        }
        #endregion

        #region "Edit Action "

        /// <summary>
        /// while we Load a Salutation By Id for edit action?
        /// </summary>
        [Test]
        public void Salutation_EditLoad_Valid()
        {
            //Arrange

            // Get that SalutationVM for id : Ms
            SalutationVM testSalutationVM = mockSalutationFactory.MockFindById(_salutationVMList, "TestMrs");

            var mockSalutationFactoryResult = new MockSalutationFactory().MockLoad(testSalutationVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            _salutationController = new SalutationController(mockSalutationFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _salutationController.Edit("TestMs");
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as SalutationVM;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsNotNull(_mockResponse); // Test if null
            Assert.IsInstanceOf(typeof(SalutationVM), _mockResponse); //Test if returns ok response   
        }

        /// <summary>
        /// When we Load a Salutation By Id for edit action if returns http not found result?
        /// </summary>
        [Test]
        public void Salutation_EditLoad_InValid()
        {
            //Arrange

            // Get that SalutationVM for id : 2
            SalutationVM testSalutationVM = mockSalutationFactory.MockFindById(_salutationVMList, "Test Ms");

            var mockSalutationFactoryResult = new MockSalutationFactory().MockLoad(testSalutationVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);
            _salutationController = new SalutationController(mockSalutationFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _salutationController.Edit("Test Ms");
            var actionResponse = actionResult.Result as NotFoundResult;

            //Assert  
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResponse); //Test if returns ok response  

        }

        /// <summary>
        /// When we edit a Salutation By Id using edit action if returns ok result?
        /// </summary>
        [Test]
        public void Salutation_EditSave_Valid()
        {
            //Arrange            
            _apiReponse.Message = "Salutation updated successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            // Get that SalutationVM for id : Ms
            SalutationVM testSalutationVM = mockSalutationFactory.MockFindById(_salutationVMList, "TestMs");

            // Change one of its properties
            testSalutationVM.crmSalutation = "1";
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            var mockSalutationFactoryResult = new MockSalutationFactory().MockUpdate(_apiReponse, _salutationVMList, testSalutationVM);
            _salutationController = new SalutationController(mockSalutationFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _salutationController.Edit(testSalutationVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreEqual("1", mockSalutationFactory.MockFindById(_salutationVMList, "TestMs").crmSalutation);
        }
       
        #endregion

        #region "Delete Action


        /// <summary>
        /// while we Load a Salutation By Id for delete action?
        /// </summary>
        [Test]
        public void Salutation_DeleteLoad()
        {
            //Arrange
            //Get Actual Count
            _salutationVMListCount = _salutationVMList.Count;
            // Get that SalutationVM for id : 2
            SalutationVM testSalutationVM = mockSalutationFactory.MockFindById(_salutationVMList, "TestMs");

            var mockSalutationFactoryResult = new MockSalutationFactory().MockDeleteLoad(testSalutationVM, "TestMs");
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            _salutationController = new SalutationController(mockSalutationFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _salutationController.Delete("TestMs");
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

        }

        /// <summary>
        ///  we delete a SalutationVM ?
        /// </summary>
        [Test]
        public void Salutation_DeleteConfirmed_Valid()
        {
            //Arrange            
            _apiReponse.Message = "Salutation deleted successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;
            //Get Actual Count
            _salutationVMListCount = _salutationVMList.Count;
            // Get that SalutationVM for id : 2
            SalutationVM testSalutationVM = mockSalutationFactory.MockFindById(_salutationVMList, "TestMs");

            var mockSalutationFactoryResult = new MockSalutationFactory().MockDelete(_apiReponse, _salutationVMList, "TestMs");
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            _salutationController = new SalutationController(mockSalutationFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _salutationController.DeleteConfirmed("TestMs");
            var actionResponse = actionResult.Result as RedirectToActionResult;



            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreNotEqual(_salutationVMList.Count, _salutationVMListCount); // Test count is not equal before and after delete.
        }


        #endregion
    }
}
