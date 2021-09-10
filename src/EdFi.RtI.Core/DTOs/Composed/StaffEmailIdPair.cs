using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Composed
{
    public class StaffEmailIdPair
    {
        public IEnumerable<string> Emails { get; set; }
        public string Id { get; set; }
        public string StaffUniqueId { get; set; }
    }
}
