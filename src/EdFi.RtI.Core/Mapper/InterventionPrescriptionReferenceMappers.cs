using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class InterventionPrescriptionReferenceMappers
    {
        public static InterventionInterventionPrescriptionsItemInterventionPrescriptionReference MapToInterventionInterventionPrescriptionsItemInterventionPrescriptionReference(this InterventionPrescriptionReferencev3 a)
        {
            return new InterventionInterventionPrescriptionsItemInterventionPrescriptionReference
            {
                EducationOrganizationId = a.EducationOrganizationId,
                IdentificationCode = a.InterventionPrescriptionIdentificationCode,
                Link = a.Link?.MapToInterventionInterventionPrescriptionsItemInterventionPrescriptionReferenceLink(),
            };
        }

        public static InterventionPrescriptionReferencev3 MapToInterventionPrescriptionv3(this InterventionInterventionPrescriptionsItemInterventionPrescriptionReference a)
        {
            return new InterventionPrescriptionReferencev3
            {
                EducationOrganizationId = a.EducationOrganizationId,
                InterventionPrescriptionIdentificationCode = a.IdentificationCode,
                Link = a.Link.MapToLinkReferencev3(),
            };
        }
    }
}
