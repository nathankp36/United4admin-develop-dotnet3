using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using United4Admin.WebApplication.ViewModels;

namespace United4Admin.UnitTests.ModelValidation
{
    public class PaymentMethodValidationTest
    {
        PaymentMethodVM _reValid = null;
        PaymentMethodVM _reInvalid = null;

        [SetUp]
        public void TestInitialize()
        {
            _reValid = new PaymentMethodVM
            {
                crmPaymentMethodName = "Test Payment",
                crmPaymentMethodType = "Test type"
            };
        }

        [Test]
        public void CreatePaymentMethod_ValidationOnValidModelPasses()
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
        public void CreatePaymentMethod_ValidationOnNameGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmPaymentMethodName = string.Empty;
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter a Payment Method Name", results[0].ErrorMessage);
        }

        [Test]
        public void CreatePaymentMethod_ValidationOnPaymentNameTooLongGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmPaymentMethodName = "01234567890123456789012345678901234567890123456789101234567890123456789012345678901234567890123456789";
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
        public void CreatePaymentMethod_ValidationOnPaymentTypeValueGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmPaymentMethodType = string.Empty;
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter a Payment Method Type", results[0].ErrorMessage);
        }

        [Test]
        public void CreatePaymentMethod_ValidationOnPaymentTypeTooLongGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmPaymentMethodType = "012345678901234567890123456789012345";
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
