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
    public class NavisionMappingController : Controller
    {
        private readonly ISalutationFactory _salutationFactory;
        private readonly IProductVariantFactory _productVariantFactory;
        private readonly IPaymentMethodFactory _paymentMethodFactory;
        private readonly ILogger<NavisionMappingController> _logger;
        private readonly string pageName = "Data Mapping";
        public NavisionMappingController(ISalutationFactory salutationFactory, IProductVariantFactory productVariant, IPaymentMethodFactory paymentMethodFactory, IPermissionFactory permissionFactory, ILogger<NavisionMappingController> logger)
        {
            _salutationFactory = salutationFactory;
            _productVariantFactory = productVariant;
            _paymentMethodFactory = paymentMethodFactory;
            _logger = logger;
        }

        //Load Salutaion, Product variant and Payment method lists
        public async Task<IActionResult> Index(string notification)
        {
            try
            {
                CRMMappingVM cRMMappingList = new CRMMappingVM();
                IList<SalutationVM> salutationVMList = await _salutationFactory.LoadList();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                IList<ProductVariantVM> productVariantVMList = await _productVariantFactory.LoadList();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                IList<PaymentMethodVM> paymentMethodVMList = await _paymentMethodFactory.LoadList();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                cRMMappingList.SalutationVM = salutationVMList;
                cRMMappingList.ProductVariantVM = productVariantVMList;
                cRMMappingList.PaymentMethodVM = paymentMethodVMList;
                return View(cRMMappingList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _salutationFactory.Dispose();
                _productVariantFactory.Dispose();
                _paymentMethodFactory.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
