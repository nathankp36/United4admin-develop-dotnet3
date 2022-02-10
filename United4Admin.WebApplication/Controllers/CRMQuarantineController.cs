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
    public class CRMQuarantineController : Controller
    {
        private readonly ICRMQuarantineFactory _crmQuarantineFactory;
        private readonly ILogger<CRMQuarantineController> _logger;
        private readonly IZipFileHelper _zipFileHelper;

        public CRMQuarantineController(ICRMQuarantineFactory crmQuarantineFactory, IZipFileHelper zipFileHelper, ILogger<CRMQuarantineController> logger)
        {
            _crmQuarantineFactory = crmQuarantineFactory;
            _zipFileHelper = zipFileHelper;
            _logger = logger;
        }
        public ActionResult Index()
        {
            try
            {
                CRMExtractParameterModelVM cRMQuarantineParameterVM = new CRMExtractParameterModelVM();
                string crmQuarantinePath = United4Admin.WebApplication.ViewModels.AppConfigValues.CRMQuarantinePath;
                if (crmQuarantinePath.ToLower().Trim('/') != "crmquarantine")
                    return Redirect(crmQuarantinePath);
                else
                    return View(cRMQuarantineParameterVM);
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
        public async Task<ActionResult> ExportCRMQuarantineData(CRMExtractParameterModelVM CRMQuarantineParameterModel)
        {
            try
            {
                string fileName = string.Empty;
                string outPut = string.Empty;
                var outputsCSV = await _crmQuarantineFactory.CRMQuarantineDataExtract(CRMQuarantineParameterModel);
                fileName = "CRMData_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".csv";
                outPut = outputsCSV == null ? "" : outputsCSV.ToString();
                if (!string.IsNullOrEmpty(outPut))
                {
                    var crmQuarantineUpdateTransaction = await _crmQuarantineFactory.UpdateCRMTransaction();
                    if (crmQuarantineUpdateTransaction.Success)
                        return File(new UTF32Encoding().GetBytes(outPut), "text/csv", fileName);
                    else
                    {
                        NoRecordVM noRecord = new NoRecordVM
                        {
                            Title = AppConfigValues.CRMQuarantineTitle,
                            Description = "Failed to Update the Quarantine Transaction"
                        };
                        return View("NoRecord", noRecord);
                    }
                }
                else
                {
                    NoRecordVM noRecord = new NoRecordVM
                    {
                        Title = AppConfigValues.CRMQuarantineTitle,
                        Description = AppConfigValues.CRMQuarantineNoRecordDescription
                    };
                    return View("NoRecord", noRecord);
                }
            }
            catch (Exception ex)
            {
                ViewBag.DownLoadError = "Couldn't create Quarantine file." + ex.Message;
                _logger.LogError(ex, "Couldn't create Quarantine file.");
                NoRecordVM noRecord = new NoRecordVM
                {
                    Title = AppConfigValues.CRMQuarantineTitle,
                    Description = AppConfigValues.CRMQuarantineErrorDownload
                };
                return View("NoRecord", noRecord);
            }
        }

    }

}
