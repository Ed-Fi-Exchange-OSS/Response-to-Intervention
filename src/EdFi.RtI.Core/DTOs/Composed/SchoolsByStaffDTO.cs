using EdFi.Ods.Api.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Composed
{
    public class SchoolsByStaffDTO
    {
        public int StaffUniqueId { get; set; }
        public IEnumerable<School> Schools { get; set; }
    }
}
