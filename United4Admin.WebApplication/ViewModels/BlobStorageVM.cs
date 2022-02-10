using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace United4Admin.WebApplication.ViewModels
{
    public class BlobStorageVM
    {
        public string StorageContainer { get; set; }
        public string StorageAccountKey { get; set; }
        public string StorageAccountName { get; set; }
    }
}