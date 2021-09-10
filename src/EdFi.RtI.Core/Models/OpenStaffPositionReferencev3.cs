using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class OpenStaffPositionReferencev3
    {
        public int EducationOrganizationId { get; set; }
        public string RequisitionNumber { get; set; }
        public OpenStaffPositionReferenceLinkv3 Link { get; set; }
    }
}
