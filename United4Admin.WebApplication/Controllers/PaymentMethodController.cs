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
    public class PaymentMethodController : Controller
    {
        private readonly IPaymentMethodFactory _paymentMethodFactory;
        private readonly ILogger<PaymentMethodController> _logger;
        private readonly string pageName = "Salutation";
        public PaymentMethodController(IPaymentMethodFactory paymentMethodFactory, IPermissionFactory permissionFactory, ILogger<PaymentMethodController> logger)
        {
            _paymentMethodFactory = paymentMethodFactory;
            _logger = logger;
        }

        public async Task<ActionResult> Create(int? id=0)
        {
            ViewBag.Action = "Create";
            PaymentMethodVM newPaymentMethod = new PaymentMethodVM
            {
                Create = true
            };
            return View("Edit", newPaymentMethod);
        }

        //Create Payment Method
        [HttpPost]
        public async Task<ActionResult> Create(PaymentMethodVM PaymentMethodVM)
        {
            ViewBag.Action = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<PaymentMethodVM> PaymentMethodVMList = await _paymentMethodFactory.LoadList();
                    var paymentMethodExistsCount = PaymentMethodVMList.Where(x => x.crmPaymentMethodName == PaymentMethodVM.crmPaymentMethodName &&
                                      x.crmPaymentMethodType == PaymentMethodVM.crmPaymentMethodType &&
                                      x.ddlWvType == PaymentMethodVM.ddlWvType).Count();
                    if (paymentMethodExistsCount == 0)
                    {
                        var response = await _paymentMethodFactory.Create(PaymentMethodVM);
                        _logger.LogInformation(ConstantMessages.Create.Replace("{event}", pageName));
                        return RedirectToAction("Index", "NavisionMapping", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this payment method information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                        PaymentMethodVM.crmPaymentMethodName = "";
                    }
                }
                return View("Edit", PaymentMethodVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return RedirectToAction("Index", "NavisionMapping");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.Action = "Edit";

            try
            {
                PaymentMethodVM PaymentMethodVM = await _paymentMethodFactory.Load(id);
                if (PaymentMethodVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View("Edit", PaymentMethodVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        //Edit Payment method
        [HttpPost]
        public async Task<ActionResult> Edit(PaymentMethodVM PaymentMethodVM)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<PaymentMethodVM> PaymentMethodVMList = await _paymentMethodFactory.LoadList();
                    var paymentMethodExistsCount = PaymentMethodVMList.Where(x => x.crmPaymentMethodName == PaymentMethodVM.crmPaymentMethodName &&
                                      x.crmPaymentMethodType == PaymentMethodVM.crmPaymentMethodType &&
                                      x.ddlWvType != PaymentMethodVM.ddlWvType).Count();
                    if (paymentMethodExistsCount == 0)
                    {
                        var response = await _paymentMethodFactory.Update(PaymentMethodVM);
                        _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                        return RedirectToAction("Index", "NavisionMapping", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this salutatoin information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                    }
                }

                ViewBag.Action = pageAction;
                return View("Edit", PaymentMethodVM);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ViewBag.Action = pageAction;
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View("Edit", PaymentMethodVM);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                PaymentMethodVM PaymentMethodVM = await _paymentMethodFactory.Load(id);
                if (PaymentMethodVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                return View(PaymentMethodVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        //Delete Payment method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _paymentMethodFactory.Delete(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", "NavisionMapping", new { notification = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                PaymentMethodVM cpVM = await _paymentMethodFactory.Load(id);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(cpVM);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _paymentMethodFactory.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
