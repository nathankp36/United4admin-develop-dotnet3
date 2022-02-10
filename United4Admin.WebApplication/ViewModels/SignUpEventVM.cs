using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace United4Admin.WebApplication.ViewModels
{
    public class SignUpEventVM : IValidatableObject
    {
        public SignUpEventVM()
        {
            TypeOfRegistrationList = new List<string> { "WV Chosen Event", "Online" };
        }
        public int SignUpEventId { get; set; }

        [Display(Name = "Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "Please don't enter more than 50 characters")]
        public string EventName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Please don't enter more than 100 characters")]
        public string Location { get; set; }

        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "Publish Date")]
        [Required]
        public DateTime PublishDate { get; set; }

        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "Closed Date")]
        [Required]
        public DateTime ClosedDate { get; set; }

        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "Event Date")]
        [Required]
        public DateTime EventDate { get; set; }

        [Display(Name = "Type Of Registration")]
        [Required]
        public string TypeOfRegistration { get; set; }

        public IList<string> TypeOfRegistrationList { get; set; }

        [Display(Name = "Short URL")]
        [Required]
        [MaxLength(25, ErrorMessage = "Please don't enter more than 25 characters")]
        public string ShortURL { get; set; }



        [Display(Name = "Specific Choosing event")]
        [Required]
        public string SpecificChoosingEvent { get; set; }

        [Display(Name = "Related Choosing event")]
        public int? ChoosingPartyId { get; set; }

        public ChoosingPartyVM ChoosingParty{ get; set; }

        [Display(Name = "Related Reveal event")]
        public int? RevealEventId { get; set; }

        public RevealEventVM RevealEvent{ get; set; }
        
        [Required]
        [Display(Name = "Workflow Status")]        
        public int WorkflowStatusId { get; set; }
        public WorkflowStatusVM WorkflowStatus { get; set; }

        public IList<WorkflowStatusVM> WorkFlowStatuses { get; set; }

        [Display(Name = "Campaign Code")]
        [Required]
        [MaxLength(50, ErrorMessage = "Please don't enter more than 50 characters")]
        public string CampaignCode { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Notes Comments")]
        public string NotesComments { get; set; }

        public bool Create { get; set; }

        [Display(Name = "Will there be a WV photo booth at the event?")]
        public bool WVPhotoBooth { get; set; }

        public bool LinkedRegistrations { get; set; }

        public List<ChoosingPartyVM> ChoosingParties { get; set; }
        public List<RevealEventVM> RevealEvents { get; set; }
        public string DisplayName
        {
            get
            {
                return this.EventName + ", " + this.EventDate.ToShortDateString();
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Create && PublishDate < DateTime.Today)
            { yield return new ValidationResult("Publish Date cannot be in the past.", new List<string>() { "PublishDate" }); }
            if (ClosedDate < PublishDate)
            {
                yield return new ValidationResult("Closing Date cannot be before Publish Date.", new List<string>() { "ClosingDate" });
            }
        }
    }
}