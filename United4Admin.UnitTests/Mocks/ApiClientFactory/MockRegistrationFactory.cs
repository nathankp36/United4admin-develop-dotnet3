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

    public class MockRegistrationFactory : Mock<IRegistrationFactory>
    {
        public async Task<MockRegistrationFactory> MockLoad(SignUpVM result)
        {

            List<StatusVM> statusVMs = new List<StatusVM>();
            List<TitleVM> titleVMs = new List<TitleVM>();
            List<ChoosingPartyVM> choosingPartyVMs = new List<ChoosingPartyVM>();
            List<RevealEventVM> revealEventVMs = new List<RevealEventVM>();
            List<SignUpEventVM> signUpEventVMs = new List<SignUpEventVM>();

            Setup(x => x.GetStatuses()).Returns(Task.Run(() => statusVMs));
            Setup(x => x.GetTitles()).Returns(Task.Run(() => titleVMs));
            Setup(x => x.GetChoosingPartyList()).Returns(Task.Run(() => choosingPartyVMs));
            Setup(x => x.GetRevealEventList()).Returns(Task.Run(() => revealEventVMs));
            Setup(x => x.GetSignupEventList()).Returns(Task.Run(() => signUpEventVMs));


            Setup(x => x.Load(It.IsAny<int>()))
                .Returns(Task.Run(() => result));

            return await Task.FromResult(this);
        }

        public async Task<MockRegistrationFactory> MockLoadList(List<SignUpVM> result)
        {
            if (result != null)
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<SignUpVM>(result)));
            }
            else
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<SignUpVM>()));
            }

            return await Task.FromResult(this);
        }

        public async Task<MockRegistrationFactory> MockCreate(ApiResponse result, IList<SignUpVM> objList, SignUpVM objCreate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<SignUpVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Create(It.IsAny<SignUpVM>())).Returns(Task.Run(() => result));
            if (objCreate != null && Convert.ToBoolean(result.ResponseObject))
            {
                objList.Add(objCreate);
            }
            return await Task.FromResult(this);
        }
        public async Task<MockRegistrationFactory> MockUpdate(ApiResponse result, IList<SignUpVM> objList, SignUpVM objUpdate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<SignUpVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Update(It.IsAny<SignUpVM>())).Returns(Task.Run(() => result));
            if (objUpdate != null && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.chosenSignUpId == objUpdate.chosenSignUpId).FirstOrDefault();
                objList.Remove(objDelete);
                objList.Add(objUpdate);
            }
            return await Task.FromResult(this);
        }

        public async Task<MockRegistrationFactory> MockDeleteLoad(SignUpVM result, bool signupExists, int id)
        {
            Setup(x => x.Load(It.IsAny<int>()))
               .Returns(Task.Run(() => result));          

            return await Task.FromResult(this); ;
        }

        public async Task<MockRegistrationFactory> MockDelete(ApiResponse result, IList<SignUpVM> objList, int id)
        {
            Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.Run(() => result));
            if (id > 0 && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.chosenSignUpId == id).FirstOrDefault();
                objList.Remove(objDelete);
            }
            return await Task.FromResult(this); ;
        }
        public async Task<MockRegistrationFactory> MockGetEchoData(bool result, List<SignUpVM> objList)
        {
            if (result)
            {
                Setup(x => x.GetEchoData(It.IsAny<SignUpExtractParameterVM>())).Returns(Task.Run(() => objList));
            }
            else
            {
                Setup(x => x.GetEchoData(It.IsAny<SignUpExtractParameterVM>())).Returns(Task.Run(() => new List<SignUpVM>()));
            }
            return await Task.FromResult(this);
        }

        public async Task<MockRegistrationFactory> MockGetFieldData(bool result, List<ImageInfoVM> objList)
        {
            if (result)
            {
                Setup(x => x.GetFieldDataExport(It.IsAny<SignUpExtractParameterVM>())).Returns(Task.Run(() => objList));
            }
            else
            {
                Setup(x => x.GetFieldDataExport(It.IsAny<SignUpExtractParameterVM>())).Returns(Task.Run(() => new List<ImageInfoVM>()));
            }
            return await Task.FromResult(this);
        }
        public async Task<MockRegistrationFactory> MockGetImage(bool result, ImageInfoVM objList)
        {
            //if (result)
            //{
                Setup(x => x.GetImage(It.IsAny<int>())).Returns(Task.Run(() => objList));
            //}
            //else
            //{
            //    Setup(x => x.GetImage(It.IsAny<int>())).Returns(Task.Run(() => null));
            //}
            return await Task.FromResult(this);
        }
        

        public SignUpVM MockFindById(IList<SignUpVM> SignUpVMModels, int id)
        {
            var result = SignUpVMModels.Where(x => x.chosenSignUpId == id).SingleOrDefault();
            return result;
        }
        public SignUpVM MockFindByName(List<SignUpVM> SignUpVMModels, string name)
        {
            var result = SignUpVMModels.Where(x => x.FirstName == name).SingleOrDefault();
            return result;
        }

    }
}
