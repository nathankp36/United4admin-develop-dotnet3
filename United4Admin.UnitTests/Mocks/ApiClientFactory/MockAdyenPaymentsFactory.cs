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
    public class MockAdyenPaymentsFactory : Mock<IAdyenPaymentsFactory>
    {
        public async Task<MockAdyenPaymentsFactory> MockLoad(AdyenTransactionVM result)
        {

            Setup(x => x.Load(It.IsAny<int>()))
                .Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockAdyenPaymentsFactory> MockLoadList(List<AdyenTransactionVM> result)
        {
            if (result != null)
            {
                Setup(x => x.LoadListSearch("", "", DateTime.Today, DateTime.Today)).Returns(Task.Run(() => new List<AdyenTransactionVM>(result)));
            }
            else
            {
                Setup(x => x.LoadListSearch("", "", DateTime.Today, DateTime.Today)).Returns(Task.Run(() => new List<AdyenTransactionVM>()));
            }

            return await Task.FromResult(this);
        }

        public async Task<MockAdyenPaymentsFactory> MockLoadListSearch(List<AdyenTransactionVM> result, string Search, string SearchDDL, DateTime? SearchFromDate, DateTime? SearchToDate)
        {
            if (result != null)
            {
                Setup(x => x.LoadListSearch(Search, SearchDDL, SearchFromDate, SearchToDate)).Returns(Task.Run(() => new List<AdyenTransactionVM>(result)));
            }
            else
            {
                Setup(x => x.LoadListSearch(Search, SearchDDL, SearchFromDate, SearchToDate)).Returns(Task.Run(() => new List<AdyenTransactionVM>()));
            }

            return await Task.FromResult(this);
        }

        public async Task<MockAdyenPaymentsFactory> MockCreate(ApiResponse result, IList<AdyenTransactionVM> objList, AdyenTransactionVM objCreate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<AdyenTransactionVM>(objList)));
            // Allows us to test saving a adyenTransaction
            Setup(x => x.Create(It.IsAny<AdyenTransactionVM>())).Returns(Task.Run(() => result));
            if (objCreate != null && Convert.ToBoolean(result.ResponseObject))
            {
                objList.Add(objCreate);
            }
            return await Task.FromResult(this);
        }

        public AdyenTransactionVM MockFindById(IList<AdyenTransactionVM> AdyenTransactionVMModels, int id)
        {
            var result = AdyenTransactionVMModels.Where(x => x.AdyenTransactionId == id).SingleOrDefault();
            return result;
        }

        public bool MockValidAmount(AdyenTransactionVM objCreate)
        {
            var result = (objCreate.Amount >= 0) ? true : false;

            return result;
        }

        public bool MockValidContactId(AdyenTransactionVM objCreate)
        {
            var result = (objCreate.ContactId >= 0) ? true : false;

            return result;
        }

        public bool MockValidLastTransId(AdyenTransactionVM objCreate)
        {
            var result = (objCreate.LastTransactionId >= 0) ? true : false;

            return result;
        }
    }
}
