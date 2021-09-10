using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SectionCharacteristicsItemMappers
    {
        public static SectionCharacteristicsItem MapToSectionCharacteristicsItem(this SectionCharacteristicsItemv3 a)
        {
            return new SectionCharacteristicsItem
            {
                Descriptor = a.Descriptor
            };
        }

        public static SectionCharacteristicsItemv3 MapToSectionCharacteristicsItem(this SectionCharacteristicsItem a)
        {
            return new SectionCharacteristicsItemv3
            {
                Descriptor = a.Descriptor
            };
        }
    }
}
