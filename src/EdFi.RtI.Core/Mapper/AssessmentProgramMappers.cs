using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentProgramMappers
    {
        public static AssessmentProgramsItem MapToAssessmentProgramsItem(this AssessmentProgramv3 a)
        {
            return new AssessmentProgramsItem
            {
                ProgramReference = a.ProgramReference?.MapToAssessmentProgramsItemProgramReference(),
            };
        }

        public static AssessmentProgramv3 MapToAssessmentProgramv3(this AssessmentProgramsItem a)
        {
            return new AssessmentProgramv3
            {
                ProgramReference = a.ProgramReference?.MapToProgramReferencev3(),
            };
        }
    }
}
