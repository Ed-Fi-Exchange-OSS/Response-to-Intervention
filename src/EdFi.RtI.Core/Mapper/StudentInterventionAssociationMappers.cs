using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StudentInterventionAssociationMappers
    {
        public static StudentInterventionAssociation MapToStudentInterventionAssociation(this StudentInterventionAssociationv3 a)
        {
            return new StudentInterventionAssociation
            {
                CohortReference = a.CohortReference?.MapToStudentInterventionAssociationCohortReference(),
                DiagnosticStatement = null, // TODO Check
                Id = a.Id,
                InterventionEffectivenesses = a.InterventionEffectivenesses?.Select(b => b.MapToStudentInterventionAssociationInterventionEffectivenessesItem()).ToList(),
                InterventionReference = a.InterventionReference?.MapToStudentInterventionAssociationInterventionReference(),
                StudentReference = a.StudentReference?.MapToStudentInterventionAssociationStudentReference(),
                _etag = a._etag,
            };
        }

        public static StudentInterventionAssociationv3 MapToStudentInterventionAssociationv3(this StudentInterventionAssociation a)
        {
            return new StudentInterventionAssociationv3
            {
                CohortReference = a.CohortReference?.MapToCohortReferencev3(),
                Dosage = 0, // TODO Check
                Id = a.Id,
                InterventionEffectivenesses = a.InterventionEffectivenesses?.Select(b => b.MapToStudentInterventionAssociationInterventionEffectivenessv3()),
                InterventionReference = a.InterventionReference?.MapToInterventionReferencev3(),
                StudentReference = a.StudentReference?.MapToStudentReferencev3(),
                _etag = a._etag,
            };
        }
    }
}
