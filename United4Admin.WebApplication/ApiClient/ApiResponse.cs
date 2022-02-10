using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace United4Admin.WebApplication.APIClient
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object ResponseObject { get; set; }
    }
}
