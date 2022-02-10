using United4Admin.UnitTests.Mocks.ApiClientFactory;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.Controllers;
using United4Admin.WebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

namespace United4Admin.UnitTests.Controllers
{
    [TestFixture]
    public class AdministrationControllerTest
    {
        private AdministrationController _administrationController;
        //private Mock<ILogger<AdministrationController>> _mockLogger;
        private MockPermissionFactory mockPermissionFactory;
        private IPermissionFactory permissionFactory;
        private PermissionsVM _mockResponse;
        private int _PermissionsVMListCount;
        private int _PermissionsVMListMaxId;
        private Mock<ILogger<AdministrationController>> _mockLogger;
        private List<PermissionsVM> _PermissionsVMList;
        private ApiResponse _apiReponse;


        [SetUp]
        public void SetUp()
        {
            //Arrange
            #region "Arrange"


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
                            WVEmail = "test.mouse@worldvision.org.uk",
                            Administrator = false,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = false,
                            DownloadFilesandImages = false
                        },
                        new PermissionsVM {  PermissionsId = 3,
                            WVEmail = "test@worldvision.org.uk",
                            Administrator = false,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = false,
                            DownloadFilesandImages = false
                        },
                        new PermissionsVM { PermissionsId = 4,
                            WVEmail = "test.rockster@worldvision.org.uk",
                            Administrator = false,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = false,
                            DownloadFilesandImages = false}
                    };

            //logger
            //  _mockLogger = new Mock<ILogger<AdministrationController>>();

            // Mock the API Response 
            _mockResponse = new PermissionsVM();

            #endregion

            //Setup mockPermissionFactory
            mockPermissionFactory = new MockPermissionFactory();
            //logger
            _mockLogger = new Mock<ILogger<AdministrationController>>();

