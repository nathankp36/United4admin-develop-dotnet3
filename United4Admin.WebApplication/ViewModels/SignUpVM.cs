using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using United4Admin.WebApplication.BLL;

namespace United4Admin.WebApplication.ViewModels
{
    public class SignUpVM : IValidatableObject
    {
        [Display(Name = "ID")]
        public int chosenSignUpId { get; set; }

        [Display(Name = "ContactId")]
        public string ContactId { get; set; }

        [Display(Name = "DDLSalesOrderId")]
        public string DDLSalesOrderId { get; set; }

        [Required(ErrorMessage = "Please enter the title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter the first name")]
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "Please don't enter more than 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter the last name")]
        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "Please don't enter more than 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter the building name or number of the address")]
        [Display(Name = "Building Number/Name")]
        [MaxLength(50, ErrorMessage = "Please don't enter more than 50 characters")]
        public string BuildingNumberName { get; set; }

        [Required(ErrorMessage = "Please enter the street name of the address")]
        [Display(Name = "Street Name")]
        [MaxLength(50, ErrorMessage = "Please don't enter more than 50 characters")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Please enter the second line of the address")]
        [Display(Name = "Address line 2")]
        [MaxLength(50, ErrorMessage = "Please don't enter more than 50 characters")]
        public string AddressLine2 { get; set; }


        [Display(Name = "Address line 3 (optional)")]
        [MaxLength(50, ErrorMessage = "Please don't enter more than 50 characters")]
        public string AddressLine3 { get; set; }

        [Required(ErrorMessage = "Please enter the town/city of the address")]
        [MaxLength(50, ErrorMessage = "Please don't enter more than 50 characters")]
        [Display(Name = "Town/city")]
        public string TownCity { get; set; }

        [Required(ErrorMessage = "Please enter the postcode of the address")]
        [RegularExpression("^[A-Za-z]{1,2}[0-9A-Za-z]{1,2}[ ]?[0-9]{0,1}[A-Za-z]{2}$", ErrorMessage = "Invalid UK postcode")]
        [MaxLength(10, ErrorMessage = "Please don't enter more than 10 characters")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Please enter the phone number")]
        [RegularExpression("^((\\(?0\\d{4}\\)?\\s?\\d{3}\\s?\\d{3})|(\\(?0\\d{3}\\)?\\s?\\d{3}\\s?\\d{4})|(\\(?0\\d{2}\\)?\\s?\\d{4}\\s?\\d{4}))(\\s?\\#(\\d{4}|\\d{3}))?$", ErrorMessage = "Invalid UK phone number")]
        [Display(Name = "Phone Number")]
        [MaxLength(16, ErrorMessage = "Please don't enter more than 16 characters")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter the email address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        [MaxLength(254, ErrorMessage = "Please don't enter more than 254 characters")]//the longest an email address can be
        public string EmailAddress { get; set; }

        [Display(Name = "County (optional)")]
        [MaxLength(50, ErrorMessage = "Please don't enter more than 50 characters")]
        public string County { get; set; }


        [ForeignKey("SignupStatus")]
        [Required(ErrorMessage = "Please select a status")]
        [Display(Name = "Status")]
        public int ChosenStatusId { get; set; }

        public StatusVM Status { get; set; }

        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "Registration Date")]
        public DateTime SignUpDate { get; set; }

        [Required(ErrorMessage = "Please select a sign-up event")]
        [Display(Name = "Sign-up Event")]
        public int SignUpEventId { get; set; }
        public SignUpEventVM SignUpEvent { get; set; }

        [Display(Name = "Choosing Party")]
        public int? ChoosingPartyId { get; set; }
        public ChoosingPartyVM ChoosingParty { get; set; }

        [Display(Name = "Reveal Event")]
        public int? RevealEventId { get; set; }
        public RevealEventVM RevealEvent { get; set; }

        [Required(ErrorMessage = "Please enter the country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please enter the account holder name")]
        public string AccountHoldersName { get; set; }

        [Required(ErrorMessage = "Please enter the bank account number")]
        [Display(Name = "Account Number")]
        public string CorrectedBankAccountNumber { get; set; }

        [Required(ErrorMessage = "Please enter the bank account sort code")]
        [Display(Name = "Sort Code")]
        public string CorrectedBankSortCode { get; set; }
        public bool DirectDebitCapable { get; set; }

        [Display(Name = "Direct Debit Status")]
        public string DirectDebitStatusInfo { get; set; }
        [Display(Name = "IBAN")]
        public string Iban { get; set; }

        [Display(Name = "Yes, Please claim Tax Consent")]
        public bool TaxConsent { get; set; }


        public bool Post { get; set; }


        public bool Email { get; set; }


        public bool Phone { get; set; }


        public bool SMS { get; set; }

        [Display(Name = "I consent to my data, including my photograph, to be processed outside the UK and outside the EU for the purpose of allowing me to be chosen and participate in child sponsorship.")]
        public bool DataConsent { get; set; }

        [NotMapped]
        public string RegistrationEventName { get; set; }
        [NotMapped]
        public string FileName { get; set; }
        [NotMapped]
        public string ImageStatusName { get; set; }
        public ImageInfoVM ImageInfo { get; set; }

        //added in the dropdown list data after changing html.dropdownlistfor controls to helper tags. MS says not to use view bags for this data
        public List<TitleVM> Titles { get; set; }

        public List<StatusVM> Statuses { get; set; }
        public List<SignUpEventVM> SignUpVMEvents { get; set; }

        public List<ChoosingPartyVM> ChoosingParties { get; set; }

        public List<RevealEventVM> RevealEvents { get; set; }
        public string RequestType { get; set; }
        public string TransactionReferenceNo { get { return ApplicationConstants.REFERENCEBEGINSTRING + Id.ToString(ApplicationConstants.REFERENCEFORMAT); } }
        public string RequestReferenceNo { get { return TransactionReferenceNo + ApplicationConstants.DEFAULTCHILDNO; } }
        public DateTime DateOfRequest { get; set; }
        public string TriggerCode { get; set; }
        public int UKConsentStatementID { get; set; }
        public string supporterID { get; set; }
        public string SupporterType { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Organisation_GroupName { get; set; }
        public string Organisation_GroupType { get; set; }
        public string gender { get; set; }
        public string YearOfBirth { get; set; }
        public string AddressStructure { get; set; }
        public string AddressLine4 { get; set; }
        public string PhoneNum1 { get; set; }
        public string PhoneNum2 { get; set; }
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
        public string Receipt_Required { get { return ApplicationConstants.RECEIPTREQUIRED; } }
        public int Fundraised { get; set; }
        public string CommitmentCode { get { return ApplicationConstants.COMMITMENTCODE; } }
        public string PaymentTransactionID { get; set; }
        public string Comments { get; set; }
        public string ResponseEntity { get; set; }
        public string ScheduledPayID { get; set; }
        public string AiName1 { get { return ApplicationConstants.AINAME1; } }
        public string AiValue1 { get; set; }
        public string AiName2 { get { return ApplicationConstants.AINAME2; } }
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
        public DateTime DateFulfilled { get; set; }
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
        public string ChosenTempSupporterID { get; set; }
        public string ChosenUKEventName { get; set; }
        public string ChosenFieldEventId { get; set; }
        public string ChosenFieldEventDate { get; set; }
        public string ChosenRevealType { get; set; }
        public string ChosenRevealDate { get; set; }
        public string ConsentStatementID { get; set; }

        public int Id { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!DataConsent)
            { yield return new ValidationResult("You must consent to the data being processed to register.", new List<string>() { "DataConsent" }); }
        }

        public SignUpVM()
        {
            RequestType = ApplicationConstants.REQUESTTYPE;
            ConsentStatementID = ApplicationConstants.CONSENTSTATEMENTID;
            SupporterType = ApplicationConstants.SUPPORTERTYPE;
            AddressStructure = ApplicationConstants.ADDRESSSTRUCTURE;
        }

        [NotMapped]
        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [NotMapped]
        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [NotMapped]
        public string ExtractType { get; set; }
        [NotMapped]
        public string FieldDataType { get; set; }
    }
}
