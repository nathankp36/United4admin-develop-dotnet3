using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

using System.Linq;
using United4Admin.WebApplication.ViewModels;
using System;

namespace United4Admin.UnitTests.ModelValidation
{
   [TestFixture]
    public class SignUpEventValidationTest
    {
        SignUpEventVM _sueValid = null;
        SignUpEventVM _sueInvalid = null;

         [SetUp]
        public void TestInitialize()
        {
            _sueValid = new SignUpEventVM
            {
                EventName = "Ps & Gs Chosen Sign-up Event",
                Location = "Edinburgh",
                PublishDate = DateTime.Today,
                ClosedDate = DateTime.Today.AddDays(14),
                EventDate = DateTime.Today.AddDays(15),
                TypeOfRegistration = "WV Chosen Event",
                SpecificChoosingEvent = "Yes",
                ShortURL = "TEST",
                ChoosingPartyId = 1,
                RevealEventId = 1,
                CampaignCode = "Test12345",
                WorkflowStatusId = 1,
                Create = true
            };

        }

        [Test]
        public void CreateSignUpEvent_ValidationOnValidModelPasses()
        {
            //Arrange           
            var context = new ValidationContext(_sueValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUpEvent_ValidationOnNameGivesError()
        {
            //Arrange
            _sueInvalid = _sueValid;
            _sueInvalid.EventName = string.Empty;
            var context = new ValidationContext(_sueInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Name field is required.", results[0].ErrorMessage);
        }
        [Test]
        public void CreateSignUpEvent_ValidationOnNameTooLongGivesError()
        {
            //Arrange
            _sueInvalid = _sueValid;
            _sueInvalid.EventName = "012345678901234567890123456789012345678901234567890";
            var context = new ValidationContext(_sueInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 50 characters", results[0].ErrorMessage);
        }
        [Test]
        public void CreateSignUpEvent_ValidationOnLocationGivesError()
        {
            //Arrange
            _sueInvalid = _sueValid;
            _sueInvalid.Location = string.Empty;
            var context = new ValidationContext(_sueInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Location field is required.", results[0].ErrorMessage);
        }
        [Test]
        public void CreateSignUpEvent_ValidationOnLocationTooLongGivesError()
        {
            //Arrange
            _sueInvalid = _sueValid;
            _sueInvalid.Location = "012345678901234567890123456789012345678901234567890012345678901234567890123456789012345678901234567890";
            var context = new ValidationContext(_sueInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 100 characters", results[0].ErrorMessage);
        }
        [Test]
        public void CreateSignUpEvent_ValidationOnPublicationDateInPastGivesError()
        {
            //Arrange
            _sueInvalid = _sueValid;
            _sueInvalid.PublishDate = DateTime.Today.AddDays(-1);
            var context = new ValidationContext(_sueInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Publish Date cannot be in the past.", results[0].ErrorMessage);
        }

        [Test]
        public void EditSignUpEvent_ValidationOnPublicationDateInPastNoesNotGiveError()
        {
            //Arrange
            _sueValid.Create = false; //edit
            _sueValid.PublishDate = DateTime.Today.AddDays(-1);
            var context = new ValidationContext(_sueValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateSignUpEvent_ValidationOnClosingDateBeforePublicationDateGivesError()
        {
            //Arrange
            _sueInvalid = _sueValid;
            _sueInvalid.ClosedDate = DateTime.Today.AddMonths(-1);
            var context = new ValidationContext(_sueInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Closing Date cannot be before Publish Date.", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUpEvent_ValidationOnTypeOfRegistrationGivesError()
        {
            //Arrange
            _sueInvalid = _sueValid;
            _sueInvalid.TypeOfRegistration = string.Empty;
            var context = new ValidationContext(_sueInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Type Of Registration field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUpEvent_ValidationOnShortURLGivesError()
        {
            //Arrange
            _sueInvalid = _sueValid;
            _sueInvalid.ShortURL = string.Empty;
            var context = new ValidationContext(_sueInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Short URL field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUpEvent_ValidationOnShortURLTooLongGivesError()
        {
            //Arrange
            _sueInvalid = _sueValid;
            _sueInvalid.ShortURL = "12345678901234567890123456";
            var context = new ValidationContext(_sueInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 25 characters", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUpEvent_ValidationOnSpecificChoosingEventGivesError()
        {
            //Arrange
            _sueInvalid = _sueValid;
            _sueInvalid.SpecificChoosingEvent = string.Empty;
            var context = new ValidationContext(_sueInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Specific Choosing event field is required.", results[0].ErrorMessage);
        }
        [Test]
        public void CreateSignUpEvent_ValidationOnTriggerCodeGivesError()
        {
            //Arrange
            _sueInvalid = _sueValid;
            _sueInvalid.CampaignCode = string.Empty;
            var context = new ValidationContext(_sueInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Campaign Code field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSignUpEvent_ValidationOnTriggerCodeTooLongGivesError()
        {
            //Arrange
            _sueInvalid = _sueValid;
            _sueInvalid.CampaignCode = "1234567890123456789012345678901234567890123456789012345";
            var context = new ValidationContext(_sueInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 50 characters", results[0].ErrorMessage);
        }


        //[Test]
        //public void CreateSignUpEvent_ValidationOnThankYouText()
        //{
        //    Arrange
        //    _sueInvalid = _sueValid;
        //    var context = new ValidationContext(_sueInvalid, null, null);
        //    var results = new List<ValidationResult>();

        //    Act
        //    var isModelStateValid = Validator.TryValidateObject(_sueInvalid, context, results, true);

        //    Assert
        //    Assert.IsFalse(isModelStateValid);
        //    Assert.AreEqual(1, results.Count);
        //    Assert.AreEqual("The Text to display on form submission field is required.", results[0].ErrorMessage);
        //}
    }

}


