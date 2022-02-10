using United4Admin.WebApplication.ApiClientFactory.Factory;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using United4Admin.WebApplication.BLL;
using Microsoft.AspNetCore.Authorization;

namespace United4Admin.WebApplication.Controllers
{
    [Authorize(Policy = "CreateEditDeleteEvents")]    
    public class ChoosingEventController : Controller
    {
        private readonly IChoosingEventFactory _choosingEventFactory;
        private readonly ILogger<ChoosingEventController> _logger;
        private readonly string pageName = "Choosing Event";
        public ChoosingEventController(IChoosingEventFactory choosingEventFactory, IPermissionFactory permissionFactory, ILogger<ChoosingEventController> logger) 
        {
            _choosingEventFactory = choosingEventFactory;
            _logger = logger;
        }

        // GET: ChoosingParty       
        public async Task<ActionResult> Index(string notification)
        {
            try
            {                
                IList<ChoosingPartyVM> choosingPartyVMList = await _choosingEventFactory.LoadList();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}",pageName));
                return View(choosingPartyVMList);
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
                ChoosingPartyVM newCE = new ChoosingPartyVM
                {
                    //set dates to today otherwsie they are default null date
                    PartyDate = DateTime.Today,
                    WorkflowStatusId = ApplicationConstants.WORKFLOWSTATUSDRAFT,
                    Create = true
                };
                await SetDataLists(newCE);
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", newCE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(ChoosingPartyVM choosingPartyVM)
        {
            ViewBag.Action = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<ChoosingPartyVM> choosingPartyVMList = await _choosingEventFactory.LoadList();
                    var eventExistsCount = choosingPartyVMList.Where(x => x.PartyDate == choosingPartyVM.PartyDate &&
                                      x.PartyName == choosingPartyVM.PartyName &&
                                      x.Location == choosingPartyVM.Location &&
                                      x.Country == choosingPartyVM.Country).Count();
                    if (eventExistsCount == 0)
                    {
                        var response = await _choosingEventFactory.Create(choosingPartyVM);
                        _logger.LogInformation(ConstantMessages.Create,  "CreateSave");
                        return RedirectToAction("Index", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this event information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                        choosingPartyVM.PartyName = "";
                    }
                }
                await SetDataLists(choosingPartyVM);
                return View("Edit", choosingPartyVM);
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, ConstantMessages.Error);
                // return RedirectToAction("Error", "Home");
                ModelState.AddModelError("Error", ConstantMessages.Error);
                await SetDataLists(choosingPartyVM);
                return View("Index");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.Action = "Edit";

            try
            {
                ChoosingPartyVM cpVM = await _choosingEventFactory.Load(id);
                if (cpVM == null)
                {
                    return NotFound();
                }
                await SetDataLists(cpVM);
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", cpVM);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ConstantMessages.Error,ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: ChoosingParty/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ChoosingPartyVM choosingPartyVM)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<ChoosingPartyVM> choosingPartyVMList = await _choosingEventFactory.LoadList();
                    var eventExistsCount = choosingPartyVMList.Where(x => x.PartyDate == choosingPartyVM.PartyDate &&
                                      x.PartyName == choosingPartyVM.PartyName &&
                                      x.Location == choosingPartyVM.Location &&
                                      x.Country == choosingPartyVM.Country
                                      && x.ChoosingPartyId != choosingPartyVM.ChoosingPartyId).Count();
                    if (eventExistsCount == 0)
                    {
                        var response = await _choosingEventFactory.Update(choosingPartyVM);
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
                await SetDataLists(choosingPartyVM);
                return View("Edit", choosingPartyVM);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                //  return RedirectToAction("Error", "Home");
                ViewBag.Action = pageAction;
                ModelState.AddModelError("Error", ConstantMessages.Error);
                await SetDataLists(choosingPartyVM);
                return View("Edit", choosingPartyVM);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                ChoosingPartyVM cpVM = await _choosingEventFactory.Load(id);
                if (cpVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();                    
                }
                cpVM.LinkedRegistrations = await _choosingEventFactory.CheckRegistrationExists(id);
                if (cpVM.LinkedRegistrations)
                { 
                    ModelState.AddModelError("", "You cannot delete this event as there are registrations connected to it.");
                    _logger.LogWarning(ConstantMessages.SignUpExists.Replace("{event}", pageName));
                }
                return View(cpVM);
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
                var response = await _choosingEventFactory.Delete(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", new { notification = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                //  return RedirectToAction("Error", "Home");
                ChoosingPartyVM cpVM = await _choosingEventFactory.Load(id);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(cpVM);
            }
        }
        private async Task SetDataLists(ChoosingPartyVM choosingPartyVM)
        {
            try
            {
                choosingPartyVM.WorkFlowStatuses = await _choosingEventFactory.GetWorkflowStatuses();
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", "SetDataLists"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                throw ex;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _choosingEventFactory.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
