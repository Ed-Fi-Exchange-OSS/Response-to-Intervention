using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class EnrollmentSectionv3
    {
        public string Id { get; set; }
        public string LocalCourseCode { get; set; }
        public string SectionIdentifier { get; set; }
        public string AcademicSubjectDescriptor { get; set; }
        public double AvailableCredits { get; set; }
        public IList<SectionClassPeriodv3> ClassPeriods { get; set; }
        public string EducationalEnvironmentDescriptor { get; set; }
        public string LocalCourseTitle { get; set; }
        public object Location { get; set; }
        public int SequenceOfCourse { get; set; }
        public SectionSessionv3 Session { get; set; }
        public int MyProperty { get; set; }
        public IList<object> Staff { get; set; }
        public IList<object> Students { get; set; }
        public string _etag { get; set; }
    }
}
