using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Composed
{
    public class SectionsByStaffDTO
    {
        public string StaffId { get; set; }
        public IEnumerable<Ods.Api.Client.Models.Section> Sections { get; set; }
    }
}
