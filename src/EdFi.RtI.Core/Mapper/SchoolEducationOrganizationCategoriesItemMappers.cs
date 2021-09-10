using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SchoolEducationOrganizationCategoriesItemMappers
    {
        public static SchoolEducationOrganizationCategoriesItem MapToSchoolEducationOrganizationCategoriesItem (this SchoolEducationOrganizationCategoriesItemv3 a)
        {
            return new SchoolEducationOrganizationCategoriesItem
            {
                Type = a.Type
            };
        }

        public static SchoolEducationOrganizationCategoriesItemv3 MapToSchoolEducationOrganizationCategoriesItem(this SchoolEducationOrganizationCategoriesItem a)
        {
            return new SchoolEducationOrganizationCategoriesItemv3
            {
                Type = a.Type
            };
        }
    }
}
