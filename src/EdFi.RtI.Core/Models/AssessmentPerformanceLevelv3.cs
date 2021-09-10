using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class AssessmentPerformanceLevelv3
    {
        public string AssessmentReportingMethodDescriptor { get; set; }
        public string PerformanceLevelDescriptor { get; set; }
        public string ResultDatatypeTypeDescriptor { get; set; }
        public string MaximumScore { get; set; }
        public string MinimumScore { get; set; }
    }
}
