using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using United4Admin.WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using United4Admin.WebApplication.BLL;

namespace United4Admin.WebApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPermissionFactory _permissionFactory;
        private readonly string pageName = "Home";
        public HomeController(IPermissionFactory permissionFactory,ILogger<HomeController> logger)
        {
            _logger = logger;
            _permissionFactory = permissionFactory;
        }
        public ActionResult Index()
        {
            try
            {
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
            return View();
        }
        public async Task<ActionResult> NotAuthorised()
        {
            try
            {
                List<PermissionsVM> adminsVM = await _permissionFactory.GetAdministrators();
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                return View(adminsVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
           
        }
        public ActionResult Error()
        {
            ErrorViewModel errorViewModel = new ErrorViewModel();
            return View(errorViewModel);
        }
    }
}
