using EdFi.Ods.Api.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Composed
{
    public class StudentsBySectionDTO
    {
        public string SectionId { get; set; }
        public IEnumerable<Ods.Api.Client.Models.Student> Students { get; set; }
    }
}
