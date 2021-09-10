using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class EducationContentMappers
    {
        public static InterventionEducationContentsItem MapToInterventionEducationContentsItemV2(this EducationContentv3 a)
        {
            return new InterventionEducationContentsItem
            {
                EducationContentReference = a.EducationContentReference?.MapToInterventionEducationContentsItemEducationContentReferenceV2(),
            };
        }

        public static EducationContentv3 MapToEducationContentV3(this InterventionEducationContentsItem a)
        {
            return new EducationContentv3
            {
                EducationContentReference = a.EducationContentReference?.MapToEducationContentReferenceV3(),
            };
        }
    }
}
