using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class InterventionReferenceMappers
    {
        public static StudentInterventionAssociationInterventionReference MapToStudentInterventionAssociationInterventionReference(this InterventionReferencev3 a)
        {
            return new StudentInterventionAssociationInterventionReference
            {
                EducationOrganizationId = a.EducationOrganizationId,
                IdentificationCode = a.InterventionIdentificationCode,
                Link = a.Link?.MapToStudentInterventionAssociationInterventionReferenceLink(),
            };
        }

        public static InterventionReferencev3 MapToInterventionReferencev3(this StudentInterventionAssociationInterventionReference a)
        {
            return new InterventionReferencev3
            {
                EducationOrganizationId = a.EducationOrganizationId,
                InterventionIdentificationCode = a.IdentificationCode,
                Link = a.Link?.MapToLinkReferencev3(),
            };
        }
    }
}
