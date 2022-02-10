using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace United4Admin.WebApplication.ViewModels
{
    public class RegistrationVM : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a title")]
        [Display(Name = "Title")]
        public int TitleId { get; set; }
        public TitleVM SignupTitle { get; set; }

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

        [ForeignKey("SignupStatus")]
        [Required(ErrorMessage = "Please select a status")]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public StatusVM SignupStatus { get; set; }

        [DataType(DataType.Date), UIHint("DatePicker"), Display(Name = "Registration Date")]
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

        public string IBAN { get; set; }

        [Display(Name = "Yes, Please claim Gift Aid")]
        public bool GiftAid { get; set; }


        public bool Post { get; set; }


        public bool Email { get; set; }


        public bool Phone { get; set; }


        public bool SMS { get; set; }

        [Display(Name = "I consent to my data, including my photograph, to be processed outside the UK and outside the EU for the purpose of allowing me to be chosen and participate in child sponsorship.")]
        public bool DataConsent { get; set; }

        public ImageInfoVM ImageInfo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!DataConsent)
            { yield return new ValidationResult("You must consent to the data being processed to register.", new List<string>() { "DataConsent" }); }
        }
    }
}