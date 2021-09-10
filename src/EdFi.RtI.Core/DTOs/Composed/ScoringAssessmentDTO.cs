using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Composed
{
    public class ScoringAssessmentDTO
    {
        public StudentDTO Student { get; set; }
        public IEnumerable<StudentAssessmentAssociationDTO> Associations { get; set; }

        public class StudentAssessmentAssociationDTO
        {
            public StudentAssessment AssociationModel { get; set; }
            public Assessment Assessment { get; set; }
        }
    }
}
