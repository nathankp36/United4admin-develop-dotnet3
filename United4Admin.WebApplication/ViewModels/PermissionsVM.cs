using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace United4Admin.WebApplication.ViewModels
{
    public class PermissionsVM 
    {
        public int PermissionsId { get; set; }
        [Display(Name = "World Vision Email Address")]
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [MaxLength(254, ErrorMessage = "Please don't enter more than 254 characters")]//the longest an email address can be
        public string WVEmail { get; set; }
        public bool Administrator { get; set; }
        [Display(Name = "Can Edit and Delete Supporter Data")]
        public bool EditDeleteSupporterData { get; set; }

        [Display(Name = "Can Create, Edit and Delete Events")]
        public bool CreateEditDeleteEvents { get; set; }

        [Display(Name = "Can Download Files and Images")]
        public bool DownloadFilesandImages { get; set; }

    }
}