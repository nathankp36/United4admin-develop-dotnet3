using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory;
using United4Admin.WebApplication.ViewModels;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IAdyenPaymentsFactory : IApiClientFactory<AdyenTransactionVM>
    {
        Task<List<AdyenTransactionVM>> LoadListSearch(string Search, string SearchDDL, DateTime? SearchFromDate, DateTime? SearchToDate);
        
    }
}
