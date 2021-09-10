using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SectionLocationReferenceMappers
    {
        public static SectionLocationReference MapToSectionLocationReference(this SectionLocationReferencev3 a)
        {
            return new SectionLocationReference
            {
                ClassroomIdentificationCode = a.ClassroomIdentificationCode,
                Link = a.Link?.MapToSectionLocationReferenceLink(),
                SchoolId = a.SchoolId
            };
        }

        public static SectionLocationReferencev3 MapToSectionLocationReference(this SectionLocationReference a)
        {
            return new SectionLocationReferencev3
            {
                ClassroomIdentificationCode = a.ClassroomIdentificationCode,
                Link = a.Link?.MapToSectionLocationReferenceLink(),
                SchoolId = a.SchoolId
            };
        }
    }
}
