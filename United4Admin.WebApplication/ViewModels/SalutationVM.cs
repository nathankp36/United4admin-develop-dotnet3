using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class SalutationVM
    {
        [Display(Name = "United 4 Salutation")]
        [Required]
        [MaxLength(100, ErrorMessage = "Please don't enter more than 100 characters")]
        public string ddlSalutation { get; set; }

        [Display(Name = "CRM Values")]
        [Required]
        [MaxLength(30, ErrorMessage = "Please don't enter more than 30 characters")]
        public string crmSalutation { get; set; }

        public bool Create { get; set; }
    }
}
