using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StudentAssessmentv3
    {
        public string Id { get; set; }
        public string StudentAssessmentIdentifier { get; set; }
        public AssessmentReferencev3 AssessmentReference { get; set; }
        public SchoolYearTypeReferencev3 SchoolYearReferenceType { get; set; }
        public IEnumerable<StudentAssessmentAccommodationv3> Accomodations { get; set; }
        public string AdministrationDate { get; set; }
        public string AdministrationEndDate { get; set; }
        public string AdministrationEnvironmentDescriptor { get; set; }
        public string AdministrationLanguageDescriptor { get; set; }
        public string EventCircumstanceDescriptor { get; set; }
        public string EventDescriptor { get; set; }
        public IEnumerable<StudentAssessmentItemv3> Items { get; set; }
        public IEnumerable<StudentAssessmentPerformanceLevelv3> PerformanceLevels { get; set; }
        public string PlatformTypeDescriptor { get; set; }
        public string ReasonNotTestedDescriptor { get; set; }
        public string RetestIndicatorDescriptor { get; set; }
        public IEnumerable<StudentAssessmentScoreResultv3> ScoreResults { get; set; }
        public string SerialNumber { get; set; }
        public IEnumerable<StudentAssessmentStudentObjectiveAssessmentv3> StudentObjectiveAssessments { get; set; }
        public StudentReferencev3 StudentReference { get; set; }
        public string WhenAssessedGradeLevelDescriptor { get; set; }
        public string _etag { get; set; }
    }
}
