using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class Sectionv3
    {
        public IList<SectionProgramsItemv3> Programs { get; set; }
        public string PopulationServedType { get; set; }
        public string MediumOfInstructionType { get; set; }
        public SectionLocationReferencev3 LocationReference { get; set; }
        public string InstructionLanguageDescriptor { get; set; }
        public string Id { get; set; }
        public string EducationalEnvironmentType { get; set; }
        public SectionCourseOfferingReferencev3 CourseOfferingReference { get; set; }
        public IList<SectionCharacteristicsItemv3> Characteristics { get; set; }
        public double? AvailableCredits { get; set; }
        public string AvailableCreditType { get; set; }
        public double? AvailableCreditConversion { get; set; }
        public string _etag { get; set; }
        public int SequenceOfCourse { get; set; }
        public string UniqueSectionCode { get; set; }
        public SectionLocationSchoolReferencev3 LocationSchoolReference { get; set; }
        public IList<SectionCourseLevelCharacteristicsv3> CourseLevelCharacteristics { get; set; }
        public IList<SectionOfferedGradeLevelsv3> OfferedGradeLevels { get; set; }
        public bool OfficialAttendancePeriod { get; set; }
        public string SectionName { get; set; }
        public IList<SectionClassPeriodv3> ClassPeriods { get; set; }
    }
}
