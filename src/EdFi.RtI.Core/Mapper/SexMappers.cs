using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SexMappers
    {
        public static InterventionAppropriateSexesItem MapToInterventionAppropriateSexesItemV2(this SexDescriptorv3 a)
        {
            return new InterventionAppropriateSexesItem
            {
                SexType = a.SexDescriptor,
            };
        }

        public static SexDescriptorv3 MapToSexDescriptorV3(this InterventionAppropriateSexesItem a)
        {
            return new SexDescriptorv3
            {
                SexDescriptor = a.SexType,
            };
        }
    }
}
