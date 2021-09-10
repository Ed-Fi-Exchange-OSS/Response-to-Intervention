using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SectionCourseOfferingReferenceMappers
    {
        public static SectionCourseOfferingReference MapToSectionCourseOfferingReference(this SectionCourseOfferingReferencev3 a)
        {
            return new SectionCourseOfferingReference
            {
                Link = a.Link?.MapToSectionCourseOfferingReferenceLink(),
                LocalCourseCode = a.LocalCourseCode,
                SchoolId = a.SchoolId,
                SchoolYear = a.SchoolYear,
                TermDescriptor = a.TermDescriptor
            };
        }

        public static SectionCourseOfferingReferencev3 MapToSectionCourseOfferingReference(this SectionCourseOfferingReference a)
        {
            return new SectionCourseOfferingReferencev3
            {
                Link = a.Link?.MapToSectionCourseOfferingReferenceLink(),
                LocalCourseCode = a.LocalCourseCode,
                SchoolId = a.SchoolId,
                SchoolYear = a.SchoolYear,
                TermDescriptor = a.TermDescriptor
            };
        }
    }
}
