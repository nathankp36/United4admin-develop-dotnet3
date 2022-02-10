using United4Admin.UnitTests.Mocks.ApiClientFactory;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.BLL;
using United4Admin.WebApplication.Controllers;
using United4Admin.WebApplication.Models;
using United4Admin.WebApplication.ViewModels;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text;
using System.Collections;
using System.Xml.Linq;

namespace United4Admin.UnitTests.Controller
{
    [TestFixture]
    class CRMQuarantineControllerTest
    {
        private CRMQuarantineController _cRMQuarantineController;
        private ApiResponse _mockResponse;

        public List<int> validTransactions = new List<int>();
        private List<CRMTransactionModel> _CRMTransactionVMList;
        private ApiResponse _apiReponse;
        private Mock<ILogger<CRMQuarantineController>> _mockLogger;
        private readonly IZipFileHelper _zipFileHelper;

        [SetUp]
        public void SetUp()

        {
            //Arrange
            #region "Arrange"
            //Creating Dummy CRMTransactionVM List
            _CRMTransactionVMList = new List<CRMTransactionModel>
                    {
                        new CRMTransactionModel {
                            Contact = new ContactModel{
                            ContactId="28c45ebc-c286-4ee9-9157-e2234fa4ddc6",
                            EmailAddress1= "CRMTesting@worldvision.org.uk",
                            MobilePhone= "17878789",
                            Salutation= "Dr",
                            FirstName= "Ramlith",
                            MiddleName= null,
                            LastName= "Khan",
                            Address1Line1= "16 Harrington Drive, Westminster",
                            Address1Line2= null,
                            BirthDate= null,
                            GenderCode= null,
                            Address1City= "Nottingham",
                            Address1PostalCode= "NG7 1JR",
                            Address1Country= "GB",
                            CustomerTypeCode= null,
                            CreatedOn= DateTime.Now,
                            },
                            Order = new OrderModel{
                             SalesOrderId= "6692fc8a-e81c-4f22-98e0-1a3dc55b1218",
                             ContactId= "28c45ebc-c286-4ee9-9157-e2234fa4ddc6",
                             WvCampaignCode= "CAMP14569",
                             DateFulfilled= DateTime.Now,
                             WvExtractStatus= true,
                             WvExtractDate= DateTime.Now
                            },
                            OrderProduct = new OrderProductModel
                            {
                                 SalesOrderId= "6692fc8a-e81c-4f22-98e0-1a3dc55b1218",
                                SalesOrderDetailId= "2d11c31b-4e19-426c-9f97-ed54dc628b8c",
                                WvProductId= 2,
                                ProductTypeCodedisplay= "VidgeTestChildRescueAppealForm",
                                PricePerUnit= 50,
                                Quantity= 1,
                                WvChildId= null,
                                WvTaxId= "3453454354",
                                WvTaxConsentOptIn= false

                            },
                            Preference = new List<PreferenceModel>
                            {
                                new PreferenceModel
                                {
                                    PreferenceId= "76ad27e0-dc68-4eef-aa4f-8818c82a0c04",
                                      SalesOrderId= "6692fc8a-e81c-4f22-98e0-1a3dc55b1218",
                                      WvPreferenceTypeId= 5,
                                      StartDate= DateTime.Now,
                                      EndDate= null
                                }

                            },
                            PaymentMethod = new PaymentMethodModel{
                             PaymentMethodId= "163de0a4-d1c4-4dc5-a82d-d78fbe5fc977",
                                Type= 2,
                                ContactId= "28c45ebc-c286-4ee9-9157-e2234fa4ddc6",
                                PaymentScheduleId= "58b14ea6-674e-4711-8121-eb87b28e9865",
                                Name= null
                            },
                            PaymentSchedule = new PaymentScheduleModel
                            {
                            PaymentScheduleId= "58b14ea6-674e-4711-8121-eb87b28e9865",
                            TransactionCurrencyId= "f899be25-5746-4488-8e1e-009e0f5b134c",
                            CurrencyCode= null,
                            LastTransactionId= 0,
                            ReceiptOnContactId= "28c45ebc-c286-4ee9-9157-e2234fa4ddc6",
                            FirstPaymentDate= DateTime.Now,
                            NextPaymentDate= DateTime.Now,
                            NextPaymentAmount= 50,
                            Frequency= 1,
                            RecurringAmount= 50,
                            StateCode= 1,
                            WvSalesOrderDetailId= "2d11c31b-4e19-426c-9f97-ed54dc628b8c",
                            WvAccountNumber= "",
                            WvPaymentRetryAttempts= 0,
                            WvAccountHolder= "",
                            WvIban= "GB60BARC20000055779911",
                            WvSortCode= "",
                            WvExternalProviderSubscriptionId= "",
                            WvPaymentProviderId= 1
                            },
                            Transaction = new TransactionModel {
                                TransactionId = "50711b62-30b0-49be-96c7-48b6edb6287c",
                                ReceiptOnContactId = "28c45ebc-c286-4ee9-9157-e2234fa4ddc6",
                                CreatedOn = DateTime.Now,
                                TransactionCurrencyId =  "f899be25-5746-4488-8e1e-009e0f5b134c",
                                TransactionPaymentScheduleId= "58b14ea6-674e-4711-8121-eb87b28e9865",
                                StateCode = 1,
                                Amount = 50 },
                            WvchildLink = new WVChildLinkModel{
                            WvChildId = null,
                            WvChildStatus = null,
                            WvParentContactId = null,
                            WvSponsorshipEndDate = DateTime.Now,
                            WvSponsorshipStartDate = DateTime.Now
                            },
                            CRMMapping = new CRMMappingModel {
                                Navision = new CRMMappingModel.NavisionModel
                                {
                                salutation = "4",
                                paymentMethodType= "Credit",
                                paymentMethodName= "Cards",
                                productVariantName= "",
                                incidentType= "",
                                purposeCode= "",
                                pledgeType= "",
                                incidentTypeName ="",
                                productCode= "",
                                countryCode= "GBR",
                                frequency= "MONTH"
                                },
                                Simma =new CRMMappingModel.SimmaModel
                                {
                                    partnerTypeId=4,
                                    motivationId=8,
                                    designationId=82,
                                    pledgeTypeId=7,
                                    paymentMethodId=1,
                                    emailTypeId=2,
                                    phoneTypeId=3,
                                    batchTypeId=1,
                                    countryCode="GBR",
                                    frequency="1"
                                }
                            },
                            CRMTransaction = new CRMTransactionsModel
                            { Id = 1 }
                        }
            };

            validTransactions.Add(1);

            // Mock the API Response 
            _mockResponse = new ApiResponse();

            #endregion

            //logger
            _mockLogger = new Mock<ILogger<CRMQuarantineController>>();

            // ApiResponse
            _apiReponse = new ApiResponse()
            {
                Success = true
            };

            //Mock the file sent via Request.Forms.Files using the httpContext
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "test.jpg");
            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
        }

