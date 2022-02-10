using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using United4Admin.WebApplication.ViewModels;

namespace United4Admin.UnitTests.ModelValidation
{
    public class SalutationValidationTest
    {
        SalutationVM _reValid = null;
        SalutationVM _reInvalid = null;

        [SetUp]
        public void TestInitialize()
        {
            _reValid = new SalutationVM
            {
                ddlSalutation = "Test Ms",
                crmSalutation = "3"
            };
        }

        [Test]
        public void CreateSalutation_ValidationOnValidModelPasses()
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
        public void CreateSalutation_ValidationOnSalutationGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.ddlSalutation = string.Empty;
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The United 4 Salutation field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSalutation_ValidationOnSalutationTooLongGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.ddlSalutation = "01234567890123456789012345678901234567890123456789101234567890123456789012345678901234567890123456789";
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
        public void CreateSalutation_ValidationOnSalutationValueGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmSalutation = string.Empty;
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The CRM Values field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void CreateSalutation_ValidationOnSalutationValueTooLongGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmSalutation = "012345678901234567890123456789012345";
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 30 characters", results[0].ErrorMessage);
        }
    }
}
