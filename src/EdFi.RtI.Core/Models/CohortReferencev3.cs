using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class CohortReferencev3
    {
        public string CohortIdentifier { get; set; }
        public int EducationOrganizationId { get; set; }
        public LinkReferencev3 Link { get; set; }
    }
}
