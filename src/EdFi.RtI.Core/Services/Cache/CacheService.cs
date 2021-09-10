using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.RequestBodies;
using EdFi.RtI.Core.Services.Catalogs;
using EdFi.RtI.Core.Services.Descriptors;
using EdFi.RtI.Core.Services.ScoringAssessments;
using EdFi.RtI.Core.Services.ScoringInterventions;
using EdFi.RtI.Core.Services.Students;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly ICacheStore _cache;
        private readonly IDomainServiceProvider _domainServiceProvider;

        public CacheService(ICacheStore cache, IDomainServiceProvider domainServiceProvider)
        {
            _cache = cache;
            _domainServiceProvider = domainServiceProvider;
        }

        private ICatalogService CatalogService => _domainServiceProvider.GetService<ICatalogService>();
        private IDescriptorService DescriptorService => _domainServiceProvider.GetService<IDescriptorService>();
        private IEdFiMapper EdFiMapper => _domainServiceProvider.GetService<IEdFiMapper>();
        private IScoringAssessmentsService ScoringAssessmentsService => _domainServiceProvider.GetService<IScoringAssessmentsService>();
        private IScoringInterventionsService ScoringInterventionsService => _domainServiceProvider.GetService<IScoringInterventionsService>();
        private IStudentService StudentsService => _domainServiceProvider.GetService<IStudentService>();

        public async Task CacheScoringAssessments(ScoringAssessmentSearchParams searchParams)
        {
            searchParams.GetFromCache = false;
            searchParams.StoreInCache = false;
            var scorings = await ScoringAssessmentsService.Search(searchParams);
            string key = CacheKeys.Composed(CacheKeys.AssessmentScoringsBySectionUniqueId, searchParams.UniqueSectionCode);
            await _cache.Set(key, scorings);
        }

        public async Task CacheScoringAssessmentsAll(ScoringAssessmentSearchParams searchParams, IEnumerable<ScoringAssessmentDTO> scorings)
        {
            string key = CacheKeys.Composed(CacheKeys.AssessmentScoringsBySectionUniqueId, searchParams.UniqueSectionCode);
            await _cache.TrySet(key, scorings);
        }

        public async Task CacheScoringInterventions(ScoringInterventionSearchFilters filters)
        {
            var associations = await ScoringInterventionsService.GetStudentInterventionsDTO(filters, getFromCache: false);
            string key = CacheKeys.Composed(CacheKeys.InterventionScoringsBySectionUniqueId, filters.SectionId);
            await _cache.Set(key, associations);
        }

        public async Task<CacheDTO> InitializeCache()
        {
            var cache = new CacheDTO
            {
                Schools = await GetSchools(),
                Staffs = new List<Staff>(),
                Sections = new List<Section>(),
                Students = new List<Student>(),
                Categories = await GetCategories("", 2002),
            };

            foreach (var school in cache.Schools)
            {
                var staffs = await GetStaffs(school.Id);
                cache.Staffs = cache.Staffs.Concat(staffs);
            }

            foreach (var staff in cache.Staffs)
            {
                var sections = await GetSections(staff.Id);
                cache.Sections = cache.Sections.Concat(sections);
            }

            foreach (var section in cache.Sections)
            {
                var students = await GetStudents(section.Id);
                cache.Students = cache.Students.Concat(students);
            }

            return cache;
        }

        public async Task<IEnumerable<School>> GetSchools()
        {
            object rawSchools = await CatalogService.GetSchoolsAll();
            return EdFiMapper.Map<IEnumerable<School>>(rawSchools);
        }

        public async Task<IEnumerable<Staff>> GetStaffs(string schoolId)
        {
            return await CatalogService.GetStaffsBySchool(schoolId);
        }

        public async Task<IEnumerable<Section>> GetSections(string staffId)
        {
            return await CatalogService.GetSectionsByStaff(staffId);
        }

        public async Task<IEnumerable<Student>> GetStudents(string sectionId)
        {
            return await StudentsService.GetStudentsBySectionId(sectionId);
        }

        public async Task<IEnumerable<AssessmentCategoryDescriptor>> GetCategories(string instanceId, int schoolYear)
        {
            object rawCategories = await DescriptorService.GetAssessmentCategoryDescriptors();
            return EdFiMapper.Map<IEnumerable<AssessmentCategoryDescriptor>>(rawCategories);
        }
    }
}
