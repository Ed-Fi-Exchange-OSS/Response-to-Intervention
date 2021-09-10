using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StudentInterventionAssociationv3
    {
        public string Id { get; set; }
        public CohortReferencev3 CohortReference { get; set; }
        public InterventionReferencev3 InterventionReference { get; set; }
        public StudentReferencev3 StudentReference { get; set; }
        public int Dosage { get; set; }
        public IEnumerable<StudentInterventionAssociationInterventionEffectivenessv3> InterventionEffectivenesses { get; set; }
        public string _etag { get; set; }
    }
}
