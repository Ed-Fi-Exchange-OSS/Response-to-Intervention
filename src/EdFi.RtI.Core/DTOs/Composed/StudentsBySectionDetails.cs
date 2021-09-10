using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DTOs.Section;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Composed
{
    public class StudentsBySectionDetails
    {
        public Ods.Api.Client.Models.Section Section { get; set; }
        public IEnumerable<Ods.Api.Client.Models.Student> Students { get; set; }
    }
}
