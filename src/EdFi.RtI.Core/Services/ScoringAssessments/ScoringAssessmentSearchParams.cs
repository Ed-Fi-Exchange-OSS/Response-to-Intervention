using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.ScoringAssessments
{
    public class ScoringAssessmentSearchParams : SearchParams
    {
        public string Category { get; set; }
        public bool? OnlyFromCurrentNamespace { get; set; }
        public bool? ShowInactiveStudents { get; set; }
        public string UniqueSectionCode { get; set; }

        public new ScoringAssessmentSearchParams Default()
        {
            return new ScoringAssessmentSearchParams
            {
                GetFromCache = true,
                PageIndex = 1,
                PageSize = 10,
                StoreInCache = true,
                Category = "All",
            };
        }
    }
}
