using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StudentAssessmentMappers
    {
        public static StudentAssessment MapToStudentAssessmentv2(this StudentAssessmentv3 a)
        {
            return new StudentAssessment
            {
                Accommodations = a.Accomodations?.Select(b => b.MapToStudentAssessmentAccommodationsItem()).ToList(),
                AdministrationDate = a.AdministrationDate.MapToDateTime(),
                AdministrationEndDate = a.AdministrationEndDate?.MapToDateTime(),
                AdministrationEnvironmentType = a.AdministrationEnvironmentDescriptor?.MapToAdministrationEnvironmentDescriptorv2(),
                AdministrationLanguageDescriptor = a.AdministrationLanguageDescriptor?.MapToAdministrationLanguageDescriptorv2(),
                AssessmentReference = a.AssessmentReference?.MapToStudentAssessmentAssessmentReference(),
                EventCircumstanceType = a.EventCircumstanceDescriptor?.MapToEventCircumstanceDescriptorv2(),
                EventDescription = a.EventDescriptor,
                Id = a.Id,
                Identifier = a.StudentAssessmentIdentifier,
                Items = a.Items?.Select(b => b.MapToStudentAssessmentItemsItem()).ToList(),
                PerformanceLevels = a.PerformanceLevels?.Select(b => b.MapToStudentAssessmentPerformanceLevelsItem()).ToList(),
                ReasonNotTestedType = a.ReasonNotTestedDescriptor?.MapToReasonNotTestedDescriptorv2(),
                RetestIndicatorType = a.RetestIndicatorDescriptor?.MapToRetestIndicatorDescriptorv2(),
                ScoreResults = a.ScoreResults?.Select(b => b.MapToStudentAssessmentScoreResultsItem()).ToList(),
                SerialNumber = a.SerialNumber,
                StudentObjectiveAssessments = a.StudentObjectiveAssessments?.Select(b => b.MapToStudentAssessmentStudentObjectiveAssessmentsItem()).ToList(),
                StudentReference = new StudentAssessmentStudentReference
                {
                    Link = new StudentAssessmentStudentReferenceLink
                    {
                        Href = a.StudentReference?.Link?.Href,
                        Rel = a.StudentReference?.Link?.Rel,
                    },
                    StudentUniqueId = a.StudentReference?.StudentUniqueId,
                }, // TODO Check
                WhenAssessedGradeLevelDescriptor = a.WhenAssessedGradeLevelDescriptor?.MapToWhenAssessedGradeLevelDescriptorv2(),
                _etag = a._etag,
            };
        }

        public static StudentAssessmentv3 MapToStudentAssessmentv3(this StudentAssessment a)
        {
            return new StudentAssessmentv3
            {
                Accomodations = a.Accommodations?.Select(b => b.MapToStudentAssessmentAccommodationv3()),
                AdministrationDate = a.AdministrationDate.MapToYYYYMMdd(),
                AdministrationEndDate = a.AdministrationEndDate.MapToYYYYMMdd(),
                AdministrationEnvironmentDescriptor = a.AdministrationEnvironmentType?.MapToAdministrationEnvironmentDescriptorv3(),
                AdministrationLanguageDescriptor = a.AdministrationLanguageDescriptor?.MapToAdministrationLanguageDescriptorv3(),
                AssessmentReference = a.AssessmentReference?.MapToAssessmentReferencev3(),
                EventCircumstanceDescriptor = a.EventCircumstanceType?.MapToEventCircumstanceDescriptorv3(),
                EventDescriptor = a.EventDescription,
                Id = a.Id,
                Items = a.Items?.Select(b => b.MapToStudentAssessmentItemv3()),
                PerformanceLevels = a.PerformanceLevels?.Select(b => b.MapToStudentAssessmentPerformanceLevelv3()),
                PlatformTypeDescriptor = null, // TODO Check
                ReasonNotTestedDescriptor = a.ReasonNotTestedType?.MapToReasonNotTestedDescriptorv3(),
                RetestIndicatorDescriptor = a.RetestIndicatorType?.MapToRetestIndicatorDescriptorv3(),
                SchoolYearReferenceType = null, // TODO Check
                ScoreResults = a.ScoreResults?.Select(b => b.MapToStudentAssessmentScoreResultv3()),
                SerialNumber = a.SerialNumber,
                StudentAssessmentIdentifier = a.Identifier,
                StudentObjectiveAssessments = a.StudentObjectiveAssessments?.Select(b => b.MapToStudentAssessmentStudentObjectiveAssessmentv3()),
                StudentReference = new StudentReferencev3
                {
                    Link = new LinkReferencev3
                    {
                        Href = a.StudentReference?.Link?.Href,
                        Rel = a.StudentReference?.Link?.Rel,
                    },
                    StudentUniqueId = a.StudentReference?.StudentUniqueId,
                },
                WhenAssessedGradeLevelDescriptor = a.WhenAssessedGradeLevelDescriptor?.MapToWhenAssessedGradeLevelDescriptorv3(),
            };
        }
    }
}
