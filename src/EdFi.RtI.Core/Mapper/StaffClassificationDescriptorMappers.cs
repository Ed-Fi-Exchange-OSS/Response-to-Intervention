using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StaffClassificationDescriptorMappers
    {
        public static StaffClassificationDescriptor ToMapStaffClassificationDescriptor(this StaffClassificationDescriptorv3 a)
        {
            return new StaffClassificationDescriptor
            {
                Id = a.Id,
                StaffClassificationDescriptorId = a.StaffClassificationDescriptorId,
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

        public static StaffClassificationDescriptorv3 ToMapStaffClassificationDescriptor(this StaffClassificationDescriptor a)
        {
            return new StaffClassificationDescriptorv3
            {
                Id = a.Id,
                StaffClassificationDescriptorId = a.StaffClassificationDescriptorId,
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
