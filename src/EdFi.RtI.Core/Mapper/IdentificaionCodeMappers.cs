using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class IdentificaionCodeMappers
    {
        public static AssessmentIdentificationCodev3 MapToAssessmentIdentificationCodev3(this AssessmentIdentificationCodesItem a)
        {
            return new AssessmentIdentificationCodev3
            {
                AssessmentIdentificationSystemDescriptor = a.AssessmentIdentificationSystemDescriptor?.MapToAssessmentIdentificationSystemDescriptorv3(),
                AssigningOrganizationIdentificationCode = a.AssigningOrganizationIdentificationCode,
                IdentificationCode = a.IdentificationCode,
            };
        }

        public static AssessmentIdentificationCodesItem MapToAssessmentIdentificationCodesItem(this AssessmentIdentificationCodev3 a)
        {
            return new AssessmentIdentificationCodesItem
            {
                AssessmentIdentificationSystemDescriptor = a.AssessmentIdentificationSystemDescriptor?.MapToAssessmentIdentificationSystemDescriptorv2(),
                AssigningOrganizationIdentificationCode = a.AssigningOrganizationIdentificationCode,
                IdentificationCode = a.IdentificationCode,
            };
        }
    }
}
