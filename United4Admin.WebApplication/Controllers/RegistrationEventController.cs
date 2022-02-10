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

namespace United4Admin.WebApplication.Controllers
{
    [Authorize(Policy = "CreateEditDeleteEvents")]
    public class RegistrationEventController : Controller
    {
        private readonly ISignupEventFactory _signupEventFactory;
        private readonly IRevealEventFactory _revealEventFactory;
        private readonly IChoosingEventFactory _choosingEventFactory;
        private readonly ILogger<RegistrationEventController> _logger;
        private readonly string pageName = "Reveal Event";
        public RegistrationEventController(ISignupEventFactory signupEventFactory, IChoosingEventFactory choosingEventFactory, IRevealEventFactory revealEventFactory, IPermissionFactory permissionsRepository, ILogger<RegistrationEventController> logger)
        {
            _signupEventFactory = signupEventFactory;
            _revealEventFactory = revealEventFactory;
            _choosingEventFactory = choosingEventFactory;
            _logger = logger;
        }
        // GET: ChoosingParty
        public async Task<ActionResult> Index(string notification)
        {
            try
            {
                IList<SignUpEventVM> signUpEventVMList = await _signupEventFactory.LoadList();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View(signUpEventVMList);
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
                SignUpEventVM signUpEventVM = new SignUpEventVM
                {
                    //set dates to today otherwsie they are default null date
                    ClosedDate = DateTime.Today,
                    EventDate = DateTime.Today,
                    PublishDate = DateTime.Today,
                    WorkflowStatusId = ApplicationConstants.WORKFLOWSTATUSDRAFT,
                    Create = true
                };
                await SetDataLists(signUpEventVM);
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", signUpEventVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }


        }

        [HttpPost]
        public async Task<ActionResult> Create(SignUpEventVM signUpEventVM)
        {
            ViewBag.Action = "Create";
            try
            {
                if (signUpEventVM.SpecificChoosingEvent == "True")
                {
                    if (signUpEventVM.ChoosingPartyId == null || signUpEventVM.ChoosingPartyId == 0)
                    {
                        ModelState.AddModelError("ChoosingPartyId", "Select Related Choosing Event.");
                    }
                    if (signUpEventVM.RevealEventId == null || signUpEventVM.RevealEventId == 0)
                    {
                        ModelState.AddModelError("RevealEventId", "Select Related Reveal Event.");
                    }
                }
                else
                {
                    if (signUpEventVM.ChoosingPartyId == 0)
                    {
                        signUpEventVM.ChoosingPartyId = null;
                    }
                    if (signUpEventVM.RevealEventId == 0)
                    {
                        signUpEventVM.RevealEventId = null;
                    }
                }
                if (ModelState.IsValid)
                {
                    IList<SignUpEventVM> signUpEventVMList = await _signupEventFactory.LoadList();
                    var eventExistsCount = signUpEventVMList.Where(x => x.EventDate == signUpEventVM.EventDate &&
                                      x.EventName == signUpEventVM.EventName &&
                                      x.Location == signUpEventVM.Location &&
                                      x.ShortURL == signUpEventVM.ShortURL.ToLower()).Count();
                    if (eventExistsCount == 0)
                    {
                        var existsUrlCount = signUpEventVMList.Where(x => x.ShortURL.Equals(signUpEventVM.ShortURL, StringComparison.OrdinalIgnoreCase)).Count();
                        var existsNameCount = signUpEventVMList.Where(x => x.EventName.Equals(signUpEventVM.EventName, StringComparison.OrdinalIgnoreCase)).Count();

                        if (existsUrlCount > 0)
                        {
                            ModelState.AddModelError("ShortURL", "This Short URL already exists for another registration event. Please enter another.");
                            _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                            signUpEventVM.ShortURL = "";
                        }
                        else if (existsNameCount > 0)
                        {
                            ModelState.AddModelError("EventName", "This Event Name already exists for another registration event. Please enter another.");
                            _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                            signUpEventVM.EventName = "";
                        }
                        else
                        {
                            var response = await _signupEventFactory.Create(signUpEventVM);
                            _logger.LogInformation(ConstantMessages.Create.Replace("{event}", pageName));
                            return RedirectToAction("Index", new { notification = response.Message });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this event information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                        signUpEventVM.EventName = "";
                    }

                }
                await SetDataLists(signUpEventVM);
                return View("Edit", signUpEventVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                await SetDataLists(signUpEventVM);
                return View("Index");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.Action = "Edit";

            try
            {
                SignUpEventVM sueVM = await _signupEventFactory.Load(id);
                if (sueVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                await SetDataLists(sueVM);
                return View("Edit", sueVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(SignUpEventVM signUpEventVM)
        {

            ViewBag.Action = "Edit";
            try
            {
                if (signUpEventVM.SpecificChoosingEvent == "True")
                {
                    if (signUpEventVM.ChoosingPartyId == null || signUpEventVM.ChoosingPartyId == 0)
                    {
                        ModelState.AddModelError("ChoosingPartyId", "Select Related Choosing Event.");
                    }
                    if (signUpEventVM.RevealEventId == null || signUpEventVM.RevealEventId == 0)
                    {
                        ModelState.AddModelError("RevealEventId", "Select Related Reveal Event.");
                    }
                }
                else
                {
                    if (signUpEventVM.ChoosingPartyId == 0)
                    {
                        signUpEventVM.ChoosingPartyId = null;
                    }
                    if (signUpEventVM.RevealEventId == 0)
                    {
                        signUpEventVM.RevealEventId = null;
                    }
                }

                if (ModelState.IsValid)
                {
                    IList<SignUpEventVM> signUpEventVMList = await _signupEventFactory.LoadList();
                    var eventExistsCount = signUpEventVMList.Where(x => x.EventDate == signUpEventVM.EventDate &&
                                      x.EventName == signUpEventVM.EventName &&
                                      x.Location == signUpEventVM.Location &&
                                      x.ShortURL == signUpEventVM.ShortURL.ToLower() &&
                                      x.SignUpEventId != signUpEventVM.SignUpEventId).Count();
                    if (eventExistsCount == 0)
                    {
                        var existsUrlCount = signUpEventVMList.Where(x => x.ShortURL.Equals(signUpEventVM.ShortURL, StringComparison.OrdinalIgnoreCase) &&
                                      x.SignUpEventId != signUpEventVM.SignUpEventId).Count();
                        var existsNameCount = signUpEventVMList.Where(x => x.EventName.Equals(signUpEventVM.EventName, StringComparison.OrdinalIgnoreCase) &&
                                      x.SignUpEventId != signUpEventVM.SignUpEventId).Count();

                        if (existsUrlCount > 0)
                        {
                            ModelState.AddModelError("ShortURL", "This Short URL already exists for another registration event. Please enter another.");
                            _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                            signUpEventVM.ShortURL = "";
                        }
                        else if (existsNameCount > 0)
                        {
                            ModelState.AddModelError("EventName", "This Event Name already exists for another registration event. Please enter another.");
                            _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                            signUpEventVM.EventName = "";
                        }
                        else
                        {
                            signUpEventVM.ShortURL = signUpEventVM.ShortURL.ToLower();
                            var response = await _signupEventFactory.Update(signUpEventVM);
                            _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                            return RedirectToAction("Index", new { notification = response.Message });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this event information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                        signUpEventVM.EventName = "";
                    }
                }
                await SetDataLists(signUpEventVM);
                return View("Edit", signUpEventVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                await SetDataLists(signUpEventVM);
                return View("Index");
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                SignUpEventVM signUpEventVM = await _signupEventFactory.Load(id);
                if (signUpEventVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                signUpEventVM.LinkedRegistrations = await _signupEventFactory.CheckRegistrationExists(id);
                if (signUpEventVM.LinkedRegistrations)
                {
                    ModelState.AddModelError("", "You cannot delete this event as there are registrations connected to it.");
                    _logger.LogWarning(ConstantMessages.SignUpExists.Replace("{event}", pageName));
                }
                return View(signUpEventVM);
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
                var response = await _signupEventFactory.Delete(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", new { notification = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                SignUpEventVM signUpEventVM = await _signupEventFactory.Load(id);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(signUpEventVM);
            }
        }
        private async Task SetDataLists(SignUpEventVM signUpEventVM)
        {
            try
            {
                signUpEventVM.WorkFlowStatuses = await _signupEventFactory.GetWorkflowStatuses();
                signUpEventVM.ChoosingParties = await _choosingEventFactory.LoadList();
                signUpEventVM.RevealEvents = await _revealEventFactory.LoadList();
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
                _signupEventFactory.Dispose();
                _choosingEventFactory.Dispose();
                _revealEventFactory.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
