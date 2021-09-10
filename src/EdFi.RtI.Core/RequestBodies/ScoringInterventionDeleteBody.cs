using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.RequestBodies
{
    public class ScoringInterventionDeleteBody
    {
        public string StudentInterventionAssociationId { get; set; }
        public string StudentUniqueId { get; set; }
        public string InterventionIdentificationCode { get; set; }
    }
}
