using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace United4Admin.WebApplication.ViewModels
{
    public class PaymentMethodVM
    {
        [Display(Name ="Payment Method ID")]
        [Required(ErrorMessage = "Please enter a Payment Method Id")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Payment Id")]
        public int ddlWvType { get; set; }

        [Display(Name ="Nav Payment Method")]
        [Required(ErrorMessage = "Please enter a Payment Method Type")]
        [MaxLength(30, ErrorMessage = "Please don't enter more than 30 characters")]
        public string crmPaymentMethodType { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter a Payment Method Name")]
        [MaxLength(100, ErrorMessage = "Please don't enter more than 100 characters")]
        public string crmPaymentMethodName { get; set; }

        public bool Create { get; set; }
    }
}
