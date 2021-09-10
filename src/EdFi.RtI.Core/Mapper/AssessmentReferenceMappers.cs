using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentReferenceMappers
    {
        public static StudentAssessmentAssessmentReference MapToStudentAssessmentAssessmentReference(this AssessmentReferencev3 a)
        {
            return new StudentAssessmentAssessmentReference
            {
                Identifier = a.AssessmentIdentifier,
                Link = a.Link?.MapToStudentAssessmentAssessmentReferenceLink(),
                NamespaceProperty = a.Namespace,
            };
        }

        public static AssessmentReferencev3 MapToAssessmentReferencev3(this StudentAssessmentAssessmentReference a)
        {
            return new AssessmentReferencev3
            {
                AssessmentIdentifier = a.Identifier,
                Link = a.Link?.MapToLinkReferencev3(),
                Namespace = a.NamespaceProperty,
            };
        }
    }
}
