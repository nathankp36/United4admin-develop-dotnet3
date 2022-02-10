using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class AdyenPaymentsFactory : IAdyenPaymentsFactory
    {
        protected ApiClient apiClient;

        public AdyenPaymentsFactory()
        {
            this.apiClient = new ApiClient();
        }

        public AdyenPaymentsFactory(ApiClient _apiClient)
        {
            this.apiClient = _apiClient;
        }

        public async Task<List<AdyenTransactionVM>> LoadListSearch(string Search, string SearchDDL, DateTime? SearchFromDate, DateTime? SearchToDate)
        {
            try
            {
                List<AdyenTransactionVM> adyenTransactionVMList = await LoadList();
               
                //apply filters 
                if (SearchDDL == "EmailId")
                {
                    adyenTransactionVMList = Search != null ? adyenTransactionVMList.Where(x => x.Supporter.EmailAddress1.ToLower().Contains(Search.ToLower())).ToList()
                        : adyenTransactionVMList;
                }
                else if (SearchDDL == "Name")
                {
                    adyenTransactionVMList = Search != null ? adyenTransactionVMList.Where(x => x.Supporter.FirstName.ToLower().Contains(Search.ToLower())
                     || x.Supporter.LastName.ToLower().Contains(Search.ToLower())).ToList() : adyenTransactionVMList;
                }
                else if (SearchDDL == "TransactionDate")
                {
                    adyenTransactionVMList = SearchFromDate != null ? adyenTransactionVMList.Where(x => x.LastPaymentDate == SearchFromDate).ToList() 
                        : adyenTransactionVMList;
                }
                else if (SearchDDL == "DateRange")
                {
                    adyenTransactionVMList = (SearchFromDate != null && SearchToDate != null) ? adyenTransactionVMList.Where(x => x.LastPaymentDate >= SearchFromDate && x.LastPaymentDate <= SearchToDate).ToList()
                        : adyenTransactionVMList;
                }

                return adyenTransactionVMList.OrderByDescending(x => x.LastPaymentDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<AdyenTransactionVM>> LoadList()
        {
            try
            {
                List<AdyenTransactionVM> adyenTransactionVMList = new List<AdyenTransactionVM>();
                var requestUrl = apiClient.CreateRequestUri(AdyenPaymentApiUrls.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                adyenTransactionVMList = JsonConvert.DeserializeObject<List<AdyenTransactionVM>>(Convert.ToString(response.ResponseObject));
                adyenTransactionVMList.ToList().ForEach(x =>
                {
                    x.LastPaymentStatus = x.LastPaymentStatus == "A" ? "Authorised" : "Failed";
                });

                return adyenTransactionVMList.OrderByDescending(x => x.LastPaymentDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AdyenTransactionVM> Load(int contactId)
        {
            try
            {
                List<AdyenTransactionVM> adyenTransactionVMList = new List<AdyenTransactionVM>();
                AdyenTransactionVM adyenTransactionVM = new AdyenTransactionVM();

                var requestUrl = apiClient.CreateRequestUri(AdyenPaymentApiUrls.LoadList);
                var response = await apiClient.GetAsync<ApiResponse>(requestUrl);
                adyenTransactionVMList = JsonConvert.DeserializeObject<List<AdyenTransactionVM>>(Convert.ToString(response.ResponseObject));
                if (adyenTransactionVMList.Count > 0)
                {
                    adyenTransactionVM = adyenTransactionVMList.Where(x => x.ContactId == contactId).FirstOrDefault();
                    adyenTransactionVM.LastPaymentStatus = adyenTransactionVM.LastPaymentStatus == "A" ? "Authorised" : "Failed";
                }
                var requestTransUrl = apiClient.CreateRequestUri(AdyenPaymentApiUrls.LoadTransaction.Replace("{contactId}", Convert.ToString(contactId)));
                var responseTransList = await apiClient.GetAsync<ApiResponse>(requestTransUrl);
                var transactionVMList = JsonConvert.DeserializeObject<List<TransactionVM>>(Convert.ToString(responseTransList.ResponseObject));
                if (transactionVMList.Count > 0)
                {
                    adyenTransactionVM.TransactionList = transactionVMList.OrderByDescending(x => x.TransactionDate).ToList();
                }
                return adyenTransactionVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse> Create(AdyenTransactionVM adyenTransactionVM)
        {
            try
            {
                RecurringTokenVM adyenRecurringTokenVM = new RecurringTokenVM();
                adyenTransactionVM.AdyenTransactionId = adyenTransactionVM.AdyenTransactionId;
                adyenRecurringTokenVM.LastTransactionId = adyenTransactionVM.LastTransactionId;
                adyenRecurringTokenVM.RecurringToken = adyenTransactionVM.RecurringToken;
                adyenRecurringTokenVM.BillCycleStartDate = adyenTransactionVM.BillCycleStartDate;
                adyenRecurringTokenVM.BillCycleNextDate = adyenTransactionVM.BillCycleNextDate;
                adyenRecurringTokenVM.LastPaymentStatus = adyenTransactionVM.LastPaymentStatus;
                adyenRecurringTokenVM.LastPaymentDate = adyenTransactionVM.LastPaymentDate;
                adyenRecurringTokenVM.NoOfRetryAttempts = adyenTransactionVM.NoOfRetryAttempts;
                adyenRecurringTokenVM.ContactId = adyenTransactionVM.ContactId;
                adyenRecurringTokenVM.Amount = adyenTransactionVM.Amount;
                adyenRecurringTokenVM.ShopperReference = adyenTransactionVM.ShopperReference;
                adyenRecurringTokenVM.CurrencyCode = adyenTransactionVM.CurrencyCode;

                var requestUrl = apiClient.CreateRequestUri(AdyenPaymentApiUrls.Create);
                var response = await apiClient.PostAsync<RecurringTokenVM>(requestUrl, adyenRecurringTokenVM);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool disposed = false;

        /// <summary>
        ///Dispose the object used
        /// </summary>
        /// <param name=""></param>
        /// <returns>no values</returns>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.apiClient.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<ApiResponse> Update(AdyenTransactionVM t)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<WorkflowStatusVM>> GetWorkflowStatuses()
        {
            throw new NotImplementedException();
        }
    }
}
