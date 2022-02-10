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
    public class MockSignupEventFactory : Mock<ISignupEventFactory>
    {
        public async Task<MockSignupEventFactory> MockLoad(SignUpEventVM result)
        {

            IList<WorkflowStatusVM> statusVMs = new List<WorkflowStatusVM>();
            Setup(x => x.GetWorkflowStatuses()).Returns(Task.Run(() => statusVMs));            
            Setup(x => x.Load(It.IsAny<int>()))
                .Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockSignupEventFactory> MockLoadList(List<SignUpEventVM> result)
        {
            if (result != null)
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<SignUpEventVM>(result)));
            }
            else
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<SignUpEventVM>()));
            }

            return await Task.FromResult(this);
        }

        public async Task<MockSignupEventFactory> MockCreate(ApiResponse result, IList<SignUpEventVM> objList, SignUpEventVM objCreate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<SignUpEventVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Create(It.IsAny<SignUpEventVM>())).Returns(Task.Run(() => result));
            if (objCreate != null && Convert.ToBoolean(result.ResponseObject))
            {
                objList.Add(objCreate);
            }
            return await Task.FromResult(this);
        }
        public async Task<MockSignupEventFactory> MockUpdate(ApiResponse result, IList<SignUpEventVM> objList, SignUpEventVM objUpdate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<SignUpEventVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Update(It.IsAny<SignUpEventVM>())).Returns(Task.Run(() => result));
            if (objUpdate != null && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.SignUpEventId == objUpdate.SignUpEventId).FirstOrDefault();
                objList.Remove(objDelete);
                objList.Add(objUpdate);
            }
            return await Task.FromResult(this);
        }

        public async Task<MockSignupEventFactory> MockDeleteLoad(SignUpEventVM result, bool signupExists, int id)
        {
            Setup(x => x.Load(It.IsAny<int>()))
               .Returns(Task.Run(() => result));
            Setup(x => x.CheckRegistrationExists(It.IsAny<int>())).Returns(Task.Run(() => signupExists));

            return await Task.FromResult(this); ;
        }

        public async Task<MockSignupEventFactory> MockDelete(ApiResponse result, IList<SignUpEventVM> objList, int id)
        {
            Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.Run(() => result));
            if (id > 0 && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.SignUpEventId == id).FirstOrDefault();
                objList.Remove(objDelete);
            }
            return await Task.FromResult(this); ;
        }
        public async Task<MockSignupEventFactory> MockSignUpExists(bool result, IList<SignUpEventVM> objList, int id)
        {
            Setup(x => x.CheckRegistrationExists(It.IsAny<int>())).Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public SignUpEventVM MockFindById(IList<SignUpEventVM> SignUpEventVMModels, int id)
        {
            var result = SignUpEventVMModels.Where(x => x.SignUpEventId == id).SingleOrDefault();
            return result;
        }
        public SignUpEventVM MockFindByName(List<SignUpEventVM> SignUpEventVMModels, string name)
        {
            var result = SignUpEventVMModels.Where(x => x.EventName == name).SingleOrDefault();
            return result;
        }

    }
}
