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
    public class PledgeDesignation : Controller
    {
        private readonly IPledgeDesignationFactory _pledgeDesignationFactory;
        private readonly ILogger<PledgeDesignation> _logger;
        private readonly string pageName = "Pledge and Designation";
        public PledgeDesignation(IPledgeDesignationFactory pledgeDesignationFactory, IPermissionFactory permissionFactory, ILogger<PledgeDesignation> logger)
        {
            _pledgeDesignationFactory = pledgeDesignationFactory;
            _logger = logger;
        }

        public async Task<ActionResult> Create(int? id = 0)
        {
            ViewBag.Action = "Create";
            PledgeDesignationVM newPledgeDesignation = new PledgeDesignationVM
            {
                Create = true
            };
            return View("Edit", newPledgeDesignation);
        }

        //Create Pledge and Designation
        [HttpPost]
        public async Task<ActionResult> Create(PledgeDesignationVM PledgeDesignationVM)
        {
            ViewBag.Action = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<PledgeDesignationVM> PledgeDesignationVMList = await _pledgeDesignationFactory.LoadList();
                    var MotivationExistsCount = PledgeDesignationVMList.Where(x => x.WvProductVariant == PledgeDesignationVM.WvProductVariant).Count();
                    if (MotivationExistsCount == 0)
                    {
                        var response = await _pledgeDesignationFactory.Create(PledgeDesignationVM);
                        _logger.LogInformation(ConstantMessages.Create.Replace("{event}", pageName));
                        return RedirectToAction("Index","SimmaMapping", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this Pledge and Designation Values information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                        PledgeDesignationVM.WvFormDescription = "";
                    }
                }
                return View("Edit", PledgeDesignationVM);
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
                PledgeDesignationVM PledgeDesignationVM = await _pledgeDesignationFactory.LoadListById(id);
                if (PledgeDesignationVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", PledgeDesignationVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        //Edit Fallback Values
        [HttpPost]
        public async Task<ActionResult> Edit(PledgeDesignationVM PledgeDesignationVM)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<PledgeDesignationVM> PledgeDesignationVMList = await _pledgeDesignationFactory.LoadList();
                    var response = await _pledgeDesignationFactory.Update(PledgeDesignationVM);
                    _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                    return RedirectToAction("Index", "SimmaMapping", new { notification = response.Message });
                }

                ViewBag.Action = pageAction;
                return View("Edit", PledgeDesignationVM);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ViewBag.Action = pageAction;
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Edit", PledgeDesignationVM);
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                PledgeDesignationVM PledgeDesignationVM = await _pledgeDesignationFactory.LoadListById(id);
                if (PledgeDesignationVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                return View(PledgeDesignationVM);
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
                var response = await _pledgeDesignationFactory.DeleteById(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", "SimmaMapping", new { notification = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                PledgeDesignationVM cpVM = await _pledgeDesignationFactory.LoadListById(id);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(cpVM);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pledgeDesignationFactory.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
