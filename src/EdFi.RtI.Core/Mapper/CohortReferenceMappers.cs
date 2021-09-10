using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class CohortReferenceMappers
    {
        public static StudentInterventionAssociationCohortReference MapToStudentInterventionAssociationCohortReference(this CohortReferencev3 a)
        {
            return new StudentInterventionAssociationCohortReference
            {
                EducationOrganizationId = a.EducationOrganizationId,
                Identifier = a.CohortIdentifier,
                Link = a.Link?.MapToStudentInterventionAssociationCohortReferenceLink(),
            };
        }

        public static CohortReferencev3 MapToCohortReferencev3(this StudentInterventionAssociationCohortReference a)
        {
            return new CohortReferencev3
            {
                CohortIdentifier = a.Identifier,
                EducationOrganizationId = a.EducationOrganizationId,
                Link = a.Link?.MapToLinkReferencev3(),
            };
        }
    }
}
