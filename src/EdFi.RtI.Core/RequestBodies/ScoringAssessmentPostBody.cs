using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.RequestBodies
{
    public class ScoringAssessmentPostBody
    {
        public string StudentUniqueId { get; set; }
        public string AssessmentId { get; set; }
        public string AdministrationDate { get; set; }
    }
}
