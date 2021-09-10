using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SectionCourseOfferingReferenceLinkMappers
    {
        public static SectionCourseOfferingReferenceLink MapToSectionCourseOfferingReferenceLink(this SectionCourseOfferingReferenceLinkv3 a)
        {
            return new SectionCourseOfferingReferenceLink
            {
                 Href = a.Href,
                 Rel = a.Rel
            };
        }

        public static SectionCourseOfferingReferenceLinkv3 MapToSectionCourseOfferingReferenceLink(this SectionCourseOfferingReferenceLink a)
        {
            return new SectionCourseOfferingReferenceLinkv3
            {
                Href = a.Href,
                Rel = a.Rel
            };
        }
    }
}
