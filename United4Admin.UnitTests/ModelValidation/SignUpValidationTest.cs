using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

using System.Linq;
using United4Admin.WebApplication.ViewModels;

namespace United4Admin.UnitTests.ModelValidation
{
   [TestFixture]
    public class SignUpValidationTest
    {
        SignUpVM _suValid = null;
        SignUpVM _suInvalid = null;



         [SetUp]
        public void TestInitialize()
        {
            _suValid = new SignUpVM
            {
                Title = "MR",
                FirstName = "Albus",
                LastName = "Dumbledore",
                BuildingNumberName = "Headteacher's Study",
                StreetName="The Magic Staircase",
                AddressLine2 = "Hogwarts",
                TownCity = "Hogsmeade",
                Postcode = "HP12 3RH",
                PhoneNumber = "0115 8471257",
                EmailAddress = "albus.dumbledore@yahoo.co.uk",
                Country = "UK",
                AccountHoldersName="A Dumbldore",
                CorrectedBankAccountNumber = "12345678",
                CorrectedBankSortCode = "000000",
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
               // TakePhotoFromDevice=false
            };
        }

        [Test]
        public void CreateSignUp_ValidationOnValidModelPasses()
        {
            //Arrange           
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnFirstNameGivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.FirstName = string.Empty;
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter the first name", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnLastNameGivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.LastName = "    ";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter the last name", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnBuildingNumberName_GivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.BuildingNumberName = " ";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter the building name or number of the address", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnStreetName_GivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.StreetName = " ";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter the street name of the address", results[0].ErrorMessage);
        }
     

