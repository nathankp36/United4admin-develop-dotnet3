using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.ViewModels
{
	public class ChildInternalDataPagedVM
	{
		public int TotalNumber { get; set; }
		public int TotalPages { get; set; }
		public int CurrentCount { get; set; }
		public bool PreviousPage { get; set; }
		public bool NextPage { get; set; }

		public int PageSize { get; set; }

		public int CurrentPage { get; set; }
		public ICollection<ChildInternalDataVM> ChildInternalDataModels { get; set; }
		
	}

}
