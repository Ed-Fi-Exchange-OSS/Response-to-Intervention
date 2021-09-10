using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SectionProgramsItemProgramReferenceMappers
    {
        public static SectionProgramsItemProgramReference MapToSectionProgramsItemProgramReference (this SectionProgramsItemProgramReferencev3 a)
        {
            return new SectionProgramsItemProgramReference
            {
                 EducationOrganizationId = a.EducationOrganizationId,
                 Link = a.Link?.MapToSectionProgramsItemProgramReferenceLink(),
                 Name = a.Name,
                 Type = a.Type
            };
        }

        public static SectionProgramsItemProgramReferencev3 MapToSectionProgramsItemProgramReference(this SectionProgramsItemProgramReference a)
        {
            return new SectionProgramsItemProgramReferencev3
            {
                EducationOrganizationId = a.EducationOrganizationId,
                Link = a.Link?.MapToSectionProgramsItemProgramReferenceLink(),
                Name = a.Name,
                Type = a.Type
            };
        }
    }
}
