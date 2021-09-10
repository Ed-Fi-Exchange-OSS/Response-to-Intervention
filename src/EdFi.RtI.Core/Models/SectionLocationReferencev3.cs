using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class SectionLocationReferencev3
    {
        public string ClassroomIdentificationCode { get; set; }
        public SectionLocationReferenceLinkv3 Link { get; set; }
        public int SchoolId { get; set; }
    }
}
