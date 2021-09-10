using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class SectionReferencev3
    {
        public string LocalSourceCode { get; set; }
        public int SchoolId { get; set; }
        public int SchoolYear { get; set; }
        public string SectionIdentifier { get; set; }
        public string SessionName { get; set; }
        public LinkReferencev3 Link { get; set; }
    }
}
