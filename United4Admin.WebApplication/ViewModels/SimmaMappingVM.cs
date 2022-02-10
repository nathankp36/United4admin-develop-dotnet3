using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class SimmaMappingVM
    {
        public IList<FallbackValuesVM> FallbackValuesVM { get; set; }
        public IList<MotivationVM> MotivationVM { get; set; }
        public IList<PledgeDesignationVM> PledgeDesignationVM { get; set; }
    }
}
