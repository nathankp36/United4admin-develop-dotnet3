using United4Admin.WebApplication.ApiClientFactory.Factory;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using United4Admin.WebApplication.BLL;
using System.Security.AccessControl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.InkML;

namespace United4Admin.WebApplication.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly IPermissionFactory _pFactoryRepo;
        private readonly ILogger<AdministrationController> _logger;
        private readonly string pageName = "Admin";
        private readonly string _UserName;

        [ActivatorUtilitiesConstructor]
        public AdministrationController(IPermissionFactory permissionFactory, ILogger<AdministrationController> logger)
        {
            _pFactoryRepo = permissionFactory;
            _logger = logger;
            _UserName = Startup.userEmail;
    }

        public AdministrationController(IPermissionFactory permissionFactory, ILogger<AdministrationController> logger, string userName)
        {
            _pFactoryRepo = permissionFactory;
            _logger = logger;
            _UserName = userName;
        }
        // GET: Admin        
        public async Task<ActionResult> UserIndex(string notification)
        {
            try
            {
                IList<PermissionsVM> permissionsVMList = await _pFactoryRepo.LoadList();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View(permissionsVMList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult UserCreate(int? Id = 0)
        {
            ViewBag.Action = "Create";
            try
            {
                PermissionsVM newPermissionModel = new PermissionsVM();
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("UserEdit", newPermissionModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> UserCreate(PermissionsVM permissionsVM)
        {
            ViewBag.Action = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    permissionsVM.WVEmail = permissionsVM.WVEmail.ToLower();
                    IList<PermissionsVM> permissionsVMList = await _pFactoryRepo.LoadList();
                    var emailExistsCount = permissionsVMList.Where(x => x.WVEmail == permissionsVM.WVEmail).Count();
                    if (emailExistsCount == 0)
                    {
                        var response = await _pFactoryRepo.Create(permissionsVM);
                        _logger.LogInformation(ConstantMessages.Create.Replace("{event}", pageName));
                        return RedirectToAction("UserIndex", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this email");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                        permissionsVM.WVEmail = "";
                    }
                }
                return View("UserEdit", permissionsVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error",ConstantMessages.Error);
                return View("UserIndex");
            }
        }

        public async Task<ActionResult> UserEdit(int id)
        {
            ViewBag.Action = "Edit";

            try
            {
                PermissionsVM permissionVM = await _pFactoryRepo.Load(id);
                if (permissionVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("UserEdit", permissionVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> UserEdit(PermissionsVM permissionVM)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    permissionVM.WVEmail = permissionVM.WVEmail.ToLower();
                    IList<PermissionsVM> permissionsVMList = await _pFactoryRepo.LoadList();
                    var emailExistsCount = permissionsVMList.Where(x => x.WVEmail == permissionVM.WVEmail && x.PermissionsId != permissionVM.PermissionsId).Count();
                    if (emailExistsCount == 0)
                    {
                        var response = await _pFactoryRepo.Update(permissionVM);
                        _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                        return RedirectToAction("UserIndex", new { notification = response.Message });
                    }
                    else
                    {
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                        ModelState.AddModelError("Exists", "Already Exists this email");
                        permissionVM.WVEmail = "";
                    }
                }

                ViewBag.Action = pageAction;
                return View("UserEdit", permissionVM);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                
                ViewBag.Action = pageAction;
                ModelState.AddModelError("Error",ConstantMessages.Error);
                return View("UserEdit", permissionVM);
            }
        }
        public async Task<ActionResult> UserDelete(int id)
        {
            bool isSelf = false;
            try
            {
                PermissionsVM permissionVM = await _pFactoryRepo.Load(id);
                if (permissionVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                if (permissionVM.WVEmail == _UserName)
                {
                    isSelf = true;                    
                    ModelState.AddModelError("", ConstantMessages.SelfDelete); 
                    _logger.LogWarning(ConstantMessages.SelfDelete);
                }
                ViewBag.Self = isSelf;
                return View("UserDelete", permissionVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost, ActionName("UserDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserDeleteConfirmed(int id)
        {
            try
            {
                var response = await _pFactoryRepo.Delete(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("UserIndex", new { notification = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);                
                PermissionsVM pVM = await _pFactoryRepo.Load(id);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(pVM);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pFactoryRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}