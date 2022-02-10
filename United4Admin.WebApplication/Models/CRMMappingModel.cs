using System;
using System.Collections.Generic;
using System.Text;

namespace United4Admin.WebApplication.Models
{
    public partial class CRMMappingModel
    {

        public NavisionModel Navision { get; set; }
        public SimmaModel Simma { get; set; }

        public partial class NavisionModel
        {
            public string salutation { get; set; }
            public string paymentMethodType { get; set; }
            public string paymentMethodName { get; set; }
            public string productVariantName { get; set; }
            public string incidentType { get; set; }
            public string purposeCode { get; set; }
            public string pledgeType { get; set; }
            public string incidentTypeName { get; set; }
            public string productCode { get; set; }
            public string countryCode { get; set; }
            public string frequency { get; set; }
        }

        public partial class SimmaModel
        {
            public int partnerTypeId { get; set; }
            public int motivationId { get; set; }
            public int designationId { get; set; }
            public int pledgeTypeId { get; set; }
            public int paymentMethodId { get; set; }
            public int emailTypeId { get; set; }
            public int phoneTypeId { get; set; }
            public int batchTypeId { get; set; }
            public string countryCode { get; set; }
            public string frequency { get; set; }
        }

    }
}

