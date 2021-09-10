using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentPeriodDescriptorMappers
    {
        public static AssessmentPeriodDescriptor MapToAssessmentPeriodDescriptor(this AssessmentPeriodDescriptorv3 a)
        {
            return new AssessmentPeriodDescriptor
            {
                Id = a.Id,
                AssessmentPeriodDescriptorId = a.AssessmentPeriodDescriptorId,
                CodeValue = a.CodeValue,
                Description = a.Description,
                EffectiveBeginDate = a.EffectiveBeginDate,
                EffectiveEndDate = a.EffectiveEndDate,
                NamespaceProperty = a.NamespaceProperty,
                PriorDescriptorId = a.PriorDescriptorId,
                ShortDescription = a.ShortDescription,
                _etag = a._etag
            };
        }

        public static AssessmentPeriodDescriptorv3 MapToAssessmentPeriodDescriptor(this AssessmentPeriodDescriptor a)
        {
            return new AssessmentPeriodDescriptorv3
            {
                Id = a.Id,
                AssessmentPeriodDescriptorId = a.AssessmentPeriodDescriptorId,
                CodeValue = a.CodeValue,
                Description = a.Description,
                EffectiveBeginDate = a.EffectiveBeginDate,
                EffectiveEndDate = a.EffectiveEndDate,
                NamespaceProperty = a.NamespaceProperty,
                PriorDescriptorId = a.PriorDescriptorId,
                ShortDescription = a.ShortDescription,
                _etag = a._etag
            };
        }
    }
}
