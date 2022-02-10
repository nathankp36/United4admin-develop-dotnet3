using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
	public class ChildInternalDataVM
	{
		public int Id { get; set; }

		[Display(Name = "Child Id")]
		public string ChildId { get; set; }

		[Display(Name = "Child Name")]
		public string Name { get; set; }

		public DateTime? SelectedDateTime { get; set; }

		public DateTime? SponsoredDateTime { get; set; }
		public bool HasHorizonData { get; set; }
		public bool HasCGPhoto { get; set; }
		public bool HasCGVideo { get; set; }

		public bool CannotDelete { get; set; }
		public bool CannotChangeStatus { get; set; }
	}
}
