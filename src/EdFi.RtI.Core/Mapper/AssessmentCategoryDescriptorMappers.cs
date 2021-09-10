using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentCategoryDescriptorMappers
    {
        public static AssessmentCategoryDescriptor MapToAssessmentCategoryDescriptor(this AssessmentCategoryDescriptorv3 a)
        {
            return new AssessmentCategoryDescriptor
            {
                Id = a.Id,
                AssessmentCategoryDescriptorId = a.AssessmentCategoryDescriptorId,
                AssessmentCategoryType = a.Description,
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

        public static AssessmentCategoryDescriptorv3 MapToAssessmentCategoryDescriptor(this AssessmentCategoryDescriptor a)
        {
            return new AssessmentCategoryDescriptorv3
            {
                Id = a.Id,
                AssessmentCategoryDescriptorId = a.AssessmentCategoryDescriptorId,
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
