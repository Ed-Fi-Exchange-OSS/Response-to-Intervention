using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class SectionSessionv3
    {
        public string Id { get; set; }
        public int SchoolId { get; set; }
        public int SchoolYear { get; set; }
        public string TermDescriptor { get; set; }
        public string SessioName { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public int TotalInstructionalDays { get; set; }
    }
}
