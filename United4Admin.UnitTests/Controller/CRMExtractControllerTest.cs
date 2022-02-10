using United4Admin.UnitTests.Mocks.ApiClientFactory;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.BLL;
using United4Admin.WebApplication.Controllers;
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

namespace United4Admin.UnitTests.Controllers
{
    [TestFixture]
    public class CRMExtractControllerTest
    {
        private CRMExtractController _cRMExtractController;
        private ActionContext _actionContext;
        private ApiResponse _mockResponse;

        private List<CRMExtractVM> _CRMExtractVMList;
        private List<PermissionsVM> _PermissionsVMList;
        private ApiResponse _apiReponse;
        private Mock<ILogger<CRMExtractController>> _mockLogger;
        private readonly IZipFileHelper _zipFileHelper;

        public IFormFile ConvertedIFormFilePhoto { get; set; }
        [SetUp]
        public void SetUp()

        {
            //Arrange
            #region "Arrange"
            //Creating Dummy CRMExtractVM List
            _CRMExtractVMList = new List<CRMExtractVM>
                    {
                        new CRMExtractVM {
                            RequestType="CB",
                            Title = "",
                            StreetName = "Privet Drive",
                            AddressLine2 = "-",
                            Postcode = "WH23 4TH",
                            EmailAddress = "theboywholived@nottinghamcity.gov.uk",
                            Dataprocessingconsent = true,
                            TaxConsentOptIn = 0,
                            PaymentTransactionID = "",
                            SortCode = "1000",
                            AccountCode = "8078097",
                            CardHolder = "James",
                            TriggerCode = "",
                            AddressLine3 = "",
                            AddressLine4 = "UK",
                            Country = "UK",
                            PhoneNum1 = "979888",
                            Location = "London",
                            AiValue1 = "1",
                            AiValue2 = "232",
                            DirectMailOptIn ="0",
                            EmailOptIn = "1",
                            PhoneOptIn = "0",
                            SMSOptIn = "0",
                            DateOfRequest = DateTime.Now,
                            Forename = "Harry",
                            Surname = "Porter",
                            supporterID = Convert.ToString(1),
                            WvSupporterId = "123445667",
                            Organisation_GroupName = "",
                            Organisation_GroupType = "",
                            gender = "Female",
                            YearOfBirth = "",
                            PhoneNum2 = "",
                            PaymentReference = "",
                            PreferredContinent = "",
                            PreferredCountry = "",
                            PreferredGender = "",
                            PreferredAge = "",
                            Fundraised = 0,
                            Comments = "",
                            ResponseEntity = "",
                            ScheduledPayID = "",
                            FaithAndFamily = "",
                            DirectMailOptOut = "",
                            EmailOptOut = "",
                            SMSOptOut = "",
                            ExternalPaymentToken = "",
                            IBAN = "S987898",
                            ProductID = Convert.ToString(8),
                            Donationamount = 2500,
                            DonationvariantID = "chosen",
                            Donationfrequency = "M",
                            AdrStatusID = "",
                            AdrTypeID = "",
                            Title1Descr = "",
                            Title2Descr = "",
                            MaritalStatusDescr = "",
                            AddDate = "",
                            FamilyName2 = "",
                            OrgName1 = "",
                            OrgName2 = "",
                            OrgDepartmentName = "",
                            OrgRoleDescr = "",
                            PartnershipName = "",
                            StatesProvCode = "",
                            RegionDescr = "",
                            NoMail = "",
                            NoMailID = "",
                            SpokenLanguageCode = "",
                            PrintedLanguageCode = "",
                            ReceiptingID = "",
                            MotivationID = "",
                            ReferenceText = "",
                            PhoneTypeID1 = "",
                            PhoneTypeID2 = "",
                            ChildID = "UASDF789u",
                            CommitmentAmount = Convert.ToString((int)(2500 * 100)),
                            TransactionAmount = "",
                            Exported=false
                        },
                        new CRMExtractVM {
                            RequestType="CB",
                            Title = "",
                            StreetName = "Privet Drive",
                            AddressLine2 = "-",
                            Postcode = "WH23 4TH",
                            EmailAddress = "fred.bloggs@worldvision.org.uk",
                            Dataprocessingconsent = true,
                            TaxConsentOptIn = 0,
                            PaymentTransactionID = "",
                            SortCode = "1000",
                            AccountCode = "8078097",
                            CardHolder = "James",
                            TriggerCode = "",
                            AddressLine3 = "",
                            AddressLine4 = "UK",
                            Country = "UK",
                            PhoneNum1 = "979888",
                            Location = "London",
                            AiValue1 = "1",
                            AiValue2 = "232",
                            DirectMailOptIn ="0",
                            EmailOptIn = "1",
                            PhoneOptIn = "0",
                            SMSOptIn = "0",
                            DateOfRequest = DateTime.Now,
                            Forename = "Harry",
                            Surname = "Porter",
                            supporterID = Convert.ToString(1),
                            WvSupporterId = "123445667",
                            Organisation_GroupName = "",
                            Organisation_GroupType = "",
                            gender = "Female",
                            YearOfBirth = "",
                            PhoneNum2 = "",
                            PaymentReference = "",
                            PreferredContinent = "",
                            PreferredCountry = "",
                            PreferredGender = "",
                            PreferredAge = "",
                            Fundraised = 0,
                            Comments = "",
                            ResponseEntity = "",
                            ScheduledPayID = "",
                            FaithAndFamily = "",
                            DirectMailOptOut = "",
                            EmailOptOut = "",
                            SMSOptOut = "",
                            ExternalPaymentToken = "",
                            IBAN = "S987898",
                            ProductID = Convert.ToString(8),
                            Donationamount = 2500,
                            DonationvariantID = "chosen",
                            Donationfrequency = "M",
                            AdrStatusID = "",
                            AdrTypeID = "",
                            Title1Descr = "",
                            Title2Descr = "",
                            MaritalStatusDescr = "",
                            AddDate = "",
                            FamilyName2 = "",
                            OrgName1 = "",
                            OrgName2 = "",
                            OrgDepartmentName = "",
                            OrgRoleDescr = "",
                            PartnershipName = "",
                            StatesProvCode = "",
                            RegionDescr = "",
                            NoMail = "",
                            NoMailID = "",
                            SpokenLanguageCode = "",
                            PrintedLanguageCode = "",
                            ReceiptingID = "",
                            MotivationID = "",
                            ReferenceText = "",
                            PhoneTypeID1 = "",
                            PhoneTypeID2 = "",
                            ChildID = "UASDF789u",
                            CommitmentAmount = Convert.ToString((int)(2500 * 100)),
                            TransactionAmount = "",
                            Exported=false
                        },
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

            // Mock the API Response 
            _mockResponse = new ApiResponse();

            #endregion

            ////setup for permission
            //var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //logger
            _mockLogger = new Mock<ILogger<CRMExtractController>>();

            // ApiResponse
            _apiReponse = new ApiResponse();

            //Mock the file sent via Request.Forms.Files using the httpContext
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "test.jpg");
            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
        }
        #region "Load Action "

