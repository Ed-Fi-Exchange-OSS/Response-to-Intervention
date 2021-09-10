using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SchoolInstitutionTelephonesItemMappers
    {
        public static SchoolInstitutionTelephonesItem MapToSchoolInstitutionTelephonesItem (this SchoolInstitutionTelephonesItemv3 a)
        {
            return new SchoolInstitutionTelephonesItem
            {
                InstitutionTelephoneNumberType = a.InstitutionTelephoneNumberType,
                TelephoneNumber = a.TelephoneNumber
            };
        }

        public static SchoolInstitutionTelephonesItemv3 MapToSchoolInstitutionTelephonesItem(this SchoolInstitutionTelephonesItem a)
        {
            return new SchoolInstitutionTelephonesItemv3
            {
                InstitutionTelephoneNumberType = a.InstitutionTelephoneNumberType,
                TelephoneNumber = a.TelephoneNumber
            };
        }
    }
}
