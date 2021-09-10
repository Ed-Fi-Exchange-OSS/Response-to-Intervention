using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SchoolAddressesItemMappers
    {
        public static SchoolAddressesItem MapToSchoolAddressesItem (this SchoolAddressesItemv3 a)
        {
            return new SchoolAddressesItem
            {
                AddressType = a.AddressType,
                ApartmentRoomSuiteNumber = null, //TODO Check v2
                BeginDate = null, //TODO Check v2
                BuildingSiteNumber = null, //TODO Check v2
                CountyFIPSCode = null, //TODO Check v2
                EndDate = null, //TODO Check v2
                Latitude = null, //TODO Check v2
                Longitude = null, //TODO Check v2
                City = a.City,
                NameOfCounty = a.NameOfCounty,
                PostalCode = a.PostalCode,
                StateAbbreviationType = a.StateAbbreviationType,
                StreetNumberName = a.StreetNumberName
            };
        }

        public static SchoolAddressesItemv3 MapToSchoolAddressesItem(this SchoolAddressesItem a)
        {
            return new SchoolAddressesItemv3
            {
                AddressType = a.AddressType,
                City = a.City,
                NameOfCounty = a.NameOfCounty,
                PostalCode = a.PostalCode,
                StateAbbreviationType = a.StateAbbreviationType,
                StreetNumberName = a.StreetNumberName
            };
        }
    }
}
