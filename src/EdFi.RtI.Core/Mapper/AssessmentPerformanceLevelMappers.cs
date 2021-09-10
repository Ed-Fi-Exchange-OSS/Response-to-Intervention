using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentPerformanceLevelMappers
    {
        public static AssessmentPerformanceLevelsItem MapToAssessmentPerformanceLevelsItem(this AssessmentPerformanceLevelv3 a)
        {
            return new AssessmentPerformanceLevelsItem
            {
                AssessmentReportingMethodType = a.AssessmentReportingMethodDescriptor?.MapToAssessmentReportingMethodDescriptorv2(),
                MaximumScore = a.MaximumScore,
                MinimumScore = a.MinimumScore,
                PerformanceLevelDescriptor = a.PerformanceLevelDescriptor?.MapToGradeLevelDescriptorv2(),
                ResultDatatypeType = a.ResultDatatypeTypeDescriptor?.MapToResultDatatypeTypeDescriptorv2(),
            };
        }

        public static AssessmentPerformanceLevelv3 MapToAssessmentPerformanceLevelv3(this AssessmentPerformanceLevelsItem a)
        {
            return new AssessmentPerformanceLevelv3
            {
                AssessmentReportingMethodDescriptor = a.AssessmentReportingMethodType?.MapToAssessmentReportingMethodDescriptorv3(),
                MaximumScore = a.MaximumScore,
                MinimumScore = a.MinimumScore,
                PerformanceLevelDescriptor = a.PerformanceLevelDescriptor?.MapToPerformanceLevelDescriptorv3(),
                ResultDatatypeTypeDescriptor = a.ResultDatatypeType?.MapToResultDatatypeTypeDescriptorv3(),
            };
        }
    }
}
