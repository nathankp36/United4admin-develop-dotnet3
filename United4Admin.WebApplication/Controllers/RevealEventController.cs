using United4Admin.WebApplication.ApiClientFactory.Factory;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.BLL;
using United4Admin.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace United4Admin.WebApplication.Controllers
{
    [Authorize(Policy = "CreateEditDeleteEvents")]
    public class RevealEventController : Controller
    {
        private readonly IRevealEventFactory _revealEventFactory;
        private readonly ILogger<RevealEventController> _logger;
        private readonly string pageName = "Reveal Event";
        public RevealEventController(IRevealEventFactory revealEventFactory, IPermissionFactory permissionFactory, ILogger<RevealEventController> logger)
        {
            _revealEventFactory = revealEventFactory;
            _logger = logger;
        }

        // GET: ChoosingParty        
        public async Task<ActionResult> Index(string notification)
        {
            try
            {                
                IList<RevealEventVM> RevealEventVMList = await _revealEventFactory.LoadList();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View(RevealEventVMList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<ActionResult> Create(int? Id = 0)
        {
            ViewBag.Action = "Create";
            try
            {
                RevealEventVM newRE = new RevealEventVM
                {
                    //set dates to today otherwsie they are default null date
                    EventDate = DateTime.Today,
                    WorkflowStatusId = ApplicationConstants.WORKFLOWSTATUSDRAFT,
                    Create = true
                };
                await SetDataLists(newRE);
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", newRE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(RevealEventVM revealEventVM)
        {
            ViewBag.Action = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<RevealEventVM> RevealEventVMList = await _revealEventFactory.LoadList();
                    var eventExistsCount = RevealEventVMList.Where(x => x.EventDate == revealEventVM.EventDate &&
                                      x.Name == revealEventVM.Name &&
                                      x.Location == revealEventVM.Location &&
                                      x.TypeOfReveal == revealEventVM.TypeOfReveal).Count();
                    if (eventExistsCount == 0)
                    {
                        var response = await _revealEventFactory.Create(revealEventVM);
                        _logger.LogInformation(ConstantMessages.Create.Replace("{event}", pageName));
                        return RedirectToAction("Index", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this event information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                        revealEventVM.Name = "";
                    }
                }
                await SetDataLists(revealEventVM);
                return View("Edit", revealEventVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);                
                ModelState.AddModelError("Error", ConstantMessages.Error);
                await SetDataLists(revealEventVM);
                return View("Index");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.Action = "Edit";

            try
            {
                RevealEventVM reVM = await _revealEventFactory.Load(id);
                if (reVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                await SetDataLists(reVM);
                return View("Edit", reVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: ChoosingParty/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(RevealEventVM revealEventVM)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<RevealEventVM> revealEventVMList = await _revealEventFactory.LoadList();
                    var eventExistsCount = revealEventVMList.Where(x => x.EventDate == revealEventVM.EventDate &&
                                      x.Name == revealEventVM.Name &&
                                      x.Location == revealEventVM.Location &&
                                      x.TypeOfReveal == revealEventVM.TypeOfReveal
                                      && x.RevealEventId != revealEventVM.RevealEventId).Count();
                    if (eventExistsCount == 0)
                    {
                        var response = await _revealEventFactory.Update(revealEventVM);
                        _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                        return RedirectToAction("Index", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this event information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                    }
                }

                ViewBag.Action = pageAction;
                await SetDataLists(revealEventVM);
                return View("Edit", revealEventVM);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);                
                ViewBag.Action = pageAction;
                ModelState.AddModelError("Error", ConstantMessages.Error);
                await SetDataLists(revealEventVM);
                return View("Edit", revealEventVM);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                RevealEventVM revealEventVM = await _revealEventFactory.Load(id);
                if (revealEventVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                revealEventVM.LinkedRegistrations = await _revealEventFactory.CheckRegistrationExists(id);
                if (revealEventVM.LinkedRegistrations)
                {
                    ModelState.AddModelError("", "You cannot delete this event as there are registrations connected to it.");
                    _logger.LogWarning(ConstantMessages.SignUpExists.Replace("{event}", pageName));
                }
                return View(revealEventVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _revealEventFactory.Delete(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", new { notification = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);                
                RevealEventVM cpVM = await _revealEventFactory.Load(id);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(cpVM);
            }
        }
        private async Task SetDataLists(RevealEventVM revealEventVM)
        {
            try
            {
                revealEventVM.WorkFlowStatuses = await _revealEventFactory.GetWorkflowStatuses();
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", "SetDataLists"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _revealEventFactory.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
