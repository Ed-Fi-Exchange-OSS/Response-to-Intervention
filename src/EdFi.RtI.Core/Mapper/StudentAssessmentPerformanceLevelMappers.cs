using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StudentAssessmentPerformanceLevelMappers
    {
        public static StudentAssessmentPerformanceLevelsItem MapToStudentAssessmentPerformanceLevelsItem(this StudentAssessmentPerformanceLevelv3 a)
        {
            return new StudentAssessmentPerformanceLevelsItem
            {
                AssessmentReportingMethodType = a.AssessmentReportingMethodDescriptor?.MapToAssessmentReportingMethodDescriptorv2(),
                PerformanceLevelDescriptor = a.PerformanceLevelDescriptor?.MapToPerformanceLevelDescriptorv2(),
                PerformanceLevelMet = a.PerformanceLevelMet,
            };
        }

        public static StudentAssessmentPerformanceLevelv3 MapToStudentAssessmentPerformanceLevelv3(this StudentAssessmentPerformanceLevelsItem a)
        {
            return new StudentAssessmentPerformanceLevelv3
            {
                AssessmentReportingMethodDescriptor = a.AssessmentReportingMethodType?.MapToAssessmentReportingMethodDescriptorv3(),
                PerformanceLevelDescriptor = a.PerformanceLevelDescriptor?.MapToPerformanceLevelDescriptorv3(),
                PerformanceLevelMet = a.PerformanceLevelMet,
            };
        }
    }
}