        [Test]
        public void CreateSignUp_ValidationOnTownCityGivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.TownCity = "                 ";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter the town/city of the address", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnPostcodeGivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.Postcode = "  ";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter the postcode of the address", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnPhoneNumberGivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.PhoneNumber = " ";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter the phone number", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnEmailGivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.EmailAddress = string.Empty;
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter the email address", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnEmailFormatGivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.EmailAddress = "@A Test EMail";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Invalid email address", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnPhoneFormat_NotEnoughNumbers_GivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.PhoneNumber = "0123456789";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Invalid UK phone number", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnPhoneFormat_TextNotNumbers_GivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.PhoneNumber = "Phone Number";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Invalid UK phone number", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnPhoneFormat_TooManyNumbers_GivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.PhoneNumber = "012345678901";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Invalid UK phone number", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnPhoneFormat_LondonNumberPasses()
        {
            //Arrange            
            _suValid.PhoneNumber = "020 7123 1234";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnPhoneFormat_MobileNumberPasses()
        {
            //Arrange           
            _suValid.PhoneNumber = "0795 1518292";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnPhoneFormat_SpecialNumberPasses()
        {
            //Arrange           
            _suValid.PhoneNumber = "0800 1234567";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnPostcodeFormat_TooShort_GivesError()
        {
            //Arrange           
            _suInvalid = _suValid;
            _suInvalid.Postcode = "NG7 1J";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Invalid UK postcode", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnPostcodeFormat_TooLong_GivesError()
        {
            //Arrange           
            _suInvalid = _suValid;
            _suInvalid.Postcode = "NG7 1JRE";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Invalid UK postcode", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnPostcodeFormat_NoSpacesPasses()
        {
            //Arrange           
            _suValid.Postcode = "NE455AR";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnPostcodeFormat_LowerCasePasses()
        {
            //Arrange           
            _suValid.Postcode = "ne65 9ng";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnPostcodeFormat_LondonPasses()
        {
            //Arrange           
            _suValid.Postcode = "W1M 6AS";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnFirstName_TooLong_GivesError()
        {
            //Arrange           
            _suInvalid = _suValid;
            _suInvalid.FirstName = "Here is a string of 51 characters that is very long";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 50 characters", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnFirstName_MaxLength_Passes()
        {
            //Arrange           
            _suValid.FirstName = "Here is a string of 50 characters that is just ok.";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnLastName_TooLong_GivesError()
        {
            //Arrange           
            _suInvalid = _suValid;
            _suInvalid.FirstName = "A double-barrelled last name in this case 51  chars";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 50 characters", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnLastName_MaxLength_Passes()
        {
            //Arrange                      
            _suValid.LastName = "Here is a string of 50 characters that is just ok.";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnBuildingNumberName_TooLong_GivesError()
        {
            //Arrange           
            _suInvalid = _suValid;
            _suInvalid.BuildingNumberName = "Here is a string of 51 characters that is very long";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 50 characters", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnStreetName_TooLong_GivesError()
        {
            //Arrange           
            _suInvalid = _suValid;
            _suInvalid.StreetName = "Here is a string of 51 characters that is very long";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 50 characters", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnBuildingNumberName_MaxLength_Passes()
        {
            //Arrange                       
            _suValid.BuildingNumberName = "Here is a string of 50 characters that is just ok.";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);

        }

        [Test]
        public void CreateSignUp_ValidationOnStreetName_MaxLength_Passes()
        {
            //Arrange                       
            _suValid.StreetName = "Here is a string of 50 characters that is just ok.";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnAddress2_TooLong_GivesError()
        {
            //Arrange           
            _suInvalid = _suValid;
            _suInvalid.AddressLine2 = "Here is a string of 51 characters that is very long";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 50 characters", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnAddress2_MaxLength_Passes()
        {
            //Arrange           
            _suValid.AddressLine2 = "Here is a string of 50 characters that is just ok.";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnCounty_TooLong_GivesError()
        {
            //Arrange           
            _suInvalid = _suValid;
            _suInvalid.County = "Here is a string of 51 characters that is very long";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 50 characters", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnCounty_MaxLength_Passes()
        {
            //Arrange                      
            _suValid.Country = "Here is a string of 50 characters that is just ok.";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnTownCity_TooLong_GivesError()
        {
            //Arrange           
            _suInvalid = _suValid;
            _suInvalid.TownCity = "Here is a string of 51 characters that is very long";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 50 characters", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnTownCity_MaxLength_Passes()
        {
            //Arrange           
            _suValid.TownCity = "Here is a string of 50 characters that is just ok.";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnPostcode_TooLong_GivesError()
        {
            //Arrange           
            _suInvalid = _suValid;
            _suInvalid.Postcode = "01234567890";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(2, results.Count);//invalid postcode will be returned as well as more than 10 chars           
            Assert.IsTrue(results.Any(x => x.ErrorMessage == "Please don't enter more than 10 characters"));
        }

        [Test]
        public void CreateSignUp_ValidationOnPostcode_MaxLength_Passes()
        {
            //Arrange          
            _suValid.Postcode = "0123456789";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            //postcode will fail on valid UK postcode, just checking here that it passes on length
            Assert.IsFalse(results.Any(x => x.ErrorMessage == "Please don't enter more than 10 characters"));
        }
        [Test]
        public void CreateSignUp_ValidationOnPhoneNumber_TooLong_GivesError()
        {
            //Arrange           
            _suInvalid = _suValid;
            _suInvalid.PhoneNumber = "01234567890 #1234";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);//invalid postcode will be returned as well as more than 10 chars           
            Assert.IsTrue(results.Any(x => x.ErrorMessage == "Please don't enter more than 16 characters"));
        }

        [Test]
        public void CreateSignUp_ValidationOnPhoneNumber_MaxLength_Passes()
        {
            //Arrange          
            _suValid.PhoneNumber = "01234567890 #123";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUp_ValidationOnEmailAddress_TooLong_GivesError()
        {
            //Arrange           
            _suInvalid = _suValid;
            //255 chars
            _suInvalid.EmailAddress = "a.really.long.email.address.a.really.long.email.address.a.really.long.email.address.a.really.long.email.address@areallylongdomainnameareallylongdomainnameareallylongdomainnameareallylongdomainnameareallylongdomainnameareallylongdomainnameareallylong.co.uk";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);//invalid postcode will be returned as well as more than 10 chars           
            Assert.IsTrue(results.Any(x => x.ErrorMessage == "Please don't enter more than 254 characters"));
        }

        [Test]
        public void CreateSignUp_ValidationOnEmailAddress_MaxLength_Passes()
        {
            //Arrange          
            //254 chars
            _suValid.EmailAddress = "a.really.long.email.address.a.really.long.email.address.a.really.long.email.address.a.really.long.email.address@areallylongdomainnameareallylongdomainnameareallylongdomainnameareallylongdomainnameareallylongdomainnameareallylongdomainnameareallylon.co.uk";
            var context = new ValidationContext(_suValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }
        [Test]
        public void CreateSignUp_ValidationOnBankAccountName_GivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.AccountHoldersName = " ";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter the account holder name", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnBankAccountNumber_GivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.CorrectedBankAccountNumber = "                 ";
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter the bank account number", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_ValidationOnBankSortCode_GivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.CorrectedBankSortCode = string.Empty;
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter the bank account sort code", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUp_NoDataConsent_GivesError()
        {
            //Arrange
            _suInvalid = _suValid;
            _suInvalid.DataConsent = false;
            var context = new ValidationContext(_suInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("You must consent to the data being processed to register.", results[0].ErrorMessage);
        }

        //[Test]
        //public void CreateSignUp_TakePhotoFromDevice_NoPhoto_GivesError()
        //{
        //    //Arrange
        //    _suInvalid = _suValid;
        //    _suInvalid.ImageInfo.TakePhotoFromDevice = true;
        //    var context = new ValidationContext(_suInvalid, null, null);
        //    var results = new List<ValidationResult>();

        //    //Act
        //    var isModelStateValid = Validator.TryValidateObject(_suInvalid, context, results, true);

        //    // Assert
        //    Assert.IsFalse(isModelStateValid);
        //    Assert.AreEqual(1, results.Count);
        //    Assert.AreEqual("Please select a photo", results[0].ErrorMessage);
        //}

        
    }



}


