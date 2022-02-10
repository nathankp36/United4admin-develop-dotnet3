using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.BLL;
using United4Admin.WebApplication.ViewModels;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace United4Admin.WebApplication.Controllers
{
    //[Authorize]
    public class CRMExtractController : Controller
    {
        private readonly ICRMExtractFactory _crmExtractFactory;
        private readonly ILogger<CRMExtractController> _logger;
        private readonly IZipFileHelper _zipFileHelper;

        public CRMExtractController(ICRMExtractFactory crmExtractFactory, IZipFileHelper zipFileHelper, ILogger<CRMExtractController> logger)
        {
            _crmExtractFactory = crmExtractFactory;
            _zipFileHelper = zipFileHelper;
            _logger = logger;
        }

        public ActionResult Index()
        {
            try
            {
                CRMExtractParameterModelVM cRMExtractParameterVM = new CRMExtractParameterModelVM();
                return View(cRMExtractParameterVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Export()
        {
            return View();
        }
        public ActionResult ExportConfirmed(string notification)
        {
            ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Download")]
        public async Task<ActionResult> ExportCRMExtractData(CRMExtractParameterModelVM CRMExtractParameterModel)
        {
            try
            {
                string fileName = string.Empty;
                string[] fileNameNF = new string[2];
                string outPut = string.Empty;
                var country = AppConfigValues.HostedCountry;
                CRMExtractParameterModel.CRMType = AppConfigValues.CRMType;
                var outputsCSV = await _crmExtractFactory.CRMDataExtract(CRMExtractParameterModel);
                fileName = "CRMData_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".csv";
                //other than UK all other countries to download financial and non financial seprately as CSV
                if (country != "GB")
                {
                    fileNameNF[0] = "CRMData_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".csv";
                    fileNameNF[1] = "CRMData_NF_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".csv";
                    var package = _zipFileHelper.ZipUpCSVFiles(fileNameNF, outputsCSV);
                    var file = "CRMData.zip";
                    return File(package, "application/zip", file);
                }
                else
                {
                    outPut = outputsCSV[0] == null ? "" : outputsCSV[0].ToString();
                }
                return File(new UTF32Encoding().GetBytes(outPut), "text/csv", fileName);
            }
            catch (Exception ex)
            {
                ViewBag.DownLoadError = "Couldn't create Echo file. " + ex.Message;
                _logger.LogError(ex, "Couldn't create Echo file. ");
                return View("ErrorDownload");
            }
        }

    }
}
