using EdFi.RtI.Core.DTOs.Section;
using EdFi.RtI.Core.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.StudentSectionAssociation
{
    public class StudentSectionAssociationDTO
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool HomeroomIndicator { get; set; }
        public string Id { get; set; }
        public object RepeatIdentifierType { get; set; }
        public SectionDTO SectionReference { get; set; }
        public StudentDTO StudentReference { get; set; }
        public object TeacherStudentDataLinkExclusion { get; set; }
        public object _etag { get; set; }
    }
}
