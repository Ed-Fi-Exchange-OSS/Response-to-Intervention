using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Assessments;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Providers.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Assessments
{
    public class AssessmentServiceV2 : IAssessmentService
    {
        private readonly ICacheStore _cacheProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;
        private readonly IOdsApiSettingsProvider _odsApiSettingsProvider;

        public AssessmentServiceV2(ICacheStore cacheProvider, IOdsApiClientProvider odsApiClientProvider, IOdsApiSettingsProvider odsApiSettingsProvider)
        {
            _cacheProvider = cacheProvider;
            _odsApiClientProvider = odsApiClientProvider;
            _odsApiSettingsProvider = odsApiSettingsProvider;
        }

        public async Task<Assessment> GetById(string assessmentId)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            return await ods.Get<Assessment>($"assessments/{assessmentId}");
        }

        public async Task<Assessment> GetByIdentifier(string identifier)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            var assessments = await ods.Get<IList<Assessment>>("assessments", new Dictionary<string, string>
            {
                ["identifier"] = identifier
            });

            var assessment = assessments.FirstOrDefault();

            if (assessment == null)
                throw new AssessmentNotFoundException(identifier);

            return assessment;
        }

        public async Task<Assessment> Create(Assessment assessment)
        {
            assessment.Identifier = Guid.NewGuid().ToString();
            assessment.NamespaceProperty = await _odsApiSettingsProvider.GetAssessmentNamespace();

            var ods = await _odsApiClientProvider.NewResourcesClient();
            var response = await ods.Post("assessments", assessment);
            var createdAssessmentId = await ods.HandleHttpResponseGetCreatedResourceId(response);
            var createdAssessment = await ods.Get<Assessment>($"assessments/{createdAssessmentId}");

            var cachedAssessments = await _cacheProvider.Get<IList<Assessment>>(CacheKeys.Assessments);
            cachedAssessments.Add(createdAssessment);

            await _cacheProvider.TrySet(CacheKeys.Assessments, cachedAssessments);

            return createdAssessment;
        }

        public async Task<Assessment> Update(string assessmentId, Assessment assessment)
        {
            if (assessment.AssessmentFamilyReference != null && string.IsNullOrWhiteSpace(assessment.AssessmentFamilyReference.Title))
                assessment.AssessmentFamilyReference = null;

            var ods = await _odsApiClientProvider.NewResourcesClient();
            var response = await ods.Put($"assessments/{assessmentId}", assessment);
            await ods.HandleHttpResponse(response);

            var cachedAssessments = await _cacheProvider.Get<List<Assessment>>(CacheKeys.Assessments);
            var foundAssessmentIndex = cachedAssessments.FindIndex(x => x.Id == assessmentId);

            if (foundAssessmentIndex == -1)
                cachedAssessments.Add(assessment);
            else
                cachedAssessments[foundAssessmentIndex] = assessment;

            await _cacheProvider.Set(CacheKeys.Assessments, cachedAssessments);

            return assessment;
        }

        public async Task<object> GetAssessmentFamilies()
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            return await ods.Get<IList<AssessmentFamily>>("assessmentFamilies");
        }

        public async Task<IEnumerable<Assessment>> Delete(string assessmentId)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            var response = await ods.Delete($"assessments/{assessmentId}");
            await ods.HandleHttpResponse(response);

            var cachedAssessments = await _cacheProvider.Get<IList<Assessment>>(CacheKeys.Assessments);

            var filteredAssessments = cachedAssessments
                .Where(x => x.Id != assessmentId)
                .ToList();

            await _cacheProvider.TrySet(CacheKeys.Assessments, filteredAssessments);

            return filteredAssessments;
        }

        public async Task<IEnumerable<Assessment>> Search(AssessmentSearchParams searchParams)
        {
            if (searchParams == null)
                throw new ArgumentNullException(nameof(searchParams));

            var ods = await _odsApiClientProvider.NewResourcesClient();
            var assessmentsFromApi = await ods.Get<IEnumerable<Assessment>>("assessments", new Dictionary<string, string>
            {
                ["limit"] = "100",
            });

            return await ApplySearchParams(assessmentsFromApi, searchParams);
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

        private async Task<IEnumerable<Assessment>> ApplyNamespace(IEnumerable<Assessment> assessments, bool? currentNamespace)
        {
            var assessmentNamespace = await _odsApiSettingsProvider.GetAssessmentNamespace();

            return !currentNamespace.HasValue
                ? assessments
                : currentNamespace.Value
                    ? assessments.Where(assessment => assessment.NamespaceProperty == assessmentNamespace)
                    : assessments.Where(assessment => assessment.NamespaceProperty != assessmentNamespace);
        }
    }
}
