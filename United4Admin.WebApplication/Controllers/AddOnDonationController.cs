using United4Admin.WebApplication.BLL;
using System.Text;
using United4Admin.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ApiClientFactory.Factory;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using static United4Admin.WebApplication.ViewModels.ExportDataVM;
using EchoDataExport = United4Admin.WebApplication.BLL.EchoDataExport;
using United4Admin.ExcelGenerator.ActionResults;

namespace United4Admin.WebApplication.Controllers
{
   
    public class AddOnDonationController : Controller
    {
        private readonly IAddOnDonationFactory _addonDonationFactory;
        private readonly IImageStoreFactory _imageStoreFactory;
        private readonly IZipFileHelper _zipFileHelper;
        private readonly ILogger<AddOnDonationController> _logger;
        private readonly string pageName = "Add-on Donation";
        public AddOnDonationController(IAddOnDonationFactory addonDonationFactory, IImageStoreFactory imageStoreFactory, IZipFileHelper zipFileHelper, IPermissionFactory permissionFactory, ILogger<AddOnDonationController> logger)
          
        {
            _addonDonationFactory = addonDonationFactory;
            _imageStoreFactory = imageStoreFactory;
            _zipFileHelper = zipFileHelper;
            _logger = logger;
        }

        // GET: AddOnDonation
        public async Task<ActionResult> Index(string notification)
        {
            try
            {
                IList<AddOnDonationVM> addOnDonationVMList = await _addonDonationFactory.GetAddOnDonationData();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View(addOnDonationVMList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<ActionResult> Details(string id)
        {
            try
            {
                AddOnDonationVM addOnDonationVM = await _addonDonationFactory.GetAddOnDonationBySalesOrderId(id);
                if (addOnDonationVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Details", addOnDonationVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Policy = "Download")]
        public async Task<IActionResult> DownloadFieldData(SignUpExtractParameterVM signUpVM)
        {
            try
            {
                List<AddOnDonationVM> result = await _addonDonationFactory.GetFieldDataExport(signUpVM);
                var reportResult = BLL.AddOnDataExport.ConvertAddOnData(result);
                return new ExcelResult<ExportDataVM.AddOnDataExport>(reportResult, "AddOnData", "AddOnData_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't create excel file. ");
                ViewBag.DownLoadError = "Couldn't create Excel file. " + ex.Message;
                return View("DownLoadError");
            }
        }

        [Authorize(Policy = "Download")]
        public async Task<FileContentResult> DownloadPhotos(SignUpExtractParameterVM signUpVM)
        {
            var imageInfoVMs = await _addonDonationFactory.GetImageNames(signUpVM);
            try
            {
                if (imageInfoVMs.Count > 0)
                {
                    var blobFileNames = imageInfoVMs.Select(x => x.BlobGUID).ToArray();
                    var package = await _zipFileHelper.ZipUpFiles(blobFileNames, _imageStoreFactory);
                    var file = "AddOnData_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".zip";
                    return File(package, "application/zip", file);
                }
                return File(new UTF32Encoding().GetBytes(""), "application/zip", "0.zip");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't create downloaded image zip file. ");
                RedirectToAction("DownloadError");                
                throw ex;
            }
        }


        public ActionResult DownloadError()
        {
            ViewBag.DownLoadError = "Couldn't create downloaded image zip file. ";
            return View("DownLoadError");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _addonDonationFactory.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
