using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StaffClassificationDescriptorv3
    {
        public string Id { get; set; }
        public int? StaffClassificationDescriptorId { get; set; }
        public string CodeValue { get; set; }
        public string Description { get; set; }
        public DateTime? EffectiveBeginDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string NamespaceProperty { get; set; }
        public int? PriorDescriptorId { get; set; }
        public string ShortDescription { get; set; }
        public string _etag { get; set; }
    }
}
