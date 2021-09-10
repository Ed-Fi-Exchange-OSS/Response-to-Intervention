using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Composed
{
    public class StudentInterventionsDTO
    {
        public StudentDTO Student { get; set; }
        public IEnumerable<StudentInterventionAssociationDTO> Associations { get; set; }

        public class StudentInterventionAssociationDTO
        {
            public StudentInterventionAssociation AssociationModel { get; set; }
            public Intervention Intervention { get; set; }
        }
    }
}