        /// <summary>
        /// while we Load CRMExtractExportALL By CRMExtractParameterModelVM?
        /// </summary>
        [Test]
        public void CRMExtract_ExportALLTransData_Valid()
        {
            //Arrange
            object[] crmList = _CRMExtractVMList.Select(x => x.ToString()).ToArray();
            string[] crmStringArr = ((IEnumerable)crmList).Cast<object>().Select(x => x.ToString()).ToArray();
            CRMExtractParameterModelVM cRMExtractParameterModel = new CRMExtractParameterModelVM();
            cRMExtractParameterModel.CRMType = "ALL";
            cRMExtractParameterModel.StartDate = DateTime.Now.AddDays(-10);
            cRMExtractParameterModel.EndDate = DateTime.Now.AddDays(10);
            AppConfigValues.HostedCountry = "GB";
            // Loading all CRMExtractVM
            var mockCRMExtractFactoryResult = new MockCRMExtractFactory().MockLoad(crmStringArr, cRMExtractParameterModel);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup CRMExtractController
            _cRMExtractController = new CRMExtractController(mockCRMExtractFactoryResult.Result.Object, _zipFileHelper, _mockLogger.Object);

            //Act  
            var actionResult = _cRMExtractController.ExportCRMExtractData(cRMExtractParameterModel);
            var actionResponse = actionResult.Result as FileContentResult;

            //Assert  
            Assert.IsInstanceOf(typeof(FileContentResult), actionResponse); //Test if returns ok response    
        }

