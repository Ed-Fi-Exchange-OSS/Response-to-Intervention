using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class GradeLevelMappers
    {
        public static InterventionAppropriateGradeLevelsItem MapToInterventionAppropriateGradeLevelsItemV2(this GradeLevelDescriptorv3 a)
        {
            return new InterventionAppropriateGradeLevelsItem
            {
                GradeLevelDescriptor = a.GradeLevelDescriptor,
            };
        }

        public static GradeLevelDescriptorv3 MapToGradeLevelDescriptorV3(this InterventionAppropriateGradeLevelsItem a)
        {
            return new GradeLevelDescriptorv3
            {
                GradeLevelDescriptor = a.GradeLevelDescriptor,
            };
        }

        public static AssessmentAssessedGradeLevelsItem MapToAssessmentAssessedGradeLevelsItem(this GradeLevelDescriptorv3 a)
        {
            return new AssessmentAssessedGradeLevelsItem
            {
                GradeLevelDescriptor = a.GradeLevelDescriptor?.MapToGradeLevelDescriptorv2(),
            };
        }

        public static GradeLevelDescriptorv3 MapToGradeLevelDescriptorv3(this AssessmentAssessedGradeLevelsItem a)
        {
            return new GradeLevelDescriptorv3
            {
                GradeLevelDescriptor = a.GradeLevelDescriptor?.MapToGradeLevelDescriptorv3(),
            };
        }
    }
}
