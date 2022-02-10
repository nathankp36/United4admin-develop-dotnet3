using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class MotivationVM
    {
        [Display(Name = "WV Campaign Code")]
        [Required]
        [MaxLength(200, ErrorMessage = "Please don't enter more than 200 characters")]
        public string CampaignCode { get; set; }

        [Display(Name = "Simma MotivationId")]
        [Required]        
        public int MotivatinId { get; set; }

        public bool Create { get; set; }
    }
}