        #region "Load Action "
        /// <summary>
        /// while we Load CRMQuarantineExportALL By CRMExtractParameterModelVM?
        /// </summary>
        [Test]
        public void CRMQuarantine_ExportALLNavisionTransData_Valid()
        {
            //Arrange
            AppConfigValues.crmMappingPath = "navisionmapping";
            string crmStringArr = string.Empty;
            XDocument xDoc = GetXMLDocument(_CRMTransactionVMList);
            crmStringArr = ConvertCSV(xDoc);
            CRMExtractParameterModelVM cRMQuarantineParameterModel = new CRMExtractParameterModelVM();
            cRMQuarantineParameterModel.CRMType = "ALL";
            cRMQuarantineParameterModel.StartDate = DateTime.Now.AddDays(-10);
            cRMQuarantineParameterModel.EndDate = DateTime.Now.AddDays(10);
            AppConfigValues.HostedCountry = "GB";
            // Loading all CRMQuarantineVM
            var mockCRMQuarantineFactoryResult = new MockCRMQuarantineFactory().MockLoad(_apiReponse, crmStringArr, cRMQuarantineParameterModel);

            //Setup CRMQuarantineController
            _cRMQuarantineController = new CRMQuarantineController(mockCRMQuarantineFactoryResult.Result.Object, _zipFileHelper, _mockLogger.Object);

            //Act  
            var actionResult = _cRMQuarantineController.ExportCRMQuarantineData(cRMQuarantineParameterModel);
            var actionResponse = actionResult.Result as FileContentResult;

            //Assert  
            Assert.IsInstanceOf(typeof(FileContentResult), actionResponse); //Test if returns ok response    
        }

