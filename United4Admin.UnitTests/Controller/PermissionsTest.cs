using ChildSponsorship.WebApplication.Controllers;
using ChildSponsorship.WebApplication.ApiClientFactory;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using AutoMapper;
using ChildSponsorship.WebApplication.ViewModels;
using ChildSponsorship.WebApplication.ApiClientFactory.FactoryInterfaces;
using System.Threading.Tasks;
using ChildSponsorship.WebApplication.BLL;

namespace ChildSponsorship.UnitTests.Controllers
{
    [TestFixture]
    class PermissionsTest
    {
        private BaseController _controller;
        private Mock<IPermissionFactory> _permissionFactory;
        private Mock<RequestContext> _httpContextMock;
        private Mock<HttpResponseBase> _response;
        private string _anotherUserWithoutRecord;
        List<PermissionsVM> _PermissionsVMList;
        private object Session;

        [SetUp]
        public void Setup()
        {

            _PermissionsVMList = new List<PermissionsVM>
                    {
                        new PermissionsVM {   Id = 1,
                            WVEmail = "hairy.mclarey@worldvision.org.uk",
                            Admininstrator = true,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = true,
                            DownloadFilesandImages = true},
                        new PermissionsVM {  Id = 2,
                            WVEmail = "test.mouse@worldvision.org.uk",
                            Admininstrator = true,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = false,
                            DownloadFilesandImages = false
                        },
                        new PermissionsVM {  Id = 3,
                            WVEmail = "test@worldvision.org.uk",
                            Admininstrator = false,
                            EditDeleteSupporterData = true,
                            CreateEditDeleteEvents = false,
                            DownloadFilesandImages = false
                        },
                        new PermissionsVM { Id = 4,
                            WVEmail = "bottomley.potts@worldvision.org.uk",
                            Admininstrator = false,
                            EditDeleteSupporterData = false,
                            CreateEditDeleteEvents = false,
                            DownloadFilesandImages = false}
                    };


            _permissionFactory = new Mock<IPermissionFactory>();
            //set up the repo to have two records
            _permissionFactory.Setup(x => x.LoadList()).Returns(Task.Run(() => _PermissionsVMList));
            //set up another mock user identity which is not found in the permissions repository
            _anotherUserWithoutRecord = "victor.voncrum@worldvision.org.uk";

            //base controller handles the permissions - create this controller passing in the mocked repository
            _controller = new BaseController(_permissionFactory.Object);

            _httpContextMock = new Mock<RequestContext>();
            _response = new Mock<HttpResponseBase>();
            _httpContextMock.Setup(x => x.HttpContext.Response).Returns(_response.Object);

        }

