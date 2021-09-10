using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DTOs.StaffEducationOrganizationAssignmentAssociations;
using EdFi.RtI.Core.DTOs.Staffs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Composed
{
    public class UserSessionProfile
    {
        public StaffDTO Staff { get; set; }
        public StaffEducationOrganizationAssignmentAssociation StaffEducationOrganizationAssignmentAssociation { get; set; }
        public School School { get; set; }
    }
}
