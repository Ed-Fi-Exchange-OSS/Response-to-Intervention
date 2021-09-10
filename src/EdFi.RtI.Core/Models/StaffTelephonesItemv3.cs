using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StaffTelephonesItemv3
    {
        public int OrderOfPriority { get; set; }
        public string TelephoneNumber { get; set; }
        public string TelephoneNumberType { get; set; }
        public bool TextMessageCapabilityIndicator { get; set; }
        public bool DoNotPublishIndicator { get; set; }
    }
}
