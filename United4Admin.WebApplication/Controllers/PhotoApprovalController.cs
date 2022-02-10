using United4Admin.WebApplication.ApiClientFactory.Factory;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.BLL;
using United4Admin.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace United4Admin.WebApplication.Controllers
{
    [Authorize(Policy = "CreateEditDeleteEvents")]
    public class PhotoApprovalController : Controller
    {
        private readonly IRegistrationFactory _registrationFactory;
        private readonly IImageStoreFactory _imageStoreFactory;
        private readonly ILogger<PhotoApprovalController> _logger;
        private readonly string pageName = "Photo Approval";
        public PhotoApprovalController(IRegistrationFactory registrationFactory, IImageStoreFactory imageStoreFactory, IPermissionFactory permissionFactory, ILogger<PhotoApprovalController> logger)
        {
            _registrationFactory = registrationFactory;
            _imageStoreFactory = imageStoreFactory;
            _logger = logger;
        }

        // GET: GetPhotoApporval
        public async Task<ActionResult> Index(string notification)
        {
            try
            {
                IList<SignUpVM> signVMList = await _registrationFactory.GetPhotoApproval();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";

                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View(signVMList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: GetPhotoApporval
        public async Task<ActionResult> ShowPhoto(int chosenSignUpId)
        {
            ViewBag.Action = "ShowPhoto";
            try
            {
                ImageInfoVM imageInfo = await _registrationFactory.GetImage(chosenSignUpId);
                if (imageInfo != null)
                {
                    var result = await _imageStoreFactory.GetSecureUrlImage(imageInfo.BlobGUID);
                    SignUpVM signUpVM = new SignUpVM();
                    imageInfo.ImageURL = result.ToString();
                }
                else
                {
                    imageInfo = new ImageInfoVM();
                    imageInfo.ChosenSignUpId = chosenSignUpId;
                }
                return Json(imageInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditDeleteSupporterData")]
        public async Task<ActionResult> Delete(int delSignUpId)
        {
            try
            {
                //Step 1: Method to load imageinfo details
                ImageInfoVM imageInfo = await _registrationFactory.GetImage(delSignUpId);
                if (imageInfo != null)
                {
                    //Step 2: Method to delete image in blobstorage containers in azure
                    var result = await _imageStoreFactory.DeleteImage(imageInfo.BlobGUID);
                    if (result.Success)
                    {
                        //Step 3: Method to update status,BlobGUID in imageinfo
                        imageInfo.ImageStatusId = ApplicationConstants.IMAGESTATUSDELTED;
                        imageInfo.BlobGUID = null;
                        imageInfo.UploadDateTime = DateTime.Now;
                        var response = await _registrationFactory.UpdateImage(imageInfo);
                        _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                        return RedirectToAction("Index", new { notification = result.Message });
                    }
                }
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", new { notification = "There is no image to delete" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Index");
            }
        }

        // POST: Registrations/Approve/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost, ActionName("Approve")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditDeleteSupporterData")]
        public async Task<ActionResult> Approve(int approveSignUpId)
        {
            try
            {
                ImageInfoVM imageInfo = await _registrationFactory.GetImage(approveSignUpId);
                if (imageInfo != null)
                {
                    imageInfo.ImageStatusId = ApplicationConstants.IMAGESTATUSAPPROVED;
                    imageInfo.UploadDateTime = DateTime.Now;
                    var response = await _registrationFactory.UpdateImage(imageInfo);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                    return RedirectToAction("Index", new { notification = response.Message });
                }
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", new { notification = "There is no image to approve" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Index");
            }
        }

        // POST: Registrations/Replace/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost, ActionName("Replace")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditDeleteSupporterData")]
        public async Task<ActionResult> Replace(IFormFile fileUploadName, int repSignUpId)
        {
            try
            {
                //Step 1: Method to load imageinfo details
                ImageInfoVM imageInfo = await _registrationFactory.GetImage(repSignUpId);
                string fileIdName = Convert.ToString(repSignUpId);
                if (imageInfo != null)
                {
                    //Step 2: Method to delete image in blobstorage containers in azure
                    var result = await _imageStoreFactory.DeleteImage(imageInfo.BlobGUID);
                    if (result.Success)
                    {
                        //Step 3: Method to upload image in blobstorage containers in azure
                        var response = await _registrationFactory.UploadImage(fileUploadName, fileIdName);
                        if (response.Success)
                        {
                            //Step 4: Method to update status,BlobGUID in imageinfo
                            imageInfo.ImageStatusId = ApplicationConstants.IMAGESTATUSNOTAPPROVED;
                            imageInfo.UploadDateTime = DateTime.Now;
                            imageInfo.BlobGUID = Convert.ToString(response.ResponseObject);
                            var responseResult = await _registrationFactory.UpdateImage(imageInfo);
                            _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                            return RedirectToAction("Index", new { notification = responseResult.Message });
                        }
                        else
                        {
                            _logger.LogInformation(ConstantMessages.CreateFailure.Replace("{event}", pageName));
                            return RedirectToAction("Index", new { notification = response.Message });
                        }
                    }
                    _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                    return RedirectToAction("Index", new { notification = "Failed to Replace Image" });
                }
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", new { notification = "There is no image to replace" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Index");
            }
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
