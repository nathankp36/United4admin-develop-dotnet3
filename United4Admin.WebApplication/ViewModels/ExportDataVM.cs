using United4Admin.ExcelGenerator.Attributes;
using United4Admin.WebApplication.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace United4Admin.WebApplication.ViewModels
{
    public class ExportDataVM
    {
        public class FieldDataExport
        {
            [IncludeInReport(Order = 1)]
            public string SponsorAccount { get; set; }
            [IncludeInReport(Order = 2)]
            public string SponsorName { get; set; }
            [IncludeInReport(Order = 3)]
            public string Country { get; set; }
            [IncludeInReport(Order = 4)]
            public string SponsorPhotoFileName { get; set; }
        }

        public class EchoDataExport
        {
            public int Id { get; set; }
            public string RequestType { get; set; }
            public string TransactionReferenceNo { get { return ApplicationConstants.REFERENCEBEGINSTRING + Id.ToString(ApplicationConstants.REFERENCEFORMAT); } }
            public string RequestReferenceNo { get { return TransactionReferenceNo + ApplicationConstants.DEFAULTCHILDNO; } }
            public DateTime DateOfRequest { get; set; }
            public string TriggerCode { get; set; }
            public string ConsentStatementID { get; set; }
            public string supporterID { get; set; }
            public string WvSupporterId { get; set; }
            public string SupporterType { get; set; }
            public string Title { get; set; }
            public string Forename { get; set; }
            public string Surname { get; set; }
            public string OrganisationOrGroupName { get; set; }
            public string OrganisationOrGroupType { get; set; }
            public string Gender { get; set; }
            public string YearOfBirth { get; set; }
            public string TaxConsent { get; set; }
            public string AddressStructure { get; set; }
            public string BuildingNumber { get; set; }
            public string BuildingName { get; set; }
            public string StreetName { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string AddressLine4 { get; set; }
            public string Postcode { get; set; }

            public string Country { get; set; }
            public string PhoneNum1 { get; set; }
            public string PhoneNum2 { get; set; }
            public string EmailAddress { get; set; }
            public string Location { get; set; }
            public string CommitmentAmount { get; set; }
            public string TransactionAmount { get; set; }
            public string PaymentMethod { get { return ApplicationConstants.PAYMENTMETHOD; } }
            public string PaymentType { get { return ApplicationConstants.PAYMENTTYPE; } }
            public string PaymentReference { get; set; }
            public string PaymentFrequency { get { return ApplicationConstants.PAYMENTFREQUENCY; } }

            public string PreferredDDDate { get { return ApplicationConstants.PREFERREDDDDATE; } }
            public string SortCode { get; set; }
            public string AccountCode { get; set; }
            public string CardHolder { get; set; }
            public string ChildID { get; set; }
            public string PreferredContinent { get; set; }
            public string PreferredCountry { get; set; }
            public string PreferredGender { get; set; }
            public string PreferredAge { get; set; }
            public string ReceiptRequired { get { return ApplicationConstants.RECEIPTREQUIRED; } }
            public string FundRaised { get; set; }
            public string CommitmentCode { get { return ApplicationConstants.COMMITMENTCODE; } }
            public string PaymentTransactionID { get; set; }
            public string Comments { get; set; }
            public string ResponseEntity { get; set; }
            public string ScheduledPayID { get; set; }
            public string AiName1 { get { return ApplicationConstants.AINAME1; } }
            public string AiValue1 { get; set; }
            public string AiName2 { get { return ApplicationConstants.AINAME2; } }
            public string AiValue2 { get; set; }
            public string PrayerAndFamily { get; set; }
            public string DirectMailOptIn { get; set; }
            public string EmailOptIn { get; set; }
            public string PhoneOptIn { get; set; }
            public string SMSOptIn { get; set; }
            public string DirectMailOptOut { get; set; }
            public string EmailOptOut { get; set; }
            public string PhoneOptOut { get; set; }
            public string SMSOptOut { get; set; }
            public string ChosenTempSupporterID { get; set; }
            public string ChosenUKEventName { get; set; }
            public string ChosenFieldEventId { get; set; }
            public string ChosenFieldEventDate { get; set; }
            public string ChosenRevealType { get; set; }
            public string ChosenRevealDate { get; set; }
            public string Exported { get; set; }
            public string ExtractDate { get; set; }
            public string MiddleName { get; set; }
            public string TaxId { get; set; }
            public string Dataprocessingconsent { get; set; }
            public string Marketingcommsconsent { get; set; }
            public string ExternalPaymentToken { get; set; }
            public string IBAN { get; set; }
            public string ProductID { get; set; }
            public string Donationamount { get; set; }
            public string DonationvariantID { get; set; }
            public string Donationfrequency { get; set; }
            public string AdrStatusID { get; set; }
            public string AdrTypeID { get; set; }
            public string Title1Descr { get; set; }
            public string Title2Descr { get; set; }
            public string MaritalStatusDescr { get; set; }
            public string AddDate { get; set; }
            public string FamilyName2 { get; set; }
            public string OrgName1 { get; set; }
            public string OrgName2 { get; set; }
            public string OrgDepartmentName { get; set; }
            public string OrgRoleDescr { get; set; }
            public string PartnershipName { get; set; }
            public string StatesProvCode { get; set; }
            public string RegionDescr { get; set; }
            public string NoMail { get; set; }
            public string NoMailID { get; set; }
            public string SpokenLanguageCode { get; set; }
            public string PrintedLanguageCode { get; set; }
            public string ReceiptingID { get; set; }
            public string MotivationID { get; set; }
            public string ReferenceText { get; set; }
            public string PhoneTypeID1 { get; set; }
            public string PhoneTypeID2 { get; set; }
            public EchoDataExport()
            {
                RequestType = ApplicationConstants.REQUESTTYPE;
                ConsentStatementID = ApplicationConstants.CONSENTSTATEMENTID;
                SupporterType = ApplicationConstants.SUPPORTERTYPE;
                AddressStructure = ApplicationConstants.ADDRESSSTRUCTURE;
            }
        }

        public class PreRevealDataExport
        {
            public int Id { get; set; }
            public string RequestType { get; set; }
            public string TransactionReferenceNo { get { return ApplicationConstants.REFERENCEBEGINSTRING + Id.ToString(ApplicationConstants.REFERENCEFORMAT); } }
            public string RequestReferenceNo { get { return TransactionReferenceNo + ApplicationConstants.DEFAULTCHILDNO; } }
            public DateTime DateOfRequest { get; set; }
            public string TriggerCode { get; set; }
            public string ConsentStatementID { get; set; }
            public string supporterID { get; set; }
            public string SupporterType { get; set; }
            public string Title { get; set; }
            public string Forename { get; set; }
            public string Surname { get; set; }
            public string OrganisationOrGroupName { get; set; }
            public string OrganisationOrGroupType { get; set; }
            public string Gender { get; set; }
            public string YearOfBirth { get; set; }
            public string TaxConsent { get; set; }
            public string AddressStructure { get; set; }
            public string BuildingNumber { get; set; }
            public string BuildingName { get; set; }
            public string StreetName { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string AddressLine4 { get; set; }
            public string Postcode { get; set; }
            public string Country { get; set; }
            public string PhoneNum1 { get; set; }
            public string PhoneNum2 { get; set; }
            public string EmailAddress { get; set; }
            public string Location { get; set; }
            public string CommitmentAmount { get; set; }
            public string TransactionAmount { get; set; }
            public string PaymentMethod { get { return ApplicationConstants.PAYMENTMETHOD; } }
            public string PaymentType { get { return ApplicationConstants.PAYMENTTYPE; } }
            public string PaymentReference { get; set; }
            public string PaymentFrequency { get { return ApplicationConstants.PAYMENTFREQUENCY; } }
            public string PreferredDDDate { get { return ApplicationConstants.PREFERREDDDDATE; } }
            public string SortCode { get; set; }
            public string AccountCode { get; set; }
            public string CardHolder { get; set; }
            public string PreferredContinent { get; set; }
            public string PreferredCountry { get; set; }
            public string PreferredGender { get; set; }
            public string PreferredAge { get; set; }
            public string ReceiptRequired { get { return ApplicationConstants.RECEIPTREQUIRED; } }
            public string FundRaised { get; set; }
            public string CommitmentCode { get { return ApplicationConstants.COMMITMENTCODE; } }
            public string PaymentTransactionID { get; set; }
            public string Comments { get; set; }
            public string ResponseEntity { get; set; }
            public string ScheduledPayID { get; set; }
            public string AiName1 { get { return ApplicationConstants.AINAME1; } }
            public string AiValue1 { get; set; }
            public string AiName2 { get { return ApplicationConstants.AINAME2; } }
            public string AiValue2 { get; set; }
            public string PrayerAndFamily { get; set; }
            public string DirectMailOptIn { get; set; }
            public string EmailOptIn { get; set; }
            public string PhoneOptIn { get; set; }
            public string SMSOptIn { get; set; }
            public string DirectMailOptOut { get; set; }
            public string EmailOptOut { get; set; }
            public string PhoneOptOut { get; set; }
            public string SMSOptOut { get; set; }
            public string ChosenTempSupporterID { get; set; }
            public string ChosenUKEventName { get; set; }
            public string ChosenFieldEventId { get; set; }
            public string ChosenFieldEventDate { get; set; }
            public string ChosenRevealType { get; set; }
            public string ChosenRevealDate { get; set; }
            public string ExternalPaymentToken { get; set; }
            public string IBAN { get; set; }
            public string ProductID { get; set; }
            public string Donationamount { get; set; }
            public string DonationvariantID { get; set; }
            public string Donationfrequency { get; set; }
            public string AdrStatusID { get; set; }
            public string AdrTypeID { get; set; }
            public string Title1Descr { get; set; }
            public string Title2Descr { get; set; }
            public string MaritalStatusDescr { get; set; }
            public string AddDate { get; set; }
            public string FamilyName2 { get; set; }
            public string OrgName1 { get; set; }
            public string OrgName2 { get; set; }
            public string OrgDepartmentName { get; set; }
            public string OrgRoleDescr { get; set; }
            public string PartnershipName { get; set; }
            public string StatesProvCode { get; set; }
            public string RegionDescr { get; set; }
            public string NoMail { get; set; }
            public string NoMailID { get; set; }
            public string SpokenLanguageCode { get; set; }
            public string PrintedLanguageCode { get; set; }
            public string ReceiptingID { get; set; }
            public string MotivationID { get; set; }
            public string ReferenceText { get; set; }
            public string PhoneTypeID1 { get; set; }
            public string PhoneTypeID2 { get; set; }
            public PreRevealDataExport()
            {
                RequestType = ApplicationConstants.REQUESTTYPE;
                ConsentStatementID = ApplicationConstants.CONSENTSTATEMENTID;
                SupporterType = ApplicationConstants.SUPPORTERTYPE;
                AddressStructure = ApplicationConstants.ADDRESSSTRUCTURE;
            }
        }

        public class AddOnDataExport
        {
            [IncludeInReport(Order = 1)]
            public string Date { get; set; }
            [IncludeInReport(Order = 2)]
            public string SponsorName { get; set; }
            [IncludeInReport(Order = 3)]
            public string ChildId { get; set; }
            [IncludeInReport(Order = 4)]
            public string Message { get; set; }
        }
    }
}