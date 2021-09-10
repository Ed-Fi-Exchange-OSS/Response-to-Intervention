using EdFi.RtI.Core.DomainServiceProvider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.CacheRefresh
{
    public interface ICacheRefreshService : IDomainService
    {
        Task<string> GetLastRefreshedDateLocal(string key);
        Task<string> GetLastRefreshedDateUTC(string key);
        Task RefreshAssessments();
        Task RefreshInterventions();
        Task RefreshSchools();
        Task RefreshScoringAssessments();
        Task RefreshScoringInterventions();
        Task RefreshSections();
        Task RefreshSectionsByStaff();
        Task RefreshStaffs();
        Task RefreshStaffsBySchool();
        Task RefreshStaffEmailIdPairs();
        Task RefreshStudents();
        Task RefreshStudentsBySection();
        Task RefreshUserProfiles();
    }
}
