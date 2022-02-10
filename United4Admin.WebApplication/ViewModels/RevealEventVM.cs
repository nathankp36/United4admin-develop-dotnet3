using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace United4Admin.WebApplication.ViewModels
{
    public class RevealEventVM : IValidatableObject
    {
        public RevealEventVM()
        {
            TypeOfRevealList = new List<string> {"WV Reveal Event", "Online" };
        }
        public int RevealEventId { get; set; }

        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "Event Date")]        
        public DateTime EventDate { get; set; }

        [Display(Name = "Event Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "Please don't enter more than 50 characters")]
        public string Name { get; set; }

        [Display(Name = "Text to be displayed")]
        public string DisplayText { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Please don't enter more than 100 characters")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Type of Reveal")]
        public string TypeOfReveal { get; set; }
        public IList<string> TypeOfRevealList { get; set; }

        [Required]
        [Display(Name = "Workflow Status")]        
        public int WorkflowStatusId { get; set; }
        public WorkflowStatusVM WorkflowStatus { get; set; }

        public bool Create { get; set; }
        public bool LinkedRegistrations { get; set; }
        public IList<WorkflowStatusVM> WorkFlowStatuses { get; set; }

        public string DisplayName
        {
            get
            {
                return this.Name + ", " + this.EventDate.ToShortDateString();
            }
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Create && EventDate < DateTime.Today)
            { yield return new ValidationResult("Event date cannot be in the past.", new List<string>() { "EventDate" }); }
        }
    }
}