using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace United4Admin.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public void SignOut()
        {
            //string callbackUrl = Url.Action("SignOutCallback", "Account", routeValues: null, protocol: Request.Url.Scheme);

            //HttpContext.GetOwinContext().Authentication.SignOut(
            //    new AuthenticationProperties { RedirectUri = callbackUrl },
            //    OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback()
        {
            //if (Request.IsAuthenticated)
            //{
            //    // Redirect to home page if the user is authenticated.
            //    return RedirectToAction("Index", "Home");
            //}

            return View();
        }
    }
}
