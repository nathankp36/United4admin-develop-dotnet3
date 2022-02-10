using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.BLL;
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
    public class CRMExtractFactory : ICRMExtractFactory
    {
        protected ApiClient apiClient;
        private readonly ILogger<CRMExtractFactory> _logger;
        private readonly IZipFileHelper _zipFileHelper;

        public CRMExtractFactory(ILogger<CRMExtractFactory> logger, IZipFileHelper zipFileHelper)
        {
            this.apiClient = new ApiClient();
            _logger = logger;
            _zipFileHelper = zipFileHelper;
        }

        public CRMExtractFactory(ApiClient _apiClient, ILogger<CRMExtractFactory> logger)
        {
            this.apiClient = _apiClient;
            _logger = logger;
        }

        //Method to load CRMDataExtract from microservices and convert in to CSV
        public async Task<string[]> CRMDataExtract(CRMExtractParameterModelVM CRMExtractParameter)
        {
            try
            {
                //load data using microservices and retreiving data
                List<CRMExtractVM> cRMExtract = new List<CRMExtractVM>();
                string[] outputDataString = new string[2];

                var requestUrl = apiClient.CreateRequestUri(CRMExtractApiUrls.crmExtractApiService);
                var response = await apiClient.PostAsync<CRMExtractParameterModelVM>(requestUrl, CRMExtractParameter);
                cRMExtract = JsonConvert.DeserializeObject<List<CRMExtractVM>>(Convert.ToString(response.ResponseObject));
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", response.Message));

                string baseServerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var country = AppConfigValues.HostedCountry;
                if (country == "GB")
                {
                    if (cRMExtract != null && cRMExtract.Count > 0)
                    {
                        //STEP 1: Convert list of objects into xml document and save the file 
                        var transformXSLTPath = Path.Combine(baseServerPath, "XsltAndXml");
                        string baseServerPathXslt = Path.Combine(baseServerPath, "XsltAndXml\\XSLT");
                        _logger.LogInformation("Country : " + country);
                        var fileName = baseServerPathXslt + "\\CRMTemplate_" + country + ".xslt";
                        _logger.LogInformation("CRMTemplate Name : " + fileName);
                        XmlDocument xmlDoc = GetXMLDocument(cRMExtract);
                        xmlDoc.Save(transformXSLTPath + "\\input.xml");

                        //STEP 2: Convert xml document into another xml document using XSLT 
                        // Changing the order of field based on XSLT
                        //// Load the style sheet.
                        XslCompiledTransform xsltTrans = new XslCompiledTransform();
                        xsltTrans.Load(fileName);

                        //Transform given input XSLT and output result
                        xsltTrans.Transform(transformXSLTPath + "\\input.xml", transformXSLTPath + "\\output.xml");

                        //STEP 3: Convert XML into CSV file
                        XmlDocument newXmlDoc = new XmlDocument();
                        newXmlDoc.Load(transformXSLTPath + "\\output.xml");
                        XDocument xDoc = XDocument.Parse(newXmlDoc.OuterXml);

                        outputDataString[0] = ConvertCSV(xDoc);
                    }
                }
                else
                {
                    if (cRMExtract != null && cRMExtract.Count > 0)
                    {
                        //Financial Transaction
                        //STEP 1: Convert list of objects into xml document and save the file 
                        var transformXSLTPath = Path.Combine(baseServerPath, "XsltAndXml");
                        string baseServerPathXslt = Path.Combine(baseServerPath, "XsltAndXml\\XSLT");
                        _logger.LogInformation("Country : " + country);
                        var fileName = baseServerPathXslt + "\\CRMTemplate_" + country + ".xslt";
                        _logger.LogInformation("CRMTemplate Name : " + fileName);
                        List<CRMExtractVM> cRMExtractFin = cRMExtract.Where(x => x.UKConsentStatementID == Convert.ToInt32(ApplicationConstants.CONSENTSTATEMENTID)).ToList();
                        if (cRMExtractFin.Count > 0)
                        {
                            XmlDocument xmlDoc = GetXMLDocument(cRMExtractFin);
                            xmlDoc.Save(transformXSLTPath + "\\input.xml");

                            //STEP 2: Convert xml document into another xml document using XSLT 
                            // Changing the order of field based on XSLT
                            //// Load the style sheet.
                            XslCompiledTransform xsltTrans = new XslCompiledTransform();
                            xsltTrans.Load(fileName);

                            //Transform given input XSLT and output result
                            xsltTrans.Transform(transformXSLTPath + "\\input.xml", transformXSLTPath + "\\output.xml");

                            //STEP 3: Convert XML into CSV file
                            XmlDocument newXmlDoc = new XmlDocument();
                            newXmlDoc.Load(transformXSLTPath + "\\output.xml");
                            XDocument xDoc = XDocument.Parse(newXmlDoc.OuterXml);

                            outputDataString[0] = ConvertCSV(xDoc);
                        }
                        //Non Financial Transaction
                        //STEP 1: Convert list of objects into xml document and save the file 
                        var transformNFXSLTPath = Path.Combine(baseServerPath, "XsltAndXml");
                        string baseServerPathXsltNF = Path.Combine(baseServerPath, "XsltAndXml\\XSLT");
                        _logger.LogInformation("Country : " + "NonFin");
                        var fileNameNF = baseServerPathXsltNF + "\\CRMTemplate_NonFin.xslt";
                        _logger.LogInformation("CRMTemplate Name : " + fileNameNF);
                        List<CRMExtractVM> cRMExtractNonFin =
                            cRMExtract.Where(x => x.UKConsentStatementID != Convert.ToInt32(ApplicationConstants.CONSENTSTATEMENTID)).ToList();
                        if (cRMExtractNonFin.Count > 0)
                        {
                            XmlDocument xmlDocNF = GetXMLDocument(cRMExtractNonFin);
                            xmlDocNF.Save(transformNFXSLTPath + "\\input.xml");

                            //STEP 2: Convert xml document into another xml document using XSLT 
                            // Changing the order of field based on XSLT
                            //// Load the style sheet.
                            XslCompiledTransform xsltTransNF = new XslCompiledTransform();
                            xsltTransNF.Load(fileNameNF);

                            //Transform given input XSLT and output result
                            xsltTransNF.Transform(transformNFXSLTPath + "\\input.xml", transformNFXSLTPath + "\\output.xml");

                            //STEP 3: Convert XML into CSV file
                            XmlDocument newXmlDocNF = new XmlDocument();
                            newXmlDocNF.Load(transformNFXSLTPath + "\\output.xml");
                            XDocument xDocNF = XDocument.Parse(newXmlDocNF.OuterXml);

                            outputDataString[1] = ConvertCSV(xDocNF);
                        }
                    }
                }
                return outputDataString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Convert List into XML
        private XmlDocument GetXMLDocument(List<CRMExtractVM> echoes)
        {
            try
            {
                XDocument xdoc = new XDocument(
               new XDeclaration("1.0", "utf-8", "yes"));


                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", String.Empty);
                xmlDoc.LoadXml("<data>" +
                 "</data>");
                xmlDoc.PrependChild(xmlDec);

                XmlNode root = xmlDoc.DocumentElement;

                foreach (var echoItem in echoes)
                {
                    XmlElement elemRoot = xmlDoc.CreateElement("Echo");
                    root.AppendChild(elemRoot);
                    XmlElement elem = null;
                    Type idType = echoItem.GetType();
                    foreach (PropertyInfo pInfo in idType.GetProperties())
                    {
                        object o = pInfo.GetValue(echoItem, null);
                        elem = xmlDoc.CreateElement(pInfo.Name);
                        elem.InnerText = Convert.ToString(o);
                        elemRoot.AppendChild(elem);
                    }
                }
                return xmlDoc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get azure blob container storage details
        private Task<CloudBlobContainer> GetStoragXSLTContainer()
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = _zipFileHelper.GetConnectionString(AppConfigValues.StorageAccountName, AppConfigValues.StorageAccountKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(AppConfigValues.XSLTStorageContainer);
                return Task.FromResult(cloudBlobContainer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Convert XML into CSV
        private static string ConvertCSV(XDocument xDocument)
        {
            // XDocument xDocument = XDocument.Load(xmlTextDate);
            var headers =
                 xDocument
                    .Descendants("Echo")
                    .First()
                    .Elements().Select(e => e.Name.LocalName);

            var delimiter = ",";

            var entries =
                xDocument
                    .Descendants("Echo")
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
