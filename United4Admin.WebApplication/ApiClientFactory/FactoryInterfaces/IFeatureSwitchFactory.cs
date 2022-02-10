using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces
{
    public interface IFeatureSwitchFactory
    {
        Task<bool> CheckFeatureSwitch(string featureName);
    }
}
