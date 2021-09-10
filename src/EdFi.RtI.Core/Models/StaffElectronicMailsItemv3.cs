using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StaffElectronicMailsItemv3
    {
        public string ElectronicMailAddress { get; set; }
        public string ElectronicMailType { get; set; }
        public bool DoNotPublishIndicator { get; set; }
        public bool? PrimaryEmailAddressIndicator { get; set; }
    }
}
