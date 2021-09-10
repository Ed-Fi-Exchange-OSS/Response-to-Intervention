using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SchoolSchoolCategoriesItemMappers
    {
        public static SchoolSchoolCategoriesItem MapToSchoolSchoolCategoriesItem (this SchoolSchoolCategoriesItemv3 a)
        {
            return new SchoolSchoolCategoriesItem
            {
                Type = a.Type
            };
        }

        public static SchoolSchoolCategoriesItemv3 MapToSchoolSchoolCategoriesItem(this SchoolSchoolCategoriesItem a)
        {
            return new SchoolSchoolCategoriesItemv3
            {
                Type = a.Type
            };
        }
    }
}
