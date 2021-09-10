using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SchoolIdentificationCodesItemMappers
    {
        public static SchoolIdentificationCodesItem MapToSchoolIdentificationCodesItem (this SchoolIdentificationCodesItemv3 a)
        {
            return new SchoolIdentificationCodesItem
            {
                EducationOrganizationIdentificationSystemDescriptor = a.EducationOrganizationIdentificationSystemDescriptor,
                IdentificationCode = a.IdentificationCode
            };
        }

        public static SchoolIdentificationCodesItemv3 MapToSchoolIdentificationCodesItem(this SchoolIdentificationCodesItem a)
        {
            return new SchoolIdentificationCodesItemv3
            {
                EducationOrganizationIdentificationSystemDescriptor = a.EducationOrganizationIdentificationSystemDescriptor,
                IdentificationCode = a.IdentificationCode
            };
        }
    }
}
