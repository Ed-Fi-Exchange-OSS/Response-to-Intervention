using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StudentAssessmentScoreResultMappers
    {
        public static StudentAssessmentScoreResultsItem MapToStudentAssessmentScoreResultsItem(this StudentAssessmentScoreResultv3 a)
        {
            return new StudentAssessmentScoreResultsItem
            {
                AssessmentReportingMethodType = a.AssessmentReportingMethodDescriptor?.MapToAssessmentReportingMethodDescriptorv2(),
                Result = a.Result,
                ResultDatatypeType = a.ResultDatatypeTypeDescriptor?.MapToResultDatatypeTypeDescriptorv2(),
            };
        }

        public static StudentAssessmentScoreResultv3 MapToStudentAssessmentScoreResultv3(this StudentAssessmentScoreResultsItem a)
        {
            return new StudentAssessmentScoreResultv3
            {
                AssessmentReportingMethodDescriptor = a.AssessmentReportingMethodType?.MapToAssessmentReportingMethodDescriptorv3(),
                Result = a.Result,
                ResultDatatypeTypeDescriptor = a.ResultDatatypeType?.MapToResultDatatypeTypeDescriptorv3(),
            };
        }
    }
}
