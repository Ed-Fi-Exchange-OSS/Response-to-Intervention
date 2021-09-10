using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StudentInterventionAssociationStudentReferenceMappers
    {
        public static StudentInterventionAssociationStudentReference MapToStudentInterventionAssociationStudentReference(this StudentReferencev3 a)
        {
            return new StudentInterventionAssociationStudentReference
            {
                Link = a.Link?.MapToStudentInterventionAssociationStudentReferenceLink(),
                StudentUniqueId = a.StudentUniqueId,
            };
        }

        public static StudentReferencev3 MapToStudentReferencev3(this StudentInterventionAssociationStudentReference a)
        {
            return new StudentReferencev3
            {
                Link = a.Link?.MapToLinkReferencev3(),
                StudentUniqueId = a.StudentUniqueId,
            };
        }
    }
}
