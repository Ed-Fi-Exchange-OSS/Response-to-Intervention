using EdFi.Ods.Api.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Composed
{
    public class StaffsBySchoolDetails
    {
        public School School { get; set; }
        public IEnumerable<Staff> Staffs { get; set; }
    }
}
