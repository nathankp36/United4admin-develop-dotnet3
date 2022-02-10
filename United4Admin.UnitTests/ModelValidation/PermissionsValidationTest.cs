using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using System.Linq;
using United4Admin.WebApplication.ViewModels;
using System;

namespace United4Admin.UnitTests.ModelValidation
{
   [TestFixture]
    public class PermissionsValidationTest
    {
        PermissionsVM _pValid = null;
        PermissionsVM _pInvalid = null;

         [SetUp]
        public void TestInitialize()
        {
            _pValid = new PermissionsVM
            {
                PermissionsId = 3,
                WVEmail = "postman.pat@worldvision.org.uk",
                Administrator = false,
                EditDeleteSupporterData = true,
                CreateEditDeleteEvents = true,
                DownloadFilesandImages = true
            };

        }

        [Test]
        public void CreatePermissions_ValidationOnValidModelPasses()
        {
            //Arrange           
            var context = new ValidationContext(_pValid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_pValid, context, results, true);

            // Assert
            Assert.IsTrue(isModelStateValid);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void CreatePermissions_ValidationOnEmailGivesError()
        {
            //Arrange
            _pInvalid = _pValid;
            _pInvalid.WVEmail = string.Empty;
            var context = new ValidationContext(_pInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_pInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("The World Vision Email Address field is required.", results[0].ErrorMessage);
        }
        [Test]
        public void CreatePermissions_ValidationOnEmailNotRightFormatGivesError()
        {
            //Arrange
            _pInvalid = _pValid;
            _pInvalid.WVEmail = "postman.pat";
            var context = new ValidationContext(_pInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_pInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Invalid email address", results[0].ErrorMessage);
        }

        [Test]
        public void CreatePermissions_ValidationOnEmailTooLongGivesError()
        {
            //Arrange
            _pInvalid = _pValid;
            _pInvalid.WVEmail = "a.really.long.email.address.a.really.long.email.address.a.really.long.email.address.a.really.long.email.address@areallylongdomainnameareallylongdomainnameareallylongdomainnameareallylongdomainnameareallylongdomainnameareallylongdomainnameareallylong.co.uk";
            var context = new ValidationContext(_pInvalid, null, null);
            var results = new List<ValidationResult>();

            //Act
            var isModelStateValid = Validator.TryValidateObject(_pInvalid, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Please don't enter more than 254 characters", results[0].ErrorMessage);
        }
     
       
    }



}


