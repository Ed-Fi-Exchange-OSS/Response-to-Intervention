using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StaffMappers
    {
        public static InterventionStaffsItem MapToInterventionStaffsItem(this StaffItemv3 a)
        {
            return new InterventionStaffsItem
            {
                StaffReference = a.StaffReference?.MapToInterventionStaffsItemStaffReference(),
            };
        }

        public static StaffItemv3 MapToStaffItemv3(this InterventionStaffsItem a)
        {
            return new StaffItemv3
            {
                StaffReference = a.StaffReference?.MapToStaffReferencev3(),
            };
        }

        public static InterventionStaffsItemStaffReference MapToInterventionStaffsItemStaffReference(this StaffReferencev3 a)
        {
            return new InterventionStaffsItemStaffReference
            {
                Link = a.Link?.MapToInterventionStaffsItemStaffReferenceLink(),
                StaffUniqueId = a.StaffUniqueId,
            };
        }

        public static StaffReferencev3 MapToStaffReferencev3(this InterventionStaffsItemStaffReference a)
        {
            return new StaffReferencev3
            {
                Link = a.Link?.MapToLinkReferencev3(),
                StaffUniqueId = a.StaffUniqueId,
            };
        }

        public static InterventionStaffsItemStaffReferenceLink MapToInterventionStaffsItemStaffReferenceLink(this LinkReferencev3 a)
        {
            return new InterventionStaffsItemStaffReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this InterventionStaffsItemStaffReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }
    }
}
