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
    public class SimmaMappingController : Controller
    {
        private readonly IFallbackValuesFactory _fallbackValuesFactory;
        private readonly IMotivationFactory _motivationFactory;
        private readonly IPledgeDesignationFactory _pledgeDesignationFactory;
        private readonly ILogger<SimmaMappingController> _logger;
        private readonly string pageName = "Data Mapping";
        public SimmaMappingController(IFallbackValuesFactory fallbackValuesFactory, IMotivationFactory motivationFactory, IPledgeDesignationFactory pledgeDesignationFactory, IPermissionFactory permissionFactory, ILogger<SimmaMappingController> logger)
        {
            _fallbackValuesFactory = fallbackValuesFactory;
            _motivationFactory = motivationFactory;
            _pledgeDesignationFactory = pledgeDesignationFactory;
            _logger = logger;
        }


        public async Task<IActionResult> Index(string notification)
        {
            try
            {
                SimmaMappingVM simmaMappingList = new SimmaMappingVM();
                IList<FallbackValuesVM> fallbackValuesVMList = await _fallbackValuesFactory.LoadList();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                IList<MotivationVM> motivationVMList = await _motivationFactory.LoadList();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                IList<PledgeDesignationVM> pledgeDesignationList = await _pledgeDesignationFactory.LoadList();
                ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                simmaMappingList.FallbackValuesVM = fallbackValuesVMList;
                simmaMappingList.MotivationVM = motivationVMList;
                simmaMappingList.PledgeDesignationVM = pledgeDesignationList;
                return View(simmaMappingList);                
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
                _fallbackValuesFactory.Dispose();
                _motivationFactory.Dispose();
                _pledgeDesignationFactory.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
