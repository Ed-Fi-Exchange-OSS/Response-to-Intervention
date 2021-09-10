using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class Schoolv3
    {
        public string Id { get; set; }
        public IList<SchoolEducationOrganizationCategoriesItemv3> EducationOrganizationCategories { get; set; }
        public IList<SchoolGradeLevelsItemv3> GradeLevels { get; set; }
        public int SchoolId { get; set; }
        public IList<SchoolAddressesItemv3> Addresses { get; set; }
        public IList<SchoolIdentificationCodesItemv3> IdentificationCodes { get; set; }
        public IList<SchoolInstitutionTelephonesItemv3> InstitutionTelephones { get; set; }
        public SchoolLocalEducationAgencyReferencev3 LocalEducationAgencyReference { get; set; }
        public string NameOfInstitution { get; set; }
        public IList<SchoolSchoolCategoriesItemv3> SchoolCategories { get; set; }
        public string Type { get; set; }
        public string ShortNameOfInstitution { get; set; }
        public string WebSite { get; set; }
        public string _etag { get; set; }
    }
}
