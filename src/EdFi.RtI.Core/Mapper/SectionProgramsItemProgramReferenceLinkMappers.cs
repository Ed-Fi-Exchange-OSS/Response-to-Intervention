using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SectionProgramsItemProgramReferenceLinkMappers
    {
        public static SectionProgramsItemProgramReferenceLink MapToSectionProgramsItemProgramReferenceLink (this SectionProgramsItemProgramReferenceLinkv3 a)
        {
            return new SectionProgramsItemProgramReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel
            };
        }

        public static SectionProgramsItemProgramReferenceLinkv3 MapToSectionProgramsItemProgramReferenceLink(this SectionProgramsItemProgramReferenceLink a)
        {
            return new SectionProgramsItemProgramReferenceLinkv3
            {
                Href = a.Href,
                Rel = a.Rel
            };
        }
    }
}
