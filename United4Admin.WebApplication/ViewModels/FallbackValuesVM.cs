using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class FallbackValuesVM
    {
        [Display(Name = "Simma Field")]
        [Required]
        [MaxLength(200, ErrorMessage = "Please don't enter more than 200 characters")]
        public string Field { get; set; }

        [Display(Name = "Simma Values")]
        [Required]       
        public int Value { get; set; }
    }
}
