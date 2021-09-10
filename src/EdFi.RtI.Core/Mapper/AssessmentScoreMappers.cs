using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentScoreMappers
    {
        public static AssessmentScoresItem MapToAssessmentScoresItem(this AssessmentScorev3 a)
        {
            return new AssessmentScoresItem
            {
                AssessmentReportingMethodType = a.AssessmentReportingMethodDescriptor?.MapToAssessmentReportingMethodDescriptorv2(),
                MaximumScore = a.MaximumScore,
                MinimumScore = a.MinimumScore,
                ResultDatatypeType = a.ResultDatatypeTypeDescriptor?.MapToResultDatatypeTypeDescriptorv2(),
            };
        }

        public static AssessmentScorev3 MapToAssessmentScorev3(this AssessmentScoresItem a)
        {
            return new AssessmentScorev3
            {
                AssessmentReportingMethodDescriptor = a.AssessmentReportingMethodType?.MapToAssessmentReportingMethodDescriptorv3(),
                MaximumScore = a.MaximumScore,
                MinimumScore = a.MinimumScore,
                ResultDatatypeTypeDescriptor = a.ResultDatatypeType?.MapToResultDatatypeTypeDescriptorv3(),
            };
        }
    }
}
