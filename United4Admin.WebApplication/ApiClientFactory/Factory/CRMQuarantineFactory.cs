using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.BLL;
using United4Admin.WebApplication.Models;
using United4Admin.WebApplication.ViewModels;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;


namespace United4Admin.WebApplication.ApiClientFactory.Factory
{
    public class CRMQuarantineFactory : ICRMQuarantineFactory
    {
        protected ApiClient apiClient;
        private readonly ILogger<CRMExtractFactory> _logger;
        private readonly IZipFileHelper _zipFileHelper;
        public List<int> validTransactions = null;

        public CRMQuarantineFactory(ILogger<CRMExtractFactory> logger, IZipFileHelper zipFileHelper)
        {
            this.apiClient = new ApiClient();
            _logger = logger;
            _zipFileHelper = zipFileHelper;
        }

        public CRMQuarantineFactory(ApiClient _apiClient, ILogger<CRMExtractFactory> logger)
        {
            this.apiClient = _apiClient;
            _logger = logger;
        }
        //Method to load CRMQuarantineDataExtract from microservices and convert in to CSV
        public async Task<string> CRMQuarantineDataExtract(CRMExtractParameterModelVM CRMQuarantineParameter)
        {
            try
            {
                //load data using microservices and retreiving data
                List<CRMTransactionModel> cRMTransaction = new List<CRMTransactionModel>();
                string outputDataString = null;

                //Request to Orchestration MS
                var requestUrl = apiClient.CreateRequestUri(CRMQuarantineApiUrls.crmQuarantineApiService);
                var response = await apiClient.PostAsync<CRMExtractParameterModelVM>(requestUrl, CRMQuarantineParameter);
                cRMTransaction = JsonConvert.DeserializeObject<List<CRMTransactionModel>>(Convert.ToString(response.ResponseObject));
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", response.Message));

                if (cRMTransaction != null && cRMTransaction.Any())
                {
                    validTransactions = GetValidTransactionID(cRMTransaction);
                    XDocument xDoc = GetXMLDocument(cRMTransaction);
                    outputDataString = ConvertCSV(xDoc);
                }
                return outputDataString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occurred while getting CRM Mapped Quarantine Transaction");
                return null;
            }
        }

        //call to UpdateCrmTransactionQuarantineInfo
        public async Task<ApiResponse> UpdateCRMTransaction()
        {
            try
            {
                //Request to UpdateCrmQuarantineInfo
                var requestUrl = apiClient.CreateRequestUri(CRMQuarantineApiUrls.crmQuarantineUpdateApiService);
                var response = await apiClient.PostAsync<List<int>>(requestUrl, validTransactions);
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", response.Message));
                return response;
            }
            catch (Exception ex)
            {
                ApiResponse _apiResponse = new ApiResponse();
                _apiResponse.Success = false;
                _apiResponse.Message = ex.Message;
                return _apiResponse;
            }
        }

