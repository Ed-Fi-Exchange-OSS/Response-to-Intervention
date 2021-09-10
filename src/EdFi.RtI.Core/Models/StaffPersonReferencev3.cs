using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StaffPersonReferencev3
    {
        public string personId { get; set; }
        public string sourceSystemDescriptor { get; set; }
        public StaffPersonReferenceLinkv3 Link { get; set; }
    }
}
