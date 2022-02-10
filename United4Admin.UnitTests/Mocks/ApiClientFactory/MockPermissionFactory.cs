using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Moq;


namespace United4Admin.UnitTests.Mocks.ApiClientFactory
{
    public class MockPermissionFactory : Mock<IPermissionFactory>
    {
        public async Task<MockPermissionFactory> MockLoad(PermissionsVM result)
        {
            Setup(x => x.Load(It.IsAny<int>()))
                .Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockPermissionFactory> MockLoadList(List<PermissionsVM> result)
        {
            if (result != null)
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<PermissionsVM>(result)));
            }
            else
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<PermissionsVM>()));
            }

            return await Task.FromResult(this);
        }

        public async Task<MockPermissionFactory> MockAdministrator(List<PermissionsVM> result)
        {
            if (result != null)
            {
                Setup(x => x.GetAdministrators()).Returns(Task.Run(() => new List<PermissionsVM>(result.Where(x=>x.Administrator ==true))));
            }
            else
            {
                Setup(x => x.GetAdministrators()).Returns(Task.Run(() => new List<PermissionsVM>()));
            }

            return await Task.FromResult(this);
        }

        public async Task<MockPermissionFactory> MockCreate(ApiResponse result, IList<PermissionsVM> objList, PermissionsVM objCreate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<PermissionsVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Create(It.IsAny<PermissionsVM>())).Returns(Task.Run(() => result));
            if (objCreate != null && Convert.ToBoolean(result.ResponseObject))
            {
                objList.Add(objCreate);
            }
            return await Task.FromResult(this);
        }
        public async Task<MockPermissionFactory> MockUpdate(ApiResponse result, IList<PermissionsVM> objList, PermissionsVM objUpdate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<PermissionsVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Update(It.IsAny<PermissionsVM>())).Returns(Task.Run(() => result));
            if (objUpdate != null && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.PermissionsId == objUpdate.PermissionsId).FirstOrDefault();
                objList.Remove(objDelete);
                objList.Add(objUpdate);
            }
            return await Task.FromResult(this);
        }

        public async Task<MockPermissionFactory> MockDeleteLoad(PermissionsVM result, bool sameUser, int id)
        {
            Setup(x => x.Load(It.IsAny<int>()))
               .Returns(Task.Run(() => result));           

            return await Task.FromResult(this); ;
        }

        public async Task<MockPermissionFactory> MockDelete(ApiResponse result, IList<PermissionsVM> objList, int id)
        {
            Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.Run(() => result));
            if (id > 0 && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.PermissionsId == id).FirstOrDefault();
                objList.Remove(objDelete);
            }
            return await Task.FromResult(this); ;
        }
      
        public PermissionsVM MockFindById(IList<PermissionsVM> PermissionsVMModels, int id)
        {
            var result = PermissionsVMModels.Where(x => x.PermissionsId == id).SingleOrDefault();
            return result;
        }
        public PermissionsVM MockFindByName(List<PermissionsVM> PermissionsVMModels, string name)
        {
            var result = PermissionsVMModels.Where(x => x.WVEmail == name).SingleOrDefault();
            return result;
        }

    }
}

    
    

