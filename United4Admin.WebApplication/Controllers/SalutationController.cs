using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.BLL;
using United4Admin.WebApplication.ViewModels;

namespace United4Admin.WebApplication.Controllers
{
    [Authorize(Policy = "CreateEditDeleteEvents")]
    public class SalutationController : Controller
    {
        private readonly ISalutationFactory _salutationFactory;
        private readonly ILogger<SalutationController> _logger;
        private readonly string pageName = "Salutation";
        public SalutationController(ISalutationFactory salutationFactory, IPermissionFactory permissionFactory, ILogger<SalutationController> logger)
        {
            _salutationFactory = salutationFactory;
            _logger = logger;
        }

        public async Task<ActionResult> Create(int? id = 0)
        {
            ViewBag.Action = "Create";
            SalutationVM newSalutation = new SalutationVM
            {
                Create = true
            };
            return View("Edit", newSalutation);
        }

        //Create Salutation
        [HttpPost]
        public async Task<ActionResult> Create(SalutationVM SalutationVM)
        {
            ViewBag.Action = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<SalutationVM> SalutationVMList = await _salutationFactory.LoadList();
                    var salutationExistsCount = SalutationVMList.Where(x => x.ddlSalutation == SalutationVM.ddlSalutation).Count();
                    if (salutationExistsCount == 0)
                    {
                        var response = await _salutationFactory.Create(SalutationVM);
                        _logger.LogInformation(ConstantMessages.Create.Replace("{event}", pageName));
                        return RedirectToAction("Index","NavisionMapping", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this salutation information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                        SalutationVM.ddlSalutation = "";
                    }
                }
                return View("Edit", SalutationVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return RedirectToAction("Index", "NavisionMapping");
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Action = "Edit";

            try
            {
                SalutationVM salutationVM = await _salutationFactory.LoadListById(id);
                if (salutationVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", salutationVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        //Edit Saluation
        [HttpPost]
        public async Task<ActionResult> Edit(SalutationVM salutationVM)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<SalutationVM> salutationVMList = await _salutationFactory.LoadList();
                    var response = await _salutationFactory.Update(salutationVM);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                    return RedirectToAction("Index", "NavisionMapping", new { notification = response.Message });
                }

                ViewBag.Action = pageAction;
                return View("Edit", salutationVM);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ViewBag.Action = pageAction;
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Edit", salutationVM);
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                SalutationVM SalutationVM = await _salutationFactory.LoadListById(id);
                if (SalutationVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                return View(SalutationVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        //Delete Salutation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var response = await _salutationFactory.DeleteById(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index","NavisionMapping", new { notification = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                SalutationVM cpVM = await _salutationFactory.LoadListById(id);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(cpVM);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _salutationFactory.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
