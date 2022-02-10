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

namespace United4Admin.UnitTests.Controllers
{
    [TestFixture]
    public class ChoosingEventControllerTest
    {

        private ChoosingEventController _choosingEventController;
        //private Mock<ILogger<ChoosingEventController>> _mockLogger;
        private MockChoosingFactory mockChoosingFactory;
              
        private ChoosingPartyVM _mockResponse;
        private int _ChoosingPartyVMListCount;
        private int _ChoosingPartyVMListMaxId;     
        
        private List<ChoosingPartyVM> _ChoosingPartyVMList;
        private List<PermissionsVM> _PermissionsVMList;
        private ApiResponse _apiReponse;
        private Mock<ILogger<ChoosingEventController>> _mockLogger;

        [SetUp]
        public void SetUp()
        
        {
            //Arrange
            #region "Arrange"

            // Creating Dummy Signup
            var signUp = new SignUpEventVM
            {
              ChoosingPartyId =4,
              SignUpEventId =1
            };
            //Creating Dummy ChoosingPartyVM List
            _ChoosingPartyVMList = new List<ChoosingPartyVM>
                    {
                        new ChoosingPartyVM {ChoosingPartyId = 1,
                                                PartyName = "Test Choosing Event 1",
                                                PartyDate = DateTime.Today,
                                                Location = "Pajule",
                                                Country = "Uganda",
                                                HorizonId = 123456,
                                                WorkflowStatusId = 1},
                        new ChoosingPartyVM {ChoosingPartyId = 2,
                                                PartyName = "Test Choosing Event 2",
                                                PartyDate = DateTime.Today,
                                                Location = "Sisian",
                                                Country = "Armenia",
                                                HorizonId = 567890,
                                                WorkflowStatusId = 2,
                                                
                        },
                        new ChoosingPartyVM {ChoosingPartyId = 3,
                                                PartyName = "Test Choosing Event 3",
                                                PartyDate = DateTime.Today,
                                                Location = "Tegloma",
                                                Country = "Sierra Leone",
                                                HorizonId = 654321,
                                                WorkflowStatusId = 3},
                        new ChoosingPartyVM {ChoosingPartyId = 4,
                                                PartyName = "Test Choosing Event 4",
                                                PartyDate = DateTime.Today,
                                                Location = "Tegloma",
                                                Country = "Sierra Leone",
                                                HorizonId = 543553,
                                                WorkflowStatusId = 3}
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
            //  _mockLogger = new Mock<ILogger<ChoosingEventController>>();

            // Mock the API Response 
            _mockResponse = new ChoosingPartyVM();

            //logger
            _mockLogger = new Mock<ILogger<ChoosingEventController>>();

            #endregion

            //Setup mockChoosingFactory
            mockChoosingFactory = new MockChoosingFactory();

            ////setup for permission
            //var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
             
            // ApiResponse
            _apiReponse = new ApiResponse();
        }


        #region "Index Action"
        /// <summary>
        /// Can we Load all choosingEvents?
        /// </summary>
        [Test]
        public void ChoosingEvent_Index_Valid()
        {
            //Arrange
            // Loading all ChoosingPartyVM
            var mockChoosingFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup ChoosingEventController
            _choosingEventController = new ChoosingEventController(mockChoosingFactoryResult.Result.Object,                                            
                                            mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _choosingEventController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<ChoosingPartyVM>;

            //Get Actual Count
            _ChoosingPartyVMListCount = _ChoosingPartyVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response 
            Assert.IsInstanceOf(typeof(List<ChoosingPartyVM>), dataResult); //Test if returns ok response               
            Assert.AreEqual(dataResult.Count, _ChoosingPartyVMListCount); //Test if returns equal count               
        }

        /// <summary>
        /// If we have no records in choosingEvents?
        /// </summary>
        [Test]
        public void ChoosingEvent_Index_NoRecords()
        {

            //Arrange
            // Loading all ChoosingPartyVM
            var mockChoosingFactoryResult = new MockChoosingFactory().MockLoadList(null);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //Setup ChoosingEventController
            _choosingEventController = new ChoosingEventController(mockChoosingFactoryResult.Result.Object,                                           
                                            mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _choosingEventController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<ChoosingPartyVM>;

            //Get Actual Count
            _ChoosingPartyVMListCount = _ChoosingPartyVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response 
            Assert.AreEqual(dataResult.Count, 0); //Test if returns equal count             
            
        }

        #endregion 

        #region "Create Action "

        /// <summary>
        /// while we Load a Choosing Event By Id for edit action?
        /// </summary>
        [Test]
        public void ChoosingEvent_CreateLoad_Valid()
        {
            //Arrange
            // Create a new ChoosingEventModel, not I do not supply an id
            ChoosingPartyVM newChoosingPartyVM = new ChoosingPartyVM
            {
                //set dates to today otherwsie they are default null date
                PartyDate = DateTime.Today,
                WorkflowStatusId = 1,
                Create = true
            };

            var mockChoosingFactoryResult = new MockChoosingFactory().MockLoad(newChoosingPartyVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _choosingEventController = new ChoosingEventController(mockChoosingFactoryResult.Result.Object,mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _choosingEventController.Create(0);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as ChoosingPartyVM;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response  

        }

        ///// <summary>
        ///// Can we insert a new ChoosingPartyVM?
        ///// </summary>
        [Test]
        public void ChoosingEvent_CreateSave_Valid()
        {
            //Arrange
            //Get Actual Count
            _ChoosingPartyVMListCount = _ChoosingPartyVMList.Count;

            //Get Max ID from List
            _ChoosingPartyVMListMaxId = Convert.ToInt32(_ChoosingPartyVMList.Max(x => x.ChoosingPartyId).ToString()) + 1;

            // Create a new ChoosingEventModel, not I do not supply an id
            ChoosingPartyVM newChoosingPartyVM = new ChoosingPartyVM
            {
                ChoosingPartyId = _ChoosingPartyVMListMaxId,
                PartyName = "Test Choosing Event " + _ChoosingPartyVMListMaxId.ToString(),
                PartyDate = DateTime.Today,
                Location = "India",
                Country = "Chennai",
                HorizonId = 898653,
                WorkflowStatusId = 1
            };
            _apiReponse.Message = "Choosing event created successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            var mockChoosingFactoryResult = new MockChoosingFactory().MockCreate(_apiReponse, _ChoosingPartyVMList, newChoosingPartyVM);
            _choosingEventController = new ChoosingEventController(
                                      mockChoosingFactoryResult.Result.Object,                                   
                                      mockPermissionFactoryResult.Result.Object,_mockLogger.Object);


            //ACT
            #region "ACT"

            // try saving our new ChoosingPartyVM
            var actionResult = _choosingEventController.Create(newChoosingPartyVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;
          

            #endregion


            //Get Expected Count
            var expectedCount = _ChoosingPartyVMList.Count;

            // Verify that our new ChoosingPartyVM has been created
            ChoosingPartyVM testChoosingPartyVM = mockChoosingFactory.MockFindById(_ChoosingPartyVMList, _ChoosingPartyVMListMaxId);

            // Assert
            #region "Assert"            
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response    
            // Check both actual and expected count is same after insert        
            Assert.AreEqual(expectedCount, _ChoosingPartyVMListCount + 1); // Verify the expected Number post-insert
            Assert.AreEqual(_ChoosingPartyVMListMaxId, testChoosingPartyVM.ChoosingPartyId); // Verify it has the expected ChoosingPartyVM Id
            #endregion
        }

        /// <summary>
        /// Can we insert a new ChoosingPartyVM?
        /// </summary>
        [Test]
        public void ChoosingEvent_CreateSave_DuplicateData()
        {
            //Arrange
            //Get Actual Count
            _ChoosingPartyVMListCount = _ChoosingPartyVMList.Count;

            //Get Max ID from List
            _ChoosingPartyVMListMaxId = Convert.ToInt32(_ChoosingPartyVMList.Max(x => x.ChoosingPartyId).ToString()) + 1;

            // Create a new ChoosingEventModel, not I do not supply an id
            ChoosingPartyVM newChoosingPartyVM = new ChoosingPartyVM
            {
                ChoosingPartyId = _ChoosingPartyVMListMaxId,
                PartyName = "Test Choosing Event 1",
                PartyDate = DateTime.Today,
                Location = "Pajule",
                Country = "Uganda",
                HorizonId = 123456,
                WorkflowStatusId = 1
            };
            _apiReponse.Message = "Failed to create Choosing event";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            var mockChoosingFactoryResult = new MockChoosingFactory().MockCreate(_apiReponse, _ChoosingPartyVMList, newChoosingPartyVM);
            _choosingEventController = new ChoosingEventController(
                                      mockChoosingFactoryResult.Result.Object,                                   
                                      mockPermissionFactoryResult.Result.Object,_mockLogger.Object);


            //ACT
            #region "ACT"

            // try saving our new ChoosingPartyVM
            var actionResult = _choosingEventController.Create(newChoosingPartyVM);
            var actionResponse = actionResult.Result as ViewResult;
            #endregion


            //Get Expected Count
            var expectedCount = _ChoosingPartyVMList.Count;


            // Assert
            #region "Assert"          
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                 
            // Check both actual and expected count is same after insert        
            Assert.AreNotEqual(expectedCount, _ChoosingPartyVMListCount + 1); // Verify the expected Number post-insert
            #endregion
        }
#endregion

        #region "Edit Action "

        /// <summary>
        /// while we Load a Choosing Event By Id for edit action?
        /// </summary>
        [Test]
        public void ChoosingEvent_EditLoad_Valid()
        {
            //Arrange

            // Get that ChoosingPartyVM for id : 2
            ChoosingPartyVM testChoosingPartyVM = mockChoosingFactory.MockFindById(_ChoosingPartyVMList, 2);

            var mockChoosingFactoryResult = new MockChoosingFactory().MockLoad(testChoosingPartyVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _choosingEventController = new ChoosingEventController(mockChoosingFactoryResult.Result.Object,mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _choosingEventController.Edit(2);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as ChoosingPartyVM;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response  
            
            Assert.IsNotNull(_mockResponse); // Test if null
            Assert.IsInstanceOf(typeof(ChoosingPartyVM), _mockResponse); //Test if returns ok response   
        }

        /// <summary>
        /// When we Load a Choosing Event By Id for edit action if returns http not found result?
        /// </summary>
        [Test]
        public void ChoosingEvent_EditLoad_InValid()
        {
            //Arrange

            // Get that ChoosingPartyVM for id : 2
            ChoosingPartyVM testChoosingPartyVM = mockChoosingFactory.MockFindById(_ChoosingPartyVMList, 5);

            var mockChoosingFactoryResult = new MockChoosingFactory().MockLoad(testChoosingPartyVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            _choosingEventController = new ChoosingEventController(mockChoosingFactoryResult.Result.Object,mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _choosingEventController.Edit(5);
            var actionResponse = actionResult.Result as NotFoundResult;

            //Assert           
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResponse); //Test if returns ok response  

        }

        /// <summary>
        /// When we edit a Choosing Event By Id using edit action if returns ok result?
        /// </summary>
        [Test]
        public void ChoosingEvent_EditSave_Valid()
        {
            //Arrange            
            _apiReponse.Message = "Choosing event updated successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            // Get that ChoosingPartyVM for id : 2
            ChoosingPartyVM testChoosingPartyVM = mockChoosingFactory.MockFindById(_ChoosingPartyVMList, 2);

            // Change one of its properties
            testChoosingPartyVM.PartyName = "Unit Test Data";
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            var mockChoosingFactoryResult = new MockChoosingFactory().MockUpdate(_apiReponse, _ChoosingPartyVMList, testChoosingPartyVM);
            _choosingEventController = new ChoosingEventController(mockChoosingFactoryResult.Result.Object,mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _choosingEventController.Edit(testChoosingPartyVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreEqual("Unit Test Data", mockChoosingFactory.MockFindById(_ChoosingPartyVMList, 2).PartyName);
        }

        /// <summary>
        /// When we edit a Choosing Event By Id using edit action if its not update?
        /// </summary>
        [Test]
        public void ChoosingEvent_EditSave_DuplicateData()
        {
            //Arrange            
            _apiReponse.Message = "Choosing event updated successfully";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            // Get that ChoosingPartyVM for id : 2
            ChoosingPartyVM testChoosingPartyVM = mockChoosingFactory.MockFindById(_ChoosingPartyVMList, 3);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            // Change one of its properties
            
            testChoosingPartyVM.PartyName = "Test Choosing Event 1";
            testChoosingPartyVM.PartyDate = DateTime.Today;
            testChoosingPartyVM.Location = "Pajule";
            testChoosingPartyVM.Country = "Uganda";
                                              
            var mockChoosingFactoryResult = new MockChoosingFactory().MockUpdate(_apiReponse, _ChoosingPartyVMList, testChoosingPartyVM);
            _choosingEventController = new ChoosingEventController(mockChoosingFactoryResult.Result.Object,mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _choosingEventController.Edit(testChoosingPartyVM);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response  
          //  Assert.AreNotEqual("Test Choosing Event 1", mockChoosingFactory.MockFindById(_ChoosingPartyVMList, 3).PartyName);

        }
        #endregion

        #region "Delete Action


        /// <summary>
        /// while we Load a Choosing Event By Id for delete action?
        /// </summary>
        [Test]
        public void ChoosingEvent_DeleteLoad_SignupExists()
        {
            //Arrange
            //Get Actual Count
            _ChoosingPartyVMListCount = _ChoosingPartyVMList.Count;
            // Get that ChoosingPartyVM for id : 2
            ChoosingPartyVM testChoosingPartyVM = mockChoosingFactory.MockFindById(_ChoosingPartyVMList, 4);

            var mockChoosingFactoryResult = new MockChoosingFactory().MockDeleteLoad(testChoosingPartyVM,true, 4);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _choosingEventController = new ChoosingEventController(mockChoosingFactoryResult.Result.Object,mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _choosingEventController.Delete(4);
            var actionResponse = actionResult.Result as ViewResult;
            

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response  
          
        }


        /// <summary>
        /// while we Load a Choosing Event By Id for delete action?
        /// </summary>
        [Test]
        public void ChoosingEvent_DeleteLoad_SignupNotExists()
        {
            //Arrange
            //Get Actual Count
            _ChoosingPartyVMListCount = _ChoosingPartyVMList.Count;
            // Get that ChoosingPartyVM for id : 2
            ChoosingPartyVM testChoosingPartyVM = mockChoosingFactory.MockFindById(_ChoosingPartyVMList, 2);

            var mockChoosingFactoryResult = new MockChoosingFactory().MockDeleteLoad(testChoosingPartyVM, false, 2);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _choosingEventController = new ChoosingEventController(mockChoosingFactoryResult.Result.Object,mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _choosingEventController.DeleteConfirmed(2);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response  
          
        }
        /// <summary>
        ///  we delete a ChoosingPartyVM if there is no registeration exists?
        /// </summary>
        [Test]
        public void ChoosingEvent_DeleteConfirmed_Valid()
        {
            //Arrange            
            _apiReponse.Message = "Choosing event deleted successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;
            //Get Actual Count
            _ChoosingPartyVMListCount = _ChoosingPartyVMList.Count;
            // Get that ChoosingPartyVM for id : 2
            ChoosingPartyVM testChoosingPartyVM = mockChoosingFactory.MockFindById(_ChoosingPartyVMList, 4);

            var mockChoosingFactoryResult = new MockChoosingFactory().MockDelete(_apiReponse, _ChoosingPartyVMList, 4);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _choosingEventController = new ChoosingEventController(mockChoosingFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _choosingEventController.DeleteConfirmed(4);
            var actionResponse = actionResult.Result as RedirectToActionResult;



            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreNotEqual(_ChoosingPartyVMList.Count, _ChoosingPartyVMListCount); // Test count is not equal before and after delete.
        }

        
        #endregion
    }
}
