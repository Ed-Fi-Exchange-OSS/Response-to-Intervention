using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AcademicSubjectMappers
    {
        public static AcademicSubjectDescriptorv3 MapToAcademicSubjectDescriptorv3(this AssessmentAcademicSubjectsItem a)
        {
            return new AcademicSubjectDescriptorv3
            {
                AcademicSubjectDescriptor = a.AcademicSubjectDescriptor?.MapToAcademicSubjectDescriptorV3(),
            };
        }

        public static AssessmentAcademicSubjectsItem MapToAssessmentAcademicSubjectsItem(this AcademicSubjectDescriptorv3 a)
        {
            return new AssessmentAcademicSubjectsItem
            {
                AcademicSubjectDescriptor = a.AcademicSubjectDescriptor?.MapToAcademicSubjectDescriptorV2(),
            };
        }
    }
}
