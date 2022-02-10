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
    public class MockRevealEventFactory : Mock<IRevealEventFactory>
    {
        public async Task<MockRevealEventFactory> MockLoad(RevealEventVM result)
        {

            IList<WorkflowStatusVM> statusVMs = new List<WorkflowStatusVM>();
            Setup(x => x.GetWorkflowStatuses()).Returns(Task.Run(() => statusVMs));
            Setup(x => x.Load(It.IsAny<int>()))
                .Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockRevealEventFactory> MockLoadList(List<RevealEventVM> result)
        {
            if (result != null)
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<RevealEventVM>(result)));
            }
            else
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<RevealEventVM>()));
            }

            return await Task.FromResult(this);
        }

        public async Task<MockRevealEventFactory> MockCreate(ApiResponse result, IList<RevealEventVM> objList, RevealEventVM objCreate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<RevealEventVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Create(It.IsAny<RevealEventVM>())).Returns(Task.Run(() => result));
            if (objCreate != null && Convert.ToBoolean(result.ResponseObject))
            {
                objList.Add(objCreate);
            }
            return await Task.FromResult(this);
        }
        public async Task<MockRevealEventFactory> MockUpdate(ApiResponse result, IList<RevealEventVM> objList, RevealEventVM objUpdate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<RevealEventVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Update(It.IsAny<RevealEventVM>())).Returns(Task.Run(() => result));
            if (objUpdate != null && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.RevealEventId == objUpdate.RevealEventId).FirstOrDefault();
                objList.Remove(objDelete);
                objList.Add(objUpdate);
            }
            return await Task.FromResult(this);
        }

        public async Task<MockRevealEventFactory> MockDeleteLoad(RevealEventVM result, bool signupExists, int id)
        {
            Setup(x => x.Load(It.IsAny<int>()))
               .Returns(Task.Run(() => result));
            Setup(x => x.CheckRegistrationExists(It.IsAny<int>())).Returns(Task.Run(() => signupExists));

            return await Task.FromResult(this); ;
        }

        public async Task<MockRevealEventFactory> MockDelete(ApiResponse result, IList<RevealEventVM> objList, int id)
        {
            Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.Run(() => result));
            if (id > 0 && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.RevealEventId == id).FirstOrDefault();
                objList.Remove(objDelete);
            }
            return await Task.FromResult(this); ;
        }
        public async Task<MockRevealEventFactory> MockSignUpExists(bool result, IList<RevealEventVM> objList, int id)
        {
            Setup(x => x.CheckRegistrationExists(It.IsAny<int>())).Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public RevealEventVM MockFindById(IList<RevealEventVM> RevealEventVMModels, int id)
        {
            var result = RevealEventVMModels.Where(x => x.RevealEventId == id).SingleOrDefault();
            return result;
        }
        public RevealEventVM MockFindByName(List<RevealEventVM> RevealEventVMModels, string name)
        {
            var result = RevealEventVMModels.Where(x => x.Name == name).SingleOrDefault();
            return result;
        }

    }
}
