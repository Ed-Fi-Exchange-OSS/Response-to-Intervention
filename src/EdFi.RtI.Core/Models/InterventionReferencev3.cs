using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class InterventionReferencev3
    {
        public int EducationOrganizationId { get; set; }
        public string InterventionIdentificationCode { get; set; }
        public LinkReferencev3 Link { get; set; }
    }
}