        /// <summary>
        /// while we Load CRMExtractExportALL By CRMExtractParameterModelVM if returns http not found result?
        /// </summary>
        [Test]
        public void CRMExtractExtract_ExportData_NoRecords()
        {
            //Arrange
            //Arrange
            CRMExtractParameterModelVM cRMExtractParameterModel = new CRMExtractParameterModelVM();
            cRMExtractParameterModel.CRMType = "";
            cRMExtractParameterModel.StartDate = DateTime.Now.AddDays(-10);
            cRMExtractParameterModel.EndDate = DateTime.Now.AddDays(10);

            // Loading all CRMExtractVM
            var mockCRMExtractFactoryResult = new MockCRMExtractFactory().MockLoad(null, cRMExtractParameterModel);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup CRMExtractController
            _cRMExtractController = new CRMExtractController(mockCRMExtractFactoryResult.Result.Object, _zipFileHelper, _mockLogger.Object);

            //Act  
            var actionResult = _cRMExtractController.ExportCRMExtractData(cRMExtractParameterModel);
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<CRMExtractVM>;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult, null); //Test if returns equal count             
        }

        /// <summary>
        /// while we Load CRMExtractExportDate By CRMExtractParameterModelVM?
        /// </summary>
        [Test]
        public void CRMExtract_ExportDateTransData_Valid()
        {
            //Arrange
            object[] crmList = _CRMExtractVMList.Select(x => x.ToString()).ToArray();
            string[] crmStringArr = ((IEnumerable)crmList).Cast<object>().Select(x => x.ToString()).ToArray();

            CRMExtractParameterModelVM cRMExtractParameterModel = new CRMExtractParameterModelVM();
            cRMExtractParameterModel.CRMType = "Date";
            cRMExtractParameterModel.StartDate = DateTime.Now.AddDays(-10);
            cRMExtractParameterModel.EndDate = DateTime.Now.AddDays(10);
            AppConfigValues.HostedCountry = "GB";

            // Loading all CRMExtractVM
            var mockCRMExtractFactoryResult = new MockCRMExtractFactory().MockLoad(crmStringArr, cRMExtractParameterModel);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup CRMExtractController
            _cRMExtractController = new CRMExtractController(mockCRMExtractFactoryResult.Result.Object, _zipFileHelper, _mockLogger.Object);

            //Act  
            var actionResult = _cRMExtractController.ExportCRMExtractData(cRMExtractParameterModel);
            var actionResponse = actionResult.Result as FileContentResult;

            //Assert  
            Assert.IsInstanceOf(typeof(FileContentResult), actionResponse); //Test if returns ok response    
        }

        /// <summary>
        /// while we Load CRMExtractExportDate By CRMExtractParameterModelVM? if returns http not found result?
        /// </summary>
        [Test]
        public void CRMExtractExtract_ExportDateData_NoRecords()
        {
            //Arrange
            //Arrange
            CRMExtractParameterModelVM cRMExtractParameterModel = new CRMExtractParameterModelVM();
            cRMExtractParameterModel.CRMType = "Date";
            cRMExtractParameterModel.StartDate = DateTime.Now.AddDays(-10);
            cRMExtractParameterModel.EndDate = DateTime.Now.AddDays(10);

            // Loading all CRMExtractVM
            var mockCRMExtractFactoryResult = new MockCRMExtractFactory().MockLoad(null, cRMExtractParameterModel);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup CRMExtractController
            _cRMExtractController = new CRMExtractController(mockCRMExtractFactoryResult.Result.Object, _zipFileHelper, _mockLogger.Object);

            //Act  
            var actionResult = _cRMExtractController.ExportCRMExtractData(cRMExtractParameterModel);
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as IList<CRMExtractVM>;

            //Assert  
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response                        
            Assert.AreEqual(dataResult, null); //Test if returns equal count             
        }
        #endregion
    }
}
