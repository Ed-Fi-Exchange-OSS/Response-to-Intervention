using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SectionLocationReferenceLinkMappers
    {
        public static SectionLocationReferenceLink MapToSectionLocationReferenceLink(this SectionLocationReferenceLinkv3 a)
        {
            return new SectionLocationReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel
            };
        }

        public static SectionLocationReferenceLinkv3 MapToSectionLocationReferenceLink(this SectionLocationReferenceLink a)
        {
            return new SectionLocationReferenceLinkv3
            {
                Href = a.Href,
                Rel = a.Rel
            };
        }
    }
}
