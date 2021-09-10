using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class TpdmStaffExtensionv3
    {
        public string GenderDescriptor { get; set; }
        public OpenStaffPositionReferencev3 OpenStaffPositionReference { get; set; }
        public IList<StaffEducatorPreparationProgramv3> EducatorPreparationPrograms { get; set; }
        public IList<StaffHighlyQualifiedAcademicSubjectv3> HighlyQualifiedAcademicSubjects { get; set; }
        public StaffEducatorResearchv3 EducatorResearch { get; set; }
    }
}
