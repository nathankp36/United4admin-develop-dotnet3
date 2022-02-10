using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using United4Admin.UnitTests.Mocks.ApiClientFactory;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.Controllers;
using United4Admin.WebApplication.ViewModels;

namespace United4Admin.UnitTests.Controller
{
    [TestFixture]
    public class PaymentMethodControllerTest
    {
        private PaymentMethodController _paymentMethodController;
        private Mock<ILogger<PaymentMethodController>> _mockLogger;
        private MockPaymentMethodFactory mockPaymentMethodFactory;

        private PaymentMethodVM _mockResponse;
        private int _paymentMethodVMListCount;
        private int _paymentMethodVMListMaxId;

        private List<PaymentMethodVM> _paymentMethodVMList;
        private List<PermissionsVM> _permissionsVMList;
        private ApiResponse _apiReponse;


        [SetUp]
        public void TestInitialize()

        {
            //Arrange
            #region "Arrange"
            //Creating Dummy PaymentMethodVM List
            _paymentMethodVMList = new List<PaymentMethodVM>
                    {
                        new PaymentMethodVM {ddlWvType = 1,
                                                crmPaymentMethodName = "Test payment 1",
                                                crmPaymentMethodType="Test type 1"},
                        new PaymentMethodVM {ddlWvType = 2,
                                                crmPaymentMethodName = "Test payment 2",
                                                crmPaymentMethodType="Test type 2"},
                        new PaymentMethodVM {ddlWvType = 3,
                                                crmPaymentMethodName = "Test payment 3",
                                                crmPaymentMethodType="Test type 3"},
                        new PaymentMethodVM {ddlWvType = 4,
                                                crmPaymentMethodName = "Test payment 4",
                                                crmPaymentMethodType="Test type 4"}
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
            //  _mockLogger = new Mock<ILogger<PaymentMethodController>>();

            // Mock the API Response 
            _mockResponse = new PaymentMethodVM();

            #endregion

            //Setup mockPaymentMethodFactory
            mockPaymentMethodFactory = new MockPaymentMethodFactory();

            ////setup for permission
            //var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //logger
            _mockLogger = new Mock<ILogger<PaymentMethodController>>();

            // ApiResponse
            _apiReponse = new ApiResponse();
        }

        #region "Create Action "

        /// <summary>
        /// while we Load a PaymentMethod By Id for edit action?
        /// </summary>
        [Test]
        public void PaymentMethod_CreateLoad_Valid()
        {
            //Arrange
            // Create a new PaymentMethodModel, not I do not supply an id
            PaymentMethodVM newPaymentMethodVM = new PaymentMethodVM
            {                
                Create = true
            };

            var mockPaymentMethodFactoryResult = new MockPaymentMethodFactory().MockLoad(newPaymentMethodVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            _paymentMethodController = new PaymentMethodController(mockPaymentMethodFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _paymentMethodController.Create(0);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as PaymentMethodVM;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
        }

        ///// <summary>
        ///// Can we insert a new PaymentMethodVM?
        ///// </summary>        
        [Test]
        public void PaymentMethod_CreateSave_Valid()
        {
            //Arrange
            //Get Actual Count
            _paymentMethodVMListCount = _paymentMethodVMList.Count;

            //Get Max ID from List
            _paymentMethodVMListMaxId = Convert.ToInt32(_paymentMethodVMList.Max(x => x.ddlWvType).ToString()) + 1;

            // Create a new PaymentMethodModel, not I do not supply an id
            PaymentMethodVM newPaymentMethodVM = new PaymentMethodVM
            {
                ddlWvType = _paymentMethodVMListMaxId,
                crmPaymentMethodName = "Test PaymentMethod " + _paymentMethodVMListMaxId.ToString(),
                crmPaymentMethodType = "Test Payment type" + _paymentMethodVMListMaxId.ToString()
            };
            _apiReponse.Message = "PaymentMethod created successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            var mockPaymentMethodFactoryResult = new MockPaymentMethodFactory().MockCreate(_apiReponse, _paymentMethodVMList, newPaymentMethodVM);
            _paymentMethodController = new PaymentMethodController(
                                      mockPaymentMethodFactoryResult.Result.Object,
                                       mockPermissionFactoryResult.Result.Object, _mockLogger.Object);


            //ACT
            #region "ACT"

            // try saving our new PaymentMethodVM
            var actionResult = _paymentMethodController.Create(newPaymentMethodVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            #endregion


            //Get Expected Count
            var expectedCount = _paymentMethodVMList.Count;

            // Verify that our new PaymentMethodVM has been created
            PaymentMethodVM testPaymentMethodVM = mockPaymentMethodFactory.MockFindById(_paymentMethodVMList, _paymentMethodVMListMaxId);

            // Assert
            #region "Assert"            
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response    
            // Check both actual and expected count is same after insert        
            Assert.AreEqual(expectedCount, _paymentMethodVMListCount + 1); // Verify the expected Number post-insert
            Assert.AreEqual(_paymentMethodVMListMaxId, testPaymentMethodVM.ddlWvType); // Verify it has the expected PaymentMethodVM Id
            #endregion
        }

        /// <summary>
        /// Can we insert a new PaymentMethodVM?
        /// </summary>
        [Test]
        public void PaymentMethod_CreateSave_DuplicateData()
        {
            var testPayment = _paymentMethodVMList;
            //Arrange
            //Get Actual Count
            _paymentMethodVMListCount = _paymentMethodVMList.Count;

            //Get Max ID from List
            _paymentMethodVMListMaxId = Convert.ToInt32(_paymentMethodVMList.Max(x => x.ddlWvType).ToString()) + 1;

            // Create a new PaymentMethodModel, not I do not supply an id
            PaymentMethodVM newPaymentMethodVM = new PaymentMethodVM
            {
                ddlWvType = 1,
                crmPaymentMethodName = "Test payment 1",
                crmPaymentMethodType = "Test type 1"
            };
            _apiReponse.Message = "Failed to create PaymentMethod";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList); 
            var mockPaymentMethodFactoryResult = new MockPaymentMethodFactory().MockCreate(_apiReponse, _paymentMethodVMList, newPaymentMethodVM);
            _paymentMethodController = new PaymentMethodController(
                                      mockPaymentMethodFactoryResult.Result.Object,
                                       mockPermissionFactoryResult.Result.Object, _mockLogger.Object);


            //ACT
            #region "ACT"

            // try saving our new PaymentMethodVM
            var actionResult = _paymentMethodController.Create(newPaymentMethodVM);
            var actionResponse = actionResult.Result as ViewResult;
            #endregion


            //Get Expected Count
            var expectedCount = _paymentMethodVMList.Count;


            // Assert
            #region "Assert"            
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response      
            // Check both actual and expected count is same after insert        
            Assert.AreNotEqual(expectedCount, _paymentMethodVMListCount + 1); // Verify the expected Number post-insert
            #endregion
        }
        #endregion

        #region "Edit Action "

        /// <summary>
        /// while we Load a PaymentMethod By Id for edit action?
        /// </summary>
        [Test]
        public void PaymentMethod_EditLoad_Valid()
        {
            //Arrange

            // Get that PaymentMethodVM for id : 2
            PaymentMethodVM testPaymentMethodVM = mockPaymentMethodFactory.MockFindById(_paymentMethodVMList, 2);

            var mockPaymentMethodFactoryResult = new MockPaymentMethodFactory().MockLoad(testPaymentMethodVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            _paymentMethodController = new PaymentMethodController(mockPaymentMethodFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _paymentMethodController.Edit(2);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as PaymentMethodVM;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsNotNull(_mockResponse); // Test if null
            Assert.IsInstanceOf(typeof(PaymentMethodVM), _mockResponse); //Test if returns ok response   
        }

        /// <summary>
        /// When we Load a PaymentMethod By Id for edit action if returns http not found result?
        /// </summary>
        [Test]
        public void PaymentMethod_EditLoad_InValid()
        {
            //Arrange

            // Get that PaymentMethodVM for id : 2
            PaymentMethodVM testPaymentMethodVM = mockPaymentMethodFactory.MockFindById(_paymentMethodVMList, 5);

            var mockPaymentMethodFactoryResult = new MockPaymentMethodFactory().MockLoad(testPaymentMethodVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);
            _paymentMethodController = new PaymentMethodController(mockPaymentMethodFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _paymentMethodController.Edit(5);
            var actionResponse = actionResult.Result as NotFoundResult;

            //Assert  
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResponse); //Test if returns ok response  

        }

        /// <summary>
        /// When we edit a PaymentMethod By Id using edit action if returns ok result?
        /// </summary>
        [Test]
        public void PaymentMethod_EditSave_Valid()
        {
            //Arrange            
            _apiReponse.Message = "PaymentMethod updated successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            // Get that PaymentMethodVM for id : 2
            PaymentMethodVM testPaymentMethodVM = mockPaymentMethodFactory.MockFindById(_paymentMethodVMList, 2);

            // Change one of its properties
            testPaymentMethodVM.crmPaymentMethodName = "Unit Test Data";
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            var mockPaymentMethodFactoryResult = new MockPaymentMethodFactory().MockUpdate(_apiReponse, _paymentMethodVMList, testPaymentMethodVM);
            _paymentMethodController = new PaymentMethodController(mockPaymentMethodFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _paymentMethodController.Edit(testPaymentMethodVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreEqual("Unit Test Data", mockPaymentMethodFactory.MockFindById(_paymentMethodVMList, 2).crmPaymentMethodName);
        }

        /// <summary>
        /// When we edit a PaymentMethod By Id using edit action if its not update?
        /// </summary>
        [Test]
        public void PaymentMethod_EditSave_DuplicateData()
        {
            //Arrange            
            _apiReponse.Message = "PaymentMethod updated successfully";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            // Get that PaymentMethodVM for id : 2
            PaymentMethodVM testPaymentMethodVM = mockPaymentMethodFactory.MockFindById(_paymentMethodVMList, 3);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            // Change one of its properties

            testPaymentMethodVM.crmPaymentMethodName = "Test payment 1";
            testPaymentMethodVM.crmPaymentMethodType = "Test type 1";

            var mockPaymentMethodFactoryResult = new MockPaymentMethodFactory().MockUpdate(_apiReponse, _paymentMethodVMList, testPaymentMethodVM);
            _paymentMethodController = new PaymentMethodController(mockPaymentMethodFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _paymentMethodController.Edit(testPaymentMethodVM);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            //  Assert.AreNotEqual("Test PaymentMethod 1", mockPaymentMethodFactory.MockFindById(_PaymentMethodVMList, 3).PartyName);

        }
        #endregion

        #region "Delete Action


        /// <summary>
        /// while we Load a PaymentMethod By Id for delete action?
        /// </summary>
        [Test]
        public void PaymentMethod_DeleteLoad()
        {
            //Arrange
            //Get Actual Count
            _paymentMethodVMListCount = _paymentMethodVMList.Count;
            // Get that PaymentMethodVM for id : 2
            PaymentMethodVM testPaymentMethodVM = mockPaymentMethodFactory.MockFindById(_paymentMethodVMList, 4);

            var mockPaymentMethodFactoryResult = new MockPaymentMethodFactory().MockDeleteLoad(testPaymentMethodVM, true, 4);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            _paymentMethodController = new PaymentMethodController(mockPaymentMethodFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _paymentMethodController.Delete(4);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

        }


        /// <summary>
        ///  we delete a PaymentMethodVM if there is no registeration exists?
        /// </summary>
        [Test]
        public void PaymentMethod_DeleteConfirmed_Valid()
        {
            //Arrange            
            _apiReponse.Message = "PaymentMethod deleted successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;
            //Get Actual Count
            _paymentMethodVMListCount = _paymentMethodVMList.Count;
            // Get that PaymentMethodVM for id : 2
            PaymentMethodVM testPaymentMethodVM = mockPaymentMethodFactory.MockFindById(_paymentMethodVMList, 4);

            var mockPaymentMethodFactoryResult = new MockPaymentMethodFactory().MockDelete(_apiReponse, _paymentMethodVMList, 4);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            _paymentMethodController = new PaymentMethodController(mockPaymentMethodFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _paymentMethodController.DeleteConfirmed(4);
            var actionResponse = actionResult.Result as RedirectToActionResult;



            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreNotEqual(_paymentMethodVMList.Count, _paymentMethodVMListCount); // Test count is not equal before and after delete.
        }


        #endregion
    }
}
