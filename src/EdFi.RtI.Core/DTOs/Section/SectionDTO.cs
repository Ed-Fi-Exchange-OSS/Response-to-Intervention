using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Section
{
    public class SectionDTO
    {
        public string ClassPeriodName { get; set; }
        public string ClassroomIdentificationCode { get; set; }
        public string LocalCourseCode { get; set; }
        public int SchoolId { get; set; }
        public int SchoolYear { get; set; }
        public int SequenceOfCourse { get; set; }
        public string TermDescriptor { get; set; }
        public string UniqueSectionCode { get; set; }
    }
}
