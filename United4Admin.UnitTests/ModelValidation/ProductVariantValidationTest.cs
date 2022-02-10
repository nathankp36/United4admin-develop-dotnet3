using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using United4Admin.WebApplication.ViewModels;

namespace United4Admin.UnitTests.ModelValidation
{
    public class ProductVariantValidationTest
    {
        ProductVariantVM _reValid = null;
        ProductVariantVM _reInvalid = null;

        [SetUp]
        public void TestInitialize()
        {
            _reValid = new ProductVariantVM
            {
                ddlProductTypeCodeDisplay = "Product type code",
                crmProductVariantName = "Product name",
                crmIncidentType = "Incident type",
                crmPurposeCode = "253683",
                crmPledgeType = "Pledge type"
            };
        }

        [Test]
        public void CreateProductVaiant_ValidationOnValidModelPasses()
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
        public void CreateProductVaiant_ValidationOnProductTypeCodeGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.ddlProductTypeCodeDisplay = string.Empty;
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter a Product Variant ID", results[0].ErrorMessage);
        }

        [Test]
        public void CreateProductVaiant_ValidationOnProductTypeCodeTooLongGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmProductVariantName = "01234567890123456789012345678901234567890123456789101234567890123456789012345678901234567890123456789";
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
        public void CreateProductVaiant_ValidationOnProductNameGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmProductVariantName = string.Empty;
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter a Product Variant Name", results[0].ErrorMessage);
        }

        [Test]
        public void CreateProductVaiant_ValidationOnProductNameTooLongGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmProductVariantName = "01234567890123456789012345678901234567890123456789101234567890123456789012345678901234567890123456789";
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
        public void CreateProductVaiant_ValidationOnIncidenttypeValueGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmIncidentType = string.Empty;
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please select a Navision Incident Type", results[0].ErrorMessage);
        }

        [Test]
        public void CreateProductVaiant_ValidationOnPurposeCodeValueGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmPurposeCode = string.Empty;
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please enter a Navision Purpose Code", results[0].ErrorMessage);
        }

        [Test]
        public void CreateProductVaiant_ValidationOnPurposeCodeTooLongGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmPurposeCode = "01234567890123";
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 10 characters", results[0].ErrorMessage);
        }

        [Test]
        public void CreateProductVaiant_ValidationOnPurposeCodeNonNumericGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmPurposeCode = "asdf$%";
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Invalid UK postcode", results[0].ErrorMessage);
        }

        [Test]
        public void CreateProductVaiant_ValidationOnPledgeTypeValueGivesError()
        {
            //Arrange
            _reInvalid = _reValid;
            _reInvalid.crmPledgeType = string.Empty;
            var context = new ValidationContext(_reInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_reInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please select a Navision Pledge Type", results[0].ErrorMessage);
        }
    }
}
