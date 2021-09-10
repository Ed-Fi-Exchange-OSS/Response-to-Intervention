using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.StaffEducationOrganizationAssignmentAssociations
{
    public class StaffEducationOrganizationAssignmentAssociationDTO
    {
        public DateTime BeginDate { get; set; }
        public EducationOrganizationReferenceDTO EducationOrganizationReference { get; set; }
        public object EmploymentStaffEducationOrganizationEmploymentAssociationReference { get; set; }
        public object EndDate { get; set; }
        public string Id { get; set; }
        public object OrderOfAssignment { get; set; }
        public string PositionTitle { get; set; }
        public string StaffClassificationDescriptor { get; set; }
        public StaffReferenceDTO StaffReference { get; set; }
    }

    public class EducationOrganizationReferenceDTO
    {
        public long EducationOrganizationId { get; set; }
        public EducationOrganizationReferenceLinkDTO Link { get; set; }
    }

    public class EducationOrganizationReferenceLinkDTO
    {
        public string Href { get; set; }
        public string Rel { get; set; }
    }

    public class StaffReferenceDTO
    {
        public StaffReferenceLinkDTO Link { get; set; }
        public string StaffUniqueId { get; set; }
    }

    public class StaffReferenceLinkDTO
    {
        public string Href { get; set; }
        public string Rel { get; set; }
    }
}
