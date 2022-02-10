using United4Admin.WebApplication.ApiClientFactory.Factory;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using United4Admin.WebApplication.BLL;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using United4Admin.WebApplication.APIClient;
using Microsoft.AspNetCore.Http;

namespace United4Admin.WebApplication.Controllers
{
    [Authorize(Policy = "CreateEditDeleteEvents")]
    public class PhotoUploadController : Controller
    {
        private readonly IRegistrationFactory _photoUploadFactory;
        private readonly ILogger<PhotoUploadController> _logger;
        private readonly string pageName = "Photo Upload";
        public PhotoUploadController(IRegistrationFactory photoUploadFactory, ILogger<PhotoUploadController> logger)
        {
            _photoUploadFactory = photoUploadFactory;
            _logger = logger;
        }

        // GET: SignUp
        public async Task<ActionResult> Index(string notification)
        {
            try
            {
                IList<SignUpVM> signUpVMList = await _photoUploadFactory.GetImageNotUploadData();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View(signUpVMList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        //Upload imageinfo and image
        [HttpPost]
        public async Task<ActionResult> Create(IFormFile fileUploadName, int signUpId)
        {
            ApiResponse responseCreateImage = new ApiResponse();
            ViewBag.Action = "Create";
            try
            {
                //Step 1: Method to upload image in blobstorage containers in azure
                string fileIdName = Convert.ToString(signUpId);
                string filename = ApplicationConstants.REFERENCEBEGINSTRING + signUpId.ToString(ApplicationConstants.REFERENCEFORMAT) + "_" + fileIdName;

                var response = await _photoUploadFactory.UploadImage(fileUploadName, filename);
                if (response.Success)
                {
                    //Step 2: Method to load image in imageinfo
                    SignUpExtractParameterVM signUpVM = new SignUpExtractParameterVM();
                    List<ImageInfoVM> imgInfo = await _photoUploadFactory.GetImageNames(signUpVM);
                    var imgList = imgInfo.ToList().Where(x => x.ChosenSignUpId == signUpId).FirstOrDefault();
                    //Step 3: if imageinfo already available Method to update or create
                    if (imgList != null)
                    {
                        imgList.BlobGUID = Convert.ToString(response.ResponseObject);
                        imgList.UploadDateTime = DateTime.Now;
                        imgList.ImageStatusId = ApplicationConstants.IMAGESTATUSNOTAPPROVED;
                        responseCreateImage = await _photoUploadFactory.UpdateImage(imgList);
                    }
                    else
                    {
                        ImageInfoVM imageInfo = new ImageInfoVM()
                        {
                            BlobGUID = Convert.ToString(response.ResponseObject),
                            ChosenSignUpId = signUpId,
                            UploadDateTime = DateTime.Now,
                            ImageStatusId = ApplicationConstants.IMAGESTATUSNOTAPPROVED,
                        };
                        responseCreateImage = await _photoUploadFactory.CreateImage(imageInfo);
                    }
                    _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                    return RedirectToAction("Index", new { notification = responseCreateImage.Message });
                }
                else
                {
                    _logger.LogInformation(ConstantMessages.CreateFailure.Replace("{event}", pageName));
                    return RedirectToAction("Index", new { notification = response.Message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Index");
            }
        }
    }
}
