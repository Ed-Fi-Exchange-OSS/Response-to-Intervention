using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.Services.ScoringAssessments;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.RequestBodies
{
    public class AssessmentScoringFiltersAndStudentAssessmentsDTO
    {
        public ScoringAssessmentSearchParams SearchParams { get; set; }
        public IEnumerable<ScoringAssessmentDTO> Scorings { get; set; }
    }
}
