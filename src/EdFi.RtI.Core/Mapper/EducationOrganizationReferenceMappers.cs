using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class EducationOrganizationReferenceMappers
    {
        public static InterventionEducationOrganizationReference MapToInterventionEducationOrganizationReference(this EducationOrganizationReferencev3 a)
        {
            return new InterventionEducationOrganizationReference
            {
                EducationOrganizationId = a.EducationOrganizationId,
                Link = a.Link?.MapToInterventionEducationOrganizationReferenceLink(),
            };
        }

        public static EducationOrganizationReferencev3 MapToEducationOrganizationReferencev3(this InterventionEducationOrganizationReference a)
        {
            return new EducationOrganizationReferencev3
            {
                EducationOrganizationId = a.EducationOrganizationId,
                Link = a.Link?.MapToLinkReferenvev3(),
            };
        }

        public static AssessmentContentStandardMandatingEducationOrganizationReference MapToAssessmentContentStandardMandatingEducationOrganizationReference(this EducationOrganizationReferencev3 a)
        {
            return new AssessmentContentStandardMandatingEducationOrganizationReference
            {
                EducationOrganizationId = a.EducationOrganizationId,
                Link = a.Link?.MapToAssessmentContentStandardMandatingEducationOrganizationReferenceLink(),
            };
        }

        public static EducationOrganizationReferencev3 MapToEducationOrganizationReferencev3(this AssessmentContentStandardMandatingEducationOrganizationReference a)
        {
            return new EducationOrganizationReferencev3
            {
                EducationOrganizationId = a.EducationOrganizationId,
                Link = a.Link?.MapToLinkReferencev3(),
            };
        }

        public static AssessmentEducationOrganizationReference MapToAssessmentEducationOrganizationReference(this EducationOrganizationReferencev3 a)
        {
            return new AssessmentEducationOrganizationReference
            {
                EducationOrganizationId = a.EducationOrganizationId,
                Link = a.Link?.MapToAssessmentEducationOrganizationReferenceLink(),
            };
        }

        public static EducationOrganizationReferencev3 MapToEducationOrganizationReferencev3(this AssessmentEducationOrganizationReference a)
        {
            return new EducationOrganizationReferencev3
            {
                EducationOrganizationId = a.EducationOrganizationId,
                Link = a.Link?.MapToLinkReferencev3(),
            };
        }
    }
}
