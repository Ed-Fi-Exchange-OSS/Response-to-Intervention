using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class PopulationServedMappers
    {
        public static InterventionPopulationServedsItem MapToInterventionPopulationServedsItem(this PopulationServedDescriptorv3 a)
        {
            return new InterventionPopulationServedsItem
            {
                PopulationServedType = a.PopulationServedDescriptor,
            };
        }

        public static PopulationServedDescriptorv3 MapToPopulationServedDescriptorv3(this InterventionPopulationServedsItem a)
        {
            return new PopulationServedDescriptorv3
            {
                PopulationServedDescriptor = a.PopulationServedType,
            };
        }
    }
}