        [Test]
        public void AdministrationController_WhenCalledWithAdministratorPermissions_PassesTrue()
        {
            //arrange
            //bool has to be passed to allow the test to be evaluated - the context mock object will not pick up the redirect if not authorised
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Administration");
            routeData.Values.Add("action", "UserIndex");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("hairy.mclarey@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsTrue(userAllowed);
        }

        [Test]
        public void AdministrationController_WhenCalledWithoutAdministratorPermissions_PassesFalse()
        {
            //arrange
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Administration");
            routeData.Values.Add("action", "UserIndex");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("bottomley.potts@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void AdministrationController_WhenCalledWithUnknownUser_PassesFalse()
        {
            //arrange
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Administration");
            routeData.Values.Add("action", "UserEdit");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions(_anotherUserWithoutRecord, _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void RegistrationController_WhenEditCalledWithEditPermissions_PassesTrue()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Registrations");
            routeData.Values.Add("action", "Edit");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("hairy.mclarey@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsTrue(userAllowed);
        }

        [Test]
        public void RegistrationController_WhenEditCalledWithoutEditPermissions_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Registrations");
            routeData.Values.Add("action", "Edit");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("bottomley.potts@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void RegistrationController_WhenEditCalledWithUnknownUser_PassesFalse()
        {
            //arrange
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Registrations");
            routeData.Values.Add("action", "Edit");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions(_anotherUserWithoutRecord, _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void RegistrationController_WhenDownloadPhotosCalledWithDownloadPermissions_PassesTrue()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Registrations");
            routeData.Values.Add("action", "DownloadPhotos");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("hairy.mclarey@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsTrue(userAllowed);
        }

        [Test]
        public void RegistrationController_WhenDownloadECHODataCalledWithDownloadPermissions_PassesTrue()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Registrations");
            routeData.Values.Add("action", "DownloadECHOData");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("hairy.mclarey@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsTrue(userAllowed);
        }

        [Test]
        public void RegistrationController_WhenDownloadPhotosCalledWithoutDownloadPermissions_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Registrations");
            routeData.Values.Add("action", "DownloadPhotos");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("bottomley.potts@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void RegistrationController_WhenDownloadECHODataCalledWithoutDownloadPermissions_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Registrations");
            routeData.Values.Add("action", "DownloadECHOData");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("bottomley.potts@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void RegistrationController_WhenDownloadFieldDataCalledWithoutDownloadPermissions_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Registrations");
            routeData.Values.Add("action", "DownloadFieldData");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("bottomley.potts@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void RegistrationController_WhenDownloadFieldDataCalledWithDownloadPermissions_PassesTrue()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Registrations");
            routeData.Values.Add("action", "DownloadECHOData");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("hairy.mclarey@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsTrue(userAllowed);
        }

        [Test]
        public void RegistrationController_WhenDownloadPhotosCalledWithUnknownUser_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Registrations");
            routeData.Values.Add("action", "DownloadPhotos");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions(_anotherUserWithoutRecord, _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void RegistrationController_WhenDownloadECHODataCalledWithUnknownUser_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Registrations");
            routeData.Values.Add("action", "DownloadECHOData");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions(_anotherUserWithoutRecord, _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void RegistrationController_WhenDownloadFieldDataCalledWithUnknownUser_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Registrations");
            routeData.Values.Add("action", "DownloadFieldData");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions(_anotherUserWithoutRecord, _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void RegistrationEventController_WhenCalledWithEventPermissions_PassesTrue()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "RegistrationEvent");
            routeData.Values.Add("action", "Index");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("hairy.mclarey@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsTrue(userAllowed);
        }

        [Test]
        public void RegistrationEventController_WhenCalledWithoutEventPermissions_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "RegistrationEvent");
            routeData.Values.Add("action", "Index");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("bottomley.potts@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void RegistrationEventController_WhenCalledWithUnknownUser_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "RegistrationEvent");
            routeData.Values.Add("action", "Index");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions(_anotherUserWithoutRecord, _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }


        [Test]
        public void ChoosingEventController_WhenCalledWithEventPermissions_PassesTrue()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "ChoosingEvent");
            routeData.Values.Add("action", "Index");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("hairy.mclarey@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsTrue(userAllowed);
        }

        [Test]
        public void ChoosingEventController_WhenCalledWithoutEventPermissions_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "ChoosingEvent");
            routeData.Values.Add("action", "Index");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("bottomley.potts@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void ChoosingEventController_WhenCalledWithUnknownUser_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "ChoosingEvent");
            routeData.Values.Add("action", "Index");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions(_anotherUserWithoutRecord, _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void RevealEventController_WhenCalledWithEventPermissions_PassesTrue()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "RevealEvent");
            routeData.Values.Add("action", "Index");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("hairy.mclarey@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsTrue(userAllowed);
        }

        [Test]
        public void RevealEventController_WhenCalledWithoutEventPermissions_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "RevealEvent");
            routeData.Values.Add("action", "Index");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions("bottomley.potts@worldvision.org.uk", _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }

        [Test]
        public void RevealEventController_WhenCalledWithUnknownUser_PassesFalse()
        {
            //arrange            
            bool userAllowed = new bool();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "RevealEvent");
            routeData.Values.Add("action", "Index");
            _httpContextMock.Setup(x => x.RouteData).Returns(routeData);

            //act
            _controller.CheckPermissions(_anotherUserWithoutRecord, _httpContextMock.Object, ref userAllowed);

            //assert
            Assert.IsFalse(userAllowed);
        }
    }
}
