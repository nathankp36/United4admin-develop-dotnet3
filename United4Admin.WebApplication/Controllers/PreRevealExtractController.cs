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

namespace United4Admin.WebApplication.Controllers
{
    public class PreRevealExtractController : Controller
    {
        private readonly IRegistrationFactory _preRevealExtractFactory;
        private readonly ILogger<PreRevealExtractController> _logger;
        public PreRevealExtractController(IRegistrationFactory preRevealExtractFactory, ILogger<PreRevealExtractController> logger)
        {
            _preRevealExtractFactory = preRevealExtractFactory;
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
        public async Task<ActionResult> ExportPreRevealaExtractData(CRMExtractParameterModelVM cRMExtractParameterModel)
        {
            var sb = new StringBuilder();
            try
            {
                string fileName = string.Empty;
                List<SignUpVM> signUpVMs = await _preRevealExtractFactory.GetPreRevealExtractData(cRMExtractParameterModel);
                sb.Append(EchoDataExport.ConvertData(signUpVMs, true));
                fileName = "PreRevealData_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".csv";

                return File(new UTF32Encoding().GetBytes(sb.ToString()), "text/csv", fileName);
            }
            catch (Exception ex)
            {
                ViewBag.DownLoadError = "Couldn't create PreReveal file. " + ex.Message;
                _logger.LogError(ex, "Couldn't create Echo file. ");
                return View("ErrorDownload");
            }
        }

    }
}
