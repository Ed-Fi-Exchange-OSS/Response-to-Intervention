using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.DTOs.Student;
using EdFi.RtI.Core.Infrastructure;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.Models;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.RequestBodies;
using EdFi.RtI.Core.Services.Assessments;
using EdFi.RtI.Core.Services.Students;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EdFi.RtI.Core.DTOs.Composed.ScoringAssessmentDTO;

namespace EdFi.RtI.Core.Services.ScoringAssessments
{
    public class ScoringAssessmentsServiceV3 : IScoringAssessmentsService
    {
        private readonly ICacheStore _cache;
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;
        private readonly IOdsApiSettingsProvider _odsApiSettingsProvider;

        public ScoringAssessmentsServiceV3(ICacheStore cache, IDomainServiceProvider domainServiceProvider, IOdsApiClientProvider odsApiClientProvider, IOdsApiSettingsProvider odsApiSettingsProvider)
        {
            _cache = cache;
            _domainServiceProvider = domainServiceProvider;
            _odsApiClientProvider = odsApiClientProvider;
            _odsApiSettingsProvider = odsApiSettingsProvider;
        }

        private IAssessmentService _assessments => _domainServiceProvider.GetService<IAssessmentService>();
        private IEdFiMapper _mapper => _domainServiceProvider.GetService<IEdFiMapper>();
        private IStudentService _students => _domainServiceProvider.GetService<IStudentService>();

        public async Task<StudentAssessment> CreateAssociation(ScoringAssessmentPostBody body)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var assessmentv3 = await odsApi.Get<AssessmentModelv3>($"assessments/{body.AssessmentId}");

            IEnumerable<StudentAssessmentScoreResultv3> createStudentAssessmentScoreResultsv3()
            {
                var score = assessmentv3.Scores?.FirstOrDefault();

                return new List<StudentAssessmentScoreResultv3>
                {
                    new StudentAssessmentScoreResultv3
                    {
                        AssessmentReportingMethodDescriptor = score != null
                            ? score.AssessmentReportingMethodDescriptor
                            : "Raw Score".MapToAssessmentReportingMethodDescriptorv3(),
                        Result = "---",
                        ResultDatatypeTypeDescriptor = score != null
                            ? score.ResultDatatypeTypeDescriptor
                            : "Integer".MapToAssessmentReportingMethodDescriptorv3(),
                    },
                };
            }

            var studentAssessmentv3 = new StudentAssessmentv3
            {
                Accomodations = new List<StudentAssessmentAccommodationv3>(),
                AdministrationDate = DateTime.Parse(body.AdministrationDate).MapToYYYYMMdd(),
                AdministrationEndDate = DateTime.Parse(body.AdministrationDate).MapToYYYYMMdd(),
                AdministrationEnvironmentDescriptor = null,
                AdministrationLanguageDescriptor = null,
                AssessmentReference = new AssessmentReferencev3
                {
                    AssessmentIdentifier = assessmentv3.AssessmentIdentifier,
                    Namespace = assessmentv3.Namespace,
                },
                EventCircumstanceDescriptor = null,
                EventDescriptor = null,
                Id = null,
                Items = new List<StudentAssessmentItemv3>(),
                PerformanceLevels = new List<StudentAssessmentPerformanceLevelv3>(),
                PlatformTypeDescriptor = null,
                ReasonNotTestedDescriptor = null,
                RetestIndicatorDescriptor = null,
                SchoolYearReferenceType = null,
                ScoreResults = createStudentAssessmentScoreResultsv3(), // At least one score result is needed for the frontend scoring assessments grid
                SerialNumber = null,
                StudentAssessmentIdentifier = Guid.NewGuid().ToString(),
                StudentObjectiveAssessments = new List<StudentAssessmentStudentObjectiveAssessmentv3>(),
                StudentReference = new StudentReferencev3
                {
                    StudentUniqueId = body.StudentUniqueId,
                },
                WhenAssessedGradeLevelDescriptor = await GetStudentGradeLevelDescriptor(body.StudentUniqueId),
                _etag = null,
            };

            var response = await odsApi.Post("studentAssessments", studentAssessmentv3);
            
            var createdStudentAssessmentId = await odsApi.HandleHttpResponseGetCreatedResourceId(response);
            var createdStudentAssessmentv3 = await odsApi.Get<StudentAssessmentv3>($"studentAssessments/{createdStudentAssessmentId}");

