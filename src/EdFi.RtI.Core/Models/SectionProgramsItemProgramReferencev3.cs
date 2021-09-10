using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class SectionProgramsItemProgramReferencev3
    {
        public int EducationOrganizationId { get; set; }
        public SectionProgramsItemProgramReferenceLinkv3 Link { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