        //Convert List into XML
        private XDocument GetXMLDocument(List<CRMTransactionModel> mappedQuarantine)
        {
            try
            {
                XDocument xDocument = new XDocument();
                xDocument.Declaration = new XDeclaration("0.1", "utf-8", "yes");
                //If united4admin is configured to Navision
                if (AppConfigValues.crmMappingPath.ToLower() == "navisionmapping")
                {
                    XElement xElement = new XElement("CRMQuarantineExtract",
                                          from quarantine in mappedQuarantine
                                          select new XElement("CRMQuarantine",
                                                         new XElement("WebReferenceID", quarantine.CRMTransaction != null ? quarantine.CRMTransaction.Id : 0),
                                                         new XElement("FirstName", quarantine.Contact != null ? quarantine.Contact.FirstName : ""),
                                                         new XElement("MiddleName", quarantine.Contact != null ? quarantine.Contact.MiddleName : ""),
                                                         new XElement("Surname", quarantine.Contact != null ? quarantine.Contact.LastName : ""),
                                                         new XElement("Street", quarantine.Contact != null ? quarantine.Contact.Address1Line1 : ""),
                                                         new XElement("HouseNo", ""),
                                                         new XElement("Address2", quarantine.Contact != null ? quarantine.Contact.Address1Line2 : ""),
                                                         new XElement("PostCode", quarantine.Contact != null ? quarantine.Contact.Address1PostalCode : ""),
                                                         new XElement("City", quarantine.Contact != null ? quarantine.Contact.Address1City : ""),
                                                         new XElement("CountryCode", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.countryCode : ""),
                                                         new XElement("Email", quarantine.Contact != null ? quarantine.Contact.EmailAddress1 : ""),
                                                         new XElement("MobilePhoneNo", quarantine.Contact != null ? quarantine.Contact.MobilePhone : ""),
                                                         new XElement("ChildCountryCode", quarantine.OrderProduct != null ? quarantine.OrderProduct.WvChildId : ""),
                                                         new XElement("ActionCode", quarantine.Order != null ? quarantine.Order.WvCampaignCode : ""),
                                                         new XElement("PaymentMethod", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.paymentMethodName : ""),
                                                         new XElement("BankAccountNo", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.WvAccountNumber : ""),
                                                         new XElement("IBAN", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.WvIban : ""),
                                                         new XElement("CreditCardHolder", quarantine.Contact != null ? quarantine.Contact.FirstName + " " + quarantine.Contact.LastName : ""),
                                                         new XElement("Salutation", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.salutation : ""),
                                                         new XElement("BankAccountHolder", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.WvAccountHolder : ""),
                                                         new XElement("ProjectID", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.productCode : ""),
                                                         new XElement("ChildSequenceNo", quarantine.OrderProduct == null ? "" : (string.IsNullOrEmpty(quarantine.OrderProduct.WvChildId) ? "" : (quarantine.OrderProduct.WvChildId.Substring(quarantine.OrderProduct.WvChildId.Length - 4)))),
                                                         new XElement("Gift", ""), //This will be used later when Gift shop is available
                                                         new XElement("ExternalReferenceNumber", quarantine.Transaction != null ? (string.IsNullOrEmpty(quarantine.Transaction.WvPaymentProviderTransactionId) ? "" : (quarantine.Transaction.WvPaymentProviderTransactionId.Length > 25 ? quarantine.Transaction.WvPaymentProviderTransactionId.Substring(quarantine.Transaction.WvPaymentProviderTransactionId.Length - 25) : quarantine.Transaction.WvPaymentProviderTransactionId)) : ""),
                                                         new XElement("ProductCode", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.productCode : ""),
                                                         new XElement("IncidentType", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.incidentType : ""),
                                                         new XElement("AmountPerPeriod", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.frequency : ""),
                                                         new XElement("BillingPeriod", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.frequency : ""),
                                                         new XElement("PledgeType", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.pledgeType : ""),
                                                         new XElement("CatalogueID", ""), //This will be used later when Gift shop is available
                                                         new XElement("CatalogueQuantity", ""), //This will be used later when Gift shop is available
                                                         new XElement("PurposeCode", quarantine.CRMMapping != null ? quarantine.CRMMapping.Navision.purposeCode : ""),
                                                         new XElement("Birthdate", ""),
                                                         new XElement("NoOfBelongingIncidents", "") //This will be used later when Gift shop is available
                                                     ));
                    xDocument.Add(xElement);
                }
                else
                {
                    XElement xElement = new XElement("CRMQuarantineExtract",
                                          from quarantine in mappedQuarantine
                                          select new XElement("CRMQuarantine",
                                                         new XElement("Salutation", quarantine.Contact != null ? quarantine.Contact.Salutation : ""),
                                                         new XElement("FirstName", quarantine.Contact != null ? quarantine.Contact.FirstName : ""),
                                                         new XElement("MiddleName", quarantine.Contact != null ? quarantine.Contact.MiddleName : ""),
                                                         new XElement("LastName", quarantine.Contact != null ? quarantine.Contact.LastName : ""),
                                                         new XElement("Street1", quarantine.Contact != null ? quarantine.Contact.Address1Line1 : ""),
                                                         new XElement("Street2", quarantine.Contact != null ? quarantine.Contact.Address1Line2 : ""),
                                                         new XElement("Street3", ""),
                                                         new XElement("City", quarantine.Contact != null ? quarantine.Contact.Address1City : ""),
                                                         new XElement("CountryCode", quarantine.Contact != null ? quarantine.Contact.Address1Country : ""),
                                                         new XElement("PostalCode", quarantine.Contact != null ? quarantine.Contact.Address1PostalCode : ""),
                                                         new XElement("Emails", quarantine.Contact != null ? quarantine.Contact.EmailAddress1 : ""),
                                                         new XElement("Phones", quarantine.Contact != null ? quarantine.Contact.MobilePhone : ""),
                                                         new XElement("TaxId", quarantine.OrderProduct != null ? quarantine.OrderProduct.WvTaxId : ""),
                                                         new XElement("ChildId", quarantine.OrderProduct != null ? quarantine.OrderProduct.WvChildId : ""),
                                                         new XElement("Amount", quarantine.Transaction != null ? quarantine.Transaction.Amount : 0),
                                                         new XElement("IBAN", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.WvIban : ""),
                                                         new XElement("BankAccount", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.WvAccountNumber : ""),
                                                         new XElement("BankNumber", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.WvSortCode : ""),
                                                         new XElement("LastDebitDate", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.FirstPaymentDate : (DateTime?)null),
                                                         new XElement("NextDebitDate", quarantine.PaymentSchedule != null ? quarantine.PaymentSchedule.NextPaymentDate : (DateTime?)null),
                                                         new XElement("PartnerTypeId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.partnerTypeId : 0),
                                                         new XElement("MotivationId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.motivationId : 0),
                                                         new XElement("DesignationId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.designationId : 0),
                                                         new XElement("PledgeTypeId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.pledgeTypeId : 0),
                                                         new XElement("PaymentMethodId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.paymentMethodId : 0),
                                                         new XElement("EmailTypeId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.emailTypeId : 0),
                                                         new XElement("PhoneTypeId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.phoneTypeId : 0),
                                                         new XElement("BatchTypeId", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.batchTypeId : 0),
                                                         new XElement("CountryCode", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.countryCode : ""),
                                                         new XElement("Frequency", quarantine.CRMMapping != null ? quarantine.CRMMapping.Simma.frequency : "")
                                                     ));
                    xDocument.Add(xElement);
                }

                return xDocument;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Transaction IDs of Export Data
        private List<int> GetValidTransactionID(List<CRMTransactionModel> mappedQuarantine)
        {
            List<int> validTransactionIDs = mappedQuarantine.Select(x => x.CRMTransaction.Id).ToList();
            return validTransactionIDs;
        }

        //Convert XML into CSV
        private static string ConvertCSV(XDocument xDocument)
        {
            var headers =
                 xDocument
                    .Descendants("CRMQuarantine")
                    .First()
                    .Elements().Select(e => e.Name.LocalName);

            var delimiter = ",";

            var entries =
                xDocument
                    .Descendants("CRMQuarantine")
                    .Select(d => string.Join(delimiter, d.Elements().Select(e => $"\"{e.Value}\"")))
                    .Aggregate(
                        new StringBuilder().AppendLine(string.Join(delimiter, headers)),
                        (current, next) => current.AppendLine(next));

            var csv = entries.ToString();
            return csv;
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

    }
}
