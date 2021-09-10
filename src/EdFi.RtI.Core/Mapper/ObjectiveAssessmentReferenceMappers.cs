using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class ObjectiveAssessmentReferenceMappers
    {
        public static StudentAssessmentStudentObjectiveAssessmentsItemObjectiveAssessmentReference MapToStudentAssessmentStudentObjectiveAssessmentsItemObjectiveAssessmentReference(this ObjectiveAssessmentReferencev3 a)
        {
            return new StudentAssessmentStudentObjectiveAssessmentsItemObjectiveAssessmentReference
            {
                AssessmentIdentifier = a.AssessmentIdentifier,
                IdentificationCode = a.IdentificationCode,
                Link = a.Link?.MapToStudentAssessmentStudentObjectiveAssessmentsItemObjectiveAssessmentReferenceLink(),
                NamespaceProperty = a.Namespace,
            };
        }

        public static ObjectiveAssessmentReferencev3 MapToObjectiveAssessmentReferencev3(this StudentAssessmentStudentObjectiveAssessmentsItemObjectiveAssessmentReference a)
        {
            return new ObjectiveAssessmentReferencev3
            {
                AssessmentIdentifier = a.AssessmentIdentifier,
                IdentificationCode = a.IdentificationCode,
                Link = a.Link?.MapToLinkReferencev3(),
                Namespace = a.NamespaceProperty,
            };
        }
    }
}
