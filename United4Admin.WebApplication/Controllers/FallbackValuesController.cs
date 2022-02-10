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
    public class FallbackValuesController : Controller
    {
        private readonly IFallbackValuesFactory _fallbackValuesFactory;
        private readonly ILogger<FallbackValuesController> _logger;
        private readonly string pageName = "Fallback Values";
        public FallbackValuesController(IFallbackValuesFactory fallbackValuesFactory, IPermissionFactory permissionFactory, ILogger<FallbackValuesController> logger)
        {
            _fallbackValuesFactory = fallbackValuesFactory;
            _logger = logger;
        }

        //public async Task<ActionResult> Create(int? id = 0)
        //{
        //    ViewBag.Action = "Create";
        //    FallbackValuesVM newFallbackValues = new FallbackValuesVM
        //    {
        //        Update = true
        //    };
        //    return View("Edit", newFallbackValues);
        //}

        //Create Fallback Value
        [HttpPost]
        public async Task<ActionResult> Create(FallbackValuesVM FallbackValuesVM)
        {
            ViewBag.Action = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<FallbackValuesVM> FallbackValuesVMList = await _fallbackValuesFactory.LoadList();
                    var FallbackValuesExistsCount = FallbackValuesVMList.Where(x => x.Field == FallbackValuesVM.Field).Count();
                    if (FallbackValuesExistsCount == 0)
                    {
                        var response = await _fallbackValuesFactory.Create(FallbackValuesVM);
                        _logger.LogInformation(ConstantMessages.Create.Replace("{event}", pageName));
                        return RedirectToAction("Index","SimmaMapping", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this Fallback Values information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                        FallbackValuesVM.Field = "";
                    }
                }
                return View("Edit", FallbackValuesVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return RedirectToAction("Index", "SimmaMapping");
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Action = "Edit";

            try
            {
                FallbackValuesVM FallbackValuesVM = await _fallbackValuesFactory.LoadListById(id);
                if (FallbackValuesVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", FallbackValuesVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        //Edit Fallback Values
        [HttpPost]
        public async Task<ActionResult> Edit(FallbackValuesVM FallbackValuesVM)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<FallbackValuesVM> fallbackValuesVMList = await _fallbackValuesFactory.LoadList();
                    var response = await _fallbackValuesFactory.Update(FallbackValuesVM);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                    return RedirectToAction("Index", "SimmaMapping", new { notification = response.Message });
                }

                ViewBag.Action = pageAction;
                return View("Edit", FallbackValuesVM);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ViewBag.Action = pageAction;
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Edit", FallbackValuesVM);
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                FallbackValuesVM FallbackValuesVM = await _fallbackValuesFactory.LoadListById(id);
                if (FallbackValuesVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                return View(FallbackValuesVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        //Delete fallback Values
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var response = await _fallbackValuesFactory.DeleteById(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", "SimmaMapping", new { notification = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                FallbackValuesVM cpVM = await _fallbackValuesFactory.LoadListById(id);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(cpVM);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fallbackValuesFactory.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
