using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class UploadCsvVM : IValidatableObject
    {
        public IFormFile CsvFile { get; set; }

        [DisplayName("Is the first row a header row in this file?")]
        public bool HeaderRow { get; set; }
        public string ErrorMessage { get; set; }
        public bool Error { get; set; }
        public string ConfirmationMessage { get; set; }
        public int ProgressId { get; set; }
        public bool ShowInputs { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CsvFile == null)
            {
                yield return new ValidationResult("Please select a file");
            }
        }

    }
}
