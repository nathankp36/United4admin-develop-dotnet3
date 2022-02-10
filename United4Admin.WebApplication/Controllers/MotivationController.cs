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
    public class MotivationController : Controller
    {
        private readonly IMotivationFactory _motivationFactory;
        private readonly ILogger<MotivationController> _logger;
        private readonly string pageName = "Campaign Code - Motivation";
        public MotivationController(IMotivationFactory motivationFactory, IPermissionFactory permissionFactory, ILogger<MotivationController> logger)
        {
            _motivationFactory = motivationFactory;
            _logger = logger;
        }

        public async Task<ActionResult> Create(int? id = 0)
        {
            ViewBag.Action = "Create";
            MotivationVM newMotivation = new MotivationVM
            {
                Create = true
            };
            return View("Edit", newMotivation);
        }

        //Create Fallback Value
        [HttpPost]
        public async Task<ActionResult> Create(MotivationVM MotivationVM)
        {
            ViewBag.Action = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<MotivationVM> MotivationVMList = await _motivationFactory.LoadList();
                    var MotivationExistsCount = MotivationVMList.Where(x => x.CampaignCode == MotivationVM.CampaignCode).Count();
                    if (MotivationExistsCount == 0)
                    {
                        var response = await _motivationFactory.Create(MotivationVM);
                        _logger.LogInformation(ConstantMessages.Create.Replace("{event}", pageName));
                        return RedirectToAction("Index","SimmaMapping", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this Fallback Values information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                        MotivationVM.CampaignCode = "";
                    }
                }
                return View("Edit", MotivationVM);
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
                MotivationVM MotivationVM = await _motivationFactory.LoadListById(id);
                if (MotivationVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", MotivationVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        //Edit Fallback Values
        [HttpPost]
        public async Task<ActionResult> Edit(MotivationVM MotivationVM)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<MotivationVM> MotivationVMList = await _motivationFactory.LoadList();
                    var response = await _motivationFactory.Update(MotivationVM);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                    return RedirectToAction("Index", "SimmaMapping", new { notification = response.Message });
                }

                ViewBag.Action = pageAction;
                return View("Edit", MotivationVM);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ViewBag.Action = pageAction;
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Edit", MotivationVM);
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                MotivationVM MotivationVM = await _motivationFactory.LoadListById(id);
                if (MotivationVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                return View(MotivationVM);
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
                var response = await _motivationFactory.DeleteById(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", "SimmaMapping", new { notification = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                MotivationVM cpVM = await _motivationFactory.LoadListById(id);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(cpVM);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _motivationFactory.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
