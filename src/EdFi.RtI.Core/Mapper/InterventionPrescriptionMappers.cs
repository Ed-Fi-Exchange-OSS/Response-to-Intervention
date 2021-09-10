using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class InterventionPrescriptionMappers
    {
        public static InterventionInterventionPrescriptionsItem MapToInterventionInterventionPrescriptionsItem(this InterventionPrescriptionv3 a)
        {
            return new InterventionInterventionPrescriptionsItem
            {
                InterventionPrescriptionReference = a.InterventionPrescriptionReference?.MapToInterventionInterventionPrescriptionsItemInterventionPrescriptionReference(),
            };
        }

        public static InterventionPrescriptionv3 MapToInterventionPrescriptionv3(this InterventionInterventionPrescriptionsItem a)
        {
            return new InterventionPrescriptionv3
            {
                InterventionPrescriptionReference = a.InterventionPrescriptionReference?.MapToInterventionPrescriptionv3(),
            };
        }
    }
}
