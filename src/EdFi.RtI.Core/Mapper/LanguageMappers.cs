using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class LanguageMappers
    {
        public static AssessmentLanguagesItem MapToAssessmentLanguagesItem(this AssessmentLanguagev3 a)
        {
            return new AssessmentLanguagesItem
            {
                LanguageDescriptor = a.LanguageDescriptor,
            };
        }

        public static AssessmentLanguagev3 MapToAssessmentLanguagev3(this AssessmentLanguagesItem a)
        {
            return new AssessmentLanguagev3
            {
                LanguageDescriptor = a.LanguageDescriptor,
            };
        }
    }
}
