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
    public class MockSalutationFactory : Mock<ISalutationFactory>
    {
        public async Task<MockSalutationFactory> MockLoad(SalutationVM result)
        {
            Setup(x => x.LoadListById(It.IsAny<string>()))
                .Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockSalutationFactory> MockLoadList(List<SalutationVM> result)
        {
            if (result != null)
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<SalutationVM>(result)));
            }
            else
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<SalutationVM>()));
            }

            return await Task.FromResult(this);
        }

        public async Task<MockSalutationFactory> MockCreate(ApiResponse result, IList<SalutationVM> objList, SalutationVM objCreate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<SalutationVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Create(It.IsAny<SalutationVM>())).Returns(Task.Run(() => result));
            if (objCreate != null && Convert.ToBoolean(result.ResponseObject))
            {
                objList.Add(objCreate);
            }
            return await Task.FromResult(this);
        }
        public async Task<MockSalutationFactory> MockUpdate(ApiResponse result, IList<SalutationVM> objList, SalutationVM objUpdate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<SalutationVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Update(It.IsAny<SalutationVM>())).Returns(Task.Run(() => result));
            if (objUpdate != null && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.ddlSalutation == objUpdate.ddlSalutation).FirstOrDefault();
                objList.Remove(objDelete);
                objList.Add(objUpdate);
            }
            return await Task.FromResult(this);
        }

        public async Task<MockSalutationFactory> MockDeleteLoad(SalutationVM result, string id)
        {
            Setup(x => x.LoadListById(It.IsAny<string>()))
               .Returns(Task.Run(() => result));

            return await Task.FromResult(this); ;
        }

        public async Task<MockSalutationFactory> MockDelete(ApiResponse result, IList<SalutationVM> objList, string id)
        {
            Setup(x => x.DeleteById(It.IsAny<string>())).Returns(Task.Run(() => result));
            if (!String.IsNullOrEmpty(id) && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.ddlSalutation == id).FirstOrDefault();
                objList.Remove(objDelete);
            }
            return await Task.FromResult(this); ;
        }

        public SalutationVM MockFindById(IList<SalutationVM> SalutationVMModels, string id)
        {
            var result = SalutationVMModels.Where(x => x.ddlSalutation == id).SingleOrDefault();
            return result;
        }
        public SalutationVM MockFindByName(List<SalutationVM> SalutationVMModels, string name)
        {
            var result = SalutationVMModels.Where(x => x.ddlSalutation == name).SingleOrDefault();
            return result;
        }
    }
}
