using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.RequestBodies
{
    public class ScoringAssessmentSearchFilters
    {
        public string Category { get; set; }
        public bool? ShowInactiveStudents { get; set; }
        public string UniqueSectionCode { get; set; }

        public ScoringAssessmentSearchFilters()
        {

        }

        public ScoringAssessmentSearchFilters(string uniqueSectionCode)
        {
            UniqueSectionCode = uniqueSectionCode;
        }
    }
}
