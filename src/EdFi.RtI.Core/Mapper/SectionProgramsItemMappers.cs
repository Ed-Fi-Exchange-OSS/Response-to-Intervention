using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SectionProgramsItemMappers
    {

        public static SectionProgramsItem MapToSectionProgramsItem (this SectionProgramsItemv3 a)
        {
            return new SectionProgramsItem
            {
                ProgramReference = a.ProgramReference?.MapToSectionProgramsItemProgramReference()
            };
        }

        public static SectionProgramsItemv3 MapToSectionProgramsItem(this SectionProgramsItem a)
        {
            return new SectionProgramsItemv3
            {
                ProgramReference = a.ProgramReference?.MapToSectionProgramsItemProgramReference()
            };
        }
    }
}