            // ApiResponse
            _apiReponse = new ApiResponse();
        }


        #region "Index Action"
        /// <summary>
        /// Can we Load all choosingEvents?
        /// </summary>
        [Test]
        public void Administration_Index_Valid()
        {
            //Arrange
            // Loading all PermissionsVM
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup AdministrationController
            _administrationController = new AdministrationController( mockPermissionFactoryResult.Result.Object,_mockLogger.Object,"test");

            //Act  
            var actionResult = _administrationController.UserIndex("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<PermissionsVM>;

            //Get Actual Count
            _PermissionsVMListCount = _PermissionsVMList.Count;

            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

            Assert.IsInstanceOf(typeof(List<PermissionsVM>), dataResult); //Test if returns ok response               
            Assert.AreEqual(dataResult.Count, _PermissionsVMListCount); //Test if returns equal count               
        }

        /// <summary>
        /// If we have no records in choosingEvents?
        /// </summary>
        [Test]
        public void Administration_Index_NoRecords()
        {

            //Arrange
            // Loading all PermissionsVM
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(null);

            //Setup AdministrationController
            _administrationController = new AdministrationController( mockPermissionFactoryResult.Result.Object,_mockLogger.Object, "test");

            //Act  
            var actionResult = _administrationController.UserIndex("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<PermissionsVM>;

            //Get Actual Count
            _PermissionsVMListCount = _PermissionsVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult.Count, 0); //Test if returns equal count             

        }

        #endregion 

        #region "Create Action "

        /// <summary>
        /// while we Load a Permission By Id for edit action?
        /// </summary>
        [Test]
        public void Administration_CreateLoad_Valid()
        {
            PermissionsVM newPermissionsVM = new PermissionsVM();
            //Arrange           
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoad(newPermissionsVM);

            _administrationController = new AdministrationController( mockPermissionFactoryResult.Result.Object,_mockLogger.Object,"test");

            //Act  
            var actionResult = _administrationController.UserCreate();
            var actionResponse = actionResult as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as PermissionsVM;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
        }

        ///// <summary>
        ///// Can we insert a new PermissionsVM?
        ///// </summary>
        [Test]
        public void Administration_CreateSave_Valid()
        {
            //Arrange
            //Get Actual Count
            _PermissionsVMListCount = _PermissionsVMList.Count;

            //Get Max ID from List
            _PermissionsVMListMaxId = Convert.ToInt32(_PermissionsVMList.Max(x => x.PermissionsId).ToString()) + 1;

            // Create a new PermissionsVM, not I do not supply an id
            PermissionsVM newPermissionsVM = new PermissionsVM
            {
                PermissionsId = _PermissionsVMListMaxId,
                WVEmail = "Vidge.test@worldvision.org.uk",
                Administrator = false,
                EditDeleteSupporterData = true,
                CreateEditDeleteEvents = false,
                DownloadFilesandImages = false
            };
            _apiReponse.Message = "Permission created successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;


            var mockPermissionFactoryResult = new MockPermissionFactory().MockCreate(_apiReponse, _PermissionsVMList, newPermissionsVM);
            _administrationController = new AdministrationController(
                                       mockPermissionFactoryResult.Result.Object,_mockLogger.Object, "test");


            //ACT
            #region "ACT"

            // try saving our new PermissionsVM
            var actionResult = _administrationController.UserCreate(newPermissionsVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            #endregion


            //Get Expected Count
            var expectedCount = _PermissionsVMList.Count;

            // Verify that our new PermissionsVM has been created
            PermissionsVM testPermissionsVM = mockPermissionFactory.MockFindById(_PermissionsVMList, _PermissionsVMListMaxId);

            // Assert
            #region "Assert"            
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response    
            // Check both actual and expected count is same after insert        
            Assert.AreEqual(expectedCount, _PermissionsVMListCount + 1); // Verify the expected Number post-insert
            Assert.AreEqual(_PermissionsVMListMaxId, testPermissionsVM.PermissionsId); // Verify it has the expected PermissionsVM Id
            #endregion
        }

        /// <summary>
        /// Can we insert a new PermissionsVM?
        /// </summary>
        [Test]
        public void Administration_CreateSave_DuplicateData()
        {
            //Arrange
            //Get Actual Count
            _PermissionsVMListCount = _PermissionsVMList.Count;

            //Get Max ID from List
            _PermissionsVMListMaxId = Convert.ToInt32(_PermissionsVMList.Max(x => x.PermissionsId).ToString()) + 1;

            // Create a new PermissionsVM, not I do not supply an id
            PermissionsVM newPermissionsVM = new PermissionsVM
            {
                PermissionsId = _PermissionsVMListMaxId,
                WVEmail = "test.rockster@worldvision.org.uk",
                Administrator = false,
                EditDeleteSupporterData = true,
                CreateEditDeleteEvents = false,
                DownloadFilesandImages = false
            };
            _apiReponse.Message = "Failed to create Permission";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;


            var mockPermissionFactoryResult = new MockPermissionFactory().MockCreate(_apiReponse, _PermissionsVMList, newPermissionsVM);
            _administrationController = new AdministrationController(
                                       mockPermissionFactoryResult.Result.Object,_mockLogger.Object,"test");


            //ACT
            #region "ACT"

            // try saving our new PermissionsVM
            var actionResult = _administrationController.UserCreate(newPermissionsVM);
            var actionResponse = actionResult.Result as ViewResult;
            #endregion


            //Get Expected Count
            var expectedCount = _PermissionsVMList.Count;


            // Assert
            #region "Assert"            
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response      
            // Check both actual and expected count is same after insert        
            Assert.AreNotEqual(expectedCount, _PermissionsVMListCount + 1); // Verify the expected Number post-insert
            #endregion
        }
        #endregion

        #region "Edit Action "

        /// <summary>
        /// while we Load a Permission By Id for edit action?
        /// </summary>
        [Test]
        public void Administration_EditLoad_Valid()
        {
            //Arrange

            // Get that PermissionsVM for id : 2
            PermissionsVM testPermissionsVM = mockPermissionFactory.MockFindById(_PermissionsVMList, 2);

            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoad(testPermissionsVM);

            _administrationController = new AdministrationController( mockPermissionFactoryResult.Result.Object,_mockLogger.Object,"test");

            //Act  
            var actionResult = _administrationController.UserEdit(2);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as PermissionsVM;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsNotNull(_mockResponse); // Test if null
            Assert.IsInstanceOf(typeof(PermissionsVM), _mockResponse); //Test if returns ok response   
        }

        /// <summary>
        /// When we Load a Permission By Id for edit action if returns http not found result?
        /// </summary>
        [Test]
        public void Administration_EditLoad_InValid()
        {
            //Arrange

            // Get that PermissionsVM for id : 5
            PermissionsVM testPermissionsVM = mockPermissionFactory.MockFindById(_PermissionsVMList, 5);

            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoad(testPermissionsVM);
            _administrationController = new AdministrationController( mockPermissionFactoryResult.Result.Object,_mockLogger.Object,"test");

            //Act  
            var actionResult = _administrationController.UserEdit(5);
            var actionResponse = actionResult.Result as NotFoundResult;

            //Assert  
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResponse); //Test if returns ok response  

        }

        /// <summary>
        /// When we edit a Permission By Id using edit action if returns ok result?
        /// </summary>
        [Test]
        public void Administration_EditSave_Valid()
        {
            //Arrange            
            _apiReponse.Message = "Permission updated successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            // Get that PermissionsVM for id : 2
            PermissionsVM testPermissionsVM = mockPermissionFactory.MockFindById(_PermissionsVMList, 2);

            // Change one of its properties
            testPermissionsVM.WVEmail = "test123@worldvision.org.uk";

            var mockPermissionFactoryResult = new MockPermissionFactory().MockUpdate(_apiReponse, _PermissionsVMList, testPermissionsVM);
            _administrationController = new AdministrationController( mockPermissionFactoryResult.Result.Object,_mockLogger.Object,"test");

            //Act  
            var actionResult = _administrationController.UserEdit(testPermissionsVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreEqual("test123@worldvision.org.uk", mockPermissionFactory.MockFindById(_PermissionsVMList, 2).WVEmail);
        }

        /// <summary>
        /// When we edit a Permission By Id using edit action if its not update?
        /// </summary>
        [Test]
        public void Administration_EditSave_Duplicate()
        {
            //Arrange            
            _apiReponse.Message = "Permission updated successfully";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            // Get that PermissionsVM for id : 2
            PermissionsVM testPermissionsVM = mockPermissionFactory.MockFindById(_PermissionsVMList, 3);

            // Change one of its properties
            testPermissionsVM.WVEmail = "test.rockster@worldvision.org.uk";

            var mockPermissionFactoryResult = new MockPermissionFactory().MockUpdate(_apiReponse, _PermissionsVMList, testPermissionsVM);
            _administrationController = new AdministrationController( mockPermissionFactoryResult.Result.Object,_mockLogger.Object, "test");

            //Act  
            var actionResult = _administrationController.UserEdit(testPermissionsVM);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            //Assert.AreNotEqual("Vidge.mouse@worldvision.org.uk", mockPermissionFactory.MockFindById(_PermissionsVMList, 3).WVEmail);

        }
        #endregion

        #region "Delete Action


        /// <summary>
        /// while we Load a Permission By Id for delete action?
        /// </summary>
        [Test]
        public void Administration_DeleteLoad_SameUser()
        {
            //Arrange
            //Get Actual Count
            _PermissionsVMListCount = _PermissionsVMList.Count;
            // Get that PermissionsVM for id : 2
            PermissionsVM testPermissionsVM = mockPermissionFactory.MockFindById(_PermissionsVMList, 4);

            var mockPermissionFactoryResult = new MockPermissionFactory().MockDeleteLoad(testPermissionsVM, true, 4);

            var identity = new GenericIdentity(testPermissionsVM.WVEmail);
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            _administrationController = new AdministrationController( mockPermissionFactoryResult.Result.Object,_mockLogger.Object, testPermissionsVM.WVEmail);
            
            //Act  
            var actionResult = _administrationController.UserDelete(4);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.AreEqual(_PermissionsVMList.Count, _PermissionsVMListCount); // Test count is not equal before and after delete.
        }


        /// <summary>
        /// while we Load a Permission By Id for delete action?
        /// </summary>
        [Test]
        public void Administration_DeleteLoad_OtherUser()
        {
            //Arrange
            //Get Actual Count
            _PermissionsVMListCount = _PermissionsVMList.Count;
            // Get that PermissionsVM for id : 4
            PermissionsVM testPermissionsVM = mockPermissionFactory.MockFindById(_PermissionsVMList, 4);

            var mockPermissionFactoryResult = new MockPermissionFactory().MockDeleteLoad(testPermissionsVM, false, 4);

            _administrationController = new AdministrationController( mockPermissionFactoryResult.Result.Object,_mockLogger.Object, "test");

            //Act  
            var actionResult = _administrationController.UserDeleteConfirmed(2);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

        }
        /// <summary>
        ///  we delete a PermissionsVM if there is no registeration exists?
        /// </summary>
        [Test]
        public void Administration_DeleteConfirmed_Valid()
        {
            //Arrange            
            _apiReponse.Message = "Permission deleted successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;
            //Get Actual Count
            _PermissionsVMListCount = _PermissionsVMList.Count;
            // Get that PermissionsVM for id : 2
            PermissionsVM testPermissionsVM = mockPermissionFactory.MockFindById(_PermissionsVMList, 4);

            var mockPermissionFactoryResult = new MockPermissionFactory().MockDelete(_apiReponse, _PermissionsVMList, 4);

            _administrationController = new AdministrationController( mockPermissionFactoryResult.Result.Object,_mockLogger.Object, "test");

            //Act  
            var actionResult = _administrationController.UserDeleteConfirmed(4);
            var actionResponse = actionResult.Result as RedirectToActionResult;



            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreNotEqual(_PermissionsVMList.Count, _PermissionsVMListCount); // Test count is not equal before and after delete.
        }


        #endregion
    }
}

