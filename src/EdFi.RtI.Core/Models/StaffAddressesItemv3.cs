using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StaffAddressesItemv3
    {
        public string AddressType { get; set; }
        public string StateAbbreviationType { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string StreetNumberName { get; set; }
        public string LocaleDescriptor { get; set; }
        public string ApartmentRoomSuiteNumber { get; set; }
        public string BuildingSiteNumber { get; set; }
        public string CongressionalDistrict { get; set; }
        public string CountyFIPSCode { get; set; }
        public bool DoNotPublishIndicator { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string NameOfCounty { get; set; }
        public IList<StaffAddressPeriodv3> Periods { get; set; }
    }
}
