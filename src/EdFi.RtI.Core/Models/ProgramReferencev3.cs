using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class ProgramReferencev3
    {
        public int EducationOrganizationId { get; set; }
        public string ProgramName { get; set; }
        public string ProgramTypeDescriptor { get; set; }
        public LinkReferencev3 Link { get; set; }
    }
}
