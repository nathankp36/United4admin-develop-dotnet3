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
    public class AdyenPaymentsControllerTest
    {
        private AdyenPaymentsController _adyenPaymentsController;
        private Mock<ILogger<AdyenPaymentsController>> _mockLogger;
        private MockAdyenPaymentsFactory mockAdyenPaymentsFactory;

        private AdyenTransactionVM _mockResponse;
        private int _AdyenTransactionVMListCount;
        private int _AdyenTransactionVMListMaxId;

        private List<AdyenTransactionVM> _adyenTransactionVMList;
        private ApiResponse _apiReponse;


        [SetUp]
        public void TestInitialize()

        {
            //Arrange
            #region "Arrange"
            //Creating Dummy TransactionItem List
            List<TransactionItemVM> newTransactionsItmModel1 = new List<TransactionItemVM>();
            newTransactionsItmModel1.Add(new TransactionItemVM
            {
                TransactionItemId = 1,
                TransactionId = 2,
                ProductId = 2,
                ProductVariantId = "2",
                ChildId = "2",
                DonationFrequency = 1,
                DonationAmount = 200,
                Quantity = 1
            });
            List<TransactionItemVM> newTransactionsItmModel2 = new List<TransactionItemVM>();
            newTransactionsItmModel2.Add(new TransactionItemVM
            {
                TransactionItemId = 2,
                TransactionId = 3,
                ProductId = 2,
                ProductVariantId = "2",
                ChildId = "2",
                DonationFrequency = 1,
                DonationAmount = 200,
                Quantity = 1
            });
            List<TransactionItemVM> newTransactionsItmModel3 = new List<TransactionItemVM>();
            newTransactionsItmModel3.Add(new TransactionItemVM
            {
                TransactionItemId = 3,
                TransactionId = 4,
                ProductId = 2,
                ProductVariantId = "2",
                ChildId = "2",
                DonationFrequency = 1,
                DonationAmount = 200,
                Quantity = 1
            });
            List<TransactionItemVM> newTransactionsItmModel4 = new List<TransactionItemVM>();
            newTransactionsItmModel4.Add(new TransactionItemVM
            {
                TransactionItemId = 4,
                TransactionId = 5,
                ProductId = 2,
                ProductVariantId = "2",
                ChildId = "2",
                DonationFrequency = 1,
                DonationAmount = 200,
                Quantity = 1
            });

            //Creating Dummy Adyen AdyenTransactionVM List
            _adyenTransactionVMList = new List<AdyenTransactionVM>
                    {
                  new AdyenTransactionVM()
                    {
                       AdyenTransactionId=1,
                        LastTransactionId=2,
                        RecurringToken="GB37BARC20000045555555",
                        BillCycleStartDate= Convert.ToDateTime("2020-08-08"),
                        BillCycleNextDate= Convert.ToDateTime("2020-09-08"),
                        LastPaymentDate=Convert.ToDateTime("2020-08-08"),
                        LastPaymentStatus="A",
                        NoOfRetryAttempts=0,
                         ContactId= 1,
                         Amount=2500,
                         ShopperReference="WVRecTest-2",
                         CurrencyCode="GBP",
                         Supporter = new SupporterVM
            {
                ContactId = 1,
                EmailAddress1 = "MuthuKumar@gmail.com",
                MobilePhone = "987651989",
                FirstName = "Muthu",
                MiddleName = "",
                LastName = "Kumar",
                Address1Line1 = "2/5 MajorStreet",
                Address1City = "London",
                Address1PostalCode = "LA 1DER",
                Address1Country = "UK",
                TaxId = "ASD90",
                DataProcessingConsent = true,
                MarketingCommsConsent = true,
                Salutation = "MR",
                Transactions = new TransactionVM
                {
                    TransactionId = 2,
                    PaymentMethodId = 1,
                    GiftAidOptIn = true,
                    ExternalPaymentId = "ASDF98789",
                    AccountHolder = "Muthu Kumar",
                    SortCode = "200007",
                    AccountNumber = "67890567",
                    ExtractDate = DateTime.Today,
                    Exported = true,
                    TransactionItem = newTransactionsItmModel1
                }
            },
                         Transaction=new TransactionVM
                {
                    TransactionId = 2,
                    PaymentMethodId = 1,
                    GiftAidOptIn = true,
                    ExternalPaymentId = "ASDF98789",
                    AccountHolder = "Muthu Kumar",
                    SortCode = "200007",
                    AccountNumber = "67890567",
                    ExtractDate = DateTime.Today,
                    Exported = true,
                    TransactionItem = newTransactionsItmModel1
                }
                    },
                   new AdyenTransactionVM()
                    {
                      AdyenTransactionId=2,
                        LastTransactionId=3,
                        RecurringToken="GB37BARC20000045555556",
                        BillCycleStartDate= Convert.ToDateTime("2020-08-10"),
                        BillCycleNextDate= Convert.ToDateTime("2020-09-10"),
                        LastPaymentDate=Convert.ToDateTime("2020-08-10"),
                        LastPaymentStatus="A",
                        NoOfRetryAttempts=0,
                         ContactId= 2,
                         Amount=2500,
                         ShopperReference="WVRecTest-2",
                         CurrencyCode="GBP",
                                       Supporter = new SupporterVM
            {
                ContactId = 2,
                EmailAddress1 = "MuthuKumar@gmail.com",
                MobilePhone = "987651989",
                FirstName = "Muthu",
                MiddleName = "",
                LastName = "Kumar",
                Address1Line1 = "2/5 MajorStreet",
                Address1City = "London",
                Address1PostalCode = "LA 1DER",
                Address1Country = "UK",
                TaxId = "ASD90",
                DataProcessingConsent = true,
                MarketingCommsConsent = true,
                Salutation = "MR",
                Transactions = new TransactionVM
                {
                    TransactionId = 3,
                    PaymentMethodId = 1,
                    GiftAidOptIn = true,
                    ExternalPaymentId = "ASDF98789",
                    AccountHolder = "Muthu Kumar",
                    SortCode = "200007",
                    AccountNumber = "67890567",
                    ExtractDate = DateTime.Today,
                    Exported = true,
                    TransactionItem = newTransactionsItmModel2
                }
                   },
                                                   Transaction=new TransactionVM
                {
                    TransactionId = 3,
                    PaymentMethodId = 1,
                    GiftAidOptIn = true,
                    ExternalPaymentId = "ASDF98789",
                    AccountHolder = "Muthu Kumar",
                    SortCode = "200007",
                    AccountNumber = "67890567",
                    ExtractDate = DateTime.Today,
                    Exported = true,
                    TransactionItem = newTransactionsItmModel2
                }
                   },
                     new AdyenTransactionVM()
                    {
                       AdyenTransactionId=3,
                        LastTransactionId=4,
                        RecurringToken="GB37BARC20000045555557",
                        BillCycleStartDate= Convert.ToDateTime("2020-08-12"),
                        BillCycleNextDate= Convert.ToDateTime("2020-09-12"),
                        LastPaymentDate=Convert.ToDateTime("2020-08-12"),
                        LastPaymentStatus="A",
                        NoOfRetryAttempts=0,
                         ContactId= 3,
                         Amount=2500,
                         ShopperReference="WVRecTest-2",
                         CurrencyCode="GBP",
                            Supporter = new SupporterVM
            {
                ContactId = 3,
                EmailAddress1 = "MuthuKumar@gmail.com",
                MobilePhone = "987651989",
                FirstName = "Muthu",
                MiddleName = "",
                LastName = "Kumar",
                Address1Line1 = "2/5 MajorStreet",
                Address1City = "London",
                Address1PostalCode = "LA 1DER",
                Address1Country = "UK",
                TaxId = "ASD90",
                DataProcessingConsent = true,
                MarketingCommsConsent = true,
                Salutation = "MR",
                Transactions = new TransactionVM
                {
                    TransactionId = 4,
                    PaymentMethodId = 1,
                    GiftAidOptIn = true,
                    ExternalPaymentId = "ASDF98789",
                    AccountHolder = "Muthu Kumar",
                    SortCode = "200007",
                    AccountNumber = "67890567",
                    ExtractDate = DateTime.Today,
                    Exported = true,
                    TransactionItem = newTransactionsItmModel3
                }
                   },
                                        Transaction=new TransactionVM
                {
                    TransactionId = 4,
                    PaymentMethodId = 1,
                    GiftAidOptIn = true,
                    ExternalPaymentId = "ASDF98789",
                    AccountHolder = "Muthu Kumar",
                    SortCode = "200007",
                    AccountNumber = "67890567",
                    ExtractDate = DateTime.Today,
                    Exported = true,
                    TransactionItem = newTransactionsItmModel3
                }
                   },
                     new AdyenTransactionVM()
                    {
                        AdyenTransactionId=4,
                        LastTransactionId=5,
                        RecurringToken="GB37BARC20000045555558",
                        BillCycleStartDate= Convert.ToDateTime("2020-08-17"),
                        BillCycleNextDate= Convert.ToDateTime("2020-09-17"),
                        LastPaymentDate=Convert.ToDateTime("2020-08-17"),
                        LastPaymentStatus="A",
                        NoOfRetryAttempts=0,
                         ContactId= 4,
                         Amount=2500,
                         ShopperReference="WVRecTest-2",
                         CurrencyCode="GBP",
                            Supporter = new SupporterVM
            {
                ContactId = 4,
                EmailAddress1 = "MuthuKumar@gmail.com",
                MobilePhone = "987651989",
                FirstName = "Muthu",
                MiddleName = "",
                LastName = "Kumar",
                Address1Line1 = "2/5 MajorStreet",
                Address1City = "London",
                Address1PostalCode = "LA 1DER",
                Address1Country = "UK",
                TaxId = "ASD90",
                DataProcessingConsent = true,
                MarketingCommsConsent = true,
                Salutation = "MR",
                Transactions = new TransactionVM
                {
                    TransactionId = 5,
                    PaymentMethodId = 1,
                    GiftAidOptIn = true,
                    ExternalPaymentId = "ASDF98789",
                    AccountHolder = "Muthu Kumar",
                    SortCode = "200007",
                    AccountNumber = "67890567",
                    ExtractDate = DateTime.Today,
                    Exported = true,
                    TransactionItem = newTransactionsItmModel4
                }
                   },
                                        Transaction=new TransactionVM
                {
                    TransactionId = 5,
                    PaymentMethodId = 1,
                    GiftAidOptIn = true,
                    ExternalPaymentId = "ASDF98789",
                    AccountHolder = "Muthu Kumar",
                    SortCode = "200007",
                    AccountNumber = "67890567",
                    ExtractDate = DateTime.Today,
                    Exported = true,
                    TransactionItem = newTransactionsItmModel4
                }
                     }
            };

            //logger
            _mockLogger = new Mock<ILogger<AdyenPaymentsController>>();

            // Mock the API Response 
            _mockResponse = new AdyenTransactionVM();

            #endregion
            //Setup mockAdyenPaymentsFactory
            mockAdyenPaymentsFactory = new MockAdyenPaymentsFactory();

            //logger
            _mockLogger = new Mock<ILogger<AdyenPaymentsController>>();
            // ApiResponse
            _apiReponse = new ApiResponse();
        }

        #region "Index Action"
        /// <summary>
        /// Can we Load all LoadAdyenPayments?
        /// </summary>
        [Test]
        public void LoadAdyenPayments_Index_Valid()
        {
            //Arrange
            // Loading all AdyenPaymentsVM
            var mockAdyenPaymentsactoryResult = new MockAdyenPaymentsFactory().MockLoadList(_adyenTransactionVMList);

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockAdyenPaymentsactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _adyenPaymentsController.Index("", "", "", DateTime.Today, DateTime.Today);
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<AdyenTransactionVM>;

            //Get Actual Count
            _AdyenTransactionVMListCount = _adyenTransactionVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsInstanceOf(typeof(List<AdyenTransactionVM>), dataResult); //Test if returns ok response               
            Assert.AreEqual(dataResult.Count, _AdyenTransactionVMListCount); //Test if returns equal count               
        }

        /// <summary>
        /// If we have no records in LoadAdyenPayments?
        /// </summary>
        [Test]
        public void LoadAdyenPayments_Index_Invalid()
        {
            //Arrange
            // Loading all AdyenPaymentsVM
            var mockAdyenPaymentsactoryResult = new MockAdyenPaymentsFactory().MockLoadList(null);

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockAdyenPaymentsactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _adyenPaymentsController.Index("", "", "", DateTime.Today, DateTime.Today);
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<AdyenTransactionVM>;

            //Get Actual Count
            _AdyenTransactionVMListCount = _adyenTransactionVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult.Count, 0); //Test if returns equal count             
        }

        /// <summary>
        /// Can we Load all LoadAdyenPayments with searchByEmail?
        /// </summary>
        [Test]
        public void LoadAdyenPayments_Index_SearchByEmail_Valid()
        {
            //Arrange
            // Loading all AdyenPaymentsVM
            var mockAdyenPaymentsactoryResult = new MockAdyenPaymentsFactory().MockLoadListSearch(_adyenTransactionVMList, "MuthuKumar@gmail.com", "EmailId", DateTime.Today, DateTime.Today);

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockAdyenPaymentsactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _adyenPaymentsController.Index("", "MuthuKumar@gmail.com", "EmailId", DateTime.Today, DateTime.Today);
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<AdyenTransactionVM>;

            //Get Actual Count
            _AdyenTransactionVMListCount = _adyenTransactionVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsInstanceOf(typeof(List<AdyenTransactionVM>), dataResult); //Test if returns ok response               
            Assert.AreEqual(dataResult.Count, _AdyenTransactionVMListCount); //Test if returns equal count               
        }

        /// <summary>
        /// If we have no records in LoadAdyenPayments with searchByEmail?
        /// </summary>
        [Test]
        public void LoadAdyenPayments_Index_SearchByEmail_Invalid()
        {
            //Arrange
            // Loading all AdyenPaymentsVM
            var mockAdyenPaymentsactoryResult = new MockAdyenPaymentsFactory().MockLoadListSearch(null, "MuthuKumar@gmail.com", "EmailId", DateTime.Today, DateTime.Today);

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockAdyenPaymentsactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _adyenPaymentsController.Index("", "MuthuKumar@gmail.com", "EmailId", DateTime.Today, DateTime.Today);
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<AdyenTransactionVM>;

            //Get Actual Count
            _AdyenTransactionVMListCount = _adyenTransactionVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult.Count, 0); //Test if returns equal count             
        }

        /// <summary>
        /// Can we Load all LoadAdyenPayments with searchByName?
        /// </summary>
        [Test]
        public void LoadAdyenPayments_Index_SearchByName_Valid()
        {
            //Arrange
            // Loading all AdyenPaymentsVM
            var mockAdyenPaymentsactoryResult = new MockAdyenPaymentsFactory().MockLoadListSearch(_adyenTransactionVMList, "MuthuKumar", "Name", DateTime.Today, DateTime.Today);

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockAdyenPaymentsactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _adyenPaymentsController.Index("", "MuthuKumar", "Name", DateTime.Today, DateTime.Today);
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<AdyenTransactionVM>;

            //Get Actual Count
            _AdyenTransactionVMListCount = _adyenTransactionVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsInstanceOf(typeof(List<AdyenTransactionVM>), dataResult); //Test if returns ok response               
            Assert.AreEqual(dataResult.Count, _AdyenTransactionVMListCount); //Test if returns equal count               
        }

        /// <summary>
        /// If we have no records in LoadAdyenPayments with searchByName?
        /// </summary>
        [Test]
        public void LoadAdyenPayments_Index_SearchByName_Invalid()
        {
            //Arrange
            // Loading all AdyenPaymentsVM
            var mockAdyenPaymentsactoryResult = new MockAdyenPaymentsFactory().MockLoadListSearch(null, "MuthuKumar", "Name", DateTime.Today, DateTime.Today);

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockAdyenPaymentsactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _adyenPaymentsController.Index("", "MuthuKumar", "Name", DateTime.Today, DateTime.Today);
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<AdyenTransactionVM>;

            //Get Actual Count
            _AdyenTransactionVMListCount = _adyenTransactionVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult.Count, 0); //Test if returns equal count             
        }

        /// <summary>
        /// Can we Load all LoadAdyenPayments with searchByTransactionRange?
        /// </summary>
        [Test]
        public void LoadAdyenPayments_Index_SearchByTransactionDate_Valid()
        {
            //Arrange
            // Loading all AdyenPaymentsVM
            var mockAdyenPaymentsactoryResult = new MockAdyenPaymentsFactory().MockLoadListSearch(_adyenTransactionVMList, 
                "", "TransactionDate", Convert.ToDateTime("2020-08-08"), DateTime.Today);

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockAdyenPaymentsactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _adyenPaymentsController.Index("", "", "TransactionDate", Convert.ToDateTime("2020-08-08"), DateTime.Today);
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<AdyenTransactionVM>;

            //Get Actual Count
            _AdyenTransactionVMListCount = _adyenTransactionVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsInstanceOf(typeof(List<AdyenTransactionVM>), dataResult); //Test if returns ok response               
            Assert.AreEqual(dataResult.Count, _AdyenTransactionVMListCount); //Test if returns equal count               
        }

        /// <summary>
        /// If we have no records in LoadAdyenPayments with searchByTransactionRange?
        /// </summary>
        [Test]
        public void LoadAdyenPayments_Index_SearchByTransactionDate_Invalid()
        {
            //Arrange
            // Loading all AdyenPaymentsVM
            var mockAdyenPaymentsactoryResult = new MockAdyenPaymentsFactory().MockLoadListSearch(null, 
                "", "TransactionDate", Convert.ToDateTime("2020-08-08"), DateTime.Today);

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockAdyenPaymentsactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _adyenPaymentsController.Index("", "", "TransactionDate", Convert.ToDateTime("2020-08-08"), DateTime.Today);
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<AdyenTransactionVM>;

            //Get Actual Count
            _AdyenTransactionVMListCount = _adyenTransactionVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult.Count, 0); //Test if returns equal count             
        }


        /// <summary>
        /// Can we Load all LoadAdyenPayments with searchByDateRange?
        /// </summary>
        [Test]
        public void LoadAdyenPayments_Index_SearchByDateRange_Valid()
        {
            //Arrange
            // Loading all AdyenPaymentsVM
            var mockAdyenPaymentsactoryResult = new MockAdyenPaymentsFactory().MockLoadListSearch(_adyenTransactionVMList, 
                "", "DateRange", Convert.ToDateTime("2020-08-08"), Convert.ToDateTime("2020-08-12"));

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockAdyenPaymentsactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _adyenPaymentsController.Index("", "", "DateRange", Convert.ToDateTime("2020-08-08"), Convert.ToDateTime("2020-08-12"));
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<AdyenTransactionVM>;

            //Get Actual Count
            _AdyenTransactionVMListCount = _adyenTransactionVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsInstanceOf(typeof(List<AdyenTransactionVM>), dataResult); //Test if returns ok response               
            Assert.AreEqual(dataResult.Count, _AdyenTransactionVMListCount); //Test if returns equal count               
        }

        /// <summary>
        /// If we have no records in LoadAdyenPayments with searchByDateRange?
        /// </summary>
        [Test]
        public void LoadAdyenPayments_Index_SearchByDateRange_Invalid()
        {
            //Arrange
            // Loading all AdyenPaymentsVM
            var mockAdyenPaymentsactoryResult = new MockAdyenPaymentsFactory().MockLoadListSearch(null, "", 
                "DateRange", Convert.ToDateTime("2020-08-08"), Convert.ToDateTime("2020-08-12"));

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockAdyenPaymentsactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _adyenPaymentsController.Index("", "", "DateRange", Convert.ToDateTime("2020-08-08"), Convert.ToDateTime("2020-08-12"));
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<AdyenTransactionVM>;

            //Get Actual Count
            _AdyenTransactionVMListCount = _adyenTransactionVMList.Count;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult.Count, 0); //Test if returns equal count             
        }
        #endregion

        #region "Create Action"
        ///// <summary>
        ///// Can we insert a new SignUpEventVM?
        ///// </summary>
        [Test]
        public void AdyenPayments_CreateSave_Valid()
        {
            //Arrange
            //Get Actual Count
            _AdyenTransactionVMListCount = _adyenTransactionVMList.Count;

            //Get Max ID from List
            _AdyenTransactionVMListMaxId = Convert.ToInt32(_adyenTransactionVMList.Max(x => x.AdyenTransactionId).ToString()) + 1;

            // Create a new AdyenTransaction Model
            AdyenTransactionVM newAdyenTransactionVM = new AdyenTransactionVM
            {
                AdyenTransactionId = 5,
                LastTransactionId = 9,
                RecurringToken = "GB37BARC2000004666",
                BillCycleStartDate = Convert.ToDateTime("2020-09-09"),
                BillCycleNextDate = Convert.ToDateTime("2020-10-09"),
                LastPaymentDate = Convert.ToDateTime("2020-09-09"),
                LastPaymentStatus = "A",
                NoOfRetryAttempts = 0,
                ContactId = 1,
                Amount = 2500,
                ShopperReference = "WVRecTest-2",
                CurrencyCode = "GBP"
            };
            _apiReponse.Message = "WV DDL Transaction created Successfully";
            _apiReponse.ResponseObject = (true) as object;
            _apiReponse.Success = true;

            // Loading all AdyenTransactionVM
            var mockSignupEventFactoryResult = new MockAdyenPaymentsFactory().MockCreate(_apiReponse, _adyenTransactionVMList, newAdyenTransactionVM);

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockSignupEventFactoryResult.Result.Object, _mockLogger.Object);

            //ACT
            #region "ACT"

            // try saving our new SignUpEventVM
            var actionResult = _adyenPaymentsController.Edit(newAdyenTransactionVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;

            #endregion

            //Get Expected Count
            var expectedCount = _adyenTransactionVMList.Count;

            // Verify that our new SignUpEventVM has been created
            AdyenTransactionVM testAdyenTransactionVM = mockAdyenPaymentsFactory.MockFindById(_adyenTransactionVMList, _AdyenTransactionVMListMaxId);

            // Assert
            #region "Assert"            
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response    
            // Check both actual and expected count is same after insert        
            Assert.AreEqual(expectedCount, _AdyenTransactionVMListCount + 1); // Verify the expected Number post-insert
            Assert.AreEqual(_AdyenTransactionVMListMaxId, testAdyenTransactionVM.AdyenTransactionId); // Verify it has the expected AdyenTransactionVM ContactId
            #endregion
        }

        /// <summary>
        /// Can we insert a new SignUpEventVM?
        /// </summary>
        [Test]
        public void AdyenPayments_CreateSave_DuplicateData()
        {
            //Arrange
            //Get Actual Count
            _AdyenTransactionVMListCount = _adyenTransactionVMList.Count;

            //Get Max ID from List
            _AdyenTransactionVMListMaxId = Convert.ToInt32(_adyenTransactionVMList.Max(x => x.AdyenTransactionId).ToString()) + 1;

            // Create a new AdyenTransactionVM, not I do not supply an id
            AdyenTransactionVM newAdyenTransactionVM = new AdyenTransactionVM
            {
                AdyenTransactionId = 4,
                LastTransactionId = 5,
                RecurringToken = "GB37BARC20000045555558",
                BillCycleStartDate = Convert.ToDateTime("2020-08-17"),
                BillCycleNextDate = Convert.ToDateTime("2020-09-17"),
                LastPaymentDate = Convert.ToDateTime("2020-08-17"),
                LastPaymentStatus = "A",
                NoOfRetryAttempts = 0,
                ContactId = 4,
                Amount = 2500,
                ShopperReference = "WVRecTest-2",
                CurrencyCode = "GBP"
            };
            _apiReponse.Message = "WV DDL Transaction Failed to create";
            _apiReponse.ResponseObject = (false) as object;
            _apiReponse.Success = false;

            // Loading all AdyenTransactionVM
            var mockSignupEventFactoryResult = new MockAdyenPaymentsFactory().MockCreate(_apiReponse, _adyenTransactionVMList, newAdyenTransactionVM);

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockSignupEventFactoryResult.Result.Object, _mockLogger.Object);

            //ACT
            #region "ACT"

            // try saving our new SignUpEventVM
            var actionResult = _adyenPaymentsController.Edit(newAdyenTransactionVM);
            var actionResponse = actionResult.Result as RedirectToActionResult;

            #endregion

            //Get Expected Count
            var expectedCount = _adyenTransactionVMList.Count;

            // Assert
            #region "Assert"            
            Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response      
            // Check both actual and expected count is same after insert        
            Assert.AreNotEqual(expectedCount, _AdyenTransactionVMListCount + 1); // Verify the expected Number post-insert
            #endregion
        }
        #endregion

        #region "Edit Action "

        /// <summary>
        /// while we Load a SignUp Event By Id for edit action?
        /// </summary>
        [Test]
        public void AdyenPayments_EditLoad_Valid()
        {
            //Arrange

            // Get that AdyenTransactionVM for id : 2
            AdyenTransactionVM testAdyenTransactionVM = mockAdyenPaymentsFactory.MockFindById(_adyenTransactionVMList, 2);

            // Loading all AdyenPaymentsVM
            var mockAdyenPaymentsactoryResult = new MockAdyenPaymentsFactory().MockLoad(testAdyenTransactionVM);

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockAdyenPaymentsactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _adyenPaymentsController.Edit(2);
            var actionResponse = actionResult.Result as ViewResult;
            _mockResponse = actionResponse.ViewData.Model as AdyenTransactionVM;


            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.IsNotNull(_mockResponse); // Test if null
            Assert.IsInstanceOf(typeof(AdyenTransactionVM), _mockResponse); //Test if returns ok response   
        }

        /// <summary>
        /// When we Load a SignUp Event By Id for edit action if returns http not found result?
        /// </summary>
        [Test]
        public void AdyenPayments_EditLoad_InValid()
        {
            //Arrange

            // Get that AdyenTransactionVM for id : 2
            AdyenTransactionVM testAdyenTransactionVM = mockAdyenPaymentsFactory.MockFindById(_adyenTransactionVMList, 5);

            // Loading all AdyenPaymentsVM
            var mockAdyenPaymentsactoryResult = new MockAdyenPaymentsFactory().MockLoad(testAdyenTransactionVM);

            //Setup AdyenPaymentsController
            _adyenPaymentsController = new AdyenPaymentsController(mockAdyenPaymentsactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _adyenPaymentsController.Edit(5);
            var actionResponse = actionResult.Result as NotFoundResult;

            //Assert  
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResponse); //Test if returns ok response  
        }

        #endregion
    }
}