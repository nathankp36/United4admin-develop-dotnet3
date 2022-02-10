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

namespace United4Admin.UnitTests.Controllers
{
    [TestFixture]
    public class PreRevealExtractControllerTest
    {
        private PreRevealExtractController _preRevealExtractController;
        private MockRegistrationFactory mockRegistrationFactory;

        private ApiResponse _mockResponse;
        private int _SignUpVMListCount;
        private int _SignUpVMListMaxId;

        private List<SignUpVM> _SignUpVMList;
        private List<PermissionsVM> _PermissionsVMList;
        private List<ImageInfoVM> _imageInfoVMList;
        private ApiResponse _apiReponse;
        private Mock<ILogger<PreRevealExtractController>> _mockLogger;
        public IFormFile ConvertedIFormFilePhoto { get; set; }
        [SetUp]
        public void SetUp()

        {
            //Arrange
            #region "Arrange"

            _imageInfoVMList = new List<ImageInfoVM>()
                {
                    new ImageInfoVM{ImageInfoId=1, BlobGUID="test1.jpg",ChosenSignUpId =1,ImageStatusId=3},
                    new ImageInfoVM{ImageInfoId=2, BlobGUID="test2.jpg",ChosenSignUpId =2,ImageStatusId=3},
                    new ImageInfoVM{ImageInfoId=3, BlobGUID="test3.jpg",ChosenSignUpId =3,ImageStatusId=3},
                     new ImageInfoVM{ImageInfoId=4, BlobGUID="test3.jpg",ChosenSignUpId =4,ImageStatusId=3}
                };

            //Creating Dummy SignUpVM List
            _SignUpVMList = new List<SignUpVM>
                    {
                        new SignUpVM {
                            RequestType="CB",
                            chosenSignUpId = 1,
                            Title = "",
                            FirstName = "Harry",
                            LastName = "Potter",
                            BuildingNumberName = "4",
                            StreetName = "Privet Drive",
                            AddressLine2 = "-",
                            TownCity = "Little Whinging",
                            Postcode = "WH23 4TH",
                            PhoneNumber = "02077582900",
                            EmailAddress = "theboywholived@nottinghamcity.gov.uk",
                            CorrectedBankAccountNumber = "12345678",
                            CorrectedBankSortCode = "123456",
                            DataConsent = true,
                            TaxConsent = false,
                            Post = true,
                            Email = false,
                            Phone = true,
                            SMS = false,
                            ChosenStatusId = 1,
                            SignUpEventId = 1,
                            ChoosingPartyId = 1,
                            RevealEventId = 1,
                            RegistrationEventName="ChosenEvent1",
                            Id = 1,
                            ContactId = "",
                            DDLSalesOrderId = "",
                            PaymentTransactionID = "",
                            SortCode = "1000",
                            AccountCode = "8078097",
                            CardHolder = "James",
                            TriggerCode = "",
                            AddressLine3 = "",
                            AddressLine4 = "UK",
                            Country = "UK",
                            PhoneNum1 = "979888",
                            SignUpDate =  DateTime.Now,
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
                            ChosenTempSupporterID = Convert.ToString(1),
                            ChosenUKEventName = "Event1",
                            ChosenFieldEventId =  "",
                            ChosenFieldEventDate = "",
                            ChosenRevealType =  "",
                            ChosenRevealDate =  "",
                            AccountHoldersName = "Gelmut",
                            DateFulfilled = DateTime.Now,
                            ChildID = "UASDF789u",
                            CommitmentAmount = Convert.ToString((int)(2500 * 100)),
                            TransactionAmount = ""
                        },
                        new SignUpVM {
                            chosenSignUpId = 2,
                            Title = "",
                            FirstName = "Lyra",
                            LastName = "Belaqua",
                            BuildingNumberName = "1",
                            StreetName = "The Rooftops",
                            AddressLine2 = "Jordan College",
                            TownCity = "Oxford",
                            Postcode = "OX1 12JC",
                            PhoneNumber = "01865 123456",
                            EmailAddress = "lyra.silvertongue@gmail.com",
                            CorrectedBankAccountNumber = "98765432",
                            CorrectedBankSortCode = "112233",
                            DataConsent = true,
                            TaxConsent = true,
                            Post = false,
                            Email = false,
                            Phone = false,
                            SMS = false,
                            ChosenStatusId = 2,
                            SignUpEventId = 2,
                            ChoosingPartyId = 2,
                            RevealEventId = 2,
                            RegistrationEventName="ChosenEvent2",
                            Id = 2,
                            ContactId = "",
                            DDLSalesOrderId = "",
                            PaymentTransactionID = "",
                            SortCode = "1000",
                            AccountCode = "8078097",
                            CardHolder = "James",
                            TriggerCode = "",
                            AddressLine3 = "",
                            AddressLine4 = "UK",
                            Country = "UK",
                            PhoneNum1 = "979888",
                            SignUpDate =  DateTime.Now,
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
                            ChosenTempSupporterID = Convert.ToString(1),
                            ChosenUKEventName = "Event1",
                            ChosenFieldEventId =  "",
                            ChosenFieldEventDate = "",
                            ChosenRevealType =  "",
                            ChosenRevealDate =  "",
                            AccountHoldersName = "Gelmut",
                            DateFulfilled = DateTime.Now,
                            ChildID = "UASDF789u",
                            CommitmentAmount = Convert.ToString((int)(2500 * 100)),
                            TransactionAmount = ""
                        },
                        new SignUpVM {
                            chosenSignUpId = 3,
                            Title = "",
                            FirstName = "Lucy",
                            LastName = "Pevensie",
                            BuildingNumberName = "Mr Tumnus' House",
                            StreetName = "Lampost Lane",
                            AddressLine2 = "Lantern Wood",
                            TownCity = "Narnia",
                            Postcode = "N43 6BS",
                            PhoneNumber = "0115 8471257",
                            EmailAddress = "lucy.pevensie@rolls-royce.com",
                            CorrectedBankAccountNumber = "55667788",
                            CorrectedBankSortCode = "000099",
                            DataConsent = true,
                            TaxConsent = true,
                            Post = true,
                            Email = true,
                            Phone = true,
                            SMS = true,
                            ChosenStatusId = 3,
                            SignUpEventId = 3,
                            ChoosingPartyId = 3,
                            RevealEventId = 3,
                            RegistrationEventName="ChosenEvent3",
                            Id = 3,
                            ContactId = "",
                            DDLSalesOrderId = "",
                            PaymentTransactionID = "",
                            SortCode = "1000",
                            AccountCode = "8078097",
                            CardHolder = "James",
                            TriggerCode = "",
                            AddressLine3 = "",
                            AddressLine4 = "UK",
                            Country = "UK",
                            PhoneNum1 = "979888",
                            SignUpDate =  DateTime.Now,
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
                            ChosenTempSupporterID = Convert.ToString(1),
                            ChosenUKEventName = "Event1",
                            ChosenFieldEventId =  "",
                            ChosenFieldEventDate = "",
                            ChosenRevealType =  "",
                            ChosenRevealDate =  "",
                            AccountHoldersName = "Gelmut",
                            DateFulfilled = DateTime.Now,
                            ChildID = "UASDF789u",
                            CommitmentAmount = Convert.ToString((int)(2500 * 100)),
                            TransactionAmount = ""
                        },
                        new SignUpVM {
                            chosenSignUpId = 4,
                            Title = "",
                            FirstName = "Lucy test",
                            LastName = "Pevensietest",
                            BuildingNumberName = "Mr Tumnus' House",
                            StreetName = "Lampost Lane",
                            AddressLine2 = "Lantern Wood",
                            TownCity = "Narnia",
                            Postcode = "N43 6BS",
                            PhoneNumber = "0115 8471257",
                            EmailAddress = "lucy.pevensie@rolls-royce.com",
                            CorrectedBankAccountNumber = "55667788",
                            CorrectedBankSortCode = "000099",
                            DataConsent = true,
                            TaxConsent = true,
                            Post = true,
                            Email = true,
                            Phone = true,
                            SMS = true,
                            ChosenStatusId = 3,
                            SignUpEventId = 4,
                            ChoosingPartyId = 4,
                            RevealEventId = 4,
                            RegistrationEventName="ChosenEvent5",
                            Id = 4,
                            ContactId = "",
                            DDLSalesOrderId = "",
                            PaymentTransactionID = "",
                            SortCode = "1000",
                            AccountCode = "8078097",
                            CardHolder = "James",
                            TriggerCode = "",
                            AddressLine3 = "",
                            AddressLine4 = "UK",
                            Country = "UK",
                            PhoneNum1 = "979888",
                            SignUpDate =  DateTime.Now,
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
                            ChosenTempSupporterID = Convert.ToString(1),
                            ChosenUKEventName = "Event1",
                            ChosenFieldEventId =  "",
                            ChosenFieldEventDate = "",
                            ChosenRevealType =  "",
                            ChosenRevealDate =  "",
                            AccountHoldersName = "Gelmut",
                            DateFulfilled = DateTime.Now,
                            ChildID = "UASDF789u",
                            CommitmentAmount = Convert.ToString((int)(2500 * 100)),
                            TransactionAmount = ""
                        }
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

            //Setup mockRegistrationFactory
            mockRegistrationFactory = new MockRegistrationFactory();

            ////setup for permission
            //var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //logger
            _mockLogger = new Mock<ILogger<PreRevealExtractController>>();

            // ApiResponse
            _apiReponse = new ApiResponse();

            //Mock the file sent via Request.Forms.Files using the httpContext
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "test.jpg");
            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
        }
        #region "Upload Action "