        /// <summary>
        /// while we Load CRMQuarantineExportALL By CRMExtractParameterModelVM?
        /// </summary>
        [Test]
        public void CRMQuarantine_ExportALLSimmaTransData_Valid()
        {
            //Arrange
            AppConfigValues.crmMappingPath = "simmamapping";
            string crmStringArr = string.Empty;
            XDocument xDoc = GetXMLDocument(_CRMTransactionVMList);
            crmStringArr = ConvertCSV(xDoc);
            CRMExtractParameterModelVM cRMQuarantineParameterModel = new CRMExtractParameterModelVM();
            cRMQuarantineParameterModel.CRMType = "ALL";
            cRMQuarantineParameterModel.StartDate = DateTime.Now.AddDays(-10);
            cRMQuarantineParameterModel.EndDate = DateTime.Now.AddDays(10);
            AppConfigValues.HostedCountry = "GB";
            // Loading all CRMQuarantineVM
            var mockCRMQuarantineFactoryResult = new MockCRMQuarantineFactory().MockLoad(_apiReponse, crmStringArr, cRMQuarantineParameterModel);

            //Setup CRMQuarantineController
            _cRMQuarantineController = new CRMQuarantineController(mockCRMQuarantineFactoryResult.Result.Object, _zipFileHelper, _mockLogger.Object);

            //Act  
            var actionResult = _cRMQuarantineController.ExportCRMQuarantineData(cRMQuarantineParameterModel);
            var actionResponse = actionResult.Result as FileContentResult;

            //Assert  
            Assert.IsInstanceOf(typeof(FileContentResult), actionResponse); //Test if returns ok response    
        }

