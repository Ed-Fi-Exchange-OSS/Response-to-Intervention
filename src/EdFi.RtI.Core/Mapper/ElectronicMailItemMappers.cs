using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class ElectronicMailItemMappers
    {
        public static StaffElectronicMailsItemv3 MapToStaffElectronicMailsItemV3(this StaffElectronicMailsItem a)
        {
            return new StaffElectronicMailsItemv3
            {
                ElectronicMailAddress = a.ElectronicMailAddress,
                ElectronicMailType = a.ElectronicMailType,
                PrimaryEmailAddressIndicator = a.PrimaryEmailAddressIndicator,
            };
        }

        public static StaffElectronicMailsItem MapToStaffElectronicMailsItemV2(this StaffElectronicMailsItemv3 a)
        {
            return new StaffElectronicMailsItem
            {
                ElectronicMailAddress = a.ElectronicMailAddress,
                ElectronicMailType = a.ElectronicMailType,
                PrimaryEmailAddressIndicator = a.PrimaryEmailAddressIndicator,
            };
        }
    }
}
