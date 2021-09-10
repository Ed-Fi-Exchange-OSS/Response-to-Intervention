using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentSectionReferenceMappers
    {
        public static AssessmentSectionsItemSectionReference MapToAssessmentSectionsItemSectionReference(this SectionReferencev3 a)
        {
            return new AssessmentSectionsItemSectionReference
            {
                ClassPeriodName = null, // TODO Check
                ClassroomIdentificationCode = null, // TODO Check
                Link = a.Link?.MapToAssessmentSectionsItemSectionReferenceLink(),
                LocalCourseCode = a.LocalSourceCode,
                SchoolId = a.SchoolId,
                SchoolYear = a.SchoolYear,
                SequenceOfCourse = 0, // TODO Check
                TermDescriptor = null, // TODO Check
                UniqueSectionCode = null, // TODO Check
            };
        }

        public static SectionReferencev3 MapToSectionReferencev3(this AssessmentSectionsItemSectionReference a)
        {
            return new SectionReferencev3
            {
                Link = a.Link?.MapToLinkReferencev3(),
                LocalSourceCode = a.LocalCourseCode,
                SchoolId = a.SchoolId,
                SchoolYear = a.SchoolYear,
                SectionIdentifier = null, // TODO Check
                SessionName = null, // TODO Check
            };
        }
    }
}
