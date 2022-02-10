using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace United4Admin.WebApplication.ViewModels
{
    public class ImageInfoVM
    {
        public int ImageInfoId { get; set; }
      
        public int ChosenSignUpId { get; set; }

        public string BlobGUID { get; set; }
        public SignUpVM ChosenSignUp { get; set; }

        public DateTime UploadDateTime { get; set; }
        public int ImageStatusId { get; set; }
        public string ImageURL { get; set; }
        [NotMapped]
        public string FirstName { get; set; }
        [NotMapped]
        public string LastName { get; set; }
        [NotMapped]
        public string Country { get; set; }
    }
}