using EdFi.Ods.Api.Client;
using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.DTOs.Student;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.RequestBodies;
using EdFi.RtI.Core.Services.Assessments;
using EdFi.RtI.Core.Services.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EdFi.RtI.Core.DTOs.Composed.ScoringAssessmentDTO;

namespace EdFi.RtI.Core.Services.ScoringAssessments
{
    public class ScoringAssessmentsServiceV2 : IScoringAssessmentsService
    {
        private readonly ICacheStore _cache;
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;
        private readonly IOdsApiSettingsProvider _odsApiSettingsProvider;

        public ScoringAssessmentsServiceV2(ICacheStore cache, IDomainServiceProvider domainServiceProvider, IOdsApiClientProvider odsApiClientProvider, IOdsApiSettingsProvider odsApiSettingsProvider)
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
            var ods = await _odsApiClientProvider.NewResourcesClient();

            var assessment = await ods.Get<Assessment>($"assessments/{body.AssessmentId}");

            var studentAssessment = new StudentAssessment
            {
                AdministrationDate = DateTime.Parse(body.AdministrationDate),
                AdministrationEndDate = DateTime.Parse(body.AdministrationDate), // TODO Set AdministrationEndDate
                AssessmentReference = new StudentAssessmentAssessmentReference
                {
                    Identifier = assessment.Identifier,
                    NamespaceProperty = assessment.NamespaceProperty,
                },
                StudentReference = new StudentAssessmentStudentReference
                {
                    StudentUniqueId = body.StudentUniqueId,
                },
                Accommodations = new List<StudentAssessmentAccommodationsItem>(),
                AdministrationEnvironmentType = null,
                AdministrationLanguageDescriptor = null,
                EventCircumstanceType = null,
                EventDescription = null,
                Id = "",
                Identifier = Guid.NewGuid().ToString(),
                Items = new List<StudentAssessmentItemsItem>(),
                PerformanceLevels = new List<StudentAssessmentPerformanceLevelsItem>(),
                ReasonNotTestedType = null,
                RetestIndicatorType = null,
                ScoreResults = new List<StudentAssessmentScoreResultsItem> // At least one score result is needed for frontend scoring assessments grid
                {
                    new StudentAssessmentScoreResultsItem
                    {
                        AssessmentReportingMethodType = assessment.Scores == null || assessment.Scores.Count == 0
                            ? "Raw score"
                            : assessment.Scores.First().AssessmentReportingMethodType,
                        ResultDatatypeType = assessment.Scores == null || assessment.Scores.Count == 0
                            ? "Integer"
                            : assessment.Scores.First().ResultDatatypeType,
                        Result = "---",
                    },
                },
                SerialNumber = null,
                StudentObjectiveAssessments = new List<StudentAssessmentStudentObjectiveAssessmentsItem>(),
                WhenAssessedGradeLevelDescriptor = await GetStudentGradeLevelDescriptor(body.StudentUniqueId),
            };

            var response = await ods.Post("studentAssessments", studentAssessment);
            await ods.HandleHttpResponse(response);

            return studentAssessment;
        }

        public async Task<ScoringAssessmentDTO> CreateScoring(ScoringAssessmentDTO scoring)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            scoring = SafeParseForPost(scoring);

            var response = await ods.Post("studentAssessments", scoring.Associations.Last().AssociationModel);
            await ods.HandleHttpResponseGetCreatedResourceId(response);

            return scoring;
        }

        public async Task DeleteStudentAssessment(string studentAssessmentId, string uniqueSectionCode, string identifier)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            if (!string.IsNullOrWhiteSpace(studentAssessmentId))
                await ods.Delete($"studentAssessments/{studentAssessmentId}");

            var key = CacheKeys.Composed(CacheKeys.AssessmentScoringsBySectionUniqueId, uniqueSectionCode);

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
            var ods = await _odsApiClientProvider.NewResourcesClient();

            var key = CacheKeys.Composed(CacheKeys.AssessmentScoringsBySectionUniqueId, uniqueSectionCode);

            if (await _cache.TryHasKey(key))
            {
                var students = await _cache.Get<IEnumerable<ScoringAssessmentDTO>>(key);

                students = students.Select(dto =>
                {
                    _mapper.MapStudentDTO(dto.Student);
                    dto.Student = _mapper.SecuredStudentDTO(dto.Student);

                    foreach (StudentAssessmentAssociationDTO element in dto.Associations)
                    {
                        DateTime adminDate = Convert.ToDateTime(date);

                        if (!string.IsNullOrWhiteSpace(element.AssociationModel.Id) && element.Assessment.Id == assessmentId && element.AssociationModel.AdministrationDate == adminDate)
                            ods.Delete($"studentAssessments/{element.Assessment.Id}");

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

        private async Task<ScoringAssessmentDTO> GetAssessmentScoringsByStudent(StudentDTO student, string assessmentCategoryDescriptor = null)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            var studentAssessments = await ods.Get<IList<StudentAssessment>>("studentAssessments", new Dictionary<string, string>
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
                    assessment = await _assessments.GetByIdentifier(studentAssessment.AssessmentReference.Identifier);
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
                    AssociationModel = studentAssessment,
                });
            }

            return associationDTO;
        }

        public async Task UpdateScorings(IEnumerable<ScoringAssessmentDTO> scorings)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            scorings = SafeParseForPost(scorings);

            foreach (var scoring in scorings)
            {
                foreach (var association in scoring.Associations)
                {
                    if (string.IsNullOrWhiteSpace(association.AssociationModel.Id))
                        await ods.Post("studentAssessments", association.AssociationModel);
                    else
                        await ods.Put($"studentAssessments/{association.AssociationModel.Id}", association.AssociationModel);
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
                    })
                    .ToList();

                return scoring;

            }).ToList();
        }

        private async Task<string> GetStudentGradeLevelDescriptor(string studentUniqueId)
        {
            StudentSchoolAssociation getLatestAssociation(IList<StudentSchoolAssociation> associations)
            {
                if (associations.Count == 0)
                    throw new Exception("Could not get latest association of student " + studentUniqueId);

                if (associations.Count == 1)
                    return associations.First();

                return associations
                    .OrderByDescending(association => association.EntryDate)
                    .First();
            }

            var ods = await _odsApiClientProvider.NewResourcesClient();

            var associations = await ods.Get<IList<StudentSchoolAssociation>>("studentSchoolAssociations", new Dictionary<string, string>
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
