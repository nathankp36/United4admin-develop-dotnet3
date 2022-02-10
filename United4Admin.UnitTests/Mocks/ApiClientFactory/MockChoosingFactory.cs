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
    public class MockChoosingFactory : Mock<IChoosingEventFactory>
    {
        public async Task<MockChoosingFactory> MockLoad(ChoosingPartyVM result)
        {

            IList<WorkflowStatusVM> statusVMs = new List<WorkflowStatusVM>();
            Setup(x => x.GetWorkflowStatuses()).Returns(Task.Run(() =>statusVMs));
            Setup(x => x.Load(It.IsAny<int>()))
                .Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockChoosingFactory> MockLoadList(List<ChoosingPartyVM> result)
        {
            if (result != null)
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<ChoosingPartyVM>(result)));
            }
            else
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<ChoosingPartyVM>()));
            }

            return await Task.FromResult(this);
        }

        public async Task<MockChoosingFactory> MockCreate(ApiResponse result, IList<ChoosingPartyVM> objList, ChoosingPartyVM objCreate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<ChoosingPartyVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Create(It.IsAny<ChoosingPartyVM>())).Returns(Task.Run(() => result));
            if (objCreate != null && Convert.ToBoolean(result.ResponseObject))
            {
                objList.Add(objCreate);
            }
            return await Task.FromResult(this);
        }
        public async Task<MockChoosingFactory> MockUpdate(ApiResponse result, IList<ChoosingPartyVM> objList, ChoosingPartyVM objUpdate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<ChoosingPartyVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Update(It.IsAny<ChoosingPartyVM>())).Returns(Task.Run(() => result));
            if (objUpdate != null && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.ChoosingPartyId == objUpdate.ChoosingPartyId).FirstOrDefault();
                objList.Remove(objDelete);
                objList.Add(objUpdate);
            }
            return await Task.FromResult(this);
        }

        public async Task<MockChoosingFactory> MockDeleteLoad(ChoosingPartyVM result, bool signupExists, int id)
        {
            Setup(x => x.Load(It.IsAny<int>()))
               .Returns(Task.Run(() => result));
            Setup(x => x.CheckRegistrationExists(It.IsAny<int>())).Returns(Task.Run(() => signupExists));
          
            return await Task.FromResult(this); ;
        }

        public async Task<MockChoosingFactory> MockDelete(ApiResponse result, IList<ChoosingPartyVM> objList, int id)
        {
            Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.Run(() => result));
            if (id > 0 && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.ChoosingPartyId == id).FirstOrDefault();
                objList.Remove(objDelete);
            }
            return await Task.FromResult(this); ;
        }
        public async Task<MockChoosingFactory> MockSignUpExists(bool result, IList<ChoosingPartyVM> objList, int id)
        {
            Setup(x => x.CheckRegistrationExists(It.IsAny<int>())).Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public ChoosingPartyVM MockFindById(IList<ChoosingPartyVM> ChoosingPartyVMModels, int id)
        {
            var result = ChoosingPartyVMModels.Where(x => x.ChoosingPartyId == id).SingleOrDefault();
            return result;
        }
        public ChoosingPartyVM MockFindByName(List<ChoosingPartyVM> ChoosingPartyVMModels, string name)
        {
            var result = ChoosingPartyVMModels.Where(x => x.PartyName == name).SingleOrDefault();
            return result;
        }
      
    }
}
