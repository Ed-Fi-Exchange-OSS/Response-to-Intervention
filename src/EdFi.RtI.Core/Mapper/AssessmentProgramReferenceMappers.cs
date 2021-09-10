using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentProgramReferenceMappers
    {
        public static AssessmentProgramsItemProgramReference MapToAssessmentProgramsItemProgramReference(this ProgramReferencev3 a)
        {
            return new AssessmentProgramsItemProgramReference
            {
                EducationOrganizationId = a.EducationOrganizationId,
                Link = a.Link?.MapToAssessmentProgramsItemProgramReferenceLink(),
                Name = a.ProgramName,
                Type = a.ProgramTypeDescriptor,
            };
        }

        public static ProgramReferencev3 MapToProgramReferencev3(this AssessmentProgramsItemProgramReference a)
        {
            return new ProgramReferencev3
            {
                EducationOrganizationId = a.EducationOrganizationId,
                Link = a.Link?.MapToLinkReferencev3(),
                ProgramName = a.Name,
                ProgramTypeDescriptor = a.Type,
            };
        }
    }
}
