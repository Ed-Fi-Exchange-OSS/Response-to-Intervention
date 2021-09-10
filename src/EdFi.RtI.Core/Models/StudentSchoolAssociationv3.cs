using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StudentSchoolAssociationv3
    {
        public string Id { get; set; }
        public string EntryDate { get; set; }
        public object CalendarReference { get; set; }
        public object ClassOfSchoolYearReference { get; set; }
        public object GraduationPlanReference { get; set; }
        public object SchoolReference { get; set; }
        public SchoolYearTypeReferencev3 SchoolYearTypeReference { get; set; }
        public StudentReferencev3 StudentReference { get; set; }
        public IList<object> AlternativeGraducationPlans { get; set; }
        public IList<object> EducationPlans { get; set; }
        public bool EmployedWhilteEnrolled { get; set; }
        public string EntryGradeLevelDescriptor { get; set; }
        public string EntryGradeLevelReasonDescriptor { get; set; }
        public string EntryTypeDescriptor { get; set; }
        public string ExitWithdrawDate { get; set; }
        public string ExitWithdrawTypeDescriptor { get; set; }
        public double FullTimeEquivalency { get; set; }
        public bool PrimarySchool { get; set; }
        public bool RepeatGradeIndicator { get; set; }
        public string ResidencyStatusDescriptor { get; set; }
        public bool SchoolChoiceTransfer { get; set; }
        public bool TermCompletionIndicator { get; set; }
        public string _etag { get; set; }
    }
}
