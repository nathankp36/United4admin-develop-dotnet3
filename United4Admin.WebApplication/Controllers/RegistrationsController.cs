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
   
    public class RegistrationsController : Controller
    {
        private readonly IRegistrationFactory _registrationFactory;
        private readonly IImageStoreFactory _imageStoreFactory;
        private readonly IZipFileHelper _zipFileHelper;
        private readonly ILogger<RegistrationsController> _logger;
        private readonly string pageName = "Choosing Event";

        public RegistrationsController(IRegistrationFactory registrationFactory, IImageStoreFactory imageStoreFactory, IZipFileHelper zipFileHelper, IPermissionFactory permissionFactory, ILogger<RegistrationsController> logger)
          
        {
            _registrationFactory = registrationFactory;
            _imageStoreFactory = imageStoreFactory;
            _zipFileHelper = zipFileHelper;
            _logger = logger;
        }

        // GET: Registrations
        public async Task<ActionResult> Index(string notification)
        {
            try
            {           
                IList<SignUpVM> signUpVMList = await _registrationFactory.LoadList();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                if (signUpVMList.Count > 0)
                {
                    await SetDataLists(signUpVMList.FirstOrDefault());
                }
                return View(signUpVMList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                SignUpVM SignUpVM = await _registrationFactory.Load(id);
                if (SignUpVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Details", SignUpVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Registrations/Edit/5
        [Authorize(Policy = "EditDeleteSupporterData")]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                SignUpVM SignUpVM = await _registrationFactory.Load(id);

                if (SignUpVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                bool status = await SetDataLists(SignUpVM);
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", SignUpVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditDeleteSupporterData")]
        public async Task<ActionResult> Edit(SignUpVM rVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //SignUpVM signUpVM = Mapper.Map(rVM, new SignUpVM());
                    var response = await _registrationFactory.Update(rVM);
                    _logger.LogWarning(ConstantMessages.Update.Replace("{event}", pageName));
                    return RedirectToAction("Index", new { notification = response.Message });
                }
                await SetDataLists(rVM);
                return View("Edit", rVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);                
                ModelState.AddModelError("Error", ConstantMessages.Error);
                await SetDataLists(rVM);
                return View("Edit", rVM);
            }
        }

        // GET: Registrations/Delete/5
        [Authorize(Policy = "EditDeleteSupporterData")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                SignUpVM SignUpVM = await _registrationFactory.Load(id);
                if (SignUpVM == null)
                {
                    return NotFound();
                }
                return View(SignUpVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditDeleteSupporterData")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ImageInfoVM imageInfoVM = await _registrationFactory.GetImage(id);
                //remove SignUpVM and image info from DB
                var response = await _registrationFactory.Delete(id);
                //remove image from blob storage
                if (imageInfoVM != null)
                {
                    var result = await _imageStoreFactory.DeleteImage(imageInfoVM.BlobGUID);
                }
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", new { notification = response.Message });
            }
            catch (Exception ex)
            {
                SignUpVM signUpVM = await _registrationFactory.Load(id);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                _logger.LogError(ex, ConstantMessages.Error);                
                return View(signUpVM);
            }
        }

        private async Task<bool> SetDataLists(SignUpVM signUpVM)
        {
            try
            {                
                signUpVM.Titles = await _registrationFactory.GetTitles();
                signUpVM.Statuses = await _registrationFactory.GetStatuses();
                signUpVM.SignUpVMEvents = await _registrationFactory.GetSignupEventList();

                signUpVM.ChoosingParties = await _registrationFactory.GetChoosingPartyList();

                signUpVM.RevealEvents = await _registrationFactory.GetRevealEventList();
                ViewBag.SignUpVMDateDisabled = true;
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", "SetDataLists"));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                throw ex;
            }
        }

        [Authorize(Policy = "Download")]
        public async Task<IActionResult> DownloadFieldData(SignUpExtractParameterVM signUpVM)
        {
            try
            {
                List<ImageInfoVM> result = await _registrationFactory.GetFieldDataExport(signUpVM);
                var reportResult = BLL.FieldDataExport.ConvertData(result);                
                return new ExcelResult<ExportDataVM.FieldDataExport>(reportResult, "FieldData", "FieldData");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't create excel file. ");
                ViewBag.DownLoadError = "Couldn't create Excel file. " + ex.Message;
                return View("DownLoadError");
            }
        }

        [Authorize(Policy = "Download")]
        public async Task<ActionResult> DownloadECHOData(SignUpExtractParameterVM signUpVM)
        {
            var sb = new StringBuilder();
            try
            {
                List<SignUpVM> signUpVMs = await _registrationFactory.GetEchoData(signUpVM);
                sb.Append(EchoDataExport.ConvertData(signUpVMs, false));
                return File(new UTF32Encoding().GetBytes(sb.ToString()), "text/csv", "chosen_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".csv");
            }
            catch (Exception ex)
            {
                ViewBag.DownLoadError = "Couldn't create Echo file. " + ex.Message;
                _logger.LogError(ex, "Couldn't create Echo file. ");
                return View("DownLoadError");
            }

        }

        public async Task<ActionResult> ShowPhoto(int id)
        {
            try
            {
                ImageInfoVM imageInfo = await _registrationFactory.GetImage(id);
                if (imageInfo != null)
                {
                    var result = await _imageStoreFactory.GetSecureUrlImage(imageInfo.BlobGUID);
                    imageInfo.ImageURL = result.ToString();
                }
                else
                {
                    imageInfo = new ImageInfoVM();
                    imageInfo.ChosenSignUpId = id;
                }
                return View(imageInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Policy = "Download")]
        public async Task<FileContentResult> DownloadPhotos(SignUpExtractParameterVM signUpVM)
        {
            var imageInfoVMs = await _registrationFactory.GetImageNames(signUpVM);
            try
            {
                if (imageInfoVMs.Count > 0)
                {
                    var blobFileNames = imageInfoVMs.Select(x => x.BlobGUID).ToArray();
                    string prelimRecord = blobFileNames[0];
                    if (prelimRecord.Length > 0)
                    {
                        prelimRecord = prelimRecord.Substring(0, prelimRecord.IndexOf("."));
                        var package = await _zipFileHelper.ZipUpFiles(blobFileNames, _imageStoreFactory);
                        var file = prelimRecord + ".zip";
                        return File(package, "application/zip", file);
                    }
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
                _registrationFactory.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
