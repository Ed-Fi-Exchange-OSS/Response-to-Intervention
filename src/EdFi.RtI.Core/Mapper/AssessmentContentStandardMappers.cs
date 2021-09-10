using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentContentStandardMappers
    {
        public static AssessmentContentStandard MapToAssessmentContentStandard(this AssessmentContentStandardv3 a)
        {
            DateTime date(string dateStr) => !string.IsNullOrWhiteSpace(dateStr)
                ? DateTime.Parse(dateStr)
                : DateTime.Parse("2000-01-01");

            return new AssessmentContentStandard
            {
                Authors = a.Authors?.Select(b => b.MapToAssessmentContentStandardAuthorsItem()).ToList(),
                BeginDate = date(a.BeginDate),
                EndDate = date(a.EndDate),
                MandatingEducationOrganizationReference = a.MandatingEducationOrganizationReference?.MapToAssessmentContentStandardMandatingEducationOrganizationReference(),
                PublicationDate = date(a.PublicationDate),
                PublicationStatusType = a.PublicationStatusDescriptor?.MapToPublicationStatusDescriptorv2(),
                PublicationYear = a.PublicationYear,
                Title = a.Title,
                Uri = a.Uri,
                Version = a.Version,
            };
        }

        public static AssessmentContentStandardv3 MapToAssessmentContentStandardv3(this AssessmentContentStandard a)
        {
            string date(DateTime? date) => date.HasValue
                ? date.Value.ToString("yyyy-MM-dd")
                : "2000-01-01";

            return new AssessmentContentStandardv3
            {
                Authors = a.Authors?.Select(b => b.MapToAssessmentContentStandardAuthorv3()),
                BeginDate = date(a.BeginDate),
                EndDate = date(a.EndDate),
                MandatingEducationOrganizationReference = a.MandatingEducationOrganizationReference?.MapToEducationOrganizationReferencev3(),
                PublicationDate = date(a.PublicationDate),
                PublicationStatusDescriptor = a.PublicationStatusType?.MapToPublicationStatusDescriptorv3(),
                PublicationYear = a.PublicationYear ?? 0,
                Title = a.Title,
                Uri = a.Uri,
                Version = a.Version,
            };
        }
    }
}
