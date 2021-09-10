using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentItemReferenceMappers
    {
        public static StudentAssessmentItemsItemAssessmentItemReference MapToStudentAssessmentItemsItemAssessmentItemReference(this AssessmentItemReferencev3 a)
        {
            return new StudentAssessmentItemsItemAssessmentItemReference
            {
                AssessmentIdentifier = a.AssessmentIdentifier,
                IdentificationCode = a.IdentificationCode,
                Link = a.Link?.MapToStudentAssessmentItemsItemAssessmentItemReferenceLink(),
                NamespaceProperty = a.Namespace,
            };
        }

        public static AssessmentItemReferencev3 MapToAssessmentItemReferencev3(this StudentAssessmentItemsItemAssessmentItemReference a)
        {
            return new AssessmentItemReferencev3
            {
                AssessmentIdentifier = a.AssessmentIdentifier,
                IdentificationCode = a.IdentificationCode,
                Link = a.Link?.MapToLinkReferencev3(),
                Namespace = a.NamespaceProperty,
            };
        }
    }
}
