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
    public class RevealEventControllerTest
    {

        private RevealEventController _revealEventController;
        private Mock<ILogger<RevealEventController>> _mockLogger;
        private MockRevealEventFactory mockRevealFactory;

        private RevealEventVM _mockResponse;
        private int _RevealEventVMListCount;
        private int _RevealEventVMListMaxId;

        private List<RevealEventVM> _RevealEventVMList;
        private List<PermissionsVM> _PermissionsVMList;
        private ApiResponse _apiReponse;


        [SetUp]
        public void TestInitialize()

        {
            //Arrange
            #region "Arrange"

            // Creating Dummy Signup
            var signUp = new SignUpEventVM
            {
                RevealEventId = 4,
                SignUpEventId = 1
            };
            //Creating Dummy RevealEventVM List
            _RevealEventVMList = new List<RevealEventVM>
                    {
                        new RevealEventVM {RevealEventId = 1,
                                                Name = "Test Reveal Event 1",
                                                EventDate = DateTime.Today,
                                                Location = "Pajule",
                                                TypeOfReveal = "Online",                                                
                                                WorkflowStatusId = 1},
                        new RevealEventVM {RevealEventId = 2,
                                                Name = "Test Reveal Event 2",
                                                EventDate = DateTime.Today,
                                                Location = "Pajule",
                                                TypeOfReveal = "Online",
                                                WorkflowStatusId = 2,

                        },
                        new RevealEventVM {RevealEventId = 3,
                                                 Name = "Test Reveal Event 3",
                                                EventDate = DateTime.Today,
                                                Location = "Pajule",
                                                TypeOfReveal = "Online",
                                                WorkflowStatusId = 3},
                        new RevealEventVM {RevealEventId = 4,
                                                Name = "Test Reveal Event 4",
                                                EventDate = DateTime.Today,
                                                Location = "Pajule",
                                                TypeOfReveal = "Online",
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
            //  _mockLogger = new Mock<ILogger<RevealEventController>>();

            // Mock the API Response 
            _mockResponse = new RevealEventVM();

            #endregion

            //Setup mockRevealFactory
            mockRevealFactory = new MockRevealEventFactory();

            ////setup for permission
            //var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //logger
            _mockLogger = new Mock<ILogger<RevealEventController>>();

            // ApiResponse
            _apiReponse = new ApiResponse();
        }


        #region "Index Action"
        /// <summary>
        /// Can we Load all RevealEvents?
        /// </summary>
        [Test]
        public void RevealEvent_Index_Valid()
        {
            //Arrange
            // Loading all RevealEventVM
            var mockRevealFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RevealEventController
            _revealEventController = new RevealEventController(mockRevealFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _revealEventController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<RevealEventVM>;

            //Get Actual Count
            _RevealEventVMListCount = _RevealEventVMList.Count;

            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

            Assert.IsInstanceOf(typeof(List<RevealEventVM>), dataResult); //Test if returns ok response               
            Assert.AreEqual(dataResult.Count, _RevealEventVMListCount); //Test if returns equal count               
        }

        /// <summary>
        /// If we have no records in RevealEvents?
        /// </summary>
        [Test]
        public void RevealEvent_Index_NoRecords()
        {

            //Arrange
            // Loading all RevealEventVM
            var mockRevealFactoryResult = new MockRevealEventFactory().MockLoadList(null);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //Setup RevealEventController
            _revealEventController = new RevealEventController(mockRevealFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _revealEventController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<RevealEventVM>;

            //Get Actual Count
            _RevealEventVMListCount = _RevealEventVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult.Count, 0); //Test if returns equal count             

        }

        #endregion 

        #region "Create Action "

        /// <summary>
        /// while we Load a Reveal Event By Id for edit action?
        /// </summary>
        [Test]
        public void RevealEvent_CreateLoad_Valid()
        {
            //Arrange
            // Create a new RevealEventModel, not I do not supply an id
            RevealEventVM newRevealEventVM = new RevealEventVM
            {
                //set dates to today otherwsie they are default null date
                EventDate = DateTime.Today,
                WorkflowStatusId = 1,
                Create = true
            };

            var mockRevealFactoryResult = new MockRevealEventFactory().MockLoad(newRevealEventVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _revealEventController = new RevealEventController(mockRevealFactoryResult.Result.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _revealEventController.Create(0);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as RevealEventVM;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
        }

        ///// <summary>
        ///// Can we insert a new RevealEventVM?
        ///// </summary>        
        [Test]
        public void RevealEvent_CreateSave_Valid()
        {
            //Arrange
            //Get Actual Count
            _RevealEventVMListCount = _RevealEventVMList.Count;

            //Get Max ID from List
            _RevealEventVMListMaxId = Convert.ToInt32(_RevealEventVMList.Max(x => x.RevealEventId).ToString()) + 1;

            // Create a new RevealEventModel, not I do not supply an id
            RevealEventVM newRevealEventVM = new RevealEventVM
            {
                RevealEventId = _RevealEventVMListMaxId,
                Name = "Test Reveal Event " + _RevealEventVMListMaxId.ToString(),
                EventDate = DateTime.Today,
                Location = "Pajule",
                TypeOfReveal = "Online",                
                WorkflowStatusId = 1
            };
            _apiReponse.Message = "Reveal event created successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            var mockRevealFactoryResult = new MockRevealEventFactory().MockCreate(_apiReponse, _RevealEventVMList, newRevealEventVM);
            _revealEventController = new RevealEventController(
                                      mockRevealFactoryResult.Result.Object,
                                       mockPermissionFactoryResult.Result.Object,_mockLogger.Object);


            //ACT
            #region "ACT"

            // try saving our new RevealEventVM
            var actionResult = _revealEventController.Create(newRevealEventVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            #endregion


            //Get Expected Count
            var expectedCount = _RevealEventVMList.Count;

            // Verify that our new RevealEventVM has been created
            RevealEventVM testRevealEventVM = mockRevealFactory.MockFindById(_RevealEventVMList, _RevealEventVMListMaxId);

            // Assert
            #region "Assert"            
             Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response    
            // Check both actual and expected count is same after insert        
            Assert.AreEqual(expectedCount, _RevealEventVMListCount + 1); // Verify the expected Number post-insert
            Assert.AreEqual(_RevealEventVMListMaxId, testRevealEventVM.RevealEventId); // Verify it has the expected RevealEventVM Id
            #endregion
        }

        /// <summary>
        /// Can we insert a new RevealEventVM?
        /// </summary>
        [Test]
        public void RevealEvent_CreateSave_DuplicateData()
        {
            //Arrange
            //Get Actual Count
            _RevealEventVMListCount = _RevealEventVMList.Count;

            //Get Max ID from List
            _RevealEventVMListMaxId = Convert.ToInt32(_RevealEventVMList.Max(x => x.RevealEventId).ToString()) + 1;

            // Create a new RevealEventModel, not I do not supply an id
            RevealEventVM newRevealEventVM = new RevealEventVM
            {
                RevealEventId = _RevealEventVMListMaxId,
                Name = "Test Reveal Event 1",
                EventDate = DateTime.Today,
                Location = "Pajule",
                TypeOfReveal = "Online",
                WorkflowStatusId = 1
            };
            _apiReponse.Message = "Failed to create Reveal event";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            var mockRevealFactoryResult = new MockRevealEventFactory().MockCreate(_apiReponse, _RevealEventVMList, newRevealEventVM);
            _revealEventController = new RevealEventController(
                                      mockRevealFactoryResult.Result.Object,
                                       mockPermissionFactoryResult.Result.Object,_mockLogger.Object);


            //ACT
            #region "ACT"

            // try saving our new RevealEventVM
            var actionResult = _revealEventController.Create(newRevealEventVM);
            var actionResponse = actionResult.Result as ViewResult;
            #endregion


            //Get Expected Count
            var expectedCount = _RevealEventVMList.Count;


            // Assert
            #region "Assert"            
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response      
            // Check both actual and expected count is same after insert        
            Assert.AreNotEqual(expectedCount, _RevealEventVMListCount + 1); // Verify the expected Number post-insert
            #endregion
        }
        #endregion

        #region "Edit Action "

        /// <summary>
        /// while we Load a Reveal Event By Id for edit action?
        /// </summary>
        [Test]
        public void RevealEvent_EditLoad_Valid()
        {
            //Arrange

            // Get that RevealEventVM for id : 2
            RevealEventVM testRevealEventVM = mockRevealFactory.MockFindById(_RevealEventVMList, 2);

            var mockRevealFactoryResult = new MockRevealEventFactory().MockLoad(testRevealEventVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _revealEventController = new RevealEventController(mockRevealFactoryResult.Result.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _revealEventController.Edit(2);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as RevealEventVM;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsNotNull(_mockResponse); // Test if null
            Assert.IsInstanceOf(typeof(RevealEventVM), _mockResponse); //Test if returns ok response   
        }

        /// <summary>
        /// When we Load a Reveal Event By Id for edit action if returns http not found result?
        /// </summary>
        [Test]
        public void RevealEvent_EditLoad_InValid()
        {
            //Arrange

            // Get that RevealEventVM for id : 2
            RevealEventVM testRevealEventVM = mockRevealFactory.MockFindById(_RevealEventVMList, 5);

            var mockRevealFactoryResult = new MockRevealEventFactory().MockLoad(testRevealEventVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            _revealEventController = new RevealEventController(mockRevealFactoryResult.Result.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _revealEventController.Edit(5);
            var actionResponse = actionResult.Result as NotFoundResult;

            //Assert  
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResponse); //Test if returns ok response  

        }

        /// <summary>
        /// When we edit a Reveal Event By Id using edit action if returns ok result?
        /// </summary>
        [Test]
        public void RevealEvent_EditSave_Valid()
        {
            //Arrange            
            _apiReponse.Message = "Reveal event updated successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            // Get that RevealEventVM for id : 2
            RevealEventVM testRevealEventVM = mockRevealFactory.MockFindById(_RevealEventVMList, 2);

            // Change one of its properties
            testRevealEventVM.Name = "Unit Test Data";
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            var mockRevealFactoryResult = new MockRevealEventFactory().MockUpdate(_apiReponse, _RevealEventVMList, testRevealEventVM);
            _revealEventController = new RevealEventController(mockRevealFactoryResult.Result.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _revealEventController.Edit(testRevealEventVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            //Assert  
             Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreEqual("Unit Test Data", mockRevealFactory.MockFindById(_RevealEventVMList, 2).Name);
        }

        /// <summary>
        /// When we edit a Reveal Event By Id using edit action if its not update?
        /// </summary>
        [Test]
        public void RevealEvent_EditSave_DuplicateData()
        {
            //Arrange            
            _apiReponse.Message = "Reveal event updated successfully";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            // Get that RevealEventVM for id : 2
            RevealEventVM testRevealEventVM = mockRevealFactory.MockFindById(_RevealEventVMList, 3);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            // Change one of its properties

            testRevealEventVM.Name = "Test Reveal Event 1";
            testRevealEventVM.EventDate = DateTime.Today;
            testRevealEventVM.Location = "Pajule";            

            var mockRevealFactoryResult = new MockRevealEventFactory().MockUpdate(_apiReponse, _RevealEventVMList, testRevealEventVM);
            _revealEventController = new RevealEventController(mockRevealFactoryResult.Result.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _revealEventController.Edit(testRevealEventVM);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            //  Assert.AreNotEqual("Test Reveal Event 1", mockRevealFactory.MockFindById(_RevealEventVMList, 3).PartyName);

        }
        #endregion

        #region "Delete Action


        /// <summary>
        /// while we Load a Reveal Event By Id for delete action?
        /// </summary>
        [Test]
        public void RevealEvent_DeleteLoad_SignupExists()
        {
            //Arrange
            //Get Actual Count
            _RevealEventVMListCount = _RevealEventVMList.Count;
            // Get that RevealEventVM for id : 2
            RevealEventVM testRevealEventVM = mockRevealFactory.MockFindById(_RevealEventVMList, 4);

            var mockRevealFactoryResult = new MockRevealEventFactory().MockDeleteLoad(testRevealEventVM, true, 4);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _revealEventController = new RevealEventController(mockRevealFactoryResult.Result.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _revealEventController.Delete(4);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

        }


        /// <summary>
        /// while we Load a Reveal Event By Id for delete action?
        /// </summary>
        [Test]
        public void RevealEvent_DeleteLoad_SignupNotExists()
        {
            //Arrange
            //Get Actual Count
            _RevealEventVMListCount = _RevealEventVMList.Count;
            // Get that RevealEventVM for id : 2
            RevealEventVM testRevealEventVM = mockRevealFactory.MockFindById(_RevealEventVMList, 2);

            var mockRevealFactoryResult = new MockRevealEventFactory().MockDeleteLoad(testRevealEventVM, false, 2);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _revealEventController = new RevealEventController(mockRevealFactoryResult.Result.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _revealEventController.DeleteConfirmed(2);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

        }
        /// <summary>
        ///  we delete a RevealEventVM if there is no registeration exists?
        /// </summary>
        [Test]
        public void RevealEvent_DeleteConfirmed_Valid()
        {
            //Arrange            
            _apiReponse.Message = "Reveal event deleted successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;
            //Get Actual Count
            _RevealEventVMListCount = _RevealEventVMList.Count;
            // Get that RevealEventVM for id : 2
            RevealEventVM testRevealEventVM = mockRevealFactory.MockFindById(_RevealEventVMList, 4);

            var mockRevealFactoryResult = new MockRevealEventFactory().MockDelete(_apiReponse, _RevealEventVMList, 4);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            _revealEventController = new RevealEventController(mockRevealFactoryResult.Result.Object,  mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _revealEventController.DeleteConfirmed(4);
            var actionResponse = actionResult.Result as RedirectToActionResult;



            //Assert  
             Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreNotEqual(_RevealEventVMList.Count, _RevealEventVMListCount); // Test count is not equal before and after delete.
        }


        #endregion
    }
}
