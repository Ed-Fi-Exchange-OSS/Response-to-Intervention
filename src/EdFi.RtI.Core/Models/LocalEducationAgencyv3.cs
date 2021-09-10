using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class LocalEducationAgencyv3
    {
        public string Id { get; set; }
        public IList<EducationOrganizationCategoryv3> Categories { get; set; }
        public int LocalEducationAgencyId { get; set; }
        public string LocalEducationAgencyCategoryDescriptor { get; set; }
        public string NameOfInstitution { get; set; }
    }
}
