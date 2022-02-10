using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;

namespace United4Admin.WebApplication.Controllers
{
    public class FeatureSwitchController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private readonly IFeatureSwitchFactory _featureSwitchFactory;

        public FeatureSwitchController( IFeatureSwitchFactory featureSwitchFactory)
        {
            _featureSwitchFactory = featureSwitchFactory;

        }

        
        public ActionResult IsFeatureSwitch(string name)
        {
            var featureName = "Name=" + name;
            var feature = Task.Run(async () =>
            {
                var result = await _featureSwitchFactory.CheckFeatureSwitch(featureName);
                return result;
            });

            var featureFlagResult = feature.Result.ToString();

            return Content(featureFlagResult);
        }
    }
}
