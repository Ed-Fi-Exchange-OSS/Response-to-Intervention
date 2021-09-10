using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SectionClassPeriodMappers
    {
        public static SectionClassPeriodReference MapToSectionClassPeriodReference(this SectionClassPeriodReferencev3 a)
        {
            return new SectionClassPeriodReference
            {
                Link = a.Link?.MapToSectionClassPeriodReferenceLink(),
                Name = a.ClassPeriodName,
                SchoolId = a.SchoolId,
            };
        }

        public static SectionClassPeriodReferencev3 MapToSectionClassPeriodReferencev3(this SectionClassPeriodReference a)
        {
            return new SectionClassPeriodReferencev3
            {
                ClassPeriodName = a.Name,
                Link = a.Link?.MapToLinkReferencev3(),
                SchoolId = a.SchoolId,
            };
        }
    }
}