        /// <summary>
        /// while we Load aRegistrations By Id for edit action?
        /// </summary>
        [Test]
        public void PreRevealExtract_ExportData_Valid()
        {
            //Arrange
            CRMExtractParameterModelVM cRMExtractParameterModel = new CRMExtractParameterModelVM();
            cRMExtractParameterModel.CRMType = "";
            cRMExtractParameterModel.StartDate = DateTime.Now.AddDays(-10);
            cRMExtractParameterModel.EndDate = DateTime.Now.AddDays(10);

            // Loading all SignUpVM
            var mockRegistrationFactoryResult = new MockPreRevealExtractFactory().MockLoad(_SignUpVMList, cRMExtractParameterModel);
            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);

            //Setup PhotoUploadController
            _preRevealExtractController = new PreRevealExtractController(mockRegistrationFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _preRevealExtractController.ExportPreRevealaExtractData(cRMExtractParameterModel);
            var actionResponse = actionResult.Result as FileContentResult;

            //Assert  
            Assert.IsInstanceOf(typeof(FileContentResult), actionResponse); //Test if returns ok response      
        }

        /// <summary>
        /// When we Load aRegistrations By Id for edit action if returns http not found result?
        /// </summary>
        [Test]
        public void PreRevealExtract_ExportData_NoRecords()
        {
            //Arrange
            List<SignUpVM> signUpVM = new List<SignUpVM>();

            CRMExtractParameterModelVM cRMExtractParameterModel = new CRMExtractParameterModelVM();
            cRMExtractParameterModel.CRMType = "";
            cRMExtractParameterModel.StartDate = DateTime.Now;
            cRMExtractParameterModel.EndDate = DateTime.Now.AddDays(10);

            // Loading all SignUpVM
            var mockRegistrationFactoryResult = new MockPreRevealExtractFactory().MockLoad(signUpVM, cRMExtractParameterModel);

            //setup for permission
            var mockPermissionFactoryResult = new MockPermissionFactory().MockLoadList(_PermissionsVMList);
            //Setup RegistrationsController
            _preRevealExtractController = new PreRevealExtractController(mockRegistrationFactoryResult.Result.Object, _mockLogger.Object);

            //Act  
            var actionResult = _preRevealExtractController.ExportPreRevealaExtractData(cRMExtractParameterModel);
            var actionResponse = actionResult.Result as FileContentResult;

            //Assert  
            Assert.IsInstanceOf(typeof(FileContentResult), actionResponse); //Test if returns ok response                        
        }
        #endregion
    }
}

