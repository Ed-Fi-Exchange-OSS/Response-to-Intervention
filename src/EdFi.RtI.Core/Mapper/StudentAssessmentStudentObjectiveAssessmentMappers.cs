using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StudentAssessmentStudentObjectiveAssessmentMappers
    {
        public static StudentAssessmentStudentObjectiveAssessmentsItem MapToStudentAssessmentStudentObjectiveAssessmentsItem(this StudentAssessmentStudentObjectiveAssessmentv3 a)
        {
            return new StudentAssessmentStudentObjectiveAssessmentsItem
            {
                ObjectiveAssessmentReference = a.ObjectiveAssessmentReference?.MapToStudentAssessmentStudentObjectiveAssessmentsItemObjectiveAssessmentReference(),
                PerformanceLevels = null, // TODO Check
                ScoreResults = null, // TODO Check
            };
        }

        public static StudentAssessmentStudentObjectiveAssessmentv3 MapToStudentAssessmentStudentObjectiveAssessmentv3(this StudentAssessmentStudentObjectiveAssessmentsItem a)
        {
            return new StudentAssessmentStudentObjectiveAssessmentv3
            {
                ObjectiveAssessmentReference = a.ObjectiveAssessmentReference?.MapToObjectiveAssessmentReferencev3(),
            };
        }
    }
}
