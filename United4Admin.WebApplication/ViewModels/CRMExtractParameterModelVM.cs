using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class CRMExtractParameterModelVM : IValidatableObject
    {
        public IFormFile CsvFile { get; set; }

        [Display(Name = "Title")]
        public string CRMType { get; set; }
        public bool Exported { get; set; } = true;

        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public string ExtractType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CsvFile == null)
            {
                yield return new ValidationResult("Please select a file");
            }
        }
    }
}
