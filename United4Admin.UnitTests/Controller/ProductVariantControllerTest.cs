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
    public class ProductVariantControllerTest
    {
        private ProductVariantController _productVariantController;
        private Mock<ILogger<ProductVariantController>> _mockLogger;
        private MockProductVariantFactory mockProductVariantFactory;

        private ProductVariantVM _mockResponse;
        private int _productVariantVMListCount;
        private string _productVariantVMListMaxId;

        private List<ProductVariantVM> _productVariantVMList;
        private List<PermissionsVM> _permissionsVMList;
        private ApiResponse _apiReponse;

        [SetUp]
        public void TestInitialize()

        {
            //Arrange
            #region "Arrange"
            //Creating Dummy ProductVariantVM List
            _productVariantVMList = new List<ProductVariantVM>
                    {
                        new ProductVariantVM {ddlProductTypeCodeDisplay = "Test ProductCode",
                                                crmProductVariantName = "Test Product",
                                                crmIncidentType="Incident 1",
                                                crmPurposeCode="345656",
                                                crmPledgeType="Pledge type 1"
                        },
                        new ProductVariantVM {ddlProductTypeCodeDisplay = "Test ProductCode1",
                                                crmProductVariantName = "Test Product1",
                                                crmIncidentType="Incident 2",
                                                crmPurposeCode="345655",
                                                crmPledgeType="Pledge type 1"
                        },
                        new ProductVariantVM {ddlProductTypeCodeDisplay = "Test ProductCode2",
                                                crmProductVariantName = "Test Product2",
                                                crmIncidentType="Incident 1",
                                                crmPurposeCode="345456",
                                                crmPledgeType="Pledge type 1"
                        },
                        new ProductVariantVM {ddlProductTypeCodeDisplay = "Test ProductCode3",
                                                crmProductVariantName = "Test Product3",
                                                crmIncidentType="Incident 2",
                                                crmPurposeCode="245656",
                                                crmPledgeType="Pledge type 1"
                        }
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
            //  _mockLogger = new Mock<ILogger<ProductVariantController>>();

            // Mock the API Response 
            _mockResponse = new ProductVariantVM();

            #endregion

            //Setup MockProductVariantFactory
            mockProductVariantFactory = new MockProductVariantFactory();

            ////setup for permission
            //var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //logger
            _mockLogger = new Mock<ILogger<ProductVariantController>>();

            // ApiResponse
            _apiReponse = new ApiResponse();
        }

        #region "Create Action "

        /// <summary>
        /// while we Load a ProductVariant By Id for edit action?
        /// </summary>
        [Test]
        public void ProductVariant_CreateLoad_Valid()
        {
            //Arrange
            // Create a new ProductVariantModel, not I do not supply an id
            ProductVariantVM newProductVariantVM = new ProductVariantVM
            {
                Create = true
            };

            var MockProductVariantFactoryResult = new MockProductVariantFactory().MockLoad(newProductVariantVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            _productVariantController = new ProductVariantController(MockProductVariantFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _productVariantController.CreateAsync(0);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as ProductVariantVM;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
        }

        ///// <summary>
        ///// Can we insert a new ProductVariantVM?
        ///// </summary>        
        [Test]
        public void ProductVariant_CreateSave_Valid()
        {
            //Arrange
            //Get Actual Count
            _productVariantVMListCount = _productVariantVMList.Count;

            //Get Max ID from List
            _productVariantVMListMaxId = _productVariantVMList.Max(x => x.ddlProductTypeCodeDisplay).ToString();

            // Create a new ProductVariantModel, not I do not supply an id
            ProductVariantVM newProductVariantVM = new ProductVariantVM
            {
                ddlProductTypeCodeDisplay = "Test code",
                crmProductVariantName="Test product name",
                crmIncidentType = "Incident test",
                crmPurposeCode="234356",
                crmPledgeType="Pledge test"
            };
            _apiReponse.Message = "ProductVariant created successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            var MockProductVariantFactoryResult = new MockProductVariantFactory().MockCreate(_apiReponse, _productVariantVMList, newProductVariantVM);
            _productVariantController = new ProductVariantController(
                                      MockProductVariantFactoryResult.Result.Object,
                                       mockPermissionFactoryResult.Result.Object, _mockLogger.Object);


            //ACT
            #region "ACT"

            // try saving our new ProductVariantVM
            var actionResult = _productVariantController.Create(newProductVariantVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            #endregion

            //Get Expected Count
            var expectedCount = _productVariantVMList.Count;

            // Verify that our new ProductVariantVM has been created
            ProductVariantVM testProductVariantVM = mockProductVariantFactory.MockFindById(_productVariantVMList, _productVariantVMListMaxId);

            // Assert
            #region "Assert"            
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response    
            // Check both actual and expected count is same after insert        
            Assert.AreEqual(expectedCount, _productVariantVMListCount + 1); // Verify the expected Number post-insert
            Assert.AreEqual(_productVariantVMListMaxId, testProductVariantVM.ddlProductTypeCodeDisplay); // Verify it has the expected ProductVariantVM Id
            #endregion
        }

        /// <summary>
        /// Can we insert a new ProductVariantVM?
        /// </summary>
        [Test]
        public void ProductVariant_CreateSave_DuplicateData()
        {
            //Arrange
            //Get Actual Count
            _productVariantVMListCount = _productVariantVMList.Count;

            //Get Max ID from List
            _productVariantVMListMaxId = _productVariantVMList.Max(x => x.ddlProductTypeCodeDisplay).ToString();

            // Create a new ProductVariantModel, not I do not supply an id
            ProductVariantVM newProductVariantVM = new ProductVariantVM
            {
                ddlProductTypeCodeDisplay = "Test ProductCode",
                crmProductVariantName = "Test Product",
                crmIncidentType = "Incident 1",
                crmPurposeCode = "345656",
                crmPledgeType = "Pledge type 1"
            };
            _apiReponse.Message = "Failed to create ProductVariant";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);
            var MockProductVariantFactoryResult = new MockProductVariantFactory().MockCreate(_apiReponse, _productVariantVMList, newProductVariantVM);
            _productVariantController = new ProductVariantController(
                                      MockProductVariantFactoryResult.Result.Object,
                                       mockPermissionFactoryResult.Result.Object, _mockLogger.Object);


            //ACT
            #region "ACT"

            // try saving our new ProductVariantVM
            var actionResult = _productVariantController.Create(newProductVariantVM);
            var actionResponse = actionResult.Result as ViewResult;
            #endregion


            //Get Expected Count
            var expectedCount = _productVariantVMList.Count;


            // Assert
            #region "Assert"            
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response      
            // Check both actual and expected count is same after insert        
            Assert.AreNotEqual(expectedCount, _productVariantVMListCount + 1); // Verify the expected Number post-insert
            #endregion
        }
        #endregion

        #region "Edit Action "

        /// <summary>
        /// while we Load a ProductVariant By Id for edit action?
        /// </summary>
        [Test]
        public void ProductVariant_EditLoad_Valid()
        {
            //Arrange

            // Get that ProductVariantVM for id : Ms
            ProductVariantVM testProductVariantVM = mockProductVariantFactory.MockFindById(_productVariantVMList, "Test ProductCode");

            var MockProductVariantFactoryResult = new MockProductVariantFactory().MockLoad(testProductVariantVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            _productVariantController = new ProductVariantController(MockProductVariantFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _productVariantController.Edit("Test ProductCode");
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as ProductVariantVM;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsNotNull(_mockResponse); // Test if null
            Assert.IsInstanceOf(typeof(ProductVariantVM), _mockResponse); //Test if returns ok response   
        }

        /// <summary>
        /// When we Load a ProductVariant By Id for edit action if returns http not found result?
        /// </summary>
        [Test]
        public void ProductVariant_EditLoad_InValid()
        {
            //Arrange

            // Get that ProductVariantVM for id : 2
            ProductVariantVM testProductVariantVM = mockProductVariantFactory.MockFindById(_productVariantVMList, "Test Product Code");

            var MockProductVariantFactoryResult = new MockProductVariantFactory().MockLoad(testProductVariantVM);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);
            _productVariantController = new ProductVariantController(MockProductVariantFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _productVariantController.Edit("Test Product Code");
            var actionResponse = actionResult.Result as NotFoundResult;

            //Assert  
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResponse); //Test if returns ok response  

        }

        /// <summary>
        /// When we edit a ProductVariant By Id using edit action if returns ok result?
        /// </summary>
        [Test]
        public void ProductVariant_EditSave_Valid()
        {
            //Arrange            
            _apiReponse.Message = "ProductVariant updated successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            // Get that ProductVariantVM for id : Ms
            ProductVariantVM testProductVariantVM = mockProductVariantFactory.MockFindById(_productVariantVMList, "Test ProductCode");

            // Change one of its properties
            testProductVariantVM.crmProductVariantName = "Unit test edit";
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            var MockProductVariantFactoryResult = new MockProductVariantFactory().MockUpdate(_apiReponse, _productVariantVMList, testProductVariantVM);
            _productVariantController = new ProductVariantController(MockProductVariantFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _productVariantController.Edit(testProductVariantVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;


            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreEqual("Unit test edit", mockProductVariantFactory.MockFindById(_productVariantVMList, "Test ProductCode").crmProductVariantName);
        }

        /// <summary>
        /// When we edit a ProductVariant By Id using edit action if its not update?
        /// </summary>
        [Test]
        public void ProductVariant_EditSave_DuplicateData()
        {
            //Arrange            
            _apiReponse.Message = "ProductVariant updated successfully";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            // Get that ProductVariantVM for id : Mrs
            ProductVariantVM testProductVariantVM = mockProductVariantFactory.MockFindById(_productVariantVMList, "Test ProductCode");
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            // Change one of its properties
            testProductVariantVM.crmProductVariantName = "Test Product1";
            testProductVariantVM.crmIncidentType = "Incident 2";
            testProductVariantVM.crmPurposeCode = "345655";
            testProductVariantVM.crmPledgeType = "Pledge type 1";

            var MockProductVariantFactoryResult = new MockProductVariantFactory().MockUpdate(_apiReponse, _productVariantVMList, testProductVariantVM);
            _productVariantController = new ProductVariantController(MockProductVariantFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _productVariantController.Edit(testProductVariantVM);
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            //  Assert.AreNotEqual("Test ProductVariant 1", MockProductVariantFactory.MockFindById(_ProductVariantVMList, 3).PartyName);

        }
        #endregion

        #region "Delete Action


        /// <summary>
        /// while we Load a ProductVariant By Id for delete action?
        /// </summary>
        [Test]
        public void ProductVariant_DeleteLoad()
        {
            //Arrange
            //Get Actual Count
            _productVariantVMListCount = _productVariantVMList.Count;
            // Get that ProductVariantVM for id : 2
            ProductVariantVM testProductVariantVM = mockProductVariantFactory.MockFindById(_productVariantVMList, "Test ProductCode");

            var MockProductVariantFactoryResult = new MockProductVariantFactory().MockDeleteLoad(testProductVariantVM, "Test ProductCode");
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            _productVariantController = new ProductVariantController(MockProductVariantFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _productVariantController.Delete("Test ProductCode");
            var actionResponse = actionResult.Result as ViewResult;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

        }

        /// <summary>
        ///  we delete a ProductVariantVM ?
        /// </summary>
        [Test]
        public void ProductVariant_DeleteConfirmed_Valid()
        {
            //Arrange            
            _apiReponse.Message = "ProductVariant deleted successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;
            //Get Actual Count
            _productVariantVMListCount = _productVariantVMList.Count;
            // Get that ProductVariantVM for id : 2
            ProductVariantVM testProductVariantVM = mockProductVariantFactory.MockFindById(_productVariantVMList, "Test ProductCode");

            var MockProductVariantFactoryResult = new MockProductVariantFactory().MockDelete(_apiReponse, _productVariantVMList, "Test ProductCode");
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            _productVariantController = new ProductVariantController(MockProductVariantFactoryResult.Result.Object, mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _productVariantController.DeleteConfirmed("Test ProductCode");
            var actionResponse = actionResult.Result as RedirectToActionResult;



            //Assert  
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response  
            Assert.AreNotEqual(_productVariantVMList.Count, _productVariantVMListCount); // Test count is not equal before and after delete.
        }


        #endregion
    }
}
