using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class PledgeDesignationVM
    {
        [Display(Name = "WV form description")]
        [Required]
        [MaxLength(100, ErrorMessage = "Please don't enter more than 100 characters")]
        public string WvFormDescription { get; set; }

        [Display(Name = "WV form ID (Product Variant)")]
        [Required]
        [MaxLength(100, ErrorMessage = "Please don't enter more than 100 characters")]
        public string WvProductVariant { get; set; }


        [Display(Name = "Simma Pledge TypeId")]
        [Required]      
        public int CrmPledgeId { get; set; }


        [Display(Name = "Simma DesignationId")]
        [Required]        
        public int CrmDesignationId { get; set; }

        public bool Create { get; set; }
    }
}
