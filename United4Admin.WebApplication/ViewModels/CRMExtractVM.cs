using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace United4Admin.WebApplication.ViewModels
{
    public class CRMExtractVM
    {
        public string TransactionId { get; set; }
        public string RequestType { get; set; }
        public string TransactionReferenceNo { get; set; }
        public string RequestReferenceNo { get; set; }
        public DateTime DateOfRequest { get; set; }
        public string TriggerCode { get; set; }
        public int UKConsentStatementID { get; set; }
        public string supporterID { get; set; }
        public string WvSupporterId { get; set; }
        public string SupporterType { get; set; }
        public string Title { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Organisation_GroupName { get; set; }
        public string Organisation_GroupType { get; set; }
        public string gender { get; set; }
        public string YearOfBirth { get; set; }
        public int TaxConsentOptIn { get; set; }
        public string AddressStructure { get; set; }
        public string buildingNumber { get; set; }
        public string buildingName { get; set; }
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
        public string PaymentMethod { get; set; }
        public string PaymentType { get; set; }
        public string PaymentReference { get; set; }
        public string PaymentFrequency { get; set; }
        public string PreferredDDDate { get; set; }
        public string SortCode { get; set; }
        public string AccountCode { get; set; }
        public string CardHolder { get; set; }
        public string ChildID { get; set; }
        public string PreferredContinent { get; set; }
        public string PreferredCountry { get; set; }
        public string PreferredGender { get; set; }
        public string PreferredAge { get; set; }
        public string Receipt_Required { get; set; }
        public int Fundraised { get; set; }
        public string CommitmentCode { get; set; }
        public string PaymentTransactionID { get; set; }
        public string Comments { get; set; }
        public string ResponseEntity { get; set; }
        public string ScheduledPayID { get; set; }
        public string AiName1 { get; set; }
        public string AiValue1 { get; set; }
        public string AiName2 { get; set; }
        public string AiValue2 { get; set; }
        public string FaithAndFamily { get; set; }
        public string DirectMailOptIn { get; set; }
        public string EmailOptIn { get; set; }
        public string PhoneOptIn { get; set; }
        public string SMSOptIn { get; set; }
        public string DirectMailOptOut { get; set; }
        public string EmailOptOut { get; set; }
        public string PhoneOptOut { get; set; }
        public string SMSOptOut { get; set; }
        public bool Exported { get; set; }
        public string ExtractDate { get; set; }
        public string MiddleName { get; set; }
        public string taxId { get; set; }
        public bool Dataprocessingconsent { get; set; }
        public bool Marketingcommsconsent { get; set; }
        public string ExternalPaymentToken { get; set; }
        public string IBAN { get; set; }
        public string ProductID { get; set; }
        public decimal Donationamount { get; set; }
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
        public string Quantity { get; set; }
        public string CurrencyCode { get; set; }
        public Guid OrderGuid { get; set; }
        public string leadID { get; set; }
        public string PledgeType { get; set; }
        public string ProductCode { get; set; }
        public string EmailSignUp { get; set; }
        public string LeadsSignUp { get; set; }
        public string ContentBasedLeadSignUp { get; set; }

    }
}
