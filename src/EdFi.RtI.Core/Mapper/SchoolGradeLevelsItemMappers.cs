using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SchoolGradeLevelsItemMappers
    {
        public static SchoolGradeLevelsItem MapToSchoolGradeLevelsItem (this SchoolGradeLevelsItemv3 a)
        {
            return new SchoolGradeLevelsItem
            {
                GradeLevelDescriptor = a.GradeLevelDescriptor
            };
        }

        public static SchoolGradeLevelsItemv3 MapToSchoolGradeLevelsItem(this SchoolGradeLevelsItem a)
        {
            return new SchoolGradeLevelsItemv3
            {
                GradeLevelDescriptor = a.GradeLevelDescriptor
            };
        }
    }
}
