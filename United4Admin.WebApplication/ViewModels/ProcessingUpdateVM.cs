using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
    public class ProcessingUpdateVM
    {
        public int Id { get; set; }
        public DateTime ProcessingStarted { get; set; }
        public DateTime? ProcessingFinished { get; set; }
        public string Narrative { get; set; }
        public int RecordsProcessed { get; set; }
        public int RecordsSubmitted { get; set; }
        public int RecordsCreated { get; set; }
        public int RecordsKept { get; set; }
        public int RecordsNotDeleted { get; set; }
        public int RecordsDeleted { get; set; }
        public int TotalRecordsAfterUpdate { get; set; }
        public int RecordsNotInUpload { get; set; }
        public int ChildIdsNotPublishable { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
        public int ProjectRecordsCreated { get; set; }
        public int ProjectRecordsProcessed { get; set; }
        public string NewProjects { get; set; }
    }
}
