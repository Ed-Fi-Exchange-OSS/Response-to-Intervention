using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StudentAssessmentItemMappers
    {
        public static StudentAssessmentItemsItem MapToStudentAssessmentItemsItem(this StudentAssessmentItemv3 a)
        {
            return new StudentAssessmentItemsItem
            {
                AssessmentItemReference = a.AssessmentItemReference?.MapToStudentAssessmentItemsItemAssessmentItemReference(),
                AssessmentItemResultType = a.AssessmentItemResultDescriptor?.MapToAssessmentItemResultDescriptorv2(),
                AssessmentResponse = a.AssessmentResponse,
                DescriptiveFeedback = a.DescriptiveFeedback,
                RawScoreResult = (int) a.RawScoreResult,
                ResponseIndicatorType = a.ResponseIndicatorDescriptor?.MapToResponseIndicatorDescriptorv2(),
                TimeAssessed = a.TimeAssessed,
            };
        }

        public static StudentAssessmentItemv3 MapToStudentAssessmentItemv3(this StudentAssessmentItemsItem a)
        {
            return new StudentAssessmentItemv3
            {
                AssessmentItemReference = a.AssessmentItemReference?.MapToAssessmentItemReferencev3(),
                AssessmentItemResultDescriptor = a.AssessmentItemResultType?.MapToAssessmentItemResultDescriptorv3(),
                AssessmentResponse = a.AssessmentResponse,
                DescriptiveFeedback = a.DescriptiveFeedback,
                RawScoreResult = a.RawScoreResult ?? 0.0,
                ResponseIndicatorDescriptor = a.ResponseIndicatorType?.MapToResponseIndicatorDescriptorv3(),
                TimeAssessed = a.TimeAssessed,
            };
        }
    }
}
