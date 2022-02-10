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
    public class RegistrationEventControllerTest
    {
        private RegistrationEventController _signUpEventController;
        //private Mock<ILogger<RegistrationEventController>> _mockLogger;
        private MockSignupEventFactory mockSignupEventFactory;
        private MockRevealEventFactory mockRevealEventFactory;
        private MockChoosingFactory mockChoosingFactory;

        private SignUpEventVM _mockResponse;
        private int _SignUpEventVMListCount;
        private int _SignUpEventVMListMaxId;

        private List<SignUpEventVM> _SignUpEventVMList;
        private List<RevealEventVM> _RevealEventVMList;
        private List<ChoosingPartyVM> _ChoosingPartyVMList;
        private List<PermissionsVM> _PermissionsVMList;
        private ApiResponse _apiReponse;
        private Mock<ILogger<RegistrationEventController>> _mockLogger;

        [SetUp]
        public void TestInitialize()

        {
            //Arrange
            #region "Arrange"

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


            //Creating dummy RevealEvent
            _RevealEventVMList = new List<RevealEventVM>
                    {
                        new RevealEventVM {RevealEventId = 1,
                                                Name = "Test SignUp Event 1",
                                                EventDate = DateTime.Today,
                                                Location = "Pajule",
                                                TypeOfReveal = "Online",
                                                WorkflowStatusId = 1},
                        new RevealEventVM {RevealEventId = 2,
                                                Name = "Test SignUp Event 2",
                                                EventDate = DateTime.Today,
                                                Location = "Pajule",
                                                TypeOfReveal = "Online",
                                                WorkflowStatusId = 2,

                        },
                        new RevealEventVM {RevealEventId = 3,
                                                 Name = "Test SignUp Event 3",
                                                EventDate = DateTime.Today,
                                                Location = "Pajule",
                                                TypeOfReveal = "Online",
                                                WorkflowStatusId = 3},
                        new RevealEventVM {RevealEventId = 4,
                                                Name = "Test SignUp Event 4",
                                                EventDate = DateTime.Today,
                                                Location = "Pajule",
                                                TypeOfReveal = "Online",
                                                WorkflowStatusId = 3}
                    };

            //Creating Dummy SignUpEventVM List
            _SignUpEventVMList = new List<SignUpEventVM>
                    {
                        new SignUpEventVM {SignUpEventId = 1,
                                            EventName = "Test Registration Event 1",
                                            PublishDate = DateTime.Today,
                                            ClosedDate = DateTime.Today.AddDays(14),
                                            EventDate = DateTime.Today.AddDays(7),
                                            Location = "Edinburgh",
                                            TypeOfRegistration = "WV Chosen Event",
                                            ShortURL = "TEST1",
                                            SpecificChoosingEvent = "Yes",
                                            ChoosingPartyId = 1,
                                            RevealEventId = 1,
                                            CampaignCode = "TEST123456",
                                            WorkflowStatusId = 1,
                                            WVPhotoBooth = true},
                        new SignUpEventVM {SignUpEventId = 2,
                                            EventName = "Test Registration Event 2",
                                            PublishDate = DateTime.Today,
                                            ClosedDate = DateTime.Today.AddDays(14),
                                            EventDate = DateTime.Today.AddDays(7),
                                            Location = "Edinburgh",
                                            TypeOfRegistration = "WV Chosen Event",
                                            ShortURL = "TEST2",
                                            SpecificChoosingEvent = "Yes",
                                            ChoosingPartyId = 1,
                                            RevealEventId = 1,
                                            CampaignCode = "TEST123456",
                                            WorkflowStatusId = 1,
                                            WVPhotoBooth = true},

                         new SignUpEventVM {SignUpEventId = 3,
                                            EventName = "Test Registration Event 3",
                                            PublishDate = DateTime.Today,
                                            ClosedDate = DateTime.Today.AddDays(14),
                                            EventDate = DateTime.Today.AddDays(7),
                                            Location = "Edinburgh",
                                            TypeOfRegistration = "WV Chosen Event",
                                            ShortURL = "TEST3",
                                            SpecificChoosingEvent = "Yes",
                                            ChoosingPartyId = 1,
                                            RevealEventId = 1,
                                            CampaignCode = "TEST123456",
                                            WorkflowStatusId = 1,
                                            WVPhotoBooth = true},
                         new SignUpEventVM {SignUpEventId = 4,
                                            EventName = "Test Registration Event 4",
                                            PublishDate = DateTime.Today,
                                            ClosedDate = DateTime.Today.AddDays(14),
                                            EventDate = DateTime.Today.AddDays(7),
                                            Location = "Edinburgh",
                                            TypeOfRegistration = "WV Chosen Event",
                                            ShortURL = "TEST4",
                                            SpecificChoosingEvent = "Yes",
                                            ChoosingPartyId = 1,
                                            RevealEventId = 1,
                                            CampaignCode = "TEST123456",
                                            WorkflowStatusId = 1,
                                            WVPhotoBooth = true},
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
            //  _mockLogger = new Mock<ILogger<RegistrationEventController>>();

            // Mock the API Response 
            _mockResponse = new SignUpEventVM();

            #endregion

            //Setup mockSignupEventFactory
            mockSignupEventFactory = new MockSignupEventFactory();

            ////setup for permission
            //var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //logger
            _mockLogger = new Mock<ILogger<RegistrationEventController>>();
            // ApiResponse
            _apiReponse = new ApiResponse();
        }


        #region "Index Action"
        /// <summary>
        /// Can we Load all SignUpEvents?
        /// </summary>
        [Test]
        public void SignUpEvent_Index_Valid()
        {
            //Arrange
            // Loading all SignUpEventVM
            var mockSignupEventFactoryResult = new MockSignupEventFactory().MockLoadList(_SignUpEventVMList);

            // Loading all choosingparyvm
            var mockChossingEventFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);

            // Loading all revealeventVM
            var mockRevealEventFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RegistrationEventController
            _signUpEventController = new RegistrationEventController(mockSignupEventFactoryResult.Result.Object,
                mockChossingEventFactoryResult.Result.Object, mockRevealEventFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _signUpEventController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<SignUpEventVM>;

            //Get Actual Count
            _SignUpEventVMListCount = _SignUpEventVMList.Count;

            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

            Assert.IsInstanceOf(typeof(List<SignUpEventVM>), dataResult); //Test if returns ok response               
            Assert.AreEqual(dataResult.Count, _SignUpEventVMListCount); //Test if returns equal count               
        }

        /// <summary>
        /// If we have no records in SignUpEvents?
        /// </summary>
        [Test]
        public void SignUpEvent_Index_NoRecords()
        {

            //Arrange
            // Loading all SignUpEventVM
            var mockSignupEventFactoryResult = new MockSignupEventFactory().MockLoadList(null);

            // Loading all choosingparyvm
            var mockChossingEventFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);

            // Loading all revealeventVM
            var mockRevealEventFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RegistrationEventController
            _signUpEventController = new RegistrationEventController(mockSignupEventFactoryResult.Result.Object,
                mockChossingEventFactoryResult.Result.Object, mockRevealEventFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _signUpEventController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<SignUpEventVM>;

            //Get Actual Count
            _SignUpEventVMListCount = _SignUpEventVMList.Count;

            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult.Count, 0); //Test if returns equal count             

        }

        #endregion 

        #region "Create Action "

        /// <summary>
        /// while we Load a SignUp Event By Id for edit action?
        /// </summary>
        [Test]
        public void SignUpEvent_CreateLoad_Valid()
        {
            //Arrange
            // Create a new RevealEventModel, not I do not supply an id
            SignUpEventVM newSignUpEventVM = new SignUpEventVM
            {
                //set dates to today otherwsie they are default null date
                ClosedDate = DateTime.Today,
                EventDate = DateTime.Today,
                PublishDate = DateTime.Today,
                WorkflowStatusId =1,
                Create = true
            };

            // Loading all SignUpEventVM
            var mockSignupEventFactoryResult = new MockSignupEventFactory().MockLoad(newSignUpEventVM);

            // Loading all choosingparyvm
            var mockChossingEventFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);

            // Loading all revealeventVM
            var mockRevealEventFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RegistrationEventController
            _signUpEventController = new RegistrationEventController(mockSignupEventFactoryResult.Result.Object,
                mockChossingEventFactoryResult.Result.Object, mockRevealEventFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _signUpEventController.Create(0);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as SignUpEventVM;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
        }

        ///// <summary>
        ///// Can we insert a new SignUpEventVM?
        ///// </summary>
        [Test]
        public void SignUpEvent_CreateSave_Valid()
        {
            //Arrange
            //Get Actual Count
            _SignUpEventVMListCount = _SignUpEventVMList.Count;

            //Get Max ID from List
            _SignUpEventVMListMaxId = Convert.ToInt32(_SignUpEventVMList.Max(x => x.SignUpEventId).ToString()) + 1;

            // Create a new SignUpEventModel, not I do not supply an id
            SignUpEventVM newSignUpEventVM = new SignUpEventVM
            {
                SignUpEventId = _SignUpEventVMListMaxId,
                EventName = "Test Registration Event 15",
                PublishDate = DateTime.Today,
                ClosedDate = DateTime.Today.AddDays(14),
                EventDate = DateTime.Today.AddDays(7),
                Location = "Edinburgh",
                TypeOfRegistration = "WV Chosen Event",
                ShortURL = "TEST",
                SpecificChoosingEvent = "Yes",
                ChoosingPartyId = 1,
                RevealEventId = 1,
                CampaignCode = "TEST123456",
                WorkflowStatusId = 1,
                WVPhotoBooth = true
            };
            _apiReponse.Message = "SignUp Event created successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            // Loading all SignUpEventVM
            var mockSignupEventFactoryResult = new MockSignupEventFactory().MockCreate(_apiReponse, _SignUpEventVMList, newSignUpEventVM);

            // Loading all choosingparyvm
            var mockChossingEventFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);

            // Loading all revealeventVM
            var mockRevealEventFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RegistrationEventController
            _signUpEventController = new RegistrationEventController(mockSignupEventFactoryResult.Result.Object,
                mockChossingEventFactoryResult.Result.Object, mockRevealEventFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //ACT
            #region "ACT"

            // try saving our new SignUpEventVM
            var actionResult = _signUpEventController.Create(newSignUpEventVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            #endregion


            //Get Expected Count
            var expectedCount = _SignUpEventVMList.Count;

            // Verify that our new SignUpEventVM has been created
            SignUpEventVM testSignUpEventVM = mockSignupEventFactory.MockFindById(_SignUpEventVMList, _SignUpEventVMListMaxId);

            // Assert
            #region "Assert"            
             Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response    
            // Check both actual and expected count is same after insert        
            Assert.AreEqual(expectedCount, _SignUpEventVMListCount + 1); // Verify the expected Number post-insert
            Assert.AreEqual(_SignUpEventVMListMaxId, testSignUpEventVM.SignUpEventId); // Verify it has the expected SignUpEventVM Id
            #endregion
        }

        /// <summary>
        /// Can we insert a new SignUpEventVM?
        /// </summary>
        [Test]
        public void SignUpEvent_CreateSave_DuplicateData()
        {
            //Arrange
            //Get Actual Count
            _SignUpEventVMListCount = _SignUpEventVMList.Count;

            //Get Max ID from List
            _SignUpEventVMListMaxId = Convert.ToInt32(_SignUpEventVMList.Max(x => x.SignUpEventId).ToString()) + 1;

            // Create a new SignUpEventModel, not I do not supply an id
            SignUpEventVM newSignUpEventVM = new SignUpEventVM
            {
                SignUpEventId = _SignUpEventVMListMaxId,
                EventName = "Test Registration Event 1",
                PublishDate = DateTime.Today,
                ClosedDate = DateTime.Today.AddDays(14),
                EventDate = DateTime.Today.AddDays(7),
                Location = "Edinburgh",
                TypeOfRegistration = "WV Chosen Event",
                ShortURL = "TEST",
                SpecificChoosingEvent = "Yes",
                ChoosingPartyId = 1,
                RevealEventId = 1,
                CampaignCode = "TEST123456",
                WorkflowStatusId = 1,
                WVPhotoBooth = true
            };
            _apiReponse.Message = "Failed to create SignUp event";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            // Loading all SignUpEventVM
            var mockSignupEventFactoryResult = new MockSignupEventFactory().MockCreate(_apiReponse, _SignUpEventVMList, newSignUpEventVM);

            // Loading all choosingparyvm
            var mockChossingEventFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);

            // Loading all revealeventVM
            var mockRevealEventFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RegistrationEventController
            _signUpEventController = new RegistrationEventController(mockSignupEventFactoryResult.Result.Object,
                mockChossingEventFactoryResult.Result.Object, mockRevealEventFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);


            //ACT
            #region "ACT"

            // try saving our new SignUpEventVM
            var actionResult = _signUpEventController.Create(newSignUpEventVM);
            var actionResponse = actionResult.Result as ViewResult;
            #endregion


            //Get Expected Count
            var expectedCount = _SignUpEventVMList.Count;


            // Assert
            #region "Assert"            
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response      
            // Check both actual and expected count is same after insert        
            Assert.AreNotEqual(expectedCount, _SignUpEventVMListCount + 1); // Verify the expected Number post-insert
            #endregion
        }
        #endregion

        #region "Edit Action "

        /// <summary>
        /// while we Load a SignUp Event By Id for edit action?
        /// </summary>
        [Test]
        public void SignUpEvent_EditLoad_Valid()
        {
            //Arrange

            // Get that SignUpEventVM for id : 2
            SignUpEventVM testSignUpEventVM = mockSignupEventFactory.MockFindById(_SignUpEventVMList, 2);

            // Loading all SignUpEventVM
            var mockSignupEventFactoryResult = new MockSignupEventFactory().MockLoad(testSignUpEventVM);

            // Loading all choosingparyvm
            var mockChossingEventFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);

            // Loading all revealeventVM
            var mockRevealEventFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RegistrationEventController
            _signUpEventController = new RegistrationEventController(mockSignupEventFactoryResult.Result.Object,
                mockChossingEventFactoryResult.Result.Object, mockRevealEventFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _signUpEventController.Edit(2);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as SignUpEventVM;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsNotNull(_mockResponse); // Test if null
            Assert.IsInstanceOf(typeof(SignUpEventVM), _mockResponse); //Test if returns ok response   
        }

        /// <summary>
        /// When we Load a SignUp Event By Id for edit action if returns http not found result?
        /// </summary>
        [Test]
        public void SignUpEvent_EditLoad_InValid()
        {
            //Arrange

            // Get that SignUpEventVM for id : 2
            SignUpEventVM testSignUpEventVM = mockSignupEventFactory.MockFindById(_SignUpEventVMList, 5);

            // Loading all SignUpEventVM
            var mockSignupEventFactoryResult = new MockSignupEventFactory().MockLoad(testSignUpEventVM);

            // Loading all choosingparyvm
            var mockChossingEventFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);

            // Loading all revealeventVM
            var mockRevealEventFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RegistrationEventController
            _signUpEventController = new RegistrationEventController(mockSignupEventFactoryResult.Result.Object,
                mockChossingEventFactoryResult.Result.Object, mockRevealEventFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _signUpEventController.Edit(5);
            var actionResponse = actionResult.Result as NotFoundResult;

            //Assert  
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResponse); //Test if returns ok response  

        }

        /// <summary>
        /// When we edit a SignUp Event By Id using edit action if returns ok result?
        /// </summary>
        [Test]
        public void SignUpEvent_EditSave_Valid()
        {
            //Arrange            
            _apiReponse.Message = "SignUp Event updated successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            // Get that SignUpEventVM for id : 2
            SignUpEventVM testSignUpEventVM = mockSignupEventFactory.MockFindById(_SignUpEventVMList, 2);

            // Change one of its properties
            testSignUpEventVM.EventName = "Unit Test Data";
            // Loading all SignUpEventVM
            var mockSignupEventFactoryResult = new MockSignupEventFactory().MockUpdate(_apiReponse, _SignUpEventVMList, testSignUpEventVM);

            // Loading all choosingparyvm
            var mockChossingEventFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);

            // Loading all revealeventVM
            var mockRevealEventFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RegistrationEventController
            _signUpEventController = new RegistrationEventController(mockSignupEventFactoryResult.Result.Object,
                mockChossingEventFactoryResult.Result.Object, mockRevealEventFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _signUpEventController.Edit(testSignUpEventVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            //Assert  
             Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreEqual("Unit Test Data", mockSignupEventFactory.MockFindById(_SignUpEventVMList, 2).EventName);
        }

        /// <summary>
        /// When we edit a SignUp Event By Id using edit action if its not update?
        /// </summary>
        [Test]
        public void SignUpEvent_EditSave_DuplicateData()
        {
            //Arrange            
            _apiReponse.Message = "SignUp Event updated successfully";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            // Get that SignUpEventVM for id : 2
            SignUpEventVM testSignUpEventVM = mockSignupEventFactory.MockFindById(_SignUpEventVMList, 3);         

            // Change one of its properties

            testSignUpEventVM.EventName = "Test Registration Event 1";
            testSignUpEventVM.ShortURL = "TEST1";
            testSignUpEventVM.Location = "Pajule";

            // Loading all SignUpEventVM
            var mockSignupEventFactoryResult = new MockSignupEventFactory().MockUpdate(_apiReponse, _SignUpEventVMList, testSignUpEventVM);

            // Loading all choosingparyvm
            var mockChossingEventFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);

            // Loading all revealeventVM
            var mockRevealEventFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RegistrationEventController
            _signUpEventController = new RegistrationEventController(mockSignupEventFactoryResult.Result.Object,
                mockChossingEventFactoryResult.Result.Object, mockRevealEventFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _signUpEventController.Edit(testSignUpEventVM);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
          //  Assert.AreNotEqual("Test SignUp Event 1", mockSignupEventFactory.MockFindById(_SignUpEventVMList, 3).PartyName);

        }
        #endregion

        #region "Delete Action


        /// <summary>
        /// while we Load a SignUp Event By Id for delete action?
        /// </summary>
        [Test]
        public void SignUpEvent_DeleteLoad_SignupExists()
        {
            //Arrange
            //Get Actual Count
            _SignUpEventVMListCount = _SignUpEventVMList.Count;
            // Get that SignUpEventVM for id : 2
            SignUpEventVM testSignUpEventVM = mockSignupEventFactory.MockFindById(_SignUpEventVMList, 4);

            var mockSignupEventFactoryResult = new MockSignupEventFactory().MockDeleteLoad(testSignUpEventVM, true, 4);         

            // Loading all choosingparyvm
            var mockChossingEventFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);

            // Loading all revealeventVM
            var mockRevealEventFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RegistrationEventController
            _signUpEventController = new RegistrationEventController(mockSignupEventFactoryResult.Result.Object,
                mockChossingEventFactoryResult.Result.Object, mockRevealEventFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _signUpEventController.Delete(4);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

        }


        /// <summary>
        /// while we Load a SignUp Event By Id for delete action?
        /// </summary>
        [Test]
        public void SignUpEvent_DeleteLoad_SignupNotExists()
        {
            //Arrange
            //Get Actual Count
            _SignUpEventVMListCount = _SignUpEventVMList.Count;

            // Get that RevealEventVM for id : 2
            SignUpEventVM testSignupEventVM = mockSignupEventFactory.MockFindById(_SignUpEventVMList, 2);
            // Loading all SignUpEventVM
            var mockSignupEventFactoryResult = new MockSignupEventFactory().MockDeleteLoad(testSignupEventVM, false, 2);

            // Loading all choosingparyvm
            var mockChossingEventFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);

            // Loading all revealeventVM
            var mockRevealEventFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RegistrationEventController
            _signUpEventController = new RegistrationEventController(mockSignupEventFactoryResult.Result.Object,
                mockChossingEventFactoryResult.Result.Object, mockRevealEventFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _signUpEventController.DeleteConfirmed(2);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
              Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

        }
        /// <summary>
        ///  we delete a SignUpEventVM if there is no registeration exists?
        /// </summary>
        [Test]
        public void SignUpEvent_DeleteConfirmed_Valid()
        {
            //Arrange            
            _apiReponse.Message = "SignUp Event deleted successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;
            //Get Actual Count
            _SignUpEventVMListCount = _SignUpEventVMList.Count;
            // Get that SignUpEventVM for id : 2
            SignUpEventVM testSignUpEventVM = mockSignupEventFactory.MockFindById(_SignUpEventVMList, 4);
            
            // Loading all SignUpEventVM
            var mockSignupEventFactoryResult = new MockSignupEventFactory().MockDelete(_apiReponse, _SignUpEventVMList, 4);

            // Loading all choosingparyvm
            var mockChossingEventFactoryResult = new MockChoosingFactory().MockLoadList(_ChoosingPartyVMList);

            // Loading all revealeventVM
            var mockRevealEventFactoryResult = new MockRevealEventFactory().MockLoadList(_RevealEventVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup RegistrationEventController
            _signUpEventController = new RegistrationEventController(mockSignupEventFactoryResult.Result.Object,
                mockChossingEventFactoryResult.Result.Object, mockRevealEventFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object,_mockLogger.Object);

            //Act  
            var actionResult = _signUpEventController.DeleteConfirmed(4);
            var actionResponse = actionResult.Result as RedirectToActionResult;

            //Assert  
             Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreNotEqual(_SignUpEventVMList.Count, _SignUpEventVMListCount); // Test count is not equal before and after delete.
        }


        #endregion
    }
}
