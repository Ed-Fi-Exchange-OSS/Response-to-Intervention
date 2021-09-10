using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentContentStandardAuthorMappers
    {
        public static AssessmentContentStandardAuthorsItem MapToAssessmentContentStandardAuthorsItem(this AssessmentContentStandardAuthorv3 a)
        {
            return new AssessmentContentStandardAuthorsItem
            {
                Author = a.Author,
            };
        }

        public static AssessmentContentStandardAuthorv3 MapToAssessmentContentStandardAuthorv3(this AssessmentContentStandardAuthorsItem a)
        {
            return new AssessmentContentStandardAuthorv3
            {
                Author = a.Author,
            };
        }
    }
}
