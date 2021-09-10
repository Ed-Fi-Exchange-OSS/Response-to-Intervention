using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class InterventionPrescriptionReferencev3
    {
        public int EducationOrganizationId { get; set; }
        public string InterventionPrescriptionIdentificationCode { get; set; }
        public LinkReferencev3 Link { get; set; }
    }
}
