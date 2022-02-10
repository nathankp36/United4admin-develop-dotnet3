using System.ComponentModel;

namespace United4Admin.ExcelGenerator.Models
{
    public class ColumnValue
    {
        public int Order { get; set; }
        public string Path { get; set; }
        public PropertyDescriptor PropertyDescriptor { get; set; }
    }
}
