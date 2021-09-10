using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class InterventionClassDescriptorv3
    {
        public string Id { get; set; }
        public string InterventionClassDescriptorId { get; set; }
        public string CodeValue { get; set; }
        public string Description { get; set; }
        public string EffectiveBeginDate { get; set; }
        public string EffectiveEndDate { get; set; }
        public string Namespace { get; set; }
        public string PriorDescriptorId { get; set; }
        public string ShortDescription { get; set; }
    }
}
