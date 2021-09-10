using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.RequestBodies
{
    public class ScoringInterventionSearchFilters
    {
        public string SectionId { get; set; }
        public bool? ShowInactiveStudents { get; set; }

        public ScoringInterventionSearchFilters()
        {

        }

        public ScoringInterventionSearchFilters(string sectionId)
        {
            SectionId = sectionId;
        }
    }
}
