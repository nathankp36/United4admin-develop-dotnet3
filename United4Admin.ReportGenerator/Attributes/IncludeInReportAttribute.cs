using System;

namespace United4Admin.ExcelGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IncludeInReportAttribute : Attribute
    {
        public int Order { get; set; }
    }
}
