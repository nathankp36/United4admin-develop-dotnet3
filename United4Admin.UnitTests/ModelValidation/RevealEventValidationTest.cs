using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using System.Linq;
using United4Admin.WebApplication.ViewModels;
using System;

namespace United4Admin.UnitTests.ModelValidation
{
   [TestFixture]
    public class RevealEventValidationTest
    {
        RevealEventVM _reValid = null;
        RevealEventVM _reInvalid = null;

         [SetUp]
        public void TestInitialize()
        {
            _reValid = new RevealEventVM
            {
                Name = "Ps & Gs Chosen Reveal Event",
                EventDate = DateTime.Today,
                Location = "Edinburgh",
                TypeOfReveal = "WV Reveal Event",
                WorkflowStatusId = 1,
                Create = true
            };

        }

        [Test]
        public void CreateRevealEvent_ValidationOnValidModelPasses()
        {
            //Arrange           
            var context = new ValidationContext(_reValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateRevealEvent_ValidationOnNameGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.Name = string.Empty;
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Event Name field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void CreateRevealEvent_ValidationOnNameTooLongGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.Name = "012345678901234567890123456789012345678901234567891";
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 50 characters", results[0].ErrorMessage);
        }

        [Test]
        public void CreateRevealEvent_ValidationOnLocationGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.Location = string.Empty;
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Location field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void CreateRevealEvent_ValidationOnLocationTooLongGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.Location = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901";
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 100 characters", results[0].ErrorMessage);
        }
       
        [Test]
        public void CreateRevealEvent_ValidationOnEventDateInPastGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.EventDate = DateTime.Today.AddDays(-1);
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Event date cannot be in the past.", results[0].ErrorMessage);
        }

        [Test]
        public void EditRevealEvent_ValidationOnEventDateInPastNoesNotGiveError()
        {
            //Arrange
            _reValid.Create = false; //edit
            _reValid.EventDate = DateTime.Today.AddDays(-1);
            var context = new ValidationContext(_reValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }


    }



}


