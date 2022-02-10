using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using System.Linq;
using United4Admin.WebApplication.ViewModels;
using System;

namespace United4Admin.UnitTests.ModelValidation
{
    [TestFixture]
    public class ChoosingPartyValidationTest
    {
        ChoosingPartyVM _cpValid = null;
        ChoosingPartyVM _cpInvalid = null;

         [SetUp]
        public void TestInitialize()
        {
            _cpValid = new ChoosingPartyVM
            {
               HorizonId=123456,
               PartyName= "Pajule Choosing Party",
               PartyDate=DateTime.Today,
               Country="Uganda",
               Location= "Pajule",
               WorkflowStatusId=1,
               Create=true
            };

        }

        [Test]
        public void CreateChoosingParty_ValidationOnValidModelPasses()
        {
            //Arrange           
            var context = new ValidationContext(_cpValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_cpValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateChoosingParty_ValidationOnNameGivesError()
        {
            //Arrange
            _cpInvalid = _cpValid;
            _cpInvalid.PartyName = string.Empty;
            var context = new ValidationContext(_cpInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_cpInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Name field is required.", results[0].ErrorMessage);
        }
        [Test]
        public void CreateChoosingParty_ValidationOnLocationGivesError()
        {
            //Arrange
            _cpInvalid = _cpValid;
            _cpInvalid.Location = string.Empty;
            var context = new ValidationContext(_cpInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_cpInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Location field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void CreateChoosingParty_ValidationOnLocationTooLongGivesError()
        {
            //Arrange
            _cpInvalid = _cpValid;
            _cpInvalid.Location = "12345678901234567890123456";
            var context = new ValidationContext(_cpInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_cpInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 25 characters", results[0].ErrorMessage);
        }

        [Test]
        public void CreateChoosingParty_ValidationOnCountryGivesError()
        {
            //Arrange
            _cpInvalid = _cpValid;
            _cpInvalid.Country = string.Empty;
            var context = new ValidationContext(_cpInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_cpInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Country field is required.", results[0].ErrorMessage);
        }
        [Test]
        public void CreateChoosingParty_ValidationOnPublicationDateInPastGivesError()
        {
            //Arrange
            _cpInvalid = _cpValid;
            _cpInvalid.PartyDate = DateTime.Today.AddDays(-1);
            var context = new ValidationContext(_cpInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_cpInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Party Date cannot be in the past.", results[0].ErrorMessage);
        }

        [Test]
        public void EditChoosingParty_ValidationOnPublicationDateInPastNoesNotGiveError()
        {
            //Arrange
            _cpValid.Create = false; //edit
            _cpValid.PartyDate = DateTime.Today.AddDays(-1);
            var context = new ValidationContext(_cpValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_cpValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);            
            Assert.AreEqual(0, results.Count);
        }
      
       
    }



}


