
using United4Admin.UnitTests.Mocks.ApiClientFactory;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.Controllers;

using United4Admin.WebApplication.ViewModels;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace United4Admin.UnitTests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        private HomeController _homeController;
        private Mock<ILogger<HomeController>> _mockLogger;
        private MockPermissionFactory mockPermissionFactory;

        private PermissionsVM _mockResponse;     

        private List<PermissionsVM> _PermissionsVMList;
        private ApiResponse _apiReponse;

        [SetUp]
        public void TestInitialize()
        {
            //Creating Dummy PermissionsVM List
            _PermissionsVMList = new List<PermissionsVM>
                    {
                        new PermissionsVM {   PermissionsId = 1,
                            WVEmail = "fred.bloggs@worldvision.org.uk",
                            Administrator = true,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = true,
                            DownloadFilesandImages = true},
                        new PermissionsVM {  PermissionsId = 2,
                            WVEmail = "test.mouse@worldvision.org.uk",
                            Administrator = true,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = false,
                            DownloadFilesandImages = false
                        },
                        new PermissionsVM {  PermissionsId = 3,
                            WVEmail = "test@worldvision.org.uk",
                            Administrator = false,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = false,
                            DownloadFilesandImages = false
                        },
                        new PermissionsVM { PermissionsId = 4,
                            WVEmail = "test.rockster@worldvision.org.uk",
                            Administrator = false,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = false,
                            DownloadFilesandImages = false}
                    };

            //logger
            //  _mockLogger = new Mock<ILogger<AdministrationController>>();

            // Mock the API Response 
            _mockResponse = new PermissionsVM();


            //Setup mockPermissionFactory
            mockPermissionFactory = new MockPermissionFactory();

            //logger
            _mockLogger = new Mock<ILogger<HomeController>>();

            // ApiResponse
            _apiReponse = new ApiResponse();
        }

        [Test]
        public void NotAuthorised_ModelsReturned()
        {
            // Loading all PermissionsVM
            var mockPermissionFactoryResult = new MockPermissionFactory().MockAdministrator(_PermissionsVMList);

            //Setup AdministrationController
            _homeController = new HomeController(mockPermissionFactoryResult.Result.Object, _mockLogger.Object);

            // Act
            var actionResult = _homeController.NotAuthorised();
            var actionResponse = actionResult.Result as ViewResult;
            var dataResult = actionResponse.ViewData.Model as List<PermissionsVM>;


            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), actionResponse); //Test if returns ok response    
            Assert.AreEqual(dataResult.Count, 2);
        }
    }
}
