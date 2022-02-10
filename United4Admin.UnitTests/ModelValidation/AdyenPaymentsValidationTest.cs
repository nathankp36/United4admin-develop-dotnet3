using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using System.Linq;
using United4Admin.WebApplication.ViewModels;
using System;
using United4Admin.UnitTests.Mocks.ApiClientFactory;

namespace United4Admin.UnitTests.ModelValidation
{
    [TestFixture]
    public class AdyenPaymentsValidationTest
    {
        AdyenTransactionVM _adyenValid = null;
        AdyenTransactionVM _adyenInvalid = null;

        [SetUp]
        public void TestInitialize()
        {
            _adyenValid = new AdyenTransactionVM
            {
                AdyenTransactionId = 5,
                LastTransactionId = 9,
                RecurringToken = "GB37BARC2000004666",
                BillCycleStartDate = Convert.ToDateTime("2020-09-09"),
                BillCycleNextDate = Convert.ToDateTime("2020-10-09"),
                LastPaymentDate = Convert.ToDateTime("2020-09-09"),
                LastPaymentStatus = "A",
                NoOfRetryAttempts = 0,
                ContactId = 1,
                Amount = 2500,
                ShopperReference = "WVRecTest-2",
                CurrencyCode = "GBP"
            };
        }

        [Test]
        public void CreateAdyenTransaction_ValidationOnValidModelPasses()
        {
            //Arrange           
            var context = new ValidationContext(_adyenValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_adyenValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreateAdyenTransaction_ValidationOnCurrCodeGivesError()
        {
            //Arrange
            _adyenInvalid = _adyenValid;
            _adyenValid.CurrencyCode = string.Empty;
            var context = new ValidationContext(_adyenValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_adyenValid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Currency Code field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void CreateAdyenTransaction_ValidationOnRecurTokGivesError()
        {
            //Arrange
            _adyenInvalid = _adyenValid;
            _adyenValid.RecurringToken = string.Empty;
            var context = new ValidationContext(_adyenValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_adyenValid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Recurring Token field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void CreateAdyenTransaction_ValidationShpRefCodeGivesError()
        {
            //Arrange
            _adyenInvalid = _adyenValid;
            _adyenValid.ShopperReference = string.Empty;
            var context = new ValidationContext(_adyenValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_adyenValid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Shopper Reference field is required.", results[0].ErrorMessage);
        }


        [Test]
        public void CreateAdyenTransaction_ValidationPayStatusGivesError()
        {
            //Arrange
            _adyenInvalid = _adyenValid;
            _adyenValid.LastPaymentStatus = string.Empty;
            var context = new ValidationContext(_adyenValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_adyenValid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The Last Payment Status field is required.", results[0].ErrorMessage);
        }

        [Test]
        public void CreateAdyenTransaction_ValidationOnAmountGivesError()
        {
            //Arrange
            _adyenValid.Amount = 0;
            var context = new ValidationContext(_adyenValid, null, null);
            var results = new List<ValidationResult>();
            var MockAdyenCardManagerResult = new MockAdyenPaymentsFactory().MockValidAmount(_adyenValid);

            //Act
            var isModelStateValid = Validator.TryValidateObject(_adyenValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(true, MockAdyenCardManagerResult);
        }

        [Test]
        public void CreateAdyenTransaction_ValidationOnContactIdGivesError()
        {
            //Arrange
            _adyenValid.ContactId = 0;
            var context = new ValidationContext(_adyenValid, null, null);
            var results = new List<ValidationResult>();
            var MockAdyenCardManagerResult = new MockAdyenPaymentsFactory().MockValidContactId(_adyenValid);

            //Act
            var isModelStateValid = Validator.TryValidateObject(_adyenValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(true, MockAdyenCardManagerResult);
        }

        [Test]
        public void CreateAdyenTransaction_ValidationOnLastTransIdGivesError()
        {
            //Arrange
            _adyenValid.LastTransactionId = 0;
            var context = new ValidationContext(_adyenValid, null, null);
            var results = new List<ValidationResult>();
            var MockAdyenCardManagerResult = new MockAdyenPaymentsFactory().MockValidContactId(_adyenValid);

            //Act
            var isModelStateValid = Validator.TryValidateObject(_adyenValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(true, MockAdyenCardManagerResult);
        }
    }
}
