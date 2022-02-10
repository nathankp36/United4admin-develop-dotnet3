using United4Admin.UnitTests.Mocks.ApiClientFactory;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.Controllers;
using United4Admin.WebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
    public class NavisionMappingControllerTest
    {
        private NavisionMappingController _NavisionMappingController;
        private Mock<ILogger<NavisionMappingController>> _mockLogger;
        private MockSalutationFactory mockSalutationFactory;
        private MockProductVariantFactory mockProductVariantFactory;
        private MockPaymentMethodFactory mockPaymentMethodFactory;

        private SalutationVM _mockSalutationResponse;
        private int _salutationVMListCount;
        private string _salutationVMListMaxId;

        private List<SalutationVM> _salutationVMList;
        private List<PermissionsVM> _permissionsVMList;
        private ApiResponse _apiReponse;

        private PaymentMethodVM _mockPaymentMethodResponse;
        private int _paymentMethodVMListCount;
        private int _paymentMethodVMListMaxId;

        private List<PaymentMethodVM> _paymentMethodVMList;

        private ProductVariantVM _mockProductVariantResponse;
        private int _productVariantVMListCount;
        private string _productVariantVMListMaxId;

        private List<ProductVariantVM> _productVariantVMList;

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
            //  _mockLogger = new Mock<ILogger<SalutationController>>();

            // Mock the API Response 
            _mockSalutationResponse = new SalutationVM();
            _mockProductVariantResponse = new ProductVariantVM();
            _mockPaymentMethodResponse = new PaymentMethodVM();

            #endregion

            //Setup mockSalutationFactory
            mockSalutationFactory = new MockSalutationFactory();
            mockProductVariantFactory = new MockProductVariantFactory();
            mockPaymentMethodFactory = new MockPaymentMethodFactory();

            ////setup for permission
            //var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //logger
            _mockLogger = new Mock<ILogger<NavisionMappingController>>();

            // ApiResponse
            _apiReponse = new ApiResponse();
        }

        #region "Index Action"
        /// <summary>
        /// Can we Load all NavisionMappings?
        /// </summary>
        [Test]
        public void CRMMapping_Index_Valid()
        {
            //Arrange
            // Loading all Salutaiton, Product varinat and Payment Method
            var mockSaluationFactoryResult = new MockSalutationFactory().MockLoadList(_salutationVMList);
            var mockProductVariantFactoryResult = new MockProductVariantFactory().MockLoadList(_productVariantVMList);
            var mockPaymentMethodFactoryResult = new MockPaymentMethodFactory().MockLoadList(_paymentMethodVMList);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            //Setup CRMMappingController
            _NavisionMappingController = new NavisionMappingController(mockSaluationFactoryResult.Result.Object,
                                            mockProductVariantFactoryResult.Result.Object,
                                            mockPaymentMethodFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _NavisionMappingController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as CRMMappingVM;

            //Get Actual Count
            _salutationVMListCount = _salutationVMList.Count;
            _productVariantVMListCount = _productVariantVMList.Count;
            _paymentMethodVMListCount = _paymentMethodVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

            Assert.IsInstanceOf(typeof(CRMMappingVM), dataResult); //Test if returns ok response               
            Assert.AreEqual(dataResult.SalutationVM.Count, _salutationVMListCount); //Test if returns equal count for Salutation
            Assert.AreEqual(dataResult.ProductVariantVM.Count, _productVariantVMListCount); //Test if returns equal count for Product Variant
            Assert.AreEqual(dataResult.PaymentMethodVM.Count, _paymentMethodVMListCount); //Test if returns equal count for Salutation                                                         
        }

        /// <summary>
        /// If we have no records in CRMMappings?
        /// </summary>
        [Test]
        public void CRMMapping_Index_NoRecords()
        {

            //Arrange
            // Loading all Salutaiton, Product varinat and Payment Method
            var mockSaluationFactoryResult = new MockSalutationFactory().MockLoadList(null);
            var mockProductVariantFactoryResult = new MockProductVariantFactory().MockLoadList(null);
            var mockPaymentMethodFactoryResult = new MockPaymentMethodFactory().MockLoadList(null);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_permissionsVMList);

            //Setup CRMMappingController
            _NavisionMappingController = new NavisionMappingController(mockSaluationFactoryResult.Result.Object,
                                            mockProductVariantFactoryResult.Result.Object,
                                            mockPaymentMethodFactoryResult.Result.Object,
                                             mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _NavisionMappingController.Index("");
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as CRMMappingVM;

            //Get Actual Count
            _salutationVMListCount = _salutationVMList.Count;
            _productVariantVMListCount = _productVariantVMList.Count;
            _paymentMethodVMListCount = _paymentMethodVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    

            Assert.IsInstanceOf(typeof(CRMMappingVM), dataResult); //Test if returns ok response               
            Assert.AreEqual(dataResult.SalutationVM.Count, 0); //Test if returns equal count for Salutation
            Assert.AreEqual(dataResult.ProductVariantVM.Count, 0); //Test if returns equal count for Product Variant
            Assert.AreEqual(dataResult.PaymentMethodVM.Count, 0); //Test if returns equal count for Salutation 
        }

        #endregion
    }
}