using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class ProductVariantVM
    {
        [Display(Name = "Product Variant ID")]
        [Required(ErrorMessage = "Please enter a Product Variant ID")]
        [MaxLength(100, ErrorMessage = "Please don't enter more than 100 characters")]

        public string ddlProductTypeCodeDisplay { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter a Product Variant Name")]
        [MaxLength(100, ErrorMessage = "Please don't enter more than 100 characters")]
        public string crmProductVariantName { get; set; }

        [Display(Name = "Nav Incident Type")]
        [Required(ErrorMessage = "Please select a Navision Incident Type")]
        public string crmIncidentType { get; set; }

        [Display(Name = "Nav Purpose Code")]
        [Required(ErrorMessage = "Please enter a Navision Purpose Code")]
        [MaxLength(10, ErrorMessage = "Please don't enter more than 10 characters")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid UK postcode")]
        public string crmPurposeCode { get; set; }

        [Display(Name = "Nav Pledge Type")]
        [Required(ErrorMessage = "Please select a Navision Pledge Type")]
        public string crmPledgeType { get; set; }

        public bool Create { get; set; }

        public List<IncidentTypeVM> IncidentTypeList { get; set; }

        public List<PledgeTypeVM> PledgeTypeList { get; set; }
    }
}
