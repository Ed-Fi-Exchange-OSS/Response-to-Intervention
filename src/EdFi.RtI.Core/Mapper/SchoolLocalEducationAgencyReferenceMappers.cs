using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SchoolLocalEducationAgencyReferenceMappers
    {
        public static SchoolLocalEducationAgencyReference MapToSchoolLocalEducationAgencyReference (this SchoolLocalEducationAgencyReferencev3 a)
        {
            return new SchoolLocalEducationAgencyReference
            {
                Link = null, //TODO Check v2
                LocalEducationAgencyId = a.LocalEducationAgencyId
            };
        }

        public static SchoolLocalEducationAgencyReferencev3 MapToSchoolLocalEducationAgencyReference(this SchoolLocalEducationAgencyReference a)
        {
            return new SchoolLocalEducationAgencyReferencev3
            {
                Id = null, //TODO Check v3
                LocalEducationAgencyId = a.LocalEducationAgencyId
            };
        }
    }
}
