using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class SectionCourseOfferingReferencev3
    {
        public SectionCourseOfferingReferenceLinkv3 Link { get; set; }
        public string LocalCourseCode { get; set; }
        public int SchoolId { get; set; }
        public int SchoolYear { get; set; }
        public string TermDescriptor { get; set; }
    }
}
