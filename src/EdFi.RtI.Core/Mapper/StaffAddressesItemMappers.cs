using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StaffAddressesItemMappers
    {
        public static StaffAddressesItem MapToStaffAddressesItem(this StaffAddressesItemv3 a)
        {
            return new StaffAddressesItem
            {
                AddressType = a.AddressType,
                ApartmentRoomSuiteNumber = a.ApartmentRoomSuiteNumber,
                BeginDate = null,
                BuildingSiteNumber = a.BuildingSiteNumber,
                City = a.City,
                CountyFIPSCode = a.CountyFIPSCode,
                EndDate = null,
                Latitude = a.Latitude,
                Longitude = a.Longitude,
                NameOfCounty = a.NameOfCounty,
                PostalCode = a.PostalCode,
                StateAbbreviationType = a.StateAbbreviationType,
                StreetNumberName = a.StreetNumberName
            };
        }

        public static StaffAddressesItemv3 MapToStaffAddressesItem(this StaffAddressesItem a)
        {
            return new StaffAddressesItemv3
            {
                AddressType = a.AddressType,
                ApartmentRoomSuiteNumber = a.ApartmentRoomSuiteNumber,
                BuildingSiteNumber = a.BuildingSiteNumber,
                City = a.City,
                CountyFIPSCode = a.CountyFIPSCode,
                Latitude = a.Latitude,
                Longitude = a.Longitude,
                NameOfCounty = a.NameOfCounty,
                PostalCode = a.PostalCode,
                StateAbbreviationType = a.StateAbbreviationType,
                StreetNumberName = a.StreetNumberName,
                CongressionalDistrict = null,
                DoNotPublishIndicator = false,
                LocaleDescriptor = null,
                Periods = new List<StaffAddressPeriodv3>
                {
                    new StaffAddressPeriodv3
                    {
                        EndDate = a.EndDate ?? DateTime.Parse("2000-01-01"),
                        BeginDate = a.BeginDate ?? DateTime.Parse("2000-01-01")
                    },
                },
            };
        }
    }
}
