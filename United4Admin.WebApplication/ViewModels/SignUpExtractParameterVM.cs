using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class SignUpExtractParameterVM
    {
        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "Start Date")]
        public DateTime? SignStartDate { get; set; }

        [DataType(DataType.Text), UIHint("DatePicker"), Display(Name = "End Date")]
        public DateTime? SignEndDate { get; set; }

        public string SignExtractType { get; set; }

        public string SignFieldDataType { get; set; }

        public int SignChoosingPartyId { get; set; }
    }
}
