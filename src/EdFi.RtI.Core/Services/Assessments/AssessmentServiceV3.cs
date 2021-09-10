using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DTOs.Assessments;
using EdFi.RtI.Core.Infrastructure;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.Models;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Providers.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Assessments
{
    public class AssessmentServiceV3 : IAssessmentService
    {
        private readonly ICacheStore _cacheProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;
        private readonly IOdsApiSettingsProvider _odsApiSettingsProvider;

        public AssessmentServiceV3(ICacheStore cacheProvider, IOdsApiClientProvider odsApiClientProvider, IOdsApiSettingsProvider odsApiSettingsProvider)
        {
            _cacheProvider = cacheProvider;
            _odsApiClientProvider = odsApiClientProvider;
            _odsApiSettingsProvider = odsApiSettingsProvider;
        }

        public async Task<Assessment> Create(Assessment assessment)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var assessmentv3 = new AssessmentModelv3
            {
                AcademicSubjects = assessment.AcademicSubjects?.Select(a => a.MapToAcademicSubjectDescriptorv3()),
                AdaptiveAssessment = assessment.AdaptiveAssessment ?? false,
                AssessmentCategoryDescriptor = assessment.CategoryDescriptor?.MapToAssessmentCategoryDescriptorv3(),
                AssessmentFamily = assessment.AssessmentFamilyReference?.Title,
                AssessmentForm = string.IsNullOrWhiteSpace(assessment.Form) ? null : assessment.Form,
                AssessmentIdentifier = Guid.NewGuid().ToString(),
                AssessmentTitle = assessment.Title,
                AssessmentVersion = assessment.Version ?? 0,
                MaxRawScore = assessment.MaxRawScore ?? 0.0,
                Namespace = await _odsApiSettingsProvider.GetAssessmentNamespace(),
                Nomenclature = assessment.Nomenclature,
                Period = assessment.PeriodDescriptor?.MapToAssessmentPeriodDescriptorV3(),
                RevisionDate = assessment.RevisionDate.HasValue ? assessment.RevisionDate.MapToYYYYMMdd() : "2000-01-01",
                Scores = assessment.Scores?.Select(a => a.MapToAssessmentScorev3()),
            };

            var postAssessmentResponse = await odsApi.Post("assessments", assessmentv3);
            var createdAssessmentId = await odsApi.HandleHttpResponseGetCreatedResourceId(postAssessmentResponse);

            var createdAssessment = await odsApi.Get<AssessmentModelv3>($"assessments/{createdAssessmentId}");
            var createdAssessmentv2 = createdAssessment.MapToAssessmentv2();

            var cachedAssessments = await _cacheProvider.GetOrDefault<IList<Assessment>>(CacheKeys.Assessments);

            if (cachedAssessments != null)
            {
                cachedAssessments.Add(createdAssessmentv2);

                cachedAssessments = cachedAssessments
                    .OrderBy(a => a.Title)
                    .ToList();

                await _cacheProvider.TrySet(CacheKeys.Assessments, cachedAssessments);
            }

            return createdAssessmentv2;
        }

        public async Task<Assessment> Update(string assessmentId, Assessment assessment)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var assessmentv3 = new AssessmentModelv3
            {
                AcademicSubjects = assessment.AcademicSubjects?.Select(a => a.MapToAcademicSubjectDescriptorv3()),
                AdaptiveAssessment = assessment.AdaptiveAssessment ?? false,
                AssessmentCategoryDescriptor = assessment.CategoryDescriptor?.MapToAssessmentCategoryDescriptorv3(),
                AssessmentFamily = assessment.AssessmentFamilyReference?.Title,
                AssessmentForm = string.IsNullOrWhiteSpace(assessment.Form) ? null : assessment.Form,
                AssessmentIdentifier = Guid.NewGuid().ToString(),
                AssessmentTitle = assessment.Title,
                AssessmentVersion = assessment.Version ?? 0,
                MaxRawScore = assessment.MaxRawScore ?? 0.0,
                Namespace = await _odsApiSettingsProvider.GetAssessmentNamespace(),
                Nomenclature = assessment.Nomenclature,
                Period = assessment.PeriodDescriptor?.MapToAssessmentPeriodDescriptorV3(),
                RevisionDate = assessment.RevisionDate.HasValue ? assessment.RevisionDate.MapToYYYYMMdd() : "2000-01-01",
                Scores = assessment.Scores?.Select(a => a.MapToAssessmentScorev3()),
            };

            var response = await odsApi.Put($"assessments/{assessmentId}", assessmentv3);
            await odsApi.HandleHttpResponse(response);

            var mappedAsStandard = assessmentv3.MapToAssessmentv2();
            mappedAsStandard.Id = assessmentId;

            var cachedAssessments = await _cacheProvider.GetOrDefault<IList<Assessment>>(CacheKeys.Assessments);

            if (cachedAssessments != null)
            {
                cachedAssessments = cachedAssessments
                    .Where(i => i.Id != assessmentId)
                    .ToList();

                cachedAssessments.Add(mappedAsStandard);

                await _cacheProvider.TrySet(CacheKeys.Assessments, cachedAssessments);
            }

            return mappedAsStandard;
        }

        public async Task<object> GetAssessmentFamilies()
        {
            // TODO How do we handle this?
            //throw new Exception("Assessment Families API is not supported in v3");
            return new List<object>();
        }

        public async Task<Assessment> GetById(string assessmentId)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var assessmentv3 = await odsApi.Get<AssessmentModelv3>($"assessments/{assessmentId}");
            return assessmentv3.MapToAssessmentv2();
        }

        public async Task<Assessment> GetByIdentifier(string identifier)
        {
            string key = CacheKeys.Composed(CacheKeys.AssessmentByIdentifier, identifier);
            bool assessmentIsCached = await _cacheProvider.TryHasKey(key);

            if (assessmentIsCached)
            {
                var cachedAssessment = await _cacheProvider.GetOrDefault<Assessment>(key);

                if (cachedAssessment != null)
                    return cachedAssessment;
            }

            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var assessmentsFromApiv3 = await odsApi.Get<IList<AssessmentModelv3>>("assessments", new Dictionary<string, string>()
            {
                { "assessmentIdentifier", identifier },
            });

            var foundAssessmentv3 = assessmentsFromApiv3.FirstOrDefault(a => a.AssessmentIdentifier == identifier);
            var assessmentv2 = foundAssessmentv3.MapToAssessmentv2();

            await _cacheProvider.TrySet(key, assessmentv2);

            return assessmentv2;
        }

        public async Task<IEnumerable<Assessment>> Delete(string assessmentId)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var response = await odsApi.Delete($"assessments/{assessmentId}");

            await odsApi.HandleHttpResponse(response);

            var cachedAssessments = await _cacheProvider.GetOrDefault<IList<Assessment>>(CacheKeys.Assessments);

            if (cachedAssessments != null)
            {
                cachedAssessments = cachedAssessments
                    .Where(a => a.Id != assessmentId)
                    .ToList();

                await _cacheProvider.TrySet(CacheKeys.Assessments, cachedAssessments);

                return cachedAssessments;
            }

            return await Search(new AssessmentSearchParams { });
        }

        public async Task<IEnumerable<Assessment>> Search(AssessmentSearchParams search)
        {
            if (search == null)
                throw new ArgumentNullException(nameof(search));

            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var assessmentsFromApi = await odsApi.Get<IList<AssessmentModelv3>>("assessments");
            var mappedStandard = assessmentsFromApi.Select(b => b.MapToAssessmentv2()).ToList();

            return await ApplySearchParams(mappedStandard, search);
        }

        private async Task<IEnumerable<Assessment>> ApplySearchParams(IEnumerable<Assessment> assessments, AssessmentSearchParams searchParams)
        {
            assessments = await ApplyNamespace(assessments, searchParams.CurrentNamespace);

            assessments = AssessmentSortFields.IsSortFieldValid(searchParams.SortField)
                ? ApplySorting(assessments, searchParams.SortField, searchParams.SortDesc)
                : ApplySorting(assessments, AssessmentSortFields.Default, searchParams.SortDesc);

            assessments = !string.IsNullOrWhiteSpace(searchParams.SearchValue)
                ? ApplySearchValue(assessments, searchParams.SearchValue)
                : assessments;

            assessments = searchParams.PageIndex.HasValue && searchParams.PageSize.HasValue
                ? ApplyPagination(assessments, searchParams.PageIndex.Value, searchParams.PageSize.Value)
                : assessments;

            return assessments;
        }

        private IEnumerable<Assessment> ApplySorting(IEnumerable<Assessment> assessments, string sortField, bool sortDesc)
        {
            if (sortField == AssessmentSortFields.LastModifiedDate)
            {
                assessments = sortDesc
                    ? assessments.OrderByDescending(i => i.RevisionDate)
                    : assessments.OrderBy(i => i.RevisionDate);
            }
            else if (sortField == AssessmentSortFields.CategoryDescriptor)
            {
                assessments = sortDesc
                    ? assessments.OrderByDescending(i => i.CategoryDescriptor)
                    : assessments.OrderBy(i => i.CategoryDescriptor);
            }
            else if (sortField == AssessmentSortFields.Title)
            {
                assessments = sortDesc
                    ? assessments.OrderByDescending(i => i.Title)
                    : assessments.OrderBy(i => i.Title);
            }
            else if (sortField == AssessmentSortFields.MaxRawScore)
            {
                assessments = sortDesc
                    ? assessments.OrderByDescending(i => i.MaxRawScore)
                    : assessments.OrderBy(i => i.MaxRawScore);
            }

            return assessments;
        }

        private IEnumerable<Assessment> ApplySearchValue(IEnumerable<Assessment> assessments, string searchValue)
        {
            string normalizedSearchValue = searchValue.Trim().ToLower();

            return assessments
                .Where(i => i.Title.Trim().ToLower().Contains(searchValue))
                .ToList();
        }

        private IEnumerable<Assessment> ApplyPagination(IEnumerable<Assessment> assessments, int pageIndex, int pageSize)
        {
            int offset = pageSize * (pageIndex - 1);

            return assessments
                .Skip(offset)
                .Take(pageSize)
                .ToList();
        }

        private async Task<IEnumerable<Assessment>> ApplyNamespace(IEnumerable<Assessment> assessments, bool? filterByNamespace)
        {
            var assessmentNamespace = await _odsApiSettingsProvider.GetAssessmentNamespace();

            return !filterByNamespace.HasValue
                ? assessments
                : filterByNamespace.Value
                    ? assessments.Where(assessment => assessment.NamespaceProperty == assessmentNamespace)
                    : assessments.Where(assessment => assessment.NamespaceProperty != assessmentNamespace);
        }
    }
}