        /// <summary>
        /// while we Load CRMQuarantineExportALL By CRMExtractParameterModelVM if returns http not found result?
        /// </summary>
        [Test]
        public void CRMQuarantineExtract_ExportData_NoRecords()
        {
            //Arrange
            CRMExtractParameterModelVM cRMQuarantineParameterModel = new CRMExtractParameterModelVM();
            cRMQuarantineParameterModel.CRMType = "";
            cRMQuarantineParameterModel.StartDate = DateTime.Now.AddDays(-10);
            cRMQuarantineParameterModel.EndDate = DateTime.Now.AddDays(10);
            ApiResponse apiResponse = new ApiResponse();

            // Loading all CRMExtractVM
            var mockCRMExtractFactoryResult = new MockCRMQuarantineFactory().MockLoad(apiResponse, null, cRMQuarantineParameterModel);

            //Setup CRMExtractController
            _cRMQuarantineController = new CRMQuarantineController(mockCRMExtractFactoryResult.Result.Object, _zipFileHelper, _mockLogger.Object);

            //Act  
            var actionResult = _cRMQuarantineController.ExportCRMQuarantineData(cRMQuarantineParameterModel);
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<CRMTransactionModel>;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult, null); //Test if returns equal count             
        }
        #endregion

        #region Helper methods
        //Convert List into XML
        private XDocument GetXMLDocument(List<CRMTransactionModel> mappedQuarantine)
        {
            try
            {
                XDocument xDocument = new XDocument();
                xDocument.Declaration = new XDeclaration("0.1", "utf-8", "yes");
                //If united4admin is configured to Navision
                if (AppConfigValues.crmMappingPath.ToLower() == "navisionmapping")
                {
                    XElement xElement = new XElement("CRMQuarantineExtract",
                                          from quarantine in mappedQuarantine
                                          select new XElement("CRMQuarantine",
                                                         new XElement("WebReferenceID", quarantine.CRMTransaction != null ? quarantine.CRMTransaction.Id : 0),
                                                         new XElement("FirstName", quarantine.Contact != null ? quarantine.Contact.FirstName : ""),
                                                         new XElement("MiddleName", quarantine.Contact != null ? quarantine.Contact.MiddleName : ""),
                                                         new XElement("Surname", quarantine.Contact != null ? quarantine.Contact.LastName : ""),
                                                         new XElement("Street", quarantine.Contact != null ? quarantine.Contact.Address1Line1 : ""),
                                                         new XElement("HouseNo", ""),
                                                         new XElement("Address2", quarantine.Contact != null ? quarantine.Contact.Address1Line2 : ""),
                                                         new XElement("PostCode", quarantine.Contact != null ? quarantine.Contact.Address1PostalCode : ""),
                                                         new XElement("City", quarantine.Contact != null ? quarantine.Contact.Address1City : ""),
                                                         new XElement("CountryCode", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.countryCode : ""),
                                                         new XElement("Email", quarantine.Contact != null ? quarantine.Contact.EmailAddress1 : ""),
                                                         new XElement("MobilePhoneNo", quarantine.Contact != null ? quarantine.Contact.MobilePhone : ""),
                                                         new XElement("ChildCountryCode", quarantine.OrderProduct != null ? quarantine.OrderProduct.WvChildId : ""),
                                                         new XElement("ActionCode", quarantine.Order != null ? quarantine.Order.WvCampaignCode : ""),
                                                         new XElement("PaymentMethod", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.paymentMethodName : ""),
                                                         new XElement("BankAccountNo", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.WvAccountNumber : ""),
                                                         new XElement("IBAN", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.WvIban : ""),
                                                         new XElement("CreditCardHolder", quarantine.Contact != null ? quarantine.Contact.FirstName + " " + quarantine.Contact.LastName : ""),
                                                         new XElement("Salutation", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.salutation : ""),
                                                         new XElement("BankAccountHolder", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.WvAccountHolder : ""),
                                                         new XElement("ProjectID", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.productCode : ""),
                                                         new XElement("ChildSequenceNo", quarantine.WvchildLink == null ? "" : (string.IsNullOrEmpty(quarantine.WvchildLink.WvChildId) ? "" : (quarantine.WvchildLink.WvChildId.Substring(quarantine.WvchildLink.WvChildId.Length - 4)))),
                                                         new XElement("Gift", ""), //This will be used later when Gift shop is available
                                                         new XElement("ExternalReferenceNumber", quarantine.Transaction != null ? (string.IsNullOrEmpty(quarantine.Transaction.WvPaymentProviderTransactionId) ? "" : (quarantine.Transaction.WvPaymentProviderTransactionId.Length > 25 ? quarantine.Transaction.WvPaymentProviderTransactionId.Substring(quarantine.Transaction.WvPaymentProviderTransactionId.Length - 25) : quarantine.Transaction.WvPaymentProviderTransactionId)) : ""),
                                                         new XElement("ProductCode", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.productCode : ""),
                                                         new XElement("IncidentType", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.incidentType : ""),
                                                         new XElement("AmountPerPeriod", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.frequency : ""),
                                                         new XElement("BillingPeriod", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.frequency : ""),
                                                         new XElement("PledgeType", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.pledgeType : ""),
                                                         new XElement("CatalogueID", ""), //This will be used later when Gift shop is available
                                                         new XElement("CatalogueQuantity", ""), //This will be used later when Gift shop is available
                                                         new XElement("PurposeCode", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.purposeCode : ""),
                                                         new XElement("Birthdate", ""),
                                                         new XElement("NoOfBelongingIncidents", "") //This will be used later when Gift shop is available
                                                     ));
                    xDocument.Add(xElement);
                }
                else
                {
                    XElement xElement = new XElement("CRMQuarantineExtract",
                                          from quarantine in mappedQuarantine
                                          select new XElement("CRMQuarantine",
                                                         new XElement("Salutation", quarantine.Contact != null ? quarantine.Contact.Salutation : ""),
                                                         new XElement("FirstName", quarantine.Contact != null ? quarantine.Contact.FirstName : ""),
                                                         new XElement("MiddleName", quarantine.Contact != null ? quarantine.Contact.MiddleName : ""),
                                                         new XElement("LastName", quarantine.Contact != null ? quarantine.Contact.LastName : ""),
                                                         new XElement("Street1", quarantine.Contact != null ? quarantine.Contact.Address1Line1 : ""),
                                                         new XElement("Street2", quarantine.Contact != null ? quarantine.Contact.Address1Line2 : ""),
                                                         new XElement("Street3", ""),
                                                         new XElement("City", quarantine.Contact != null ? quarantine.Contact.Address1City : ""),
                                                         new XElement("CountryCode", quarantine.Contact != null ? quarantine.Contact.Address1Country : ""),
                                                         new XElement("PostalCode", quarantine.Contact != null ? quarantine.Contact.Address1PostalCode : ""),
                                                         new XElement("Emails", quarantine.Contact != null ? quarantine.Contact.EmailAddress1 : ""),
                                                         new XElement("Phones", quarantine.Contact != null ? quarantine.Contact.MobilePhone : ""),
                                                         new XElement("TaxId", quarantine.OrderProduct != null ? quarantine.OrderProduct.WvTaxId : ""),
                                                         new XElement("ChildId", quarantine.WvchildLink == null ? "" : (string.IsNullOrEmpty(quarantine.WvchildLink.WvChildId) ? "" : (quarantine.WvchildLink.WvChildId.Substring(quarantine.WvchildLink.WvChildId.Length - 4)))),
                                                         new XElement("Amount", quarantine.Transaction != null ? quarantine.Transaction.Amount : 0),
                                                         new XElement("IBAN", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.WvIban : ""),
                                                         new XElement("BankAccount", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.WvAccountNumber : ""),
                                                         new XElement("BankNumber", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.WvSortCode : ""),
                                                         new XElement("LastDebitDate", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.FirstPaymentDate : (DateTime?)null),
                                                         new XElement("NextDebitDate", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.NextPaymentDate : (DateTime?)null),
                                                         new XElement("PartnerTypeId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.partnerTypeId : 0),
                                                         new XElement("MotivationId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.motivationId : 0),
                                                         new XElement("DesignationId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.designationId : 0),
                                                         new XElement("PledgeTypeId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.pledgeTypeId : 0),
                                                         new XElement("PaymentMethodId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.paymentMethodId : 0),
                                                         new XElement("EmailTypeId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.emailTypeId : 0),
                                                         new XElement("PhoneTypeId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.phoneTypeId : 0),
                                                         new XElement("BatchTypeId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.batchTypeId : 0),
                                                         new XElement("CountryCode", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.countryCode : ""),
                                                         new XElement("Frequency", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.frequency : "")
                                                     ));
                    xDocument.Add(xElement);
                }

                return xDocument;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Convert XML into CSV
        private static string ConvertCSV(XDocument xDocument)
        {
            var headers =
                 xDocument
                    .Descendants("CRMQuarantine")
                    .First()
                    .Elements().Select(e => e.Name.LocalName);

            var delimiter = ",";

            var entries =
                xDocument
                    .Descendants("CRMQuarantine")
                    .Select(d => string.Join(delimiter, d.Elements().Select(e => $"\"{e.Value}\"")))
                    .Aggregate(
                        new StringBuilder().AppendLine(string.Join(delimiter, headers)),
                        (current, next) => current.AppendLine(next));

            var csv = entries.ToString();
            return csv;
        }
        #endregion
    }
}
