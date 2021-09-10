using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentSectionMappers
    {
        public static AssessmentSectionsItem MapToAssessmentSectionsItem(this AssessmentSectionv3 a)
        {
            return new AssessmentSectionsItem
            {
                SectionReference = a.SectionReference?.MapToAssessmentSectionsItemSectionReference(),
            };
        }

        public static AssessmentSectionv3 MapToAssessmentSectionv3(this AssessmentSectionsItem a)
        {
            return new AssessmentSectionv3
            {
                SectionReference = a.SectionReference?.MapToSectionReferencev3(),
            };
        }
    }
}
