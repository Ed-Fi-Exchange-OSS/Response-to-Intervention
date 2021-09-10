using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class DiagnosisMappers
    {
        public static InterventionDiagnosesItem MapToInterventionDiagnosesItemV2(this DiagnosisDescriptorv3 a)
        {
            return new InterventionDiagnosesItem
            {
                DiagnosisDescriptor = a.DiagnosisDescriptor,
            };
        }

        public static DiagnosisDescriptorv3 MapToDiagnosisDescriptorV3(this InterventionDiagnosesItem a)
        {
            return new DiagnosisDescriptorv3
            {
                DiagnosisDescriptor = a.DiagnosisDescriptor,
            };
        }
    }
}
