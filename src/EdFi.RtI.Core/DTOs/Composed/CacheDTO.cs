using EdFi.Ods.Api.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Composed
{
    public class CacheDTO
    {
        public IEnumerable<School> Schools { get; set; }
        public IEnumerable<Staff> Staffs { get; set; }
        public IEnumerable<Ods.Api.Client.Models.Section> Sections { get; set; }
        public object Categories { get; set; }
        public IEnumerable<Ods.Api.Client.Models.Student> Students { get; set; }
    }
}
