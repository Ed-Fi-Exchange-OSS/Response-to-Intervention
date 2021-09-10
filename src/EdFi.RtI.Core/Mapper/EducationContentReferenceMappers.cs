using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class EducationContentReferenceMappers
    {
        public static InterventionEducationContentsItemEducationContentReference MapToInterventionEducationContentsItemEducationContentReferenceV2(this EducationContentReferencev3 a)
        {
            return new InterventionEducationContentsItemEducationContentReference
            {
                ContentIdentifier = a.ContentIdentifier,
                Link = a.Link?.MapToInterventionEducationContentsItemEducationContentReferenceLink(),
            };
        }

        public static EducationContentReferencev3 MapToEducationContentReferenceV3(this InterventionEducationContentsItemEducationContentReference a)
        {
            return new EducationContentReferencev3
            {
                ContentIdentifier = a.ContentIdentifier,
                Link = a.Link?.MapToLinkReferencev3(),
            };
        }
    }
}
