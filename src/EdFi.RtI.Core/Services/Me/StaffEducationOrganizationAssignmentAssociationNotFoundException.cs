using EdFi.RtI.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.Me
{
    public class StaffEducationOrganizationAssignmentAssociationNotFoundException : DomainException
    {
        public StaffEducationOrganizationAssignmentAssociationNotFoundException(string staffUniqueId)
            : base($"Could not find StaffEducationOrganizationAssignmentAssociation with StaffUniqueId \"{staffUniqueId}\"")
        {
            StatusCode = System.Net.HttpStatusCode.NotFound;
        }
    }
}
