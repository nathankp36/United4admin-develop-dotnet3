using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using United4Admin.WebApplication.BLL;

namespace United4Admin.WebApplication.ViewModels
{
    public class AddOnDonationVM
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string ChildID { get; set; }
        public Guid OrderGuid { get; set; }
        public bool Exported { get; set; }
        public DateTime DateOfRequest { get; set; }
        public string Message { get; set; }
        public string PhotoFileName { get; set; }

        [NotMapped]
        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [NotMapped]
        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [NotMapped]
        public string ExtractType { get; set; }
        [NotMapped]
        public string FieldDataType { get; set; }
    }
}