            return createdStudentAssessmentv3.MapToStudentAssessmentv2();
        }

        public async Task<ScoringAssessmentDTO> CreateScoring(ScoringAssessmentDTO scoring)
        {
            scoring = SafeParseForPost(scoring);

            var studentAssessmentv3 = scoring
                .Associations
                .Last()
                .AssociationModel
                .MapToStudentAssessmentv3();

            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var response = await odsApi.Post("studentAssessments", studentAssessmentv3);
            await odsApi.HandleHttpResponse(response);

            return scoring;
        }

        public async Task DeleteStudentAssessment(string studentAssessmentId, string uniqueSectionCode, string identifier)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var response = await odsApi.Delete($"studentAssessments/{studentAssessmentId}");
            await odsApi.HandleHttpResponse(response);

            string key = CacheKeys.Composed(CacheKeys.AssessmentScoringsBySectionUniqueId, uniqueSectionCode);

            if (await _cache.TryHasKey(key))
            {
                var students = await _cache.Get<IEnumerable<ScoringAssessmentDTO>>(key);

                students = students.Select(dto =>
                {
                    _mapper.MapStudentDTO(dto.Student);
                    dto.Student = _mapper.SecuredStudentDTO(dto.Student);

                    if (string.IsNullOrWhiteSpace(studentAssessmentId))
                        dto.Associations = dto.Associations.Where(association => association.AssociationModel.Identifier != identifier).ToList();
                    else
                        dto.Associations = dto.Associations.Where(association => association.AssociationModel.Id != studentAssessmentId).ToList();

                    return dto;

                }).ToList();

                try
                {
                    await _cache.Set(key, students);
                }
                catch (Exception ex)
                {
                    // Sometimes caching takes too long so request timeout will expire
                    // causing the method th throw an exception. Catch the error to return
                    // the scorings back to the frontend even though this error happens
                }
            }
        }

        public async Task DeleteStudentAssessmentAll(string uniqueSectionCode, string assessmentId, string date)
        {
            string key = CacheKeys.Composed(CacheKeys.AssessmentScoringsBySectionUniqueId, uniqueSectionCode);

            if (await _cache.TryHasKey(key))
            {
                var students = await _cache.Get<IEnumerable<ScoringAssessmentDTO>>(key);

                var odsApi = await _odsApiClientProvider.NewResourcesClient();

                students = students.Select(dto =>
                {
                    _mapper.MapStudentDTO(dto.Student);
                    dto.Student = _mapper.SecuredStudentDTO(dto.Student);

                    foreach (StudentAssessmentAssociationDTO element in dto.Associations)
                    {
                        DateTime adminDate = Convert.ToDateTime(date);

                        if (!string.IsNullOrWhiteSpace(element.AssociationModel.Id) && element.Assessment.Id == assessmentId && element.AssociationModel.AdministrationDate == adminDate)
                            odsApi.Delete($"studentAssessments/{element.AssociationModel.Id}");

                        if (element.Assessment.Id == assessmentId && element.AssociationModel.AdministrationDate == adminDate)
                            dto.Associations = dto.Associations.Where(association => association.AssociationModel.Identifier != element.AssociationModel.Identifier).ToList();
                    }

                    return dto;

                }).ToList();

                try
                {
                    await _cache.Set(key, students);
                }
                catch (Exception ex)
                {
                    // Sometimes caching takes too long so request timeout will expire
                    // causing the method th throw an exception. Catch the error to return
                    // the scorings back to the frontend even though this error happens
                }
            }
        }

        public async Task<IEnumerable<ScoringAssessmentDTO>> Search(ScoringAssessmentSearchParams searchParams)
        {
            if (searchParams.GetFromCache)
            {
                var scoringsFromCache = await GetAssessmentScoringsFromCacheOrDefault(searchParams.UniqueSectionCode);

                if (scoringsFromCache != null && scoringsFromCache.Count() > 0)
                {
                    var scoringsFromCacheList = scoringsFromCache
                        .OrderBy(student => student.Student.LastSurname)
                        .ToList();

                    return await ApplyFilters(scoringsFromCacheList, searchParams);
                }
            }

            var scoringsFromApi = new List<ScoringAssessmentDTO>();
            var studentsBySection = await _students.GetStudentsBySectionId(searchParams.UniqueSectionCode);

            foreach (var student in studentsBySection)
            {
                try
                {
                    var studentDTO = _mapper.MapStudentDTO(student);
                    var associationDTO = await GetAssessmentScoringsByStudent(studentDTO, searchParams.Category);
                    scoringsFromApi.Add(associationDTO);
                }
                catch (Exception ex)
                {
                    // If an error ocurres when generating the scorings, skip and continue looping
                }
            }

            if (searchParams.StoreInCache)
            {
                string key = CacheKeys.Composed(CacheKeys.AssessmentScoringsBySectionUniqueId, searchParams.UniqueSectionCode);
                bool successfullyCachedScorings = await _cache.TrySet(key, scoringsFromApi);
            }

            return await ApplyFilters (scoringsFromApi, searchParams);
        }

        public async Task UpdateScorings(IEnumerable<ScoringAssessmentDTO> scorings)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            scorings = SafeParseForPost(scorings);

            foreach (var scoring in scorings)
            {
                foreach (var association in scoring.Associations)
                {
                    if (string.IsNullOrWhiteSpace(association.AssociationModel.Id))
                        await odsApi.Post("studentAssessments", association.AssociationModel);
                    else
                    {
                        string id = association.AssociationModel.Id;
                        association.AssociationModel.Id = null;
                        await odsApi.Post($"studentAssessments/{id}", association.AssociationModel);
                    }
                }
            }
        }

        private async Task<IEnumerable<ScoringAssessmentDTO>> ApplyFilters(IEnumerable<ScoringAssessmentDTO> scorings, ScoringAssessmentSearchParams searchParams)
        {
            scorings = string.IsNullOrWhiteSpace(searchParams.Category)
                ? scorings
                : FilterByCategoryDescriptor(scorings, searchParams.Category);

            scorings = searchParams.OnlyFromCurrentNamespace.HasValue
                ? await FilterByNamespace(scorings, searchParams.OnlyFromCurrentNamespace.Value)
                : scorings;

            // Remove sensitive data
            scorings = scorings.Select(scoring =>
            {
                scoring.Student = _mapper.SecuredStudentDTO(scoring.Student);
                return scoring;
            }).ToList();

            // Add score result if none existent
            foreach (var scoring in scorings)
            {
                foreach (var association in scoring.Associations)
                {
                    if (association.AssociationModel.ScoreResults.Count == 0)
                    {
                        var score = association.Assessment.Scores?.FirstOrDefault();
                        association.AssociationModel.ScoreResults.Add(new StudentAssessmentScoreResultsItem
                        {
                            AssessmentReportingMethodType = score?.AssessmentReportingMethodType ?? "Raw Score",
                            Result = "",
                            ResultDatatypeType = score?.ResultDatatypeType ?? "Integer",
                        });
                    }
                }
            }

            return scorings;
        }

        private IEnumerable<ScoringAssessmentDTO> FilterByCategoryDescriptor(IEnumerable<ScoringAssessmentDTO> scorings, string category)
        {
            var filtered = scorings.Select(scoring =>
            {
                string categoryNormalized = category.Trim().ToLower();

                scoring.Associations = scoring.Associations.Where(association =>
                {
                    bool categoryMatches = association.Assessment.CategoryDescriptor.Trim().ToLower().Contains(categoryNormalized);
                    return categoryMatches;
                }).ToList();

                return scoring;
            }).ToList();

            return filtered;
        }

        private async Task<IEnumerable<ScoringAssessmentDTO>> FilterByNamespace(IEnumerable<ScoringAssessmentDTO> scorings, bool onlyFromCurrentNamespace)
        {
            var currentNamespace = await _odsApiSettingsProvider.GetAssessmentNamespace();

            return scorings.Select(scoring =>
            {
                scoring.Associations = scoring.Associations
                    .Where(assoc =>
                    {
                        return onlyFromCurrentNamespace
                            ? assoc.Assessment.NamespaceProperty == currentNamespace
                            : assoc.Assessment.NamespaceProperty != currentNamespace;
                    }).ToList();

                return scoring;

            }).ToList();
        }

        private async Task<ScoringAssessmentDTO> GetAssessmentScoringsByStudent(StudentDTO student, string assessmentCategoryDescriptor = null)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var studentAssessments = await odsApi.Get<IList<StudentAssessmentv3>>("studentAssessments", new Dictionary<string, string>
            {
                { "studentUniqueId", student.StudentUniqueId },
            });

            var associationDTO = new ScoringAssessmentDTO()
            {
                Student = student,
                Associations = new List<StudentAssessmentAssociationDTO>(),
            };

            foreach (var studentAssessment in studentAssessments)
            {
                Assessment assessment;

                try
                {
                    assessment = await _assessments.GetByIdentifier(studentAssessment.AssessmentReference.AssessmentIdentifier);
                }
                catch (Exception ex)
                {
                    // Prevent loop from ending
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(assessmentCategoryDescriptor))
                {
                    if (assessment.CategoryDescriptor != assessmentCategoryDescriptor)
                    {
                        continue;
                    }
                }

                associationDTO.Associations = associationDTO.Associations.Append(new StudentAssessmentAssociationDTO
                {
                    Assessment = assessment,
                    AssociationModel = studentAssessment?.MapToStudentAssessmentv2(),
                });
            }

            return associationDTO;
        }

        private async Task<IEnumerable<ScoringAssessmentDTO>> GetAssessmentScoringsFromCacheOrDefault(string uniqueSectionCode)
        {
            string key = CacheKeys.Composed(CacheKeys.AssessmentScoringsBySectionUniqueId, uniqueSectionCode);
            bool scoringsAreCached = await _cache.TryHasKey(key);

            if (!scoringsAreCached)
                return null;

            var students = await _cache.GetOrDefault<IEnumerable<ScoringAssessmentDTO>>(key);

            if (students == null)
                return null;

            // Remove sensitive data
            students = students.Select(dto =>
            {
                dto.Student = _mapper.SecuredStudentDTO(dto.Student);
                return dto;
            });

            // Sort by last name
            students = students.OrderBy(student => student.Student.LastSurname);

            return students.ToList();
        }

        private async Task<string> GetStudentGradeLevelDescriptor(string studentUniqueId)
        {
            StudentSchoolAssociationv3 getLatestAssociation(IList<StudentSchoolAssociationv3> associations)
            {
                if (associations.Count == 0)
                    throw new Exception("Could not get latest association of student " + studentUniqueId);

                if (associations.Count == 1)
                    return associations.First();

                return associations
                    .OrderByDescending(association => DateTime.Parse(association.EntryDate))
                    .First();
            }

            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var associations = await odsApi.Get<IList<StudentSchoolAssociationv3>>("studentSchoolAssociations", new Dictionary<string, string>
            {
                ["studentUniqueId"] = studentUniqueId,
            });

            var latest = getLatestAssociation(associations);

            return latest.EntryGradeLevelDescriptor;
        }

        private ScoringAssessmentDTO SafeParseForPost(ScoringAssessmentDTO scoring)
        {
            _mapper.SecuredStudentDTO(_mapper.MapStudentDTO(scoring.Student));

            scoring.Associations = scoring.Associations.Select(association =>
            {
                association.AssociationModel.AssessmentReference.Identifier = association.Assessment.Identifier;

                if (string.IsNullOrWhiteSpace(association.AssociationModel.AssessmentReference.NamespaceProperty))
                    association.AssociationModel.AssessmentReference.NamespaceProperty = _odsApiSettingsProvider.GetAssessmentNamespace().Result;

                association.AssociationModel.StudentReference.StudentUniqueId = scoring.Student.StudentUniqueId;

                association.AssociationModel.Items = association.AssociationModel.Items.Select(item =>
                {
                    item.AssessmentItemReference.AssessmentIdentifier = association.Assessment.Identifier;

                    if (string.IsNullOrWhiteSpace(item.AssessmentItemReference.NamespaceProperty))
                        item.AssessmentItemReference.NamespaceProperty = _odsApiSettingsProvider.GetAssessmentNamespace().Result;

                    return item;
                }).ToList();

                return association;
            }).ToList();

            return scoring;
        }

        private IEnumerable<ScoringAssessmentDTO> SafeParseForPost(IEnumerable<ScoringAssessmentDTO> scorings)
        {
            scorings = scorings.Select(dto =>
            {
                _mapper.MapStudentDTO(dto.Student);
                dto.Student = _mapper.SecuredStudentDTO(dto.Student);
                return dto;
            }).ToList();

            return scorings
                .Select(scoring => SafeParseForPost(scoring))
                .ToList();
        }
    }
}
