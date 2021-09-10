using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StaffInternationalAddressesItemv3
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressType { get; set; }
        public DateTime? BeginDate { get; set; }
        public string CountryDescriptor { get; set; }
        public DateTime? EndDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
