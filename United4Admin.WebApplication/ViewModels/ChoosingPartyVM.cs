using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace United4Admin.WebApplication.ViewModels
{
    public class ChoosingPartyVM : IValidatableObject
    {
        public int ChoosingPartyId { get; set; }

        public int HorizonId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string PartyName { get; set; }

        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "Party Date")]        
        [Required]
        public DateTime PartyDate { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "Please don't enter more than 25 characters")]
        public string Location { get; set; }

        public string DisplayText { get; set; }

        [Required]
        [Display(Name = "Workflow Status")]        
        public int WorkflowStatusId { get; set; }
        public WorkflowStatusVM WorkflowStatus { get; set; }

        public IList<WorkflowStatusVM> WorkFlowStatuses { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Notes and Comments")]
        public string NotesComments { get; set; }

        public bool Create { get; set; }
        public bool LinkedRegistrations { get; set; }

        public string DisplayName
        {
            get
            {
                return this.HorizonId + ", " + this.Location + ", " + this.PartyDate.ToShortDateString();
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Create && PartyDate < DateTime.Today)
            { yield return new ValidationResult("Party Date cannot be in the past.", new List<string>() { "PartyDate" }); }

        }
    }
}