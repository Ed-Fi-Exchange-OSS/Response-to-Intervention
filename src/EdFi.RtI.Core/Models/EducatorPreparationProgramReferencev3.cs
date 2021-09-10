using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class EducatorPreparationProgramReferencev3
    {
        public int educationOrganizationId { get; set; }
        public string ProgramName { get; set; }
        public string programTypeDescriptor { get; set; }
        public EducatorPreparationProgramReferenceLinkv3 Link { get; set; }
    }
}
