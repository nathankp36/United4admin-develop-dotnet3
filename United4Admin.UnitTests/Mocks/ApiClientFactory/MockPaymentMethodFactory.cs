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
    public class MockPaymentMethodFactory: Mock<IPaymentMethodFactory>
    {
        public async Task<MockPaymentMethodFactory> MockLoad(PaymentMethodVM result)
        {
            Setup(x => x.Load(It.IsAny<int>()))
                .Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockPaymentMethodFactory> MockLoadList(List<PaymentMethodVM> result)
        {
            if (result != null)
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<PaymentMethodVM>(result)));
            }
            else
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<PaymentMethodVM>()));
            }

            return await Task.FromResult(this);
        }

        public async Task<MockPaymentMethodFactory> MockCreate(ApiResponse result, IList<PaymentMethodVM> objList, PaymentMethodVM objCreate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<PaymentMethodVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Create(It.IsAny<PaymentMethodVM>())).Returns(Task.Run(() => result));
            if (objCreate != null && Convert.ToBoolean(result.ResponseObject))
            {
                objList.Add(objCreate);
            }
            return await Task.FromResult(this);
        }
        public async Task<MockPaymentMethodFactory> MockUpdate(ApiResponse result, IList<PaymentMethodVM> objList, PaymentMethodVM objUpdate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<PaymentMethodVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Update(It.IsAny<PaymentMethodVM>())).Returns(Task.Run(() => result));
            if (objUpdate != null && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.ddlWvType == objUpdate.ddlWvType).FirstOrDefault();
                objList.Remove(objDelete);
                objList.Add(objUpdate);
            }
            return await Task.FromResult(this);
        }

        public async Task<MockPaymentMethodFactory> MockDeleteLoad(PaymentMethodVM result, bool signupExists, int id)
        {
            Setup(x => x.Load(It.IsAny<int>()))
               .Returns(Task.Run(() => result));

            return await Task.FromResult(this); ;
        }

        public async Task<MockPaymentMethodFactory> MockDelete(ApiResponse result, IList<PaymentMethodVM> objList, int id)
        {
            Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.Run(() => result));
            if (id > 0 && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.ddlWvType == id).FirstOrDefault();
                objList.Remove(objDelete);
            }
            return await Task.FromResult(this); ;
        }

        public PaymentMethodVM MockFindById(IList<PaymentMethodVM> PaymentMethodVMModels, int id)
        {
            var result = PaymentMethodVMModels.Where(x => x.ddlWvType == id).SingleOrDefault();
            return result;
        }
        public PaymentMethodVM MockFindByName(List<PaymentMethodVM> PaymentMethodVMModels, string name)
        {
            var result = PaymentMethodVMModels.Where(x => x.crmPaymentMethodName == name).SingleOrDefault();
            return result;
        }
    }
}
