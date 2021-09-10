using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.RequestBodies;
using EdFi.RtI.Core.Services.ScoringAssessments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Cache
{
    public interface ICacheService
    {
        Task<CacheDTO> InitializeCache();
        Task CacheScoringAssessments(ScoringAssessmentSearchParams filters);
        Task CacheScoringAssessmentsAll(ScoringAssessmentSearchParams searchParams, IEnumerable<ScoringAssessmentDTO> scorings);
        Task CacheScoringInterventions(ScoringInterventionSearchFilters filters);
        Task<IEnumerable<School>> GetSchools();
        Task<IEnumerable<Staff>> GetStaffs(string schoolId);
        Task<IEnumerable<Section>> GetSections(string staffId);
        Task<IEnumerable<Student>> GetStudents(string sectionId);
        Task<IEnumerable<AssessmentCategoryDescriptor>> GetCategories(string instanceId, int schoolYear);
    }
}
