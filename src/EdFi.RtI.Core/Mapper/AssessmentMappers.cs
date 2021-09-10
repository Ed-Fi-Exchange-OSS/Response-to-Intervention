using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class AssessmentMappers
    {
        public static Assessment MapToAssessmentv2(this AssessmentModelv3 a)
        {
            return new Assessment
            {
                AcademicSubjects = a.AcademicSubjects?.Select(b => b.MapToAssessmentAcademicSubjectsItem()).ToList(),
                AdaptiveAssessment = a.AdaptiveAssessment,
                AssessedGradeLevels = a.AssessedGradeLevels?.Select(b => b.MapToAssessmentAssessedGradeLevelsItem()).ToList(),
                AssessmentFamilyReference = new AssessmentAssessmentFamilyReference
                {
                    Title = a.AssessmentFamily,
                },
                CategoryDescriptor = a.AssessmentCategoryDescriptor?.MapToAssessmentCategoryDescriptorv2(),
                ContentStandard = a.ContentStandard?.MapToAssessmentContentStandard(),
                EducationOrganizationReference = a.EducationOrganizationReference?.MapToAssessmentEducationOrganizationReference(),
                Form = a.AssessmentForm,
                Id = a.Id,
                IdentificationCodes = a.IdentificationCodes?.Select(b => b.MapToAssessmentIdentificationCodesItem()).ToList(),
                Identifier = a.AssessmentIdentifier,
                Languages = a.Languages?.Select(b => b.MapToAssessmentLanguagesItem()).ToList(),
                MaxRawScore = (int) a.MaxRawScore,
                NamespaceProperty = a.Namespace,
                Nomenclature = a.Nomenclature,
                PerformanceLevels = a.PerformanceLevels?.Select(b => b.MapToAssessmentPerformanceLevelsItem()).ToList(),
                PeriodDescriptor = a.Period?.AssessmentPeriodDescriptor?.MapToAssessmentPeriodDescriptorv2(),
                Programs = a.Programs?.Select(b => b.MapToAssessmentProgramsItem()).ToList(),
                RevisionDate = !string.IsNullOrWhiteSpace(a.RevisionDate)
                    ? DateTime.Parse(a.RevisionDate)
                    : DateTime.Parse("2000-01-01"),
                Scores = a.Scores?.Select(b => b.MapToAssessmentScoresItem()).ToList(),
                Sections = a.Sections?.Select(b => b.MapToAssessmentSectionsItem()).ToList(),
                Title = a.AssessmentTitle,
                Version = a.AssessmentVersion,
                _etag = a._etag,
            };
        }

        public static AssessmentModelv3 MapToAssessmentModelv3(this Assessment a)
        {
            return new AssessmentModelv3
            {
                AcademicSubjects = a.AcademicSubjects?.Select(b => b.MapToAcademicSubjectDescriptorv3()),
                AdaptiveAssessment = a.AdaptiveAssessment.HasValue && a.AdaptiveAssessment.Value,
                AssessedGradeLevels = a.AssessedGradeLevels?.Select(b => b.MapToGradeLevelDescriptorv3()),
                AssessmentCategoryDescriptor = a.CategoryDescriptor?.MapToAssessmentCategoryDescriptorv3(),
                AssessmentFamily = a.AssessmentFamilyReference?.Title,
                AssessmentForm = a.Form,
                AssessmentIdentifier = a.Identifier,
                AssessmentTitle = a.Title,
                AssessmentVersion = a.Version ?? 0,
                ContentStandard = a.ContentStandard?.MapToAssessmentContentStandardv3(),
                EducationOrganizationReference = a.EducationOrganizationReference?.MapToEducationOrganizationReferencev3(),
                Id = a.Id,
                IdentificationCodes = a.IdentificationCodes?.Select(b => b.MapToAssessmentIdentificationCodev3()),
                Languages = a.Languages?.Select(b => b.MapToAssessmentLanguagev3()),
                MaxRawScore = a.MaxRawScore ?? 0,
                Namespace = a.NamespaceProperty,
                Nomenclature = a.Nomenclature,
                PerformanceLevels = a.PerformanceLevels?.Select(b => b.MapToAssessmentPerformanceLevelv3()),
                Period = a.PeriodDescriptor?.MapToAssessmentPeriodDescriptorV3(),
                PlatformTypes = new List<AssessmentPlatformTypev3>(),
                Programs = a.Programs?.Select(b => b.MapToAssessmentProgramv3()),
                RevisionDate = a.RevisionDate.HasValue
                    ? a.RevisionDate.Value.ToString("yyyy-MM-dd")
                    : "2000-01-01",
                Scores = a.Scores?.Select(b => b.MapToAssessmentScorev3()),
                Sections = a.Sections?.Select(b => b.MapToAssessmentSectionv3()),
                _etag = a._etag,
            };
        }
    }
}
