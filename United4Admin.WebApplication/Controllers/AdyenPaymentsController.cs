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
using Newtonsoft.Json;

namespace United4Admin.WebApplication.Controllers
{
    [Authorize(Policy = "CreateEditDeleteEvents")]
    public class AdyenPaymentsController : Controller
    {
        private readonly IAdyenPaymentsFactory _adyenPaymentsFactory;
        private readonly ILogger<AdyenPaymentsController> _logger;
        private readonly string pageName = "Adyen Payments";
        public AdyenPaymentsController(IAdyenPaymentsFactory adyenPaymentsFactory, ILogger<AdyenPaymentsController> logger)
        {
            _adyenPaymentsFactory = adyenPaymentsFactory;
            _logger = logger;
        }

        // GET: Adyen Subscriptions with search 
        public async Task<ActionResult> Index(string notification, string Search, string SearchDDL,DateTime? SearchFromDate,DateTime? SearchToDate)
        {
            try
            {
                IList<AdyenTransactionVM> adyenTransactionVM = await _adyenPaymentsFactory.LoadListSearch(Search, SearchDDL, SearchFromDate, SearchToDate);
               
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View(adyenTransactionVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        // EDIT: Adyen Transaction
        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.Action = "Edit";

            try
            {
                AdyenTransactionVM adyenTransactionVM = await _adyenPaymentsFactory.Load(id);
                if (adyenTransactionVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", adyenTransactionVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        // Create: Adyen Transaction
        [HttpPost]
        public async Task<ActionResult> Edit(AdyenTransactionVM adyenTransactionVM)
        {
            ViewBag.Action = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _adyenPaymentsFactory.Create(adyenTransactionVM);
                    _logger.LogInformation(ConstantMessages.Create.Replace("{event}", pageName));
                    return RedirectToAction("Index", new { notification = response.Message });
                }
                return View("Index");
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
